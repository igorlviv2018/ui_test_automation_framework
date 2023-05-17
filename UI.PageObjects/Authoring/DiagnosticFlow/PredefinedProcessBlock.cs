using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;

namespace Taf.UI.PageObjects
{
    public class PredefinedProcessBlock : BasePage
    {
        private readonly Spinner spinner = new Spinner(App.Taf);

        private readonly string input = "//div[@class='block-body']//input[@type='text']";

        private readonly string searchSpinner = "//span[contains(@class,'spinner-grow')]";

        private readonly string specificResultInSection = "//div[contains(@class,'results-wrap')]/ul/li/span[contains(@class,'{0}')]/../..//div[@class='text-truncate' and contains(text(),'{1}')]";

        public void SetInputText(string blockXpath, string text)
        {
            new Element(blockXpath + input).SetText(text);
        }

        public bool IsSearchSpinnerDisappeared(string blockXpath)
        {
            spinner.WaitSpinnerToAppear("search spinner", blockXpath + searchSpinner);

            return Spinner.WaitSpinnerToDisappear("search spinner", blockXpath + searchSpinner, throwException: false);
        }

        public void SelectSearchResult(string blockXpath, string section, string text) =>
            new Element(blockXpath + string.Format(specificResultInSection, section, text)).ClickIfExists();

        public bool IsSearchSuccessful(string blockXpath, string section, string text) =>
            new Element(blockXpath + string.Format(specificResultInSection, section, text)).Exists();
    }
}
