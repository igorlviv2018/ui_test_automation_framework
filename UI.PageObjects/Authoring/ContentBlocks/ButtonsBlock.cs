using Taf.UI.Core.Element;

namespace Taf.UI.PageObjects.Authoring.ContentBlocks
{
    public class ButtonsBlock : BasePage
    {
        private readonly string clickActionSelect = "//div[contains(@class,'edit-btn-w')]//select[@class='custom-select']";

        private readonly string targetSelect = "//div[contains(@class,'edit-btn-w')]//select[@class='custom-select']";

        private readonly string urlInput = "//input[@type='text' and contains(@placeholder,'https://')]";

        private readonly string labelInput = "//input[@type='text' and contains(@placeholder,'Click')]";

        private readonly string doneButton = "//button[contains(text(),'Done')]";

        private readonly string addButton = "//button[contains(text(),'Add button')]";

        private readonly string button = "//div[contains(@class,'buttons-wrap')]/button";

        public void SetLabel(string blockXpath, string labelText) => new Element($"{blockXpath}{labelInput}").SetText(labelText);

        public void SetLinkUrl(string blockXpath, string url) => new Element($"{blockXpath}{urlInput}").SetText(url);

        public void ClickDone() => new Element(doneButton).ClickIfExists();

        public void ClickAddButton() => new Element(addButton).ClickIfExists();

        public void ClickButton(string blockXpath) => new Element($"{blockXpath}{button}").ClickIfExists();

        public void SelectClickAction(string blockXpath, string selectValue) =>
            new Element(IndexedXpath($"{blockXpath}{clickActionSelect}", 1)).SelectFromDropdown(selectValue);

        public void SelectTarget(string blockXpath, string selectValue) =>
            new Element(IndexedXpath($"{blockXpath}{targetSelect}", 2)).SelectFromDropdown(selectValue);
    }
}
