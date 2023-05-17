using NLog;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models;
using Taf.UI.PageObjects;
using Taf.UI.PageObjects.Helpers;
using System.Collections.Generic;
using System.Drawing;

namespace Taf.UI.Steps.Authoring
{
    public class CustomArticleCreateSteps : BaseSteps
    {
        public CustomArticleCreateSteps(ILogger logger) : base(App.Taf, logger)
        {
            blockConfigurationSteps = new CustomArticleConfigurationSteps(log);
        }

        private readonly ArticleEditContentPage contentPage = new ArticleEditContentPage();

        private readonly AuthoringContentBlock contentBlock = new AuthoringContentBlock();

        private readonly ContentBlockProperties contentBlockProperties = new ContentBlockProperties();

        private readonly TafAuthHelper authoringHelper = new TafAuthHelper();

        private readonly CustomArticleConfigurationSteps blockConfigurationSteps;

        public string AddContentElement(TafEmArticleElement elementToPlaceOnContentHolder)
        {
            string blockToDropElementToXpath = authoringHelper.ContentBlockToDropElementToXpath(elementToPlaceOnContentHolder.Parent);

            if (elementToPlaceOnContentHolder.ElementType == ArticleContentElementType.Collapse
                && elementToPlaceOnContentHolder.ElementPosition > 1)
            {
                contentBlock.ClickPlusButton(blockToDropElementToXpath); // add collapse
            }

            if (elementToPlaceOnContentHolder.Parent.ElementType == ArticleContentElementType.Collapse)
            {
                contentBlock.OpenCollapse(elementToPlaceOnContentHolder.Parent.XPath);
            }

            string err;

            if (elementToPlaceOnContentHolder.ElementType != ArticleContentElementType.Collapse)
            {
                string instrumentBlockName = CommonHelper.GetAuthoringInstrumentBlockTitle(elementToPlaceOnContentHolder);

                err = CheckElementHeight(blockToDropElementToXpath);

                bool isBlockSizeValid = string.IsNullOrEmpty(err);

                if (!isBlockSizeValid)
                {
                    return ErrorHelper.AddPrefixToError(err, $"{elementToPlaceOnContentHolder.Parent.ElementType}: ");
                }

                contentPage.PlaceContentElement(instrumentBlockName, blockToDropElementToXpath);
            }

            if (elementToPlaceOnContentHolder.ElementType != ArticleContentElementType.ButtonsBlock)
            {
                SetContentBlockProperties(elementToPlaceOnContentHolder, elementToPlaceOnContentHolder.Title);
            }

            err = blockConfigurationSteps.CheckContentElementPlaced(elementToPlaceOnContentHolder);

            return err;
        }

        public string CheckElementHeight(string xPath)
        {
            Size pageSize = contentBlock.GetViewPortSize();

            Size elementSize = contentBlock.GetElementSize(xPath);

            string err = string.Empty;

            if (elementSize.Height > pageSize.Height)
            {
                err = $"Element height exceeds page height (element height: {elementSize.Height}, page height: {pageSize.Height})";
            }

            return err;
        }

        public string CreateArticleContent(List<TafEmArticleElement> elementSequence)
        {
            string err = PlaceArticleContentBlocks(elementSequence);

            if (!string.IsNullOrEmpty(err))
            {
                return err;
            }

            err = blockConfigurationSteps.ConfigureArticleContentBlocks(elementSequence);

            return err;
        }

        public string PlaceArticleContentBlocks(List<TafEmArticleElement> elementSequence)
        {
            string err = string.Empty;

            foreach (var element in elementSequence)
            {
                err = AddContentElement(element);

                LogHelper.LogResult(log, $"Placed article content block: {element}", err);

                if (!string.IsNullOrEmpty(err))
                {
                    break;
                }
            }
            
            return err;
        }

        public void SetContentBlockProperties(TafEmArticleElement element, string title="", string caption="")
        {
            if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(caption))
            {
                return;
            }

            bool isCollapse = element.ElementType == ArticleContentElementType.Collapse;

            if (isCollapse)
            {
                contentBlock.ClickCollapseHeader(element.XPath);
            }
            else
            {
                contentBlock.ClickHeader(element.XPath);
            }

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

        public List<TafEmArticleElement> GetCustomArticleBlockSequence(List<TafEmArticleElement> articleElements)
        {
            foreach (var path in articleElements[0].FindArticlePaths(articleElements))
            {
                authoringHelper.AddXpathToPathElements(path);
            }

            return articleElements[0].TreeTraversalSequence;
        }
    }
}

