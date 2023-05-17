using Taf.UI.Core.Constants;
using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Models;
using System.Collections.Generic;

namespace Taf.UI.PageObjects
{
    public class DropdownMenu : BasePage
    {
        private string baseXpath;

        private readonly bool isRedesign;

        public DropdownMenu(App app, bool isRedesign=false)
        {
            locators = SetLocators(app, locatorsCommon, locatorsEmbed, locatorsTaf,
                agentsRedesign: locatorsTafRedesign, isRedesign: isRedesign);

            this.isRedesign = isRedesign;
        }

        private readonly Dictionary<string, string> locators;

        private readonly Dictionary<string, string> locatorsCommon =

            new Dictionary<string, string>()
            {
                { "dropdown toggle", "//div[contains(@class,'dropdown')]//button[contains(@class, 'dropdown-toggle')]"}
            };

        private readonly Dictionary<string, string> locatorsTaf =

            new Dictionary<string, string>()
            {
                { "menu item", "//div[contains(@class,'dropdown')]//button[contains(@role, 'menuitem')]"},
                { "menu item image", "//div[contains(@class,'dropdown')]//button[contains(@role, 'menuitem')]//div[contains(@class,'image-wrap')]/img"},
                { "selected menu item image", "//div[contains(@class,'image-wrap')]/img"},
                { "selected menu item", "//button[contains(@class, 'dropdown-toggle')]/div[@class='selected-wrap']/span"}
            };

        private readonly Dictionary<string, string> locatorsEmbed =

            new Dictionary<string, string>()
            {
                { "menu item", "//div[contains(@class,'dropdown')]//button[contains(@role, 'menuitem')]"},
                { "menu item image", "//div[contains(@class,'dropdown')]//button[contains(@role, 'menuitem')]//div[contains(@class,'image-wrap')]/img"},
                { "selected menu item image", "//div[contains(@class,'image-wrap')]/img"},
                { "selected menu item", "//button[contains(@class, 'dropdown-toggle')]/div[@class='selected-wrap']/span"}
            };

        private readonly Dictionary<string, string> locatorsTafRedesign =

            new Dictionary<string, string>()
            {
                { "menu item", "//div[contains(@class,'menu-items')]/div[@role='menuitem']"},
                { "menu item image", "//img"},
                { "selected menu item", "//button[contains(@class,'dropdown')]//span"},
                { "selected menu item image", "/../div/img"},
            };

        private string DropdownToggleXpath() => PrependBaseXpath(baseXpath, GetXpath("dropdown toggle", locators));

        private string MenuItemXpath() => PrependBaseXpath(baseXpath, GetXpath("menu item", locators));

        private string MenuItemAtPositionXpath(int position) => IndexedXpath(MenuItemXpath(), position);

        private string MenuItemImageAtPositionXpath(int position) => isRedesign
            ? MenuItemAtPositionXpath(position) + GetXpath("menu item image", locators)
            : IndexedXpath(PrependBaseXpath(baseXpath, GetXpath("menu item", locators)), position);

        private string SelectedMenuItemImageXpath() => isRedesign
            ? PrependBaseXpath(baseXpath, GetXpath("selected menu item", locators) + GetXpath("selected menu item image", locators))
            : PrependBaseXpath(baseXpath, GetXpath("selected menu item", locators) + "/.." + GetXpath("selected menu item image", locators));

        public string BaseXpath
        {
            set { baseXpath = value; }
        }

        public void OpenMenu()
        {
            Element toggle = new Element(DropdownToggleXpath());

            if (!toggle.IsAriaExpanded())
            {
                toggle.ClickIfExists();

                new Element(MenuItemAtPositionXpath(1)).IsDisplayed(WaitConstants.CheckElementExistInSec);
            }
        }

        public void CloseMenu()
        {
            Element toggle = new Element(DropdownToggleXpath());

            if (toggle.IsAriaExpanded())
            {
                toggle.ClickIfExists();
            }
        }

        public void SelectMenuItem(int position)
        {
            OpenMenu();

            new Element(MenuItemAtPositionXpath(position)).ClickIfExists();
        }

        public DropdownMenuItem GetSelectedDropdownMenuItem()
        {
            string selectedMenuItemXpath = PrependBaseXpath(baseXpath, GetXpath("selected menu item", locators));

            Element item = new Element(selectedMenuItemXpath);

            if (!item.Exists())
            {
                return null;
            }

            if (isRedesign && item.Text == "Select an answer")
            {
                return null;
            }

            Element image = new Element(SelectedMenuItemImageXpath());

            return new DropdownMenuItem()
            {
                Name = item.Text,

                HasImage = image.Exists(),

                IsImageDisplayed = image.IsImageVisible()
            };
        }

        public List<DropdownMenuItem> GetMenuItems()
        {
            List<DropdownMenuItem> menuItems = new List<DropdownMenuItem>();

            int menuItemCount = new Element(MenuItemXpath()).Count;

            for (int i = 0; i < menuItemCount; i++)
            {
                Element image = new Element(MenuItemImageAtPositionXpath(i + 1));

                menuItems.Add(new DropdownMenuItem()
                {
                    Name = new Element(MenuItemAtPositionXpath(i + 1)).Text,
                    HasImage = image.Exists(),
                    IsImageDisplayed = image.IsImageVisible()
                });
            }

            return menuItems;
        }
    }
}
