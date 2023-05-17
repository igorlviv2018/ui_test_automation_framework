using NLog;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models;
using Taf.UI.PageObjects;
using System.Collections.Generic;

namespace Taf.UI.Steps
{
    public class StepByStepSteps : BaseSteps
    {
        private readonly StepByStepBlock stepByStepBlock;

        private readonly bool isRedesign;

        public StepByStepSteps(App app, ILogger logger, bool isRedesign=false) : base(app, logger)
        {
            stepByStepBlock = new StepByStepBlock(app, isRedesign);

            this.isRedesign = isRedesign;
        }

        public void SetViewMode(StepByStepViewType type) => stepByStepBlock.ClickViewSwitch(type);

        public string CheckStepByStep(string stepByStepXpath, TafEmStepByStepData stepByStepData)
        {
            List<string> errors = new List<string>();

            stepByStepBlock.BaseXpath = stepByStepXpath;

            string err = CheckSwiping(stepByStepData);

            ErrorHelper.AddToErrorList(errors, err, "Step-by-step slider view: ");

            err = CheckListView(stepByStepData);

            ErrorHelper.AddToErrorList(errors, err, "Step-by-step list view: ");

            return ErrorHelper.ConvertErrorsToString(errors);
        }

        public string CheckSwiping(TafEmStepByStepData stepByStepData)
        {
            SetViewMode(StepByStepViewType.Slider);

            int currentSwiperPage = stepByStepBlock.GetSwiperActivePageNumber();

            int actualStepCount = stepByStepBlock.GetSwiperStepCount();

            List<string> errors = new List<string>();

            string err = CheckSwiperPageCount(stepByStepData);

            if (!string.IsNullOrEmpty(err))
            {
                return err;
            }

            if (currentSwiperPage == -1)
            {
                return "Step-by-step: No current page bullet is present in swiper mode";
            }

            if (currentSwiperPage != 1) // swipe to first step
            {
                for (int i = 0; i < currentSwiperPage - 1; i++)
                {
                    stepByStepBlock.ClickSwiperPrevButton();
                }

                if (isRedesign)
                {
                    if (stepByStepBlock.IsSwiperStepActive(1))
                    {
                        errors.Add("Failed to move to first swiper step.");
                    }
                }
                else
                {
                    currentSwiperPage = stepByStepBlock.GetSwiperActivePageNumber();

                    if (currentSwiperPage != 1)
                    {
                        errors.Add("Failed to move to first swiper step.");
                    }
                }
            }

            string errorPrefix = "Step {0}: (Swiping forwards): ";

            for (int i = 1; i <= actualStepCount; i++)
            {
                err = CheckSwiperStep(stepByStepData, i, actualStepCount);

                ErrorHelper.AddToErrorList(errors, err, string.Format(errorPrefix, i));

                err = CheckSwiperPageCount(stepByStepData);

                ErrorHelper.AddToErrorList(errors, err, string.Format(errorPrefix, i));

                if (i < actualStepCount)
                {
                    bool isButtonAvailable = isRedesign && stepByStepBlock.IsSwiperNextButtonDisplayed()
                        || !isRedesign && stepByStepBlock.IsSwiperNextButtonActive();

                    if (isButtonAvailable)
                    {
                        stepByStepBlock.ClickSwiperNextButton();
                    }
                    else 
                    {
                        ErrorHelper.AddToErrorList(errors, "Next button is disabled (but should be enabled)", string.Format(errorPrefix, i));
                    }
                }
            }

            errorPrefix = "Step {0}: (Swiping backwards): ";

            // swipe backwards
            for (int i = actualStepCount; i >= 1; i--)
            {
                err = CheckSwiperStep(stepByStepData, i, actualStepCount);

                ErrorHelper.AddToErrorList(errors, err, string.Format(errorPrefix, i));

                err = CheckSwiperPageCount(stepByStepData);

                ErrorHelper.AddToErrorList(errors, err, string.Format(errorPrefix, i));

                if (i > 1)
                {
                    bool isButtonAvailable = isRedesign && stepByStepBlock.IsSwiperPrevButtonDisplayed()
                        || !isRedesign && stepByStepBlock.IsSwiperPrevButtonActive();

                    if (isButtonAvailable)
                    {
                        stepByStepBlock.ClickSwiperPrevButton();
                    }
                    else
                    {
                        ErrorHelper.AddToErrorList(errors, "Prev button is disabled (but should be enabled)", string.Format(errorPrefix, i));
                    }
                }
            }

            return ErrorHelper.ConvertErrorsToString(errors);
        }

        public string CheckListView(TafEmStepByStepData stepByStepData)
        {
            SetViewMode(StepByStepViewType.List);

            List<string> errors = new List<string>();

            string err;

            int actualStepCount = stepByStepBlock.GetListStepCount();

            int expectedStepCount = stepByStepData.Steps.Count;

            if (actualStepCount != expectedStepCount)
            {
                ErrorHelper.AddToErrorList(errors,
                    $"Step-by-step page count is invalid: actual - {actualStepCount}, expected - {expectedStepCount}");
            }

            for (int i = 1; i <= actualStepCount; i++)
            {
                err = CheckListStep(stepByStepData, i);

                ErrorHelper.AddToErrorList(errors, err);
            }

            return ErrorHelper.ConvertErrorsToString(errors);
        }

