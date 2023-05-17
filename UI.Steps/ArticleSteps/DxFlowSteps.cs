using NLog;
using Taf.UI.Core.Enums;
using Taf.UI.Core.ExtensionMethods;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models;
using Taf.UI.PageObjects;
using System.Collections.Generic;
using System.Linq;

namespace Taf.UI.Steps
{
    public class DxFlowSteps : BaseSteps
    {
        private readonly CurrentPage currentPage;

        private readonly DxFlowPathItemBlock pathItemBlock;

        private readonly DxFlowDecisionBlock decisionBlock;

        private readonly DxFlowProcessBlock processBlock;

        private readonly ImageBlock imageBlock;

        private readonly DxFlowHelper dxFlowHelper;

        private readonly DataHelper dataHelper;

        private readonly DeviceSteps deviceSteps;

        private readonly GuideModalSteps guideSteps;

        private readonly StepByStepSteps stepByStepSteps;

        public DxFlowSteps(App app, ILogger logger, bool isRedesign=false) : base(app, logger)
        {
            currentPage = new CurrentPage();

            pathItemBlock = new DxFlowPathItemBlock(app, isRedesign);

            decisionBlock = new DxFlowDecisionBlock(app, isRedesign);
            
            processBlock = new DxFlowProcessBlock(app, isRedesign);

            imageBlock = new ImageBlock(app, isRedesign);

            dxFlowHelper = new DxFlowHelper();

            dataHelper = new DataHelper();

            deviceSteps = new DeviceSteps(app, log, isRedesign);

            guideSteps = new GuideModalSteps(app, log, isRedesign);

            stepByStepSteps = new StepByStepSteps(app, log, isRedesign);
        }

        public void ClearTestedItemIds() => dxFlowHelper.ClearTestedItemIds();

        public string CheckAllPaths(List<TafEmArticleElement> articleElements)
        {
            IEnumerable<ArticlePath> paths = articleElements[0].FindArticlePaths(articleElements);

            List<string> allErrs = new List<string>();

            bool isFirstPath = true;

            foreach (var path in paths)
            {
                LogHelper.LogInfo(log, $"- Checking path: {path}");

                List<string> errs = CheckPath(path, isFirstPath);

                isFirstPath = false;

                LogHelper.LogErrorsIfAny(log, errs);

                ErrorHelper.AddToErrorList(allErrs, errs);
            }

            return ErrorHelper.ConvertErrorsToString(ErrorHelper.GetUniqueErrors(allErrs));
        }

        /// <summary>
        /// Check path that includes processes with buttons (Link, Next step, Previous step, restart flow)
        /// </summary>
        /// <param name="articlePath"></param>
        /// <param name="isFirstPath"></param>
        /// <returns>List of errors</returns>
        public List<string> CheckPath(ArticlePath articlePath, bool isFirstPath)
        {
            List<TafEmArticleElement> path = articlePath.Path;

            List<string> errors = new List<string>();

            int currentPosition = 0;

            bool isEndOfPath = false;

            //string fullPath = "";

            while (!isEndOfPath)
            {
                DxFlowPath renderedSubpath = dxFlowHelper.GetExpectedPathItemsAfterMovingForward(path, currentPosition, isFirstPath);

                LogHelper.LogInfo(log, $"Expected path after moving forward: {renderedSubpath}");

                int itemToBeProcessedPosition = renderedSubpath.ItemToProcessPosition;

                string err = MoveForward(path, renderedSubpath, itemToBeProcessedPosition);

                ErrorHelper.AddToErrorList(errors, err);

                if (ErrorHelper.IsAnyCriticalError(errors))
                {
                    return errors;
                }

                List<string> errorsOfPathItems = CheckPathItems(renderedSubpath.Items);

                ErrorHelper.AddToErrorList(errors, errorsOfPathItems);

                if (ErrorHelper.IsAnyCriticalError(errors))
                {
                    return errors;
                }

                // buttons in Process to check (order matters)
                List<ProcessButtonClickAction> clickActions = new List<ProcessButtonClickAction>()
                {
                    ProcessButtonClickAction.Link,
                    ProcessButtonClickAction.PreviousStep,
                    ProcessButtonClickAction.RestartFlow
                };

                foreach (var clickAction in clickActions)
                {
                    err = CheckButtonsInProcesses(path, renderedSubpath, itemToBeProcessedPosition, clickAction);

                    ErrorHelper.AddToErrorList(errors, err);

                    if (ErrorHelper.IsCriticalError(err))
                    {
                        return errors;
                    }
                }

                currentPosition = renderedSubpath.NextItemToProcessPosition;

                isEndOfPath = currentPosition == path.Count - 1;

                if (isEndOfPath)
                {
                    log.Info("Extra check of Restart flow button");

                    err = ExtraCheckRestartFlowButton(path, renderedSubpath);

                    ErrorHelper.AddToErrorList(errors, ErrorHelper.AddPostfixToError(err, $"({renderedSubpath})"));

                    if (ErrorHelper.IsCriticalError(err))
                    {
                        return errors;
                    }

                    //fullPath = $"{renderedSubpath}"; //to rem
                }
            }

            return errors; //ErrorHelper.GetUniqueErrors(errors); possibly?
        }

