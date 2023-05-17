using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models;
using Taf.UI.PageObjects;
using Taf.UI.PageObjects.Authoring.ContentBlocks;
using System.Collections.Generic;
using System.IO;
using ImageBlock = Taf.UI.PageObjects.Authoring.ContentBlocks.ImageBlock;
using ButtonsBlock = Taf.UI.PageObjects.Authoring.ContentBlocks.ButtonsBlock;
using NLog;

namespace Taf.UI.Steps.Authoring
{
    public class FlowBlockConfigurationSteps : BaseSteps
    {
        public FlowBlockConfigurationSteps(ILogger logger) : base(App.Taf, logger)
        {
            textSteps = new TextSteps(log);
        }

        private readonly AuthoringContentBlock contentBlock = new AuthoringContentBlock();

        private readonly FlowContentBlock flowContentBlock = new FlowContentBlock();

        private readonly VideoBlock videoBlock = new VideoBlock();

        private readonly ImageBlock imageBlock = new ImageBlock();

        private readonly PredefinedProcessBlock predefinedProcessBlock = new PredefinedProcessBlock();

        private readonly InternalConnectorBlock internalConnectorBlock = new InternalConnectorBlock();

        private readonly ExternalConnectorBlock externalConnectorBlock = new ExternalConnectorBlock();

        private readonly ButtonsBlock buttonsBlock = new ButtonsBlock();

        private readonly ContentBlockProperties contentBlockProperties = new ContentBlockProperties();

        private readonly TextSteps textSteps;

        public string ConfigureFlowContentBlock(TafEmArticleElement element)
        {
            string err = string.Empty;

            LogHelper.LogInfo(log, $"Configuring content block: {element}");

            if (element.ElementType == ArticleContentElementType.Process || element.ElementType == ArticleContentElementType.Terminator)
            {
                err = ConfigureProcess(element);
            }
            else if (element.ElementType == ArticleContentElementType.PredefinedProcess)
            {
                err = ConfigurePredefinedProcessBlock(element);
            }
            else if (element.ElementType == ArticleContentElementType.Decision)
            {
                err = ConfigureDecision(element);
            }
            else if (element.ElementType == ArticleContentElementType.Branch)
            {
                err = ConfigureBranch(element);
            }
            else if (element.ElementType == ArticleContentElementType.InternalConnector)
            {
                err = ConfigureInternalConnectorBlock(element);
            }
            else if (element.ElementType == ArticleContentElementType.ExternalConnector)
            {
                err = ConfigureExternalConnectorBlock(element);
            }

            LogHelper.LogError(log, err);

            return err;
        }

        public string AddStepsToStepByStepElement(TafEmStepByStepData stepByStepData, string stepByStepXpath)
        {
            string err = string.Empty;

            ScrollToView(stepByStepXpath);

            List<TafEmStepData> steps = stepByStepData.Steps;

            for (int i = 0; i < steps.Count; i++)
            {
                contentBlock.ClickPlusButton(stepByStepXpath); //add step

                string stepXpath = contentBlock.StepInStepByStepAtPosition(stepByStepXpath, i + 1);

                if (ErrorHelper.IsCriticalError(err))
                {
                    break;
                }

                // set step image and/or text
                ConfigureImageBlock(stepXpath, steps[i].ImageFilePath, false);

                err += textSteps.ConfigureTextBlock(stepXpath, steps[i].TextData);
            }

            return err;
        }

        public string ConfigureProcess(TafEmArticleElement process)
        {
            string err = string.Empty;

            if (process.Data.GetType() == typeof(DxFlowProcessBlockData))
            {
                List<object> blocks = ((DxFlowProcessBlockData)process.Data).BlockData;

                for (int i = 0; i < blocks.Count; i++)
                {
                    string innerBlockXpath = flowContentBlock.InnerBlockInProcessAtPosition(process.XPath, i + 1);

                    AddProcessInnerBlock(process.XPath, blocks[i]);

                    err = CheckInnerBlockAdded(process, i + 1);

                    if (!string.IsNullOrEmpty(err))
                    {
                        break;
                    }

                    err = ConfigureProcessInnerBlock(innerBlockXpath, blocks[i]);

                    if (!string.IsNullOrEmpty(err))
                    {
                        break;
                    }
                }
            }

            return err;
        }

