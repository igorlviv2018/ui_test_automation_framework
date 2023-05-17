using Taf.UI.Core.Element;
using Taf.UI.Core.Helpers;

namespace Taf.UI.PageObjects
{
    public class EmbedSnippetBlock
    {
        private readonly string snippetOpenBtn = "//div[@class='container']/details/summary";

        private readonly string openedSnippetBlock = "//div[@class='container']/details[@open]";

        private readonly string snippetTextArea = "//div[@class='container']/details[@open]/textarea";

        private readonly string showContentBtn = "//div[@class='container']/details[@open]//button";

        public void Open()
        {
            Element openedBlock = new Element(openedSnippetBlock);

            if (!openedBlock.Exists())
            {
                new Element(snippetOpenBtn).Click();
            }

            bool IsShowContentBtnDisplayed() => new Element(showContentBtn).IsDisplayed();

            UiWaitHelper.Wait(IsShowContentBtnDisplayed, 2);
        }

        public void SetSnippet(string text) =>
            new Element(snippetTextArea).SetText(text);

        public void ClickShowContentBtn() =>
            new Element(showContentBtn).Click();
    }
}
