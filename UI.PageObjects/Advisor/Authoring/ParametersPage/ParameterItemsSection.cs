using OpenQA.Selenium;
using Taf.UI.Core.Element;

namespace Taf.UI.PageObjects.TafTest.Authoring
{
    public class ParameterItemsSection
    {
        private readonly string itemsSection = "//section[contains(@class,'items-section')]/div[contains(@class,'items-wrap')]";

        private readonly string itemXpath = "//section[contains(@class,'items-section')]/div[contains(@class,'items-wrap')]/div/div[@draggable]";

        private readonly string manufacturer = "/span[contains(@class,'manufacturer')]";

        private readonly string model = "/span[contains(@class,'model') and contains(text(),'S21')]";
    }
}
