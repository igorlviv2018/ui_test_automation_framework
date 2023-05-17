using OpenQA.Selenium;
using Taf.UI.Core.Element;
using Taf.UI.Core.Models;

namespace Taf.UI.PageObjects.Authoring.ContentBlocks
{
    public class TextBlock : BasePage
    {
        private readonly string textArea = "//div[contains(@class,'ql-editor')]";

        private readonly string textLine = "//div[contains(@class,'ql-editor')]/p";

        private const string toolBar = "//div[contains(@class,'ql-toolbar')]";

        private const string toolTip = "//div[contains(@class,'ql-tooltip')]";

        private readonly string linkButton = $"{toolBar}//button[contains(@class,'ql-link')]";

        private readonly string toolTipAction = $"{toolTip}//a[contains(@class,'ql-action')]";

        private readonly string toolTipInput = $"{toolTip}/input[@type='text']";

        public string GetLastTextLine(string blockXpath) => $"({blockXpath}{textLine})[last()]";

        public bool IsTextAreaEmpty(string blockXpath) =>
            new Element($"{blockXpath}{textArea}").GetAttribute("class").Contains("ql-blank");

        public string LinkInTextAreaAtPositionXpath(string blockXpath, int index) => IndexedXpath($"{blockXpath}{textArea}//a", index);

        public void AddLine(string blockXpath, string text)
        {
            if (IsTextAreaEmpty(blockXpath))
            {
                new Element($"{blockXpath}{textArea}").SendKeys(text);
            }
            else
            {
                new Element(GetLastTextLine(blockXpath)).SendKeys(Keys.End + Keys.Enter + text);
            }
        }

        public void AddTextToLastLine(string blockXpath, string textToAdd)
        {
            Element lastLine = new Element(GetLastTextLine(blockXpath));

            if (lastLine.Exists())
            {
                lastLine.SendKeys(Keys.End + textToAdd);
            }
        }

        public void MoveToLineEnd(string blockXpath)
        {
            Element lastLine = new Element(GetLastTextLine(blockXpath));

            if (lastLine.Exists())
            {
                lastLine.SendKeys(Keys.End);
            }
        }

        public void SelectTextInLine(string blockXpath, int startPosition, int selectionLength) =>
            new Element(GetLastTextLine(blockXpath)).SelectText(startPosition, selectionLength);

        public void ClickLinkButton(string blockXpath) => new Element($"{blockXpath}{linkButton}").ClickIfExists();

        public void ClickTooltipAction(string blockXpath) => new Element($"{blockXpath}{toolTipAction}").ClickIfExists();

        public void SetTooltipInputText(string blockXpath, string url) => new Element($"{blockXpath}{toolTipInput}").SetText(url);

        public int GetLastLineLength(string blockXpath) => new Element(GetLastTextLine(blockXpath)).Text.Length;

        //public int GetTextLineCount(string blockXpath) => new Element($"{blockXpath}{textLine}").Count;

        public TafLinkData GetLinkDataAuthoring(string blockXpath, int position)
        {
            Element link = new Element(LinkInTextAreaAtPositionXpath(blockXpath, position));

            TafLinkData linkData = new TafLinkData();

            if (link.Exists())
            {
                string href = link.GetAttribute("href");

                if (!string.IsNullOrEmpty(href))
                {
                    linkData.LinkText = link.Text;
                    linkData.LinkUri = href.TrimEnd('/');
                }
            }

            return linkData;
        }
    }
}
