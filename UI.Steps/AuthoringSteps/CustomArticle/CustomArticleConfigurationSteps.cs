using NLog;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models;
using Taf.UI.PageObjects;
using Taf.UI.PageObjects.Authoring.ContentBlocks;
using System.Collections.Generic;
using System.IO;
using ImageBlock = Taf.UI.PageObjects.Authoring.ContentBlocks.ImageBlock;

namespace Taf.UI.Steps.Authoring
{
    public class CustomArticleConfigurationSteps : BaseSteps
    {
        public CustomArticleConfigurationSteps(ILogger logger) : base(App.Taf, logger)
        {
            textSteps = new TextSteps(log);
        }

        private readonly AuthoringContentBlock contentBlock = new AuthoringContentBlock();

        private readonly VideoBlock videoBlock = new VideoBlock();

        private readonly ImageBlock imageBlock = new ImageBlock();

        private readonly PageObjects.Authoring.ContentBlocks.ButtonsBlock buttonsBlock = new PageObjects.Authoring.ContentBlocks.ButtonsBlock();

        private readonly ContentBlockProperties contentBlockProperties = new ContentBlockProperties();

        private readonly TextSteps textSteps;

        public string ConfigureArticleContentBlocks(List<TafEmArticleElement> elementSequence)
        {
            string err = string.Empty;

            foreach (var element in elementSequence)
            {
                LogHelper.LogInfo(log, $"Configuring content block: {element}");

                if (element.ElementType == ArticleContentElementType.Video)
                {
                    err = ConfigureVideoBlock(element);
                }

                if (element.ElementType == ArticleContentElementType.Image)
                {
                    err = ConfigureImageBlock(element);
                }

                if (element.ElementType == ArticleContentElementType.Text)
                {
                    err = textSteps.ConfigureTextBlock(element);
                }

                if (element.ElementType == ArticleContentElementType.StepByStep)
                {
                    err = AddStepsToStepByStepElement(element);
                }

                if (element.ElementType == ArticleContentElementType.ButtonsBlock)
                {
                    err = ConfigureButtonsBlock(element);
                }

                LogHelper.LogError(log, err);

                if (!string.IsNullOrEmpty(err))
                {
                    break;
                }
            }

            return err;
        }

        public string AddStepsToStepByStepElement(TafEmArticleElement stepByStep)
        {
            string err = string.Empty;

            ScrollToView(stepByStep.XPath);

            if (stepByStep.Data.GetType() == typeof(TafEmStepByStepData))
            {
                List<TafEmStepData> steps = ((TafEmStepByStepData)stepByStep.Data).Steps;    

                for (int i = 0; i < steps.Count; i++)
                {
                    contentBlock.ClickPlusButton(stepByStep.XPath); //add step

                    string stepXpath = contentBlock.StepInStepByStepAtPosition(stepByStep.XPath, i + 1);

                    SetStepProperties(stepXpath, steps[i].Title);

                    err = CheckContentElementPlaced(stepXpath, ArticleContentElementType.StepInStepByStep, steps[i].Title);

                    if (ErrorHelper.IsCriticalError(err))
                    {
                        break;
                    }

                    ConfigureImageBlock(stepXpath, steps[i].ImageFilePath); // set step image and/or text

                    err += textSteps.ConfigureTextBlock(stepXpath, steps[i].TextData);
                }
            }

            return err;
        }

        public string ConfigureVideoBlock(TafEmArticleElement video)
        {
            string err = string.Empty;

            if (video.Data.GetType() == typeof(TafEmVideoBlockData))
            {
                string url = ((TafEmVideoBlockData)video.Data).VideoUrl;

                if (!string.IsNullOrEmpty(url))
                {
                    videoBlock.SetVideoUrl(video.XPath, url);

                    videoBlock.ClickVideoAdd(video.XPath);

                    if (!videoBlock.IsVideoUrlValid(video.XPath))
                    {
                        err = $"[Critical] Invalid video url (video block '{video.Title}')";
                    }

                    contentBlockProperties.ClickDoneButton();
                }
            }

            return err;
        }

        public string ConfigureImageBlock(TafEmArticleElement image)
        {
            string err = string.Empty;

            if (image.Data.GetType() == typeof(TafEmImageData))
            {
                err = ConfigureImageBlock(image.XPath, ((TafEmImageData)image.Data).FilePath);
            }

            return err;
        }

        public string ConfigureImageBlock(string blockXpath, string filePath)
        {
            string err = string.Empty;

            if (!string.IsNullOrEmpty(filePath))
            {
                if (File.Exists(filePath))
                {
                    imageBlock.SetImagePath(blockXpath, filePath);

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

        public string ConfigureButtonsBlock(TafEmArticleElement buttonsElement)
        {
            string err = string.Empty;

            ScrollToView(buttonsElement.XPath);

            if (buttonsElement.Data.GetType() == typeof(TafEmButtonsBlockData))
            {
                List<TafEmProcessButtonData> buttons = ((TafEmButtonsBlockData)buttonsElement.Data).Buttons;

                for (int i = 0; i < buttons.Count; i++)
                {
                    if (i > 0)
                    {
                        buttonsBlock.ClickAddButton();
                    }

                    if (i == 0)
                    {
                        buttonsBlock.ClickButton(buttonsElement.XPath);
                    }

                    buttonsBlock.SelectTarget(buttonsElement.XPath, buttons[i].LinkButtonTarget.ToString());

                    buttonsBlock.SetLabel(buttonsElement.XPath, buttons[i].Label);

                    buttonsBlock.SetLinkUrl(buttonsElement.XPath, buttons[i].Url);

                    buttonsBlock.ClickDone();
                }
            }

            return err;
        }

        public void SetStepProperties(string stepXpath, string title = "", string caption = "")
        {
            if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(caption))
            {
                return;
            }

            contentBlock.ClickHeader(stepXpath);

            if (!string.IsNullOrEmpty(title))
            {
                contentBlockProperties.SetTitle(title);
            }

            if (!string.IsNullOrEmpty(caption))
            {
                contentBlockProperties.SetCaption(caption);
            }

            contentBlockProperties.ClickDoneButton();
        }

        public string CheckContentElementPlaced(string elementXpath, ArticleContentElementType elementType, string elementTitle)
        {
            string err = string.Empty;

            bool isElementTypeCorrect = UiWaitHelper.Wait(() => GetContentElementType(elementXpath) == elementType, WaitConstants.OneSecond);

            string actualTitle = contentBlock.GetTitle(elementXpath, elementType == ArticleContentElementType.Collapse);

            bool isTitleCorrect = true;

            if (elementType != ArticleContentElementType.ButtonsBlock)
            {
                isTitleCorrect = actualTitle == elementTitle;
            }

            if (!isElementTypeCorrect || !isTitleCorrect)
            {
                err = $"[Critical] Failed to place {elementType} element (title='{elementTitle}') to the correct location";
            }

            return err;
        }

        public string CheckContentElementPlaced(TafEmArticleElement element) =>
            CheckContentElementPlaced(element.XPath, element.ElementType, element.Title);

        public ArticleContentElementType GetContentElementType(string elementXpath) =>
            CommonHelper.GetAuthoringContentElementType(contentBlock.GetContentBlockType(elementXpath));

        public ArticleContentElementType GetTypeInElementProperties() =>
            CommonHelper.GetElementTypeInProperties(contentBlockProperties.GetElementType());
    }
}