        public string ConfigureProcessInnerBlock(string innerBlockXpath, object innerBlockData)
        {
            string err = string.Empty;

            if (innerBlockData.GetType() == typeof(TafEmTextBlockData))
            {
                err = textSteps.ConfigureTextBlock(innerBlockXpath, (TafEmTextBlockData)innerBlockData);
            }
            else if (innerBlockData.GetType() == typeof(TafEmImageData))
            {
                err = ConfigureImageBlock(innerBlockXpath, ((TafEmImageData)innerBlockData).FilePath, false);
            }
            else if (innerBlockData.GetType() == typeof(TafEmVideoBlockData))
            {
                err = ConfigureVideoBlock((TafEmVideoBlockData)innerBlockData, innerBlockXpath);
            }
            else if (innerBlockData.GetType() == typeof(TafEmStepByStepData))
            {
                err = AddStepsToStepByStepElement((TafEmStepByStepData)innerBlockData, innerBlockXpath);
            }
            else if (innerBlockData.GetType() == typeof(TafEmButtonsBlockData))
            {
                err = ConfigureButtonsBlock((TafEmButtonsBlockData)innerBlockData, innerBlockXpath);
            }

            return err;
        }

        public void AddProcessInnerBlock(string processXpath, object innerBlockData)
        {
            string innerBlockType = CommonHelper.GetProcessInnerBlockType(innerBlockData);

            if (!flowContentBlock.IsButtonInProcessVisible(processXpath, innerBlockType))
            {
                flowContentBlock.ClickBlockIcon(processXpath);
            }

            flowContentBlock.ClickButtonInProcess(processXpath, innerBlockType);
        }

        public string CheckInnerBlockAdded(TafEmArticleElement process, int innerBlockNum)
        {
            string err = string.Empty;

            if (!flowContentBlock.InnerBlockInProcessAtPositionExists(process.XPath, innerBlockNum))
            {
                string innerBlockType = CommonHelper.GetProcessInnerBlockType(((DxFlowProcessBlockData)process.Data).BlockData[innerBlockNum - 1]);

                err = $"Failed to add inner block ('{innerBlockType}', position: {innerBlockNum}) to {process.ElementType} ('{process.Title}')";
            }

            return err;
        }

        public string ConfigurePredefinedProcessBlock(TafEmArticleElement predefinedProcess)
        {
            string err = string.Empty;

            DxFlowPredefinedProcessBlockData predefinedData = (DxFlowPredefinedProcessBlockData)predefinedProcess.Data;

            predefinedProcessBlock.SetInputText(predefinedProcess.XPath, predefinedData.GuideTitle);

            if (!predefinedProcessBlock.IsSearchSpinnerDisappeared(predefinedProcess.XPath))
            {
                return $"[Critical] Search for '{predefinedData.GuideTitle}' {predefinedData.GuideType} was not finished";
            }

            string searchResultsSection = predefinedData.GuideType == UiContentType.Article ? "ss-file" : "ss-book";

            if (predefinedProcessBlock.IsSearchSuccessful(predefinedProcess.XPath, searchResultsSection, predefinedData.GuideTitle))
            {
                predefinedProcessBlock.SelectSearchResult(predefinedProcess.XPath, searchResultsSection, predefinedData.GuideTitle);
            }
            else
            {
                err = $"{predefinedData.GuideTitle} {predefinedData.GuideType} not found in search results";
            }

            return err;
        }

        public string ConfigureDecision(TafEmArticleElement decision)
        {
            string err = string.Empty;

            if (decision.Data.GetType() == typeof(TafEmDecisionData) && !((TafEmDecisionData)decision.Data).HasBranchesButtonView)
            {
                SelectMenuItemInBlockDropdown(decision, AuthoringFlowBlockMenuItem.Dropdown);
            }

            return err;
        }

        public string ConfigureBranch(TafEmArticleElement branch)
        {
            string err = string.Empty;

            if (branch.Data.GetType() == typeof(TafEmBranchData) && ((TafEmBranchData)branch.Data).HasImage)
            {
                SelectMenuItemInBlockDropdown(branch, AuthoringFlowBlockMenuItem.AddImage);

                flowContentBlock.ClickAddImageButtonInBranch(branch.XPath);

                string imageFilePath = ((TafEmBranchData)branch.Data).ImageFilePath;

                err = ConfigureImageBlock(branch.XPath, imageFilePath, isBranch: true);
            }

            return err;
        }

        public string ConfigureImageBlock(string blockXpath, string filePath, bool isBranch)
        {
            string err = string.Empty;

            if (!string.IsNullOrEmpty(filePath))
            {
                if (File.Exists(filePath))
                {
                    if (isBranch)
                    {
                        imageBlock.SetImagePathInBranch(blockXpath, filePath);
                    }
                    else
                    {
                        imageBlock.SetImagePath(blockXpath, filePath);
                    }

                    bool spinnerDisappeared = Spinner.WaitSpinnerToDisappear("Image loading spinner",
                        imageBlock.GetLoadingSpinnerXpath(blockXpath), throwException: false);

                    err = !spinnerDisappeared ?
                        $"[Critical] Image loading is not finished within {WaitConstants.SpinnerToDisappearInSec} s (file path '{filePath}')"
                        : string.Empty;

                    contentBlockProperties.ClickDoneButton();
                }
                else
                {
                    err = $"[Critical] Image file not found (file path '{filePath}')";
                }
            }

            return err;
        }