        public string MoveForward(List<TafEmArticleElement> path, DxFlowPath renderedPath, int itemToBeProcessedPosition)
        {
            int endOfPathPos = path.Count - 1;

            string actionDescription = string.Empty;

            List<string> errors = new List<string>();

            string err;

            if (itemToBeProcessedPosition != endOfPathPos && path[itemToBeProcessedPosition].ElementType == ArticleContentElementType.Decision)
            {
                int decisionId = path[itemToBeProcessedPosition].Id;

                string decisionTitle = path[itemToBeProcessedPosition].Title;

                //select answer in the decision block (decision path item)
                int pathItemPosition = dxFlowHelper.GetRenderedPathItemPosition(renderedPath.Items, decisionId);

                int answerToSelectPosition = dxFlowHelper.DecisionAnswerToSelectPosition(renderedPath, decisionId);

                bool isButtonView = dxFlowHelper.HasDecisionButtonView(renderedPath, decisionId);

                err = SelectDecisionAnswer(pathItemPosition, answerToSelectPosition, isButtonView, path[itemToBeProcessedPosition]);

                ErrorHelper.AddToErrorList(errors, err);

                string answerToSelect = dxFlowHelper.DecisionAnswerToSelect(renderedPath, decisionId);

                actionDescription = $"selecting Answer: '{answerToSelect}' in '{decisionTitle}' decision (path item no: {pathItemPosition})";

                // save decision and branch id of the processed decision
                if (!dxFlowHelper.IsDecisionUnderInternalConnector(path, itemToBeProcessedPosition))
                {
                    dxFlowHelper.ProcessedDecisons[decisionId] = dxFlowHelper.GetDecisionActiveBranchId(path, itemToBeProcessedPosition);
                }
            }
            else if (itemToBeProcessedPosition != endOfPathPos && path[itemToBeProcessedPosition].ElementType == ArticleContentElementType.Process)
            {
                int processId = path[itemToBeProcessedPosition].Id;

                string processTitle = path[itemToBeProcessedPosition].Title;

                //click 'Next step' button in the process buttons block (decision path item)
                DxFlowProcessButtonPosition nextStepButtonToClickPosition = dxFlowHelper.GetButtonInProcessPosition(renderedPath, processId, ProcessButtonClickAction.NextStep);

                if (dxFlowHelper.IsProcessButtonPositionValid(nextStepButtonToClickPosition))
                {
                    processBlock.ClickButton(nextStepButtonToClickPosition); //sometimes click not handled
                }

                actionDescription = $"clicking Button: 'Next step' in '{processTitle}' process (path item no: {nextStepButtonToClickPosition.ProcessPosition})";

                // save id of the process with clicked 'Next step' button as the last action
                dxFlowHelper.SaveIdOfProcessWithClickedNextStep(processId);
            }

            LogHelper.LogInfo(log, $"{actionDescription.CapitalizeFirstLetter()}");

            // check if expected count of path items is displayed
            int expectedItemCount = renderedPath.Items.Count;

            err = WaitForExpectedPathItemCount(expectedItemCount, actionDescription);

            ErrorHelper.AddToErrorList(errors, err);

            return ErrorHelper.ConvertErrorsToString(errors);
        }

