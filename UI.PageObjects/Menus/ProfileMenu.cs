using Taf.UI.Core.Constants;
using Taf.UI.Core.Element;

namespace Taf.UI.PageObjects
{
    public class ProfileMenu
    {
        private const string commonXpath = "//li[contains(@class,'profile-menu')]/ul";

        private const string commonXpathRedesign = "//div[contains(@class,'profile-dropdown')]/div";

        private const string profileMenuDropDown = "//li[contains(@class,'profile-menu')]/a";

        private const string profileMenuDropDownRedesign = "//div[contains(@class,'profile')]/button";

        private readonly string userInitials = profileMenuDropDown + "/em";

        private readonly string userInitialsRedesign = profileMenuDropDownRedesign + "/div[1]";

        private readonly string userName = commonXpath + "//span[contains(@class,'user-full-name')]";

        private readonly string userNameRedesign = commonXpathRedesign + "/h3";

        private readonly string userEmail = commonXpath + "//span[contains(@class,'user-email')]";

        private readonly string userEmailRedesign = commonXpathRedesign + "/p";

        private readonly string menuItem = commonXpath + "//a[@role='menuitem' and contains(text(),'{0}')]";

        private readonly string menuItemRedesign = "//div[@class='menu-items']/div[contains(.,'{0}')]";

        private readonly string notificationsCount = commonXpath + "//a[@role='menuitem']//span[contains(@class,'badge')]";

        public void OpenProfileMenu(bool isRedesign=false)
        {
            Element dropdown = new Element(isRedesign ? profileMenuDropDownRedesign: profileMenuDropDown);

            if (dropdown.Exists(WaitConstants.CheckElementExistInSec) && dropdown.GetAttribute("aria-expanded") == "false")
            {
                dropdown.Click();
            }
        }

        public void SelectMenuItem(string menuItemName, bool isRedesign=false)
        {
            string menuItemXpath = string.Format(isRedesign ? menuItemRedesign : menuItem, menuItemName);

            Element item = new Element(menuItemXpath);

            item.ClickIfExists();
        }

        public int GetNotificationCount()
        {
            Element countElement = new Element(notificationsCount);

            int count = 0;

            if (countElement.Exists(WaitConstants.CheckElementExistInSec))
            {   
                int.TryParse(countElement.Text, out count);
            }

            return count;
        }

        public string GetUserInitials(bool isRedesign = false) => new Element(isRedesign? userInitialsRedesign: userInitials).Text;

        public bool IsProfileMenuButtonDisplayed(bool isRedesign = false) =>
            new Element(isRedesign? profileMenuDropDownRedesign: profileMenuDropDown).IsDisplayed();

        public string GetUserName(bool isRedesign = false)
        {
            Element nameElement = new Element(isRedesign ? userNameRedesign : userName);

            return nameElement.IsDisplayed(WaitConstants.CheckElementExistInSec) ? nameElement.Text : string.Empty;
        }

        public string GetUserEmail(bool isRedesign = false)
        {
            Element emailElement = new Element(isRedesign ? userEmailRedesign : userEmail);

            return emailElement.IsDisplayed(WaitConstants.CheckElementExistInSec) ? emailElement.Text : string.Empty;
        }
    }
}
