using NLog;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Exceptions;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models.TafAd;
using Taf.UI.PageObjects;
using Taf.UI.PageObjects.TafAd.Taf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Taf.UI.Steps.JourneyPreviewSteps
{
    public class JourneyPreviewSteps
    {
        private readonly JourneyStartPage journeyStartPage;

        private readonly NavigationElements navigationElements;

        private readonly ParameterStep parameterStep;

        private readonly JourneyResultPage resultPage;

        private readonly TafAdTestDataHelper TafAdHelper;

        private readonly EditorHeaderSteps editorHeaderSteps;

        private readonly Spinner spinner;

        private readonly ILogger log;

        private readonly App app;

        public JourneyPreviewSteps(ILogger logger, App app)
        {
            journeyStartPage = new JourneyStartPage();

            navigationElements = new NavigationElements();

            parameterStep = new ParameterStep();

            resultPage = new JourneyResultPage(app);

            TafAdHelper = new TafAdTestDataHelper();

            editorHeaderSteps = new EditorHeaderSteps(app);

            spinner = new Spinner(app);

            log = logger;

            this.app = app;
        }

        public void StartJourney(bool restart=false)
        {
            if (restart)
            {
                if (app == App.Taf)
                {
                    navigationElements.ClickStartOverButton();
                }
                else //SP Advisor
                {
                    editorHeaderSteps.GoBackToEditorFromPreview();

                    editorHeaderSteps.SelectPreview();
                }

                navigationElements.ClickStartOverButton();
            }
            else
            {
                if (app == App.Taf)
                {
                    journeyStartPage.ClickStartButton();
                }
                else //SP Advisor
                {
                    editorHeaderSteps.SelectPreview();
                }
            }

            CheckJourneyPreviewIsOpened();
        }

        public void CheckJourneyPreviewIsOpened()
        {
            bool isOpened = navigationElements.WaitButtonToAppear();

            if (!isOpened)
            {
                LogHelper.LogError(log, "Failed to open/start journey - first step not displayed");

                throw new Exception("Failed to open/start journey - first step not displayed");
            }
        }

        public void SelectParameters(List<string> parameterTitles, string stepTitle)
        {
            foreach (var parameterTitle in parameterTitles)
            {
                if (parameterStep.IsButtonPresent(parameterTitle))
                {
                    parameterStep.ClickButton(parameterTitle);
                }
                else
                {
                    throw new ElementNotFoundException($"Button '{parameterTitle}' is not present in the '{stepTitle}' step");
                }
            }
        }

        public void SelectParameters(List<bool> parametersToSelect, JourneyStep step)
        {
            List<string> buttonTitles = GetButtonTitles();

            step.ParametersToSelectTitles.Clear();

            for (int i = 0; i < parametersToSelect.Count; i++)
            {
                if (parametersToSelect[i])
                {
                    int buttonPosition = i + (step.HasIdontMind ? 2 : 1);

                    parameterStep.ClickButton(buttonPosition);

                    if (app == App.TafAd)
                    {
                        spinner.WaitSpinnerToDisappear(SpinnerType.JourneyEditorPreviewStepButtonClick);
                    }

                    step.ParametersToSelectTitles.Add(buttonTitles[buttonPosition - 1]);
                }
            }

            LogHelper.LogInfo(log, $"Step '{step.Title}' ({step.StepType}): selected parameters - '{string.Join(", ", step.ParametersToSelectTitles)}'");
        }

        public List<string> GetButtonTitles()
        {
            int buttonCount = parameterStep.GetButtonCount();

            List<string> titles = new List<string>();

            for (int i = 0; i < buttonCount; i++)
            {
                titles.Add(parameterStep.GetButtonName(i + 1));
            }

            return titles;
        }

        public void MoveToNextStep()
        {
            navigationElements.ClickButton("Next");
        }

        public void CheckJourney(List<JourneyStep> journeySteps)
        {
            List<Parameter> infinityParameters = new List<Parameter>();

            List<Parameter> intervalParameters = new List<Parameter>();

            List<string> brandParameters = new List<string>();

            LogHelper.LogInfo(log, "Journey check started");

            for (int i = 0; i < journeySteps.Count; i++)
            {
                JourneyStep currentStep = journeySteps[i];

                if (currentStep.StepType == JourneyStepType.Infinity)
                {
                    CheckStep(currentStep, i + 1);

                    SelectParameters(currentStep.SelectedParameters, currentStep);

                    TafAdHelper.AddToSelectedParameters(currentStep.ParametersToSelectTitles, currentStep, infinityParameters);
                }

                if (currentStep.StepType == JourneyStepType.Interval)
                {
                    CheckStep(currentStep, i + 1);

                    SelectParameters(currentStep.SelectedParameters, currentStep);

                    Parameter intervalParameter = currentStep.Parameters[0];

                    List<int> selectedIntervals = TafAdHelper.GetIntervals(currentStep.SelectedParameters);

                    TafAdHelper.AddIntervalRatings(intervalParameter, selectedIntervals);

                    intervalParameters.Add(intervalParameter);
                }

                if (currentStep.StepType == JourneyStepType.Brands)
                {
                    CheckStep(currentStep, i + 1);

                    SelectParameters(currentStep.SelectedParameters, currentStep);

                    brandParameters = currentStep.ParametersToSelectTitles;
                }

                MoveToNextStep();
            }

            // none Infinity selected parameter case support
            if (TafAdHelper.IsNoneInfinityParameterSelected(journeySteps))
            {
                TafAdHelper.AddAllInfinityParamsToSelectedParameters(journeySteps, infinityParameters);
            }

            string selectedParametersAsText = " [" + TafAdHelper.GetJourneyParameterCombinationAsText(journeySteps) + "]";
            
            CheckResults(infinityParameters, intervalParameters, brandParameters, selectedParametersAsText);
        }

        public void CheckJourneyMultiple(List<JourneyStep> journeySteps, int numOfCombinations)
        {
            for (int i = 0; i < numOfCombinations; i++)
            {
                bool restartJourney = i != 0;
                
                StartJourney(restartJourney);

                TafAdHelper.SetJourneyParameterCombinationInSteps(journeySteps);

                CheckJourney(journeySteps);
            }
        }

        public void CheckStep(JourneyStep expectedStep, int stepNumber)
        {
            List<string> errors = new List<string>();

            string err = CheckCurrentStepNumber(stepNumber);

            ErrorHelper.AddToErrorList(errors, err);

            err = CheckStepParameterButtons(expectedStep);

            ErrorHelper.AddToErrorList(errors, err);

            if (errors.Count > 0)
            {
                throw new ElementNotFoundException(ErrorHelper.ConvertErrorsToString(errors));
            }
        }

        private string CheckCurrentStepNumber(int expectedStepNumber)
        {
            string err = string.Empty;

            navigationElements.WaitProgressBarStepToBecomeActive(expectedStepNumber);

            int actualCurrentStepNumber = GetCurrentStepNumber();

            if (expectedStepNumber != actualCurrentStepNumber)
            {
                err = $"Invalid current step number: {actualCurrentStepNumber}, expected: {expectedStepNumber}";
            }

            return err;
        }

        public int GetCurrentStepNumber()
        {
            int stepCount = navigationElements.GetStepCount();

            int currentStep = -1;

            for (int i = 1; i <= stepCount; i++)
            {
                if (navigationElements.IsProgressBarStepActive(i))
                {
                    currentStep = i;

                    break;
                }
            }

            return currentStep;
        }

        public string CheckStepParameterButtons(JourneyStep expectedStep)
        {
            int actualButtonCount = parameterStep.GetButtonCount();

            int expectedButtonCount = TafAdHelper.GetStepParametersCount(expectedStep);

            if (expectedStep.HasIdontMind)
            {
                expectedButtonCount++;
            }

            List<string> actualButtonTitles = new List<string>();

            for (int i = 0; i < actualButtonCount; i++)
            {
                actualButtonTitles.Add(parameterStep.GetButtonName(i + 1));
            }

            List<string> expectedButtonTitles = TafAdHelper.GetExpectedParameterTitles(expectedStep);

            string err = string.Empty;

            if (actualButtonCount != expectedButtonCount)
            {
                err = $"{err} Invalid button count: {actualButtonCount} (expected: {expectedButtonCount})";
            }
            else
            {
                // workaround - bug AGE-477
                if (expectedStep.StepType == JourneyStepType.Infinity)
                {
                    List<string> missingButtons = expectedButtonTitles.Where(t => !parameterStep.IsButtonPresent(t)).ToList();

                    err = missingButtons.Count > 0 ? $"Buttons missing: {string.Join(", ", missingButtons)}" : string.Empty;
                }
                else if (expectedStep.StepType == JourneyStepType.Brands)
                {
                    List<string> missingButtons = expectedButtonTitles.Where(t => !parameterStep.IsButtonPresent(t)).ToList();

                    err = missingButtons.Count > 0 ? $"Buttons missing: {string.Join(", ", missingButtons)}" : string.Empty;
                }
                else 
                {
                    for (int i = 0; i < expectedButtonCount; i++)
                    {
                        if (actualButtonTitles[i] != expectedButtonTitles[i])
                        {
                            err = $"Invalid button title at postion ({i}): {actualButtonTitles[i]} (expected: {expectedButtonTitles[i]})";
                        }
                    }
                }
            }

            return err;
        }

        public bool IsResultsPageDisplayed()
        {
            bool isBackToPrevStepAvailable = UiWaitHelper.Wait(() => resultPage.IsBackToPreviousStepButtonDisplayed(), WaitConstants.CheckElementExistInSec);

            return isBackToPrevStepAvailable;
        }

        public void WaitExpectedRecommendedItemsDisplayed(int expectedRecommendedItemsCount)
        {
            //bool isBackToPrevStepAvailable = UiWaitHelper.Wait(() => resultPage.IsBackToPreviousStepButtonDisplayed(), WaitConstants.CheckElementExistInSec);

            bool isExpectedRecomItemsCountDisplayed = UiWaitHelper.Wait(
                () => resultPage.GetRecommendedItemsCount() == expectedRecommendedItemsCount, WaitConstants.CheckElementExistInSec);
        }

        public void CheckResults(List<Parameter> selectedParameters, List<Parameter> intervalParameters, List<string> selectedBrands, string selectedParametersAsText="")
        {
            List<RatedDevice> allDevicesSortedDesc = TafAdHelper.GetItemsListSortedByRatingDesc(selectedParameters, intervalParameters, selectedBrands);

            List<RatedDevice> expectedDevices = allDevicesSortedDesc.Count > 3 ? allDevicesSortedDesc.Take(3).ToList(): allDevicesSortedDesc;

            bool isResultPageDisplayed = IsResultsPageDisplayed();

            if (!isResultPageDisplayed)
            {
                throw new Exception("Result page is not displayed (loading wait time expired).");
            }

            WaitExpectedRecommendedItemsDisplayed(expectedDevices.Count);

            List<RatedDevice> actualDevices = GetActualRecommendedDevices();

            if (expectedDevices.Count == 0)
            {
                CheckEmptyResults();
            }

            List<string> mismatches = new List<string>();

            if (actualDevices.Count != expectedDevices.Count)
            {
                throw new Exception(
                    $"Results check failed: Invalid actual item count: {actualDevices.Count}, expected: {expectedDevices.Count} " +
                    $"(actual devices: {TafAdHelper.GetDeviceListAsString(actualDevices)}, " +
                    $"expected devices: {TafAdHelper.GetDeviceListAsString(expectedDevices)}) [Selected parameters: {selectedParametersAsText}]");
            }
            else
            {
                for (int i = 0; i < actualDevices.Count; i++)
                {
                    if (actualDevices[i].Manufacturer != expectedDevices[i].Manufacturer || actualDevices[i].Model != expectedDevices[i].Model)
                    {
                        string mismatch = $"Position {i + 1}: Invalid item: " + TafAdHelper.GetDeviceAsString(actualDevices[i]) + ", expected: "
                             + TafAdHelper.GetDeviceAsString(expectedDevices[i]);

                        mismatches.Add(mismatch);
                    }
                }
            }

            if (mismatches.Count > 0)
            {
                throw new Exception("Results check failed: " + string.Join("; ", mismatches) + $" [Selected parameters: {selectedParametersAsText}]");
            }
        }

        public List<RatedDevice> GetActualRecommendedDevices()
        {
            List<RatedDevice> actualDevices = new List<RatedDevice>();

            for (int i = 0; i < resultPage.GetRecommendedItemsCount(); i++)
            {
                RatedDevice device = new RatedDevice
                {
                    Manufacturer = resultPage.GetManufacturer(i + 1),

                    Model = resultPage.GetModel(i + 1)
                };

                actualDevices.Add(device);
            }

            return actualDevices;
        }

        public void CheckEmptyResults()
        {
            UiWaitHelper.Wait(() => resultPage.GetNoMatchingText().Length > 0, WaitConstants.CheckElementExistInSec);

            string actualText = resultPage.GetNoMatchingText();

            string expectedText = MessageConstants.NoMatchingDevicesText;

            if (actualText != expectedText)
            {
                throw new ElementNotFoundException($"Invalid text: {actualText}, expected: {expectedText}");
            }
        }
    }
}