        public string MoveToPathItemWithId(List<TafEmArticleElement> path, int id)
        {
            int endOfPathPosition = path.Count - 1;

            int currentPosition = 0;

            bool isEndOfPath = false;

            List<string> errors = new List<string>();

            while (!isEndOfPath)
            {
                DxFlowPath renderedPath = dxFlowHelper.GetExpectedPathItemsAfterMovingForward(path, currentPosition, true);

                int itemToBeProcessedPosition = renderedPath.ItemToProcessPosition;

                string err = MoveForward(path, renderedPath, itemToBeProcessedPosition);

                ErrorHelper.AddToErrorList(errors, err);
                
                if (errors.Count > 0 || dxFlowHelper.HasDxFlowPathItemWithId(renderedPath.Items, id))
                {
                    break;
                }

                currentPosition = renderedPath.NextItemToProcessPosition;

                isEndOfPath = currentPosition == endOfPathPosition;
            }

            return ErrorHelper.ConvertErrorsToString(errors);
        }

        public string WaitForExpectedPathItemCount(int expectedPathItemCount, string actionDescription="")
        {
            string err = string.Empty;

            LogHelper.LogInfo(log, $"Waiting for expected path item count: {expectedPathItemCount}");

            if (!pathItemBlock.WaitNumberOfPathItems(expectedPathItemCount))
            {
                int actualItemCount = pathItemBlock.GetDxFlowPathItemCount();

                err = $"[Critical] Invalid actual path items count ({actualItemCount}, expected: {expectedPathItemCount})";

                err = !string.IsNullOrEmpty(actionDescription) ? $"{err} after {actionDescription}" : err;
            }

            LogHelper.LogError(log, err);

            return err;
        }

        public void ClearProcessedPathItemsIds()
        {
            dxFlowHelper.ClearProcessedItemsIds();
        }

        public void ClearProcessedExternalConnectorPathItemsIds(List<TafEmArticleElement> path)
        {
            dxFlowHelper.ClearProcessedExternalConnectorItemsIds(path);
        }

        public string CheckButtonsInProcesses(List<TafEmArticleElement> path, DxFlowPath renderedPath, int itemToProcessPosition, ProcessButtonClickAction btnClickAction)
        {
            List<string> errors = new List<string>();

            string err;

            string errorPrefix = string.Empty;

            DxFlowProcessButtonPosition buttonPos = dxFlowHelper.GetUntestedButtonPositionAfterMovingForward(renderedPath, btnClickAction);

            while (dxFlowHelper.IsProcessButtonPositionValid(buttonPos))
            {
                List<DxFlowPathItemData> expectedPathItemsAfterBtnClick = new List<DxFlowPathItemData>();

                bool isProcessInExternalFlow = false;

                if (btnClickAction == ProcessButtonClickAction.PreviousStep)
                {
                    expectedPathItemsAfterBtnClick = renderedPath.ItemsBeforeMoveForward;
                }
                else if (btnClickAction == ProcessButtonClickAction.RestartFlow)
                {
                    int extConnectorPosition = dxFlowHelper.GetExternalConnectorPosition(renderedPath);

                    isProcessInExternalFlow = extConnectorPosition != -1 && buttonPos.ProcessPosition - 1 > extConnectorPosition;

                    expectedPathItemsAfterBtnClick = isProcessInExternalFlow
                        ? dxFlowHelper.GetInitialPathItemsInExternalFlow(renderedPath)
                        : dxFlowHelper.GetInitialPathItems(renderedPath);
                }
                else if (btnClickAction == ProcessButtonClickAction.Link)
                {
                    expectedPathItemsAfterBtnClick = renderedPath.Items;
                }

                if (btnClickAction == ProcessButtonClickAction.Link)
                {
                    currentPage.SaveWindowHandles();
                }

                string buttonDescription = $"'{btnClickAction}' button (in {dataHelper.GetElementDescriptionByButtonPosition(buttonPos, path)}) functionality";

                log.Info($"Checking {buttonDescription}");

                processBlock.ClickButton(buttonPos);

                if (btnClickAction == ProcessButtonClickAction.Link)
                {
                    string expectedUrl = buttonPos.ButtonData.Url;

                    err = currentPage.CheckLink(expectedUrl, $"Invalid {btnClickAction} button functionality ({buttonPos}): ");

                    ErrorHelper.AddToErrorList(errors, err);
                }

                int expectedPathItemCount = expectedPathItemsAfterBtnClick.Count;

                err = WaitForExpectedPathItemCount(expectedPathItemCount, $"Clicking {btnClickAction} button");

                ErrorHelper.AddToErrorList(errors, err);

                int pathItemId = renderedPath.Items[buttonPos.ProcessPosition - 1].AssociatedArticleElementId;

                dxFlowHelper.MarkButtonAsTested(pathItemId, btnClickAction);

                log.Info($"Checking path items after clicking {buttonDescription}");

                List<string> errorsOfPathItems = CheckPathItems(expectedPathItemsAfterBtnClick);

                ErrorHelper.AddToErrorList(errors, errorsOfPathItems);

                if (ErrorHelper.IsAnyCriticalError(errorsOfPathItems))
                {
                    errorPrefix = $"Invalid {btnClickAction} button functionality ({buttonPos}): ";

                    break;
                }

                if (btnClickAction == ProcessButtonClickAction.PreviousStep)
                {
                    log.Info($"Moving forward after {buttonDescription} clicked");

                    err = MoveForward(path, renderedPath, itemToProcessPosition);
                }

                if (btnClickAction == ProcessButtonClickAction.RestartFlow)
                {
                    if (isProcessInExternalFlow)
                    {
                        ClearProcessedExternalConnectorPathItemsIds(path);
                    }
                    else
                    {
                        ClearProcessedPathItemsIds();
                    }

                    // to remove [if] ?
                    if (path[buttonPos.ProcessPosition].ElementType != ArticleContentElementType.Terminator)
                    {
                        err = MoveToPathItemWithId(path, pathItemId);
                    }
                }

                ErrorHelper.AddToErrorList(errors, err);

                buttonPos = dxFlowHelper.GetUntestedButtonPositionAfterMovingForward(renderedPath, btnClickAction);
            }

            return ErrorHelper.ConvertErrorsToString(errors, errorPrefix);
        }

