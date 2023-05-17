using Taf.UI.Core.Constants;
using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;

namespace Taf.UI.PageObjects.Authoring
{
    public class SideMenu
    {
        // to rewrite (get data from DB)
        //private readonly string articlesSideMenuItem = "//ul[contains(@class,'side-menu')]/li[@class='nav-item']/a[contains(@href,'articles')]";

        private readonly string sideMenuItem = "//ul[contains(@class,'side-menu')]/li[@class='nav-item']/a[contains(@href,'/{0}')]";

        private readonly string sideMenuSubMenuItem = "//ul[contains(@class,'side-menu')]/li[@class='nav-item']/a[contains(@href,'{0}')]//a[contains(@href,'{1}')]";

        private readonly string sideMenuAccessoriesSubMenuItem = "//ul[contains(@class,'side-menu')]/li[@class='nav-item']/a[contains(@href,'/accessories')]//div[@class='item-title' and contains(text(),'{0}')]";

        private readonly string pageTitle = "//main[contains(@class,'page')]//h1";

        private string GetMenuItemXpath(AuthoringMenuItem menuItem) => string.Format(sideMenuItem, menuItem.ToString().ToLower());

        public bool IsMenuItemActive(AuthoringMenuItem menuItem) =>
            new Element(GetMenuItemXpath(menuItem)).GetAttribute("class").Contains("nuxt-link-active");

        public bool IsMenuItemDisplayed(AuthoringMenuItem menuItem)
        {
            string menuItemXpath = string.Format(sideMenuItem, menuItem.ToString().ToLower());

            return new Element(menuItemXpath).IsDisplayed();
        }

        public void ClickMenuItem(AuthoringMenuItem menuItem) => new Element(GetMenuItemXpath(menuItem)).ClickIfExists();

        public void ClickSubMenuItem(AuthoringMenuItem menuItem, AuthoringSubMenuItem subMenuItem)
        {
            string menuItemXpath = string.Format(sideMenuSubMenuItem, menuItem.ToString().ToLower(), subMenuItem.ToString().ToLower());

            new Element(menuItemXpath).ClickIfExists();
        }

        public bool IsSubMenuItemDisplayed(AuthoringMenuItem menuItem, AuthoringSubMenuItem subMenuItem)
        {
            string menuItemXpath = string.Format(sideMenuSubMenuItem, menuItem.ToString().ToLower(),
                subMenuItem.ToString().ToLower());

            return new Element(menuItemXpath).IsDisplayed();// Exists();
        }

        public bool WaitPageLoad(AuthoringMenuItem menuItem)
        {
            bool isPageLoaded = UiWaitHelper.Wait(() => new Element(pageTitle).Text.StartsWith(menuItem.ToString()),
                WaitConstants.CheckElementExistInSec);

            return isPageLoaded;
        }

        // move to steps???
        public void WaitMenuItemActive(AuthoringMenuItem menuItem)
        {
            Element menuItemElement = new Element(GetMenuItemXpath(menuItem));

            bool IsActive() => menuItemElement.GetAttribute("class").Contains("nuxt-link-active");

            bool isMenuItemActive = UiWaitHelper.Wait(IsActive, WaitConstants.CheckElementExistInSec);

            if (!isMenuItemActive)
            { 
            // -----to compl
            }
        }
    }
}