        public string CheckSwiperPageCount(TafEmStepByStepData stepData)
        {
            int actualStepCount = stepByStepBlock.GetSwiperStepCount();

            int expectedSwiperPageCount = stepData.Steps.Count;

            return actualStepCount != expectedSwiperPageCount
                ? $"Page count is invalid: actual - {actualStepCount}, expected - {expectedSwiperPageCount}"
                : string.Empty;
        }

        public string CheckSwiperActivePageNumber(int stepNumber)
        {
            int actualPageNumber = isRedesign ? stepByStepBlock.GetSwiperActivePageNumberRedesign(stepNumber) : stepByStepBlock.GetSwiperActivePageNumber();

            return actualPageNumber != stepNumber
                ? $"Swiper step {stepNumber}: invalid page number - {actualPageNumber} (expected: {stepNumber})"
                : string.Empty;
        }

        public string CheckSwiperPaginationBulletsCount(int stepCount, int stepNumber)
        {
            int actualPageBulletCount = stepByStepBlock.GetSwiperPageBulletsCount();

            return actualPageBulletCount != stepCount
                ? $"Swiper step {stepNumber}: invalid page bullet count - {actualPageBulletCount} (expected: {stepCount})"
                : string.Empty;
        }

        public string CheckSwiperStepTitle(int stepNum, TafEmStepData stepData)
        {
            string expectedStepTitle = stepData.Title;

            if (expectedStepTitle.Length > 0)
            {
                if (isRedesign)
                {
                    UiWaitHelper.Wait(() => stepByStepBlock.GetSwiperStepTitle(stepNum).Length > 0, WaitConstants.CheckElementExistInSec);
                }
                else
                {
                    UiWaitHelper.Wait(() => stepByStepBlock.GetSwiperStepTitle().Length > 0, WaitConstants.CheckElementExistInSec);
                }
            }

            string actualStepTitle = isRedesign ? stepByStepBlock.GetSwiperStepTitle(stepNum) : stepByStepBlock.GetSwiperStepTitle();

            return actualStepTitle != expectedStepTitle
                ? $"Step {stepNum} title is invalid: actual - {actualStepTitle}, expected - {expectedStepTitle}"
                : string.Empty;
        }

        public string CheckListStepTitle(int stepNum, TafEmStepData stepData)
        {
            string actualStepTitle = stepByStepBlock.GetListStepTitle(stepNum);

            string expectedStepTitle = stepData.Title;

            return actualStepTitle != expectedStepTitle
                ? $"Step {stepNum} title is invalid: actual - {actualStepTitle}, expected - {expectedStepTitle}"
                : string.Empty;
        }

        public string CheckStepImage(int stepNumber, bool isListView=false)
        {
            List<string> errors = new List<string>();

            bool isImageDisplayed = isListView
                ? stepByStepBlock.IsImageInListStepDisplayed(stepNumber)
                : stepByStepBlock.IsImageInSwiperStepDisplayed(stepNumber);

            if (!isImageDisplayed)
            {
                errors.Add($"step {stepNumber}: image is not displayed");
            }

            return ErrorHelper.ConvertErrorsToString(errors);
        }

        public string CheckSwiperStep(TafEmStepByStepData expectedStepByStepData, int stepNumber, int actualStepCount)
        {
            List<string> errors = new List<string>();

            string err;

            if (app == App.Taf)
            {
                stepByStepBlock.WaitSpinnerToAppear();

                stepByStepBlock.WaitSpinnerToDisappear();
            }

            TafEmStepData expectedStepData = expectedStepByStepData.Steps[stepNumber - 1];

            //check swiper navigation buttons
            err = isRedesign ? CheckSwiperNavigateButtonsRedesign(stepNumber, actualStepCount) : CheckSwiperNavigateButtons(stepNumber, actualStepCount);
            ErrorHelper.AddToErrorList(errors, err);

            //check step is active
            if (isRedesign && !stepByStepBlock.IsSwiperStepActive(stepNumber))
            {
                err = $"Step {stepNumber} is not active";
                ErrorHelper.AddToErrorList(errors, err);
            }

            //check active page number
            err = CheckSwiperActivePageNumber(stepNumber);
            ErrorHelper.AddToErrorList(errors, err);

            //check step title
            err = CheckSwiperStepTitle(stepNumber, expectedStepData);
            ErrorHelper.AddToErrorList(errors, err);

            //check pagination bullets count (Embed only)
            if (app == App.Embed)
            {
                err = CheckSwiperPaginationBulletsCount(actualStepCount, stepNumber);
                ErrorHelper.AddToErrorList(errors, err);
            }

            //check images in a step
            err = CheckStepImage(stepNumber);
            ErrorHelper.AddToErrorList(errors, err);

            // step text links
            err = CheckLinks(linkXpath: stepByStepBlock.TextLinksInSwiperStepXpath(isRedesign ? stepNumber : 1), expectedStepData.TextData.Links);
            ErrorHelper.AddToErrorList(errors, err);

            return ErrorHelper.ConvertErrorsToString(errors);
        }

