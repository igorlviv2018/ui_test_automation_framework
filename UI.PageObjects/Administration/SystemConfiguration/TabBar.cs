using Taf.UI.Core.Element;

namespace Taf.UI.PageObjects.Administration.SystemConfiguration
{
    public class TabBar
    {
        private readonly string configPageTab = "//ul[contains(@class,'navigation-tabs')]/li/a[contains(text(),'{0}')]";

        private readonly string configPageTabRedesign = "//div[contains(@class,'nav-tabs')]//a[contains(.,'{0}')]";

        public TabBar(bool isRedesign=false)
        { 
            if (isRedesign)
            {
                configPageTab = configPageTabRedesign;
            }
        }

        public void ClickTab(string tabName) => new Element(string.Format(configPageTab, tabName)).ClickIfExists();

        public bool IsTabActive(string tabName) => new Element(string.Format(configPageTab, tabName)).GetAttribute("class").Contains("link-active");
    }
}
