using OpenQA.Selenium.Support.UI;

namespace Taf.UI.Core.Element
{
    public class Select : Element
    {
        private readonly SelectElement selectElement;

        public Select(string selector) : base(selector)
        {
            selectElement = new SelectElement(ToIWebElement());
        }

        public void SelectByValue(string value) => selectElement.SelectByValue(value);
    }
}
