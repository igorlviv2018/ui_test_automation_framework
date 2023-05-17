using OpenQA.Selenium;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Element;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models;
using System.Collections.Generic;

namespace Taf.UI.PageObjects
{
    public class AppsMenu
    {
        private readonly string appsMenuXpath = "//li[contains(@class,'apps-menu')]/a[contains(@class, 'dropdown-toggle')]";

        private readonly string menuItemXpath = "//li[contains(@class,'apps-menu')]/ul[contains(@class, 'dropdown-menu')]//a[@href='{0}']";

        private readonly string clientName = "//li[contains(@class,'apps-menu')]/ul[contains(@class, 'dropdown-menu')]//a[contains(@href, '#')]";

        private readonly string menuItems = "//li[contains(@class,'apps-menu')]/ul[contains(@class, 'dropdown-menu')]//li/*[contains(@role, '')]";

        public void OpenAppsMenu()
        {
            Element appsMenuButton = new Element(appsMenuXpath);

            if (appsMenuButton.Exists(WaitConstants.CheckElementExistInSec) && appsMenuButton.GetAttribute("aria-expanded") == "false")
            {
                appsMenuButton.Click();
            }
        }

        public void SelectMenuItem(string item)
        {
            Element menuItem = new Element(string.Format(menuItemXpath, item));

            menuItem.ClickIfExists();
        }

        public string GetClientName()
        {
            Element clientNameElem = new Element(clientName);

            string name = "";

            if (clientNameElem.Exists())
            { 
                name = clientNameElem.Text;
            }

            return name;
        }

        public List<AppsMenuItem> GetAllMenuItems()
        {
            Element menuItemElements = new Element(menuItems);

            // todo remove reference to 'OpenQA.Selenium'
            List<IWebElement> itemElements =  menuItemElements.FindElements();

            List<AppsMenuItem> items = new List<AppsMenuItem>();

            foreach (var item in itemElements)
            {
                string role = item.GetAttribute("role");

                string href = item.GetAttribute("href");

                bool enabled = item.GetAttribute("aria-disabled") != "true";

                string text = item.Text;

                AppsMenuItem menuItem = new AppsMenuItem()
                {
                    Role = role,
                    Href = href,
                    IsEnabled = enabled,
                    Text = text
                };

                menuItem.Type = CommonHelper.GetAppsMenuItemType(menuItem);

                menuItem.App = CommonHelper.GetApp(href);

                items.Add(menuItem);
            }

            return items;
        }
    }
}
