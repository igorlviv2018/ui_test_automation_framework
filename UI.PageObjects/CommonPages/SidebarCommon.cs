using Taf.UI.Core.Element;

namespace Taf.UI.PageObjects.CommonPages
{
    public class SidebarCommon : BasePage
    {
        //private const string sideBar = "//div[contains(@class,'sidebar-wrap')]";

        //private const string recentArticle = sideBar + "//div[@class='recent-articles']/div[contains(@class,'article')]";

        //private const string recentArticleRedesign = "//ul[contains(@class,'nav-group')]/li[contains(@class,'flex')]/../li[contains(@class,'nav')]";

        //redesign only
        private readonly string menuLockButton = "//aside//i[contains(@class,'lock')]";

        public void ClickLockButton() => new Element(menuLockButton).ClickIfExists();

        public bool IsMenuLocked() => new Element(menuLockButton).GetAttribute("data-type") == "lock";
    }
}
