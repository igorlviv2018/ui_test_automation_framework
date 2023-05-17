using NLog;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models;
using Taf.UI.PageObjects;
using System.Collections.Generic;

namespace Taf.UI.Steps.Authoring
{
    public class TextSteps : BaseSteps
    {
        public TextSteps(ILogger logger) : base(App.Taf, logger)
        {

        }

        private readonly PageObjects.Authoring.ContentBlocks.TextBlock textBlock = new PageObjects.Authoring.ContentBlocks.TextBlock();

        private readonly ContentBlockProperties contentBlockProperties = new ContentBlockProperties();

        public string ConfigureTextBlock(TafEmArticleElement text)
        {
            string err = string.Empty;

            if (text.Data.GetType() == typeof(TafEmTextBlockData))
            {
                TafEmTextBlockData textData = ((TafEmTextBlockData)text.Data);

                err = ConfigureTextBlock(text.XPath, textData);
            }

            return err;
        }

        public string ConfigureTextBlock(string textBlockXpath, TafEmTextBlockData textData)
        {
            string err = string.Empty;

            List<TafEmLinkData> links = textData.Links;

            ScrollToView(textBlockXpath);

            textBlock.AddLine(textBlockXpath, textData.Text);

            for (int i = 0; i < links.Count; i++)
            {
                textBlock.AddTextToLastLine(textBlockXpath, " please visit the link: ");

                AddTextLink(textBlockXpath, links[i].LinkText, links[i].LinkUri);

                err += CheckTextLinkAuthoring(textBlockXpath, i + 1, links[i]);
            }

            return err;
        }

        public void AddTextLink(string blockXpath, string linkText, string linkUrl)
        {
            int lineLength = textBlock.GetLastLineLength(blockXpath);

            textBlock.AddTextToLastLine(blockXpath, linkText);

            textBlock.SelectTextInLine(blockXpath, lineLength, linkText.Length);

            textBlock.ClickLinkButton(blockXpath);

            textBlock.SetTooltipInputText(blockXpath, linkUrl);

            textBlock.ClickTooltipAction(blockXpath);

            textBlock.MoveToLineEnd(blockXpath);

            contentBlockProperties.ClickDoneButton();
        }

        public string CheckTextLinkAuthoring(string blockXpath, int linkPosition, TafEmLinkData expectedLink)
        {
            TafEmLinkData actualLink = textBlock.GetLinkDataAuthoring(blockXpath, linkPosition);

            string err = string.Empty;

            if (actualLink.LinkText != expectedLink.LinkText)
            {
                err = $"invalid actual link text: '{actualLink.LinkText}' (Expected: '{expectedLink.LinkText}'); ";
            }

            if (actualLink.LinkUri != expectedLink.LinkUri)
            {
                err = $"{err}invalid actual link URL: '{actualLink.LinkUri}' (Expected: '{expectedLink.LinkUri}');";
            }

            return ErrorHelper.AddPrefixToError(err, $"Link at position {linkPosition}: ");
        }
    }
}

