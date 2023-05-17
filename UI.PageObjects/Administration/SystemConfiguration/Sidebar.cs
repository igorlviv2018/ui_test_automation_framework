using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Taf.UI.PageObjects.Administration.SystemConfiguration
{
    public class Sidebar
    {
        private readonly string sideMenu = "//aside[@class='sidebar']";

        private readonly string sideMenuRedesign = "//div[@class='sidebar-wrap']";

        private readonly string appBlockExpandButton = "//aside/ul/button[contains(text(),'{0}')]";

        private readonly string appBlockExpandButtonRedesign = "//div[@class='sidebar-wrap']//ul[contains(@class,'nav-group')]/li[contains(@class,'nav-collapse')]//span[contains(text(),'{0}')]/../../../../..";

        private readonly string subMenuItem = "//div[@id='{0}']//a[text()='{1}']";

        private readonly string subMenuItemRedesign = "//li[contains(@class,'nav-item')]/a//span[contains(text(),'{0}')]/../../../..";

        private readonly string configPageTab = "//ul[contains(@class,'navigation-tabs')]/li/a[contains(text(),'{0}')]";

        //redesign only
        private readonly string menuLockButton = "//aside//i[contains(@class,'lock')]";

        private readonly bool isRedesign;

        public Sidebar(bool isRedesign=false)
        {
            this.isRedesign = isRedesign;
        }

        public void ClickApplicationBlockExpandButton(string appName) =>
            new Element(string.Format(isRedesign ? appBlockExpandButtonRedesign : appBlockExpandButton, appName)).ClickIfExists();

        public void ClickSubmenuItem(App app, string configPageNameName) =>
            new Element(string.Format(subMenuItem, CommonHelper.GetIdByApp(app), configPageNameName)).ClickIfExists();

        public void ClickSubmenuItemRedesign(App app, string configPageNameName) =>
            new Element(string.Format(appBlockExpandButtonRedesign, CommonHelper.GetAppName(app)) +
                string.Format(subMenuItemRedesign, configPageNameName)).ClickIfExists();

        public bool IsSubmenuItemActive(App app, string configPageNameName) => isRedesign
            ? new Element(string.Format(appBlockExpandButtonRedesign, CommonHelper.GetAppName(app)) +
                string.Format(subMenuItemRedesign, configPageNameName)).GetAttribute("class").Contains("link-active")
            : new Element(string.Format(subMenuItem, CommonHelper.GetIdByApp(app), configPageNameName)).GetAttribute("class").Contains("link-active");

        public bool IsApplicationBlockExpanded(string appName) => isRedesign
            ? !new Element(string.Format(appBlockExpandButtonRedesign, appName)).GetAttribute("class").Contains("collapsed")
            : new Element(string.Format(appBlockExpandButton, appName)).GetAttribute("aria-expanded") == "true";

        public bool IsSideMenuPresent() => new Element(isRedesign ? sideMenuRedesign : sideMenu).IsDisplayed();

        //public void ClickTab(string tabName) => new Element(string.Format(configPageTab, tabName)).ClickIfExists();

        //public void IsTabActive(string tabName) => new Element(string.Format(configPageTab, tabName)).GetAttribute("class").Contains("link-active");
    }
}