        public string ExtraCheckRestartFlowButton(List<TafEmArticleElement> path, DxFlowPath renderedPath)
        {
            List<string> errors = new List<string>();

            string err;

            string errorPrefix = string.Empty;

            List<DxFlowPathItemData> expectedPathItemsAfterBtnClick;

            List<DxFlowProcessButtonPosition> buttonsToCheck = new List<DxFlowProcessButtonPosition>();

            // restart flow button in External connector
            DxFlowProcessButtonPosition buttonPos = dxFlowHelper.GetRestartFlowButtonToAdditionalyTestPosition(renderedPath, true);

            buttonsToCheck.Add(buttonPos);

            buttonPos = dxFlowHelper.GetRestartFlowButtonToAdditionalyTestPosition(renderedPath); //non-external connector button

            buttonsToCheck.Add(buttonPos);

            bool isButtonInExternalConnector = true;

            foreach (var button in buttonsToCheck)
            {
                if (dxFlowHelper.IsProcessButtonPositionValid(button)
                    && !dxFlowHelper.AdditionallyTestedRestartFlowButtonProcessIds.Contains(button.ProcessId))
                {
                    expectedPathItemsAfterBtnClick = isButtonInExternalConnector
                        ? dxFlowHelper.GetInitialPathItemsInExternalFlow(renderedPath)
                        : dxFlowHelper.GetInitialPathItems(renderedPath);

                    int expectedPathItemCount = expectedPathItemsAfterBtnClick.Count;

                    processBlock.ClickButton(button);

                    err = WaitForExpectedPathItemCount(expectedPathItemCount, $"Clicking Restart flow button");

                    ErrorHelper.AddToErrorList(errors, err);

                    int processPathItemId = renderedPath.Items[button.ProcessPosition - 1].AssociatedArticleElementId;

                    dxFlowHelper.MarkRestartFlowButtonAsExtraTested(processPathItemId);

                    List<string> errorsOfPathItems = CheckPathItems(expectedPathItemsAfterBtnClick);

                    ErrorHelper.AddToErrorList(errors, errorsOfPathItems);

                    errorPrefix = $"Invalid Restart flow button functionality ({button}): ";

                    if (isButtonInExternalConnector)
                    {
                        ClearProcessedExternalConnectorPathItemsIds(path);
                    }
                    else
                    {
                        ClearProcessedPathItemsIds();
                    }
                }
                
                isButtonInExternalConnector = false;
            }

            return ErrorHelper.ConvertErrorsToString(errors, errorPrefix);
        }

