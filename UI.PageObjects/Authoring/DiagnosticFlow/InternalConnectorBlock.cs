using Taf.UI.Core.Element;

namespace Taf.UI.PageObjects
{
    public class InternalConnectorBlock : BasePage
    {
        // to update
        private readonly string input = "//div[@class='block-body']//input[@type='text']";

        private readonly string menuItem = "//ul//span[@class='icon-{0}']/../../span[@class='text-truncate' and contains(text(),'{1}')]";

        private readonly string dropdownCaret = "//span[contains(@class,'icon-caret')]";

        private readonly string dropdownMenu = "//ul[contains(@class,'search-ref-menu')]";

        public bool IsDropdownExpanded(string blockXpath) =>
            new Element(blockXpath + dropdownMenu).GetAttribute("class").Contains("show");

        public void ClickDropdownCaret(string blockXpath) => new Element(blockXpath + dropdownCaret).ClickIfExists();

        public bool IsMenuItemPresent(string intConnectorXpath, string icon, string blockTitle) =>
            new Element($"{intConnectorXpath}" + string.Format(menuItem, icon, blockTitle)).Exists();

        public void SelectMenuItem(string intConnectorXpath, string icon, string blockTitle) =>
            new Element($"{intConnectorXpath}" + string.Format(menuItem, icon, blockTitle)).ClickIfExists();

        //public void SetInputText(string blockXpath, string text) => new Element(blockXpath + input).SetText(text);

        // public bool IsSearchSuccessful(string blockXpath, string text) =>
        // new Element(blockXpath + string.Format(specificResult, text)).Exists();
    }
}
