using Taf.UI.Core.Element;

namespace Taf.UI.PageObjects.TafTest.Taf
{
    public class ParameterStep : BasePage
    {
        private readonly string buttonByName = "//div[contains(@class,'items-group')]/button/div[@class='name' and contains(text(),\"{0}\")]/..";

        private readonly string button = "//div[contains(@class,'items-group')]/button";

        private readonly string buttonName = "//div[contains(@class,'items-group')]/button/div[@class='name']";

        private readonly string title = "//div[contains(@class,'advisor_step')]//h1";

        private readonly string description = "//div[contains(@class,'advisor_step')]//p[@class='description']";

        public void ClickButton(string buttonName) => new Element(string.Format(buttonByName, buttonName)).ClickIfExists();

        public void ClickButton(int buttonPosition) => new Element(IndexedXpath(button, buttonPosition)).ClickIfExists();

        public bool IsButtonPressed(string buttonName) => new Element(string.Format(buttonByName, buttonName)).GetAttribute("aria-pressed") == "true";

        public bool IsButtonPresent(string buttonName) => new Element(string.Format(buttonByName, buttonName)).Exists();

        public string GetButtonName(int position) => new Element(IndexedXpath(buttonName, position)).Text;

        public int GetButtonCount() => new Element(button).Count;
    }
}