        public List<string> CheckPathItems(List<DxFlowPathItemData> expectedPathItems, bool checkPathItemOnlyOnce=true)
        {
            int actualPathItemCount = pathItemBlock.GetDxFlowPathItemCount();

            int expectedPathItemCount = expectedPathItems.Count;

            List<string> errors = new List<string>();

            if (actualPathItemCount != expectedPathItemCount)
            {
                errors.Add($"[Critical] Invalid actual path item count: {actualPathItemCount}, expected: {expectedPathItemCount}");

                return errors;
            }

            for (int i = 1; i <= actualPathItemCount; i++) // check path items types
            {
                PathItemType actualType = pathItemBlock.GetDxFlowPathItemType(i);

                PathItemType expectedType = expectedPathItems[i - 1].ItemType;

                if (actualType != expectedType)
                {
                    errors.Add($"[Critical] Invalid path item type (at position {i}): {actualType}, expected: {expectedType}");

                    return errors;
                }
            }

            List<string> errorsOfPathItem = new List<string>();

            List<string> errorsOfTitleDescription = new List<string>();

            string errPrefix;

            for (int i = 0; i < expectedPathItemCount; i++)
            {
                if (dxFlowHelper.IsPathItemTested(expectedPathItems[i].AssociatedArticleElementId)) // skip path item if tested
                {
                    continue;
                }

                errorsOfPathItem.Clear();

                errorsOfTitleDescription.Clear();

                errPrefix = "";

                DxFlowPathItemData pathItem = expectedPathItems[i];

                string err = CheckPathItemTitle(i + 1, pathItem.Title); // check title & description

                ErrorHelper.AddToErrorList(errorsOfTitleDescription, err);

                err = CheckPathItemDescription(i + 1, pathItem.Description);

                ErrorHelper.AddToErrorList(errorsOfTitleDescription, err);

                if (pathItem.ItemType == PathItemType.Decision)
                {
                    errorsOfPathItem = CheckDecisionBlock(i + 1, pathItem, pathItem.IsProcessed);

                    errPrefix = $"Decision";
                }
                else if (pathItem.ItemType == PathItemType.Process)
                {
                    errorsOfPathItem = CheckProcessBlock(i + 1, pathItem);

                    errPrefix = $"Process";
                }
                else if (pathItem.ItemType == PathItemType.Terminator)
                {
                    errorsOfPathItem = CheckProcessBlock(i + 1, pathItem, true);

                    errPrefix = $"Terminator";
                }
                else if (pathItem.ItemType == PathItemType.PredefinedProcess)
                {
                    errorsOfPathItem = CheckPredefinedProcessBlock(i + 1, pathItem);

                    errPrefix = $"Predefined process";
                }
                else if (pathItem.ItemType == PathItemType.ExternalConnector)
                {
                    errorsOfPathItem = CheckExternalConnectorBlock(i + 1, pathItem);

                    errPrefix = $"External connector";
                }

                if (checkPathItemOnlyOnce && (pathItem.ItemType == PathItemType.Process
                    || pathItem.ItemType == PathItemType.PredefinedProcess
                    || pathItem.ItemType == PathItemType.Terminator))
                {
                    dxFlowHelper.MarkPathItemAsTested(pathItem.AssociatedArticleElementId);
                }

                errPrefix = $"{errPrefix} '{pathItem.Title}' at position: {i + 1}: ";

                ErrorHelper.AddToErrorList(errorsOfPathItem, errorsOfTitleDescription);

                err = ErrorHelper.ConvertErrorsToString(errorsOfPathItem);

                ErrorHelper.AddToErrorList(errors, err, errPrefix);

                LogHelper.LogErrorsIfAny(log, errors);

                //to add path item data to error message
            }

            return errors;
        }

