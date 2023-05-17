using NLog;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models;
using Taf.UI.PageObjects;
using Taf.UI.PageObjects.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Taf.UI.Steps
{
    public class CustomArticleSteps : BaseSteps
    {
        private readonly CurrentPage currentPage;

        private readonly ContentBlock contentBlock;

        private readonly EmbedSnippetBlock snippetBlock;

        private readonly ImageBlock imageBlock;

        private readonly AccordionBlock accordionBlock;

        private readonly TextBlock textBlock;

        private readonly ButtonsBlock buttonsBlock;

        private readonly StepByStepSteps stepByStepSteps;

        private readonly BrowserSteps browserSteps;

        private readonly TafEmHelper embedHelper;

        private readonly Spinner spinner;

        private readonly bool isRedesign;

        public CustomArticleSteps(App app, ILogger logger, bool isRedesign=false) : base(app, logger)
        {
            contentBlock = new ContentBlock(app, isRedesign);

            imageBlock = new ImageBlock(app, isRedesign);

            accordionBlock = new AccordionBlock(app, isRedesign);

            buttonsBlock = new ButtonsBlock(app);

            stepByStepSteps = new StepByStepSteps(app, log, isRedesign);

            browserSteps = new BrowserSteps(log);

            embedHelper = new TafEmHelper(app, isRedesign);

            spinner = new Spinner(app);

            currentPage = new CurrentPage();

            snippetBlock = new EmbedSnippetBlock();

            textBlock = new TextBlock();

            this.isRedesign = isRedesign;
        }

        public void TypeSnippetAndShowContent(string contentId)
        {
            string snippet = "<div class=\"sp-content-widget\"" +
                             "\ndata-widget-type = \"1\"" +
                             "\ndata-content-id = \"{0}\"" +
                             "\ndata-client-id = \"112\"" +
                             "\n></ div > ";

            snippetBlock.Open();

            snippetBlock.SetSnippet(string.Format(snippet, contentId));

            snippetBlock.ClickShowContentBtn();

            spinner.WaitSpinnerToDisappear(SpinnerType.TafEmContentLoading);
        }

        public void OpenEmbedAppAndShowContent(string contentId)
        {
            browserSteps.OpenAppDeepLink(App.Embed);

            TypeSnippetAndShowContent(contentId);
        }

        public string CheckCustomArticle(List<TafEmArticleElement> articleElements)
        {
            IEnumerable<ArticlePath> paths = articleElements[0].FindArticlePaths(articleElements);

            List<string> errors = new List<string>();

            bool isArticleLoaded = contentBlock.WaitArticleLoad();

            if (!isArticleLoaded)
            {
                return "Failed to open article: ";
            }

            string err = CheckTopLevelElementCount(articleElements);

            errors.Add(err);

            foreach (var path in paths)
            {
                err = CheckCustomArticlePath(path);

                errors.Add(ErrorHelper.AddPostfixToError(err, $" ({path})"));

                if (ErrorHelper.IsCriticalError(err))
                {
                    break;
                }
            }

            return ErrorHelper.ConvertErrorsToString(errors);
        }

        public string CheckTextBlock(string textBlockXpath, TafEmTextBlockData textData)
        {
            string err = CheckLinks(textBlock.TextLinksXpath(textBlockXpath), textData.Links);

            return err;
        }

        public string CheckCollapseTitle(TafEmArticleElement collapse)
        {
            List<string> errors = new List<string>();

            string actualTitle = accordionBlock.GetCollapseTitle(collapse.XPath);

            if (collapse.Title != actualTitle)
            {
                errors.Add($"Collape title is invalid: {actualTitle} (expected: {collapse.Title})");
            }

            return ErrorHelper.ConvertErrorsToString(errors);
        }

        public string CheckAccordionCollapsesCount(TafEmArticleElement accordion, TafEmArticleElement childCollapse)
        {
            if (accordion.IsTested)
            {
                return string.Empty;
            }

            List<string> errors = new List<string>();

            int actualCount = accordionBlock.GetAccordionCollapseCount(childCollapse.XPath);

            int expectedCount = accordion.ChildrenIds.Count;

            if (expectedCount != actualCount)
            {
                errors.Add($"Collape count is invalid: {actualCount} (expected: {expectedCount})");
            }

            accordion.IsTested = true;

            return ErrorHelper.ConvertErrorsToString(errors);
        }

        public string CheckElementCountInCollase(ArticlePath articlePath)
        {
            string error = "";

            List<TafEmArticleElement> path = articlePath.Path;

            int collapseToCheckPosition = -1;

            for (int i = 0; i < path.Count; i++)
            {
                bool isNextElementCollapse = (i < path.Count - 1) && path[i + 1].ElementType == ArticleContentElementType.Collapse;

                if (path[i].ElementType == ArticleContentElementType.Accordion && isNextElementCollapse)
                {
                    collapseToCheckPosition = i + 1;
                }
            }

            if (collapseToCheckPosition > 0)
            {
                int actualCount = accordionBlock.GetElementsInCollapseCount(path[collapseToCheckPosition].XPath);

                int expectedCount = path.Count - collapseToCheckPosition - 1;

                if (expectedCount != actualCount)
                {
                    string collapseTitle = path[collapseToCheckPosition].Title;
                    
                    error = $"Count of elements in collapse ('{collapseTitle}') is invalid: {actualCount} (expected: {expectedCount})";
                }
            }

            return error;
        }

        public string CheckElementCountInCollaseRedesign(ArticlePath articlePath, int collapsePositionInPath)
        {
            string error = string.Empty;

            List<TafEmArticleElement> path = articlePath.Path;

            int expectedCount= 0;

            for (int i = collapsePositionInPath; i < path.Count; i++)
            {
                bool isEndOfPath = i == path.Count - 1;

                bool isNextElementCollapse = !isEndOfPath && path[i + 1].ElementType == ArticleContentElementType.Collapse;

                if (isNextElementCollapse || isEndOfPath)
                {
                    break;
                }
                else
                {
                    expectedCount++;
                }
            }
            
            int actualCount = accordionBlock.GetElementsInCollapseCount(path[collapsePositionInPath].XPath);

            if (expectedCount != actualCount)
            {
                string collapseTitle = path[collapsePositionInPath].Title;

                error = $"Count of elements in collapse ('{collapseTitle}') is invalid: {actualCount} (expected: {expectedCount})";
            }

            return error;
        }

        public string CheckCustomArticlePath(ArticlePath articlePath)
        {
            List<string> errors = new List<string>();

            embedHelper.AddXpathToPathElements(articlePath);

            string err;

            List<TafEmArticleElement> path = articlePath.Path;

            if (!isRedesign) //old design check
            {
                err = CheckElementCountInCollase(articlePath);

                errors.Add(err);
            }
            
            for (int i = 0; i < path.Count; i++)
            {
                if (!CommonHelper.IsElementVisible(path[i])
                    || (path[i].IsTested && path[i].ElementType != ArticleContentElementType.Collapse))
                {
                    continue;
                }
                
                ArticleContentElementType actualType = contentBlock.GetContentElementType(path[i].XPath);

                ArticleContentElementType expectedType = path[i].ElementType;

                if (actualType != expectedType)
                {
                    errors.Add($"[Critical] Invalid path item type: {actualType}, expected: {expectedType}");

                    break;
                }

                if (path[i].ElementType == ArticleContentElementType.Collapse)
                {
                    err = accordionBlock.OpenCollapse(path[i].XPath);

                    errors.Add(ErrorHelper.AddPrefixToError(err, $"[Critical] Collapse '{path[i].Title}': "));

                    //new redesign - check count of elements in collapse
                    if (isRedesign && accordionBlock.IsRecentCollapseExpandedFromClosedState)
                    {
                        err = CheckElementCountInCollaseRedesign(articlePath, i);

                        errors.Add(err);
                    }

                    err = CheckCollapseTitle(path[i]);

                    errors.Add(err);

                    err = CheckAccordionCollapsesCount(path[i - 1], path[i]);

                    errors.Add(err);
                }
                else if (path[i].ElementType == ArticleContentElementType.Video)
                {
                    err = CheckVideoBlock(path[i].XPath, (TafEmVideoBlockData)path[i].Data);

                    errors.Add(err);
                }
                else if (path[i].ElementType == ArticleContentElementType.Image)
                {
                    err = CheckImage(path[i].XPath + imageBlock.GetImageRelativeXpath());//, (TafEmImageData)path[i].Data, app);

                    errors.Add(err);

                    err = CheckImageTitle(path[i].XPath, path[i].Title);

                    errors.Add(err);
                }
                else if (path[i].ElementType == ArticleContentElementType.Text)
                {
                    err = CheckTextBlock(path[i].XPath, (TafEmTextBlockData)path[i].Data);

                    errors.Add(err);
                }
                else if (path[i].ElementType == ArticleContentElementType.StepByStep)
                {
                    err = stepByStepSteps.CheckStepByStep(path[i].XPath, (TafEmStepByStepData)path[i].Data);

                    errors.Add(err);
                }
                else if (path[i].ElementType == ArticleContentElementType.ButtonsBlock)
                {
                    err = CheckButtonsBlock(path[i].XPath, (TafEmButtonsBlockData)path[i].Data);

                    errors.Add(err);
                }
                //else if (path[i].ElementType == ArticleContentElementType.Table)
                //{
                //    //err = CheckButtonsBlock(path[i].XPath, (TafEmButtonsBlockData)path[i].Data);

                //    //errors.Add(err);
                //}

                MarkAsTested(path[i]);

                if (ErrorHelper.IsAnyCriticalError(errors))
                {
                    break;
                }
            }

            return ErrorHelper.ConvertErrorsToString(errors);
        }

        public string CheckTopLevelElementCount(List<TafEmArticleElement> articleElements)
        {
            int actualCount = contentBlock.GetArticleTopLevelElementCount();

            int expectedCount = GetTopLevelElementsIds(articleElements);

            return actualCount != expectedCount
                ? $"Invalid top level element count: {actualCount} (expected: {expectedCount})"
                : string.Empty;
        }

        public string CheckButtonsBlock(string buttonsBlockXpath, TafEmButtonsBlockData expectedData)
        {
            List<string> errors = new List<string>();

            List<string> actualButtonLabels = buttonsBlock.GetButtonLabels(buttonsBlockXpath);

            List<string> expectedButtonLabels = expectedData.Buttons.Select(b => b.Label).ToList();

            string err = actualButtonLabels.Count != expectedButtonLabels.Count
                ? $"Invalid actual count of buttons: {actualButtonLabels.Count} - expected: {expectedButtonLabels.Count}"
                : DataHelper.CompareStringLists(actualButtonLabels, expectedButtonLabels);

            errors.Add(err);

            for (int i = 0; i < expectedData.Buttons.Count; i++) // check links
            {
                if (expectedData.Buttons[i].ClickAction == ProcessButtonClickAction.Link)
                {
                    currentPage.SaveWindowHandles();

                    buttonsBlock.ClickButton(buttonsBlockXpath, i + 1);

                    err = currentPage.CheckLink(expectedData.Buttons[i].Url, $"Link button '{expectedData.Buttons[i].Label}' at position {i + 1}: ");

                    errors.Add(err);
                }
            }

            return ErrorHelper.AddPrefixToError(ErrorHelper.ConvertErrorsToString(errors), $"Buttons check failed: ");
        }

        public int GetTopLevelElementsIds(List<TafEmArticleElement> articleElements)
        {
            List<int> topLevelElementIds = new List<int>();

            TafEmArticleElement root = articleElements
                .Where(e => e.ElementType == ArticleContentElementType.Root).FirstOrDefault();

            return root != null ? root.ChildrenIds.Count : 0;
        }
    }
}

