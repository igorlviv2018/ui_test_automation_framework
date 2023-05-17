using Taf.UI.Core.Element;

namespace Taf.UI.PageObjects.Taf.DashboardPage
{
    public class DashboardBlock : BasePage
    {
        protected const string blockName = "//div[contains(@class,'links-group')]/h3[contains(text(),'{0}')]";

        protected const string blockByName = blockName + "/..";

        protected const string blockItem = blockByName + "//div[@class='link-wrap']";

        public bool IsBlockPresent(string blockTitle) => new Element(string.Format(blockByName, blockTitle)).IsDisplayed();
    }
}