        public string SelectDecisionAnswer(int pathItemPosition, int answerToSelectPosition, bool isButtonView, TafEmArticleElement decisionElement)
        {
            string err;

            if (isButtonView) 
            {
                err = decisionBlock.ClickButton(pathItemPosition, answerToSelectPosition);
            }
            else //dropdown view
            {
                decisionBlock.OpenDropdownMenu(pathItemPosition);

                List<TafEmBranchData> actualAnswers = decisionBlock.GetDropdownMenuItems(pathItemPosition);

                List<TafEmBranchData> expectedAnswers = dxFlowHelper.GetDecisionAnswersData(decisionElement);

                decisionBlock.SelectDropdownMenuItem(pathItemPosition, answerToSelectPosition);

                err = CheckDecisionAnswers(expectedAnswers, actualAnswers, isButtonView);
            }

            return ErrorHelper.AddPrefixToError( err, $"Decision '{decisionElement.Title}' at position {pathItemPosition}: ");
        }

        public List<string> CheckDecisionBlock(int pathItemNum, DxFlowPathItemData expectedDecisionData, bool isProcessed)
        {
            List<string> errors = new List<string>();

            if (expectedDecisionData.Data.GetType() != typeof(DxFlowDecisionBlockData))
            {
                errors.Add("Invalid type of expected data.");

                return errors;
            }

            DxFlowDecisionBlockData expectedDecisionBlockData = (DxFlowDecisionBlockData)expectedDecisionData.Data;

            bool isButtonView = expectedDecisionBlockData.HasBranchesButtonView;

            DxFlowDecisionBlockData actualDecisionBlockData = decisionBlock.GetActualDecisionBlockData(pathItemNum, isButtonView);

            if (isButtonView)
            {
                string err = CheckDecisionAnswers(expectedDecisionBlockData.AnswersData, actualDecisionBlockData.AnswersData, isButtonView);

                errors.Add(err);

                List<string> pressedButtonNames = decisionBlock.GetPressedButtonNames(pathItemNum);

                if (isProcessed && pressedButtonNames.Count != 1)
                {
                    errors.Add($"Invalid pressed buttons count: {pressedButtonNames.Count}, expected: 1 (pressed buttons: {string.Join(", ", pressedButtonNames)})");
                }

                if (!isProcessed && pressedButtonNames.Count != 0)
                {
                    errors.Add($"Unprocessed decision has pressed button(s): {string.Join(", ", pressedButtonNames)}");
                }
            }

            if (isProcessed && actualDecisionBlockData.SelectedAnswerData == null)
            {
                errors.Add($"Processed decision has no selected answer");
            }

            if (isProcessed && actualDecisionBlockData.SelectedAnswerData?.Answer != expectedDecisionBlockData.SelectedAnswerData?.Answer)
            {
                errors.Add($"Invalid selected answer: {actualDecisionBlockData.SelectedAnswerData?.Answer}, expected: {expectedDecisionBlockData.SelectedAnswerData?.Answer}");
            }

            if (!isProcessed && actualDecisionBlockData.SelectedAnswerData != null)
            {
                errors.Add($"Unprocessed decision has selected answer: {actualDecisionBlockData.SelectedAnswerData.Answer}");
            }

            if (!isButtonView) //dropdown view
            {
                // image check
                if (isProcessed && expectedDecisionBlockData.SelectedAnswerData.HasImage && !actualDecisionBlockData.SelectedAnswerData.IsImageDisplayed)
                {
                    errors.Add($"Image is not diplayed (or is broken) in selected answer: {actualDecisionBlockData.SelectedAnswerData?.Answer}");
                }

                if (isProcessed && !expectedDecisionBlockData.SelectedAnswerData.HasImage && actualDecisionBlockData.SelectedAnswerData.IsImageDisplayed)
                {
                    errors.Add($"Image is diplayed (it should not be displayed) in selected answer: {actualDecisionBlockData.SelectedAnswerData?.Answer}");
                }
            }

            return errors;
        }

