using Taf.UI.Core.Element;
using System.Collections.Generic;

namespace Taf.UI.PageObjects.TafTest.Taf
{
    public class JourneyStartPage : BasePage
    {
        private readonly string startButton = "//div[contains(@class,'start-journey')]//a[contains(@class,'btn-primary')]";

        private readonly string parameterCheckbox = "//div[@class='parameter-item']//input";

        private readonly string parameterTitle = "//div[@class='parameter-item']//label/span";

        public void ClickStartButton() => new Element(startButton).ClickIfExists();

        public void CheckCheckbox(int position) => new Checkbox(IndexedXpath(parameterCheckbox, position)).Check();

        public void SetCheckboxState(int position, bool isChecked) => new Checkbox(IndexedXpath(parameterCheckbox, position)).IsChecked = isChecked;

        public int GetCheckboxCount() => new Element(parameterCheckbox).Count;

        public List<string> GetParameterTitles() => GetTextOfElements(parameterTitle);
    }
}