        public string ConfigureVideoBlock(TafEmVideoBlockData videoBlockData, string videoBlockXpath)
        {
            string err = string.Empty;

            string url = videoBlockData.VideoUrl;

            if (!string.IsNullOrEmpty(url))
            {
                videoBlock.SetVideoUrl(videoBlockXpath, url);

                videoBlock.ClickVideoAdd(videoBlockXpath);

                if (!videoBlock.IsVideoUrlValid(videoBlockXpath))
                {
                    err = $"[Critical] Invalid video url ('{videoBlockData.VideoUrl}')";
                }

                contentBlockProperties.ClickDoneButton();
            }

            return err;
        }

        public string ConfigureButtonsBlock(TafEmButtonsBlockData buttonsData, string buttonsBlockXpath)
        {
            string err = string.Empty;

            ScrollToView(buttonsBlockXpath);

            List<TafEmProcessButtonData> buttons = buttonsData.Buttons;

            for (int i = 0; i < buttons.Count; i++)
            {
                if (i > 0)
                {
                    buttonsBlock.ClickAddButton();
                }

                if (i == 0)
                {
                    buttonsBlock.ClickButton(buttonsBlockXpath);
                }

                buttonsBlock.SelectClickAction(buttonsBlockXpath, CommonHelper.GetClickActionInFlowButtonsBlock(buttons[i].ClickAction));

                buttonsBlock.SetLabel(buttonsBlockXpath, buttons[i].Label);

                if (buttons[i].ClickAction == ProcessButtonClickAction.Link)
                {
                    buttonsBlock.SetLinkUrl(buttonsBlockXpath, buttons[i].Url);

                    buttonsBlock.SelectTarget(buttonsBlockXpath, buttons[i].LinkButtonTarget.ToString());
                }

                buttonsBlock.ClickDone();
            }

            return err;
        }

        public string ConfigureInternalConnectorBlock(TafEmArticleElement internalConnector)
        {
            string err = string.Empty;

            if (!internalConnectorBlock.IsDropdownExpanded(internalConnector.XPath))
            {
                internalConnectorBlock.ClickDropdownCaret(internalConnector.XPath);
            }

            string iconTypeInDropdown = CommonHelper.GetIconTypeInIntConnectorDropdown(internalConnector.IntConnectorConnectionPoint.ElementType);

            string intConnectorConnectionPointTitle = internalConnector.IntConnectorConnectionPoint.Title;

            if (internalConnectorBlock.IsMenuItemPresent(internalConnector.XPath, iconTypeInDropdown, intConnectorConnectionPointTitle))
            {
                internalConnectorBlock.SelectMenuItem(internalConnector.XPath, iconTypeInDropdown, intConnectorConnectionPointTitle);
            }
            else
            {
                err = $"[Critical] Menu item '{intConnectorConnectionPointTitle}' (icon type: {iconTypeInDropdown}) not found in dropdown";
            }

            return err;
        }

        public string ConfigureExternalConnectorBlock(TafEmArticleElement external)
        {
            string err = string.Empty;

            string externalFlowTitle = ((DxFlowExternalConnectorBlockData)external.Data).ExternalFlowTitle;

            externalConnectorBlock.SetInputText(external.XPath, externalFlowTitle);

            if (!externalConnectorBlock.IsSearchSpinnerDisappeared(external.XPath))
            {
                return $"[Critical] Search for '{externalFlowTitle}' diagnostic flow was not finished";
            }

            if (externalConnectorBlock.IsSearchSuccessful(external.XPath, externalFlowTitle))
            {
                externalConnectorBlock.SelectSearchResult(external.XPath, externalFlowTitle);
            }
            else
            {
                err = $"'{externalFlowTitle}' diagnostic flow not found in search results";
            }

            return err;
        }

        public void SelectMenuItemInBlockDropdown(TafEmArticleElement block, AuthoringFlowBlockMenuItem menuItem)
        {
            bool isBranch = block.ElementType == ArticleContentElementType.Branch;

            if (!flowContentBlock.IsBlockDropdownExpanded(block.XPath, isBranch))
            {
                flowContentBlock.ClickBlockMenu(block.XPath, isBranch);
            }

            if (block.ElementType == ArticleContentElementType.Decision
                && (menuItem == AuthoringFlowBlockMenuItem.Buttons || menuItem == AuthoringFlowBlockMenuItem.Dropdown))
            {
                flowContentBlock.ClickRadioButton(block.XPath, menuItem);
            }
            else
            {
                flowContentBlock.ClickDropdownMenuItem(block.XPath, menuItem, isBranch);
            }

            if (flowContentBlock.IsBlockDropdownExpanded(block.XPath, isBranch)) //close dropdown
            {
                flowContentBlock.ClickBlockMenu(block.XPath, isBranch);
            }
        }
    }
}