        public List<string> CheckProcessBlock(int pathItemNum, DxFlowPathItemData expectedData, bool isTerminator=false)
        {
            List<string> errors = new List<string>();

            PathItemType expectedType = isTerminator ? PathItemType.Terminator : PathItemType.Process;

            if (expectedData.Data.GetType() != typeof(DxFlowProcessBlockData))
            {
                errors.Add("Invalid type of expected data.");

                return errors;
            }

            DxFlowProcessBlockData expectedProcessBlockData = (DxFlowProcessBlockData)expectedData.Data;

            List<ArticleContentElementType> expectedContentBlockTypes = CommonHelper.GetContentElementTypes(expectedProcessBlockData.BlockData);

            List<ArticleContentElementType> actualContentBlockTypes = processBlock.GetContentBlockTypes(pathItemNum);

            string err = DataHelper.CompareObjects(actualContentBlockTypes, expectedContentBlockTypes);

            ErrorHelper.AddToErrorList(errors, err, $"{expectedType} content block sequence is invalid: ");

            if (!string.IsNullOrEmpty(err))
            {
                return errors;
            }

            for (int i = 0; i < expectedContentBlockTypes.Count; i++)
            {
                err = CheckContentBlock(pathItemNum, i + 1, expectedProcessBlockData.BlockData[i]);

                ErrorHelper.AddToErrorList(errors, err, $"Content block ({expectedContentBlockTypes[i]}, position: {i + 1}): ");
            }

            return errors;
        }

        public List<string> CheckPredefinedProcessBlock(int pathItemNum, DxFlowPathItemData expectedData)
        {
            List<string> errors = new List<string>();

            if (expectedData.Data.GetType() != typeof(DxFlowPredefinedProcessBlockData))
            {
                errors.Add("Invalid type of expected data.");

                return errors;
            }

            //flowHelper.MarkPathItemAsTested(expectedData.AssociatedArticleElementId);

            DxFlowPredefinedProcessBlockData expectedProcessBlockData = (DxFlowPredefinedProcessBlockData)expectedData.Data;

            DxFlowPredefinedProcessBlock processBlock = new DxFlowPredefinedProcessBlock(App.Embed);

            processBlock.ClickButton(pathItemNum);

            string err = string.Empty;

            if (expectedProcessBlockData.GuideType != UiContentType.Article)
            {
                string randomDevice = deviceSteps.SelectRandomDeviceInDeviceSelector();

                err = string.IsNullOrEmpty(randomDevice)
                    ? $"Failed to select a random device ({randomDevice}) or Device selector popup not displayed!"
                    : string.Empty;
            }
            else
            {
                if (!guideSteps.IsGuideModalOpened()) // workaround as sometimes modal is not displayed after click
                {
                    processBlock.ClickButton(pathItemNum);
                }
            }

            if (!string.IsNullOrEmpty(err))
            {
                errors.Add(err);

                return errors;
            }

            if (guideSteps.IsGuideLoaded()) // check guide/custom article
            {
                if (expectedProcessBlockData.GuideType == UiContentType.Article)
                {
                    err = guideSteps.CheckArticleIsOpened(expectedProcessBlockData.GuideTitle);
                }

                guideSteps.CloseGuideModal();
            }
            else
            {
                err = $"Failed to open the guide/article ({expectedProcessBlockData.GuideTitle})";
            }

            if (!string.IsNullOrEmpty(err))
            {
                errors.Add(err);

                return errors;
            }

            return errors;
        }

        public List<string> CheckExternalConnectorBlock(int pathItemNum, DxFlowPathItemData expectedData)
        {
            List<string> errors = new List<string>();

            if (expectedData.Data.GetType() != typeof(DxFlowExternalConnectorBlockData))
            {
                errors.Add("Invalid type of expected data.");

                return errors;
            }

            string expectedExternalFlowTitle = ((DxFlowExternalConnectorBlockData)expectedData.Data).ExternalFlowTitle;

            string actualExternalFlowTitle = pathItemBlock.GetDxFlowExternalFlowTitle(pathItemNum);

            if (actualExternalFlowTitle != expectedExternalFlowTitle)
            {
                errors.Add($"Invalid actual external flow title: {actualExternalFlowTitle} - expected: {expectedExternalFlowTitle}");
            }

            return errors;
        }