        public string CheckListStep(TafEmStepByStepData expectedStepByStepData, int stepNumber)
        {
            List<string> errors = new List<string>();

            string err;

            TafEmStepData expectedStepData = expectedStepByStepData.Steps[stepNumber - 1];

            //check step title - commented as in Embed step title is not displayed (bug?)
            if (app != App.Embed)
            {
                err = CheckListStepTitle(stepNumber, expectedStepData);
                ErrorHelper.AddToErrorList(errors, err);
            }

            //check images in a step
            err = CheckStepImage(stepNumber, isListView: true);
            ErrorHelper.AddToErrorList(errors, err);

            // step text links
            err = CheckLinks(stepByStepBlock.TextLinksInListStepXpath(stepNumber), expectedStepData.TextData.Links);
            ErrorHelper.AddToErrorList(errors, err);

            return ErrorHelper.ConvertErrorsToString(errors);
        }

        public string CheckSwiperNavigateButtons(int stepNum, int stepsCount)
        {
            List<string> errors = new List<string>();

            if (stepNum > stepsCount)
            {
                return $"Invalid step number (step num: {stepNum}): Step number cannot be greater that total steps count ({stepsCount})!";
            }

            bool isPrevBtnDisplayed = stepByStepBlock.IsSwiperPrevButtonDisplayed();

            if (!isPrevBtnDisplayed)
            {
                errors.Add($"Step {stepNum}: Prev button is not displayed on the swiper");
            }

            bool isNextBtnDisplayed = stepByStepBlock.IsSwiperNextButtonDisplayed();

            if (!isNextBtnDisplayed)
            {
                errors.Add($"Step {stepNum}: Next button is not displayed on the swiper");
            }

            bool isFirstStep = stepNum == 1;

            bool isLastStep = stepNum == stepsCount;

            bool isPrevBtnActive = stepByStepBlock.IsSwiperPrevButtonActive();

            bool isNextBtnActive = stepByStepBlock.IsSwiperNextButtonActive();

            string navBtnStates = $"Prev button active: {isPrevBtnActive}, Next button active: {isNextBtnActive}";

            if (isFirstStep && !isLastStep && (isPrevBtnActive || !isNextBtnActive))
            {
                errors.Add($"First step (step count > 1): {navBtnStates}");
            }

            if (!isFirstStep && isLastStep && (!isPrevBtnActive || isNextBtnActive))
            {
                errors.Add($"Last step (step count > 1): {navBtnStates}");
            }

            if (!isFirstStep && !isLastStep && (!isPrevBtnActive || !isNextBtnActive))
            {
                errors.Add($"Step {stepNum} (not first and not last): {navBtnStates}");
            }

            if (isFirstStep && isLastStep && (isPrevBtnActive || isNextBtnActive))
            {
                errors.Add($"Step (only one step in Step-by-step): {navBtnStates}");
            }

            return ErrorHelper.ConvertErrorsToString(errors);
        }

        public string CheckSwiperNavigateButtonsRedesign(int stepNum, int stepsCount)
        {
            List<string> errors = new List<string>();

            if (stepNum > stepsCount)
            {
                return $"Invalid step number (step num: {stepNum}): Step number cannot be greater that total steps count ({stepsCount})!";
            }

            bool isPrevBtnDisplayed = stepByStepBlock.IsSwiperPrevButtonDisplayed();

            bool isNextBtnDisplayed = stepByStepBlock.IsSwiperNextButtonDisplayed();

            bool isFirstStep = stepNum == 1;

            bool isLastStep = stepNum == stepsCount;

            string navBtnStates = $"Prev button displayed: {isPrevBtnDisplayed}, Next button displayed: {isNextBtnDisplayed}";

            if (isFirstStep && !isLastStep && (isPrevBtnDisplayed || !isNextBtnDisplayed))
            {
                errors.Add($"First step (step count > 1): {navBtnStates}");
            }

            if (!isFirstStep && isLastStep && (!isPrevBtnDisplayed || isNextBtnDisplayed))
            {
                errors.Add($"Last step (step count > 1): {navBtnStates}");
            }

            if (!isFirstStep && !isLastStep && (!isPrevBtnDisplayed || !isNextBtnDisplayed))
            {
                errors.Add($"Step {stepNum} (not first and not last): {navBtnStates}");
            }

            if (isFirstStep && isLastStep && (isPrevBtnDisplayed || isNextBtnDisplayed))
            {
                errors.Add($"Step (only one step in Step-by-step): {navBtnStates}");
            }

            return ErrorHelper.ConvertErrorsToString(errors);
        }
    }
}