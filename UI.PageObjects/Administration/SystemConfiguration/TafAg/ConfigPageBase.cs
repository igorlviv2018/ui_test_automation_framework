using Taf.UI.Core.Element;

namespace Taf.UI.PageObjects.Administration.SystemConfiguration.Taf
{
    public class ConfigPageBase : BasePage
    {
        protected readonly string pageName = "//div[@class='row']/div/h1";

        protected readonly string pageNameRedesign = "//main/div/h1";

        protected readonly string saveButton = "//main//button[contains(text(),'Save')]";

        protected readonly string saveButtonRedesign = "//main//button[contains(.,'Save')]";

        private readonly bool isRedesign;

        public ConfigPageBase(bool isRedesign=false)
        {
            this.isRedesign = isRedesign;
        }

        public string GetPageName() => new Element(isRedesign ? pageNameRedesign : pageName).Text;

        public void ClickSaveButton() => new Element(isRedesign ? saveButtonRedesign : saveButton).ClickIfExists();

        public void ClickSaveButton(int position) => 
            new Element(IndexedXpath(isRedesign ? saveButtonRedesign : saveButton, position)).ClickIfExists();

        public void SaveButtonScrollToView() => new Element(saveButton).ScrollToView();

        public bool IsSaveButtonEnabled(int position) => isRedesign
            ? !new Element(IndexedXpath(saveButtonRedesign, position)).HasAttribute("disabled")
            : !new Element(saveButton).GetAttribute("class").Contains("disabled");
    }
}