        public string CheckProcessButtonsBlock(int pathItemNum, int blockNum, TafEmButtonsBlockData expectedData)
        {
            List<string> actualButtonLabels = processBlock.GetButtons(pathItemNum, blockNum);

            List<string> expectedButtonLabels = expectedData.Buttons.Select(b => b.Label).ToList();

            string err = actualButtonLabels.Count != expectedButtonLabels.Count
                ? $"Invalid actual count of buttons: {actualButtonLabels.Count} - expected: {expectedButtonLabels.Count}"
                : DataHelper.CompareStringLists(actualButtonLabels, expectedButtonLabels);

            return ErrorHelper.AddPrefixToError(err, $"Buttons check failed: ");
        }

        public string CheckPathItemTitle(int pathItemNum, string expectedTitle)
        {
            if (app == App.Embed) // to remove 'if' after AGE-97 is fixed.
            {
                return string.Empty;
            }

            bool isTitleCorrect = UiWaitHelper.WaitInMs(() => pathItemBlock.GetDxFlowPathItemTitle(pathItemNum) == expectedTitle, 500);

            string actualTitle = pathItemBlock.GetDxFlowPathItemTitle(pathItemNum);

            return !isTitleCorrect
                ? $"Invalid title: {actualTitle} - expected: {expectedTitle}"
                : string.Empty;
        }

        public string CheckPathItemDescription(int pathItemNum, string expectedDescription)
        {
            string actualDescription = pathItemBlock.GetDxFlowPathItemDescription(pathItemNum);

            return actualDescription != expectedDescription
                ? $"Invalid description: {actualDescription} - expected: {expectedDescription}"
                : string.Empty;
        }

        public string CheckContentBlock(int pathItemNum, int blockNum, object expectedData)
        {
            string err = string.Empty;

            string baseXpath = processBlock.DxFlowProcessContentBlockXpath(pathItemNum, blockNum);

            if (expectedData.GetType() == typeof(TafEmImageData))
            {
                err = CheckImage(imageBlock.ImageInDxFlowProcessXpath(pathItemNum, blockNum));
            }
            else if (expectedData.GetType() == typeof(TafEmTextBlockData))
            {
                err = CheckLinks($"{baseXpath}//a", ((TafEmTextBlockData)expectedData).Links);
            }
            else if (expectedData.GetType() == typeof(TafEmVideoBlockData))
            {
                err = CheckVideoBlock(baseXpath, (TafEmVideoBlockData)expectedData);
            }
            else if (expectedData.GetType() == typeof(TafEmButtonsBlockData))
            {
                err = CheckProcessButtonsBlock(pathItemNum, blockNum, (TafEmButtonsBlockData)expectedData);
            }
            else if (expectedData.GetType() == typeof(TafEmStepByStepData))
            {
                processBlock.DxFlowContentBlockScrollToView(pathItemNum, blockNum);

                err = stepByStepSteps.CheckStepByStep(baseXpath, (TafEmStepByStepData)expectedData);
            }

            return err;
        }

        public string CheckDecisionAnswers(List<TafEmBranchData> expectedAnswers, List<TafEmBranchData> actualAnswers, bool isButtonView)
        {
            List<string> errors = new List<string>();

            string answerPresentedAs = isButtonView ? "button" : "dropdown menu item";

            if (expectedAnswers.Count != actualAnswers.Count)
            {
                errors.Add($"[Critical] Invalid {answerPresentedAs} count: {actualAnswers.Count}, expected: {expectedAnswers.Count}");

                return ErrorHelper.ConvertErrorsToString(errors);
            }

            for (int i = 0; i < expectedAnswers.Count; i++)
            {
                if (expectedAnswers[i].Answer != actualAnswers[i].Answer)
                {
                    errors.Add($"Invalid {answerPresentedAs} (position: {i + 1}) name: '{actualAnswers[i].Answer}', expected: '{expectedAnswers[i].Answer}'");
                }

                if (expectedAnswers[i].HasImage && !actualAnswers[i].IsImageDisplayed)
                {
                    errors.Add($"{answerPresentedAs} image (position: {i + 1}) is not displayed");
                }

                if (!expectedAnswers[i].HasImage && actualAnswers[i].IsImageDisplayed)
                {
                    errors.Add($"{answerPresentedAs} image (position: {i + 1}) is displayed (but it should not)");
                }
            }

            return ErrorHelper.ConvertErrorsToString(errors);
        }
    }
}