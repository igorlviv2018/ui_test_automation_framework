using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;

namespace Taf.UI.PageObjects
{
    public class ExternalConnectorBlock : BasePage
    {
        private readonly Spinner spinner = new Spinner(App.Taf);

        private readonly string input = "//div[@class='block-body']//input[@type='text']";

        private readonly string searchSpinner = "//span[contains(@class,'spinner-grow')]";

        private readonly string specificResult = "//div[contains(@class,'results-wrap')]/ul/li//div[@class='text-truncate' and contains(text(),'{0}')]";

        public void SetInputText(string blockXpath, string text)
        {
            new Element(blockXpath + input).SetText(text);
        }

        public bool IsSearchSpinnerDisappeared(string blockXpath)
        {
            spinner.WaitSpinnerToAppear("search spinner", blockXpath + searchSpinner);

            return Spinner.WaitSpinnerToDisappear("search spinner", blockXpath + searchSpinner, throwException: false);
        }

        public void SelectSearchResult(string blockXpath, string text) =>
            new Element(blockXpath + string.Format(specificResult, text)).ClickIfExists();

        public bool IsSearchSuccessful(string blockXpath, string text) =>
            new Element(blockXpath + string.Format(specificResult, text)).Exists();
    }
}
