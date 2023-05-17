using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;

namespace Taf.UI.PageObjects.Authoring
{
    public class ConfirmModal : BasePage
    {
        private readonly string modalXpath = "//div[@class='modal-content']";

        private readonly string title = "//div[contains(@class,'modal-body')]/h2[@class='title']";

        private readonly string message = "//div[contains(@class,'modal-body')]/p[@class='message']";

        private readonly string buttonByName = "//footer[contains(@class,'modal-footer')]/button[contains(text(),'{0}')]";

        // to Base class ?
        public bool IsModalDisplayed() => new Element(modalXpath).IsDisplayed();

        public string GetTitle() => new Element(title).Text;

        public string GetMessage() => new Element(message).Text;

        public void ClickButton(string name) => new Element(string.Format(buttonByName, name)).ClickIfExists();

        public bool WaitModalDisappeared() => WaitModalDisappeared(modalXpath);

        public bool WaitModalAppeared() => WaitModalAppeared(modalXpath);
    }
}
