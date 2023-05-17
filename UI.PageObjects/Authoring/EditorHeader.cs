using Taf.UI.Core.Constants;
using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using System.Collections.Generic;

namespace Taf.UI.PageObjects.Authoring
{
    public class EditorHeader : BasePage
    {
        private readonly string activeTab = "//ul[contains(@class,'nav-tabs')]//a[contains(@class,'link-active')]//span[contains(@class,'{0}')]";

        private readonly string headerTab = "//ul[contains(@class,'nav-tabs')]//span[contains(@class,'{0}')]";

        private readonly string previewBtn = "//div[@class='buttons']//div[@class='first']//a//span[contains(@class,'ss-view')]";

        private readonly string historyBtn = "//div[@class='buttons']//div[@class='first']//a//span[contains(@class,'ss-clock')]";

        private readonly string saveBtn = "(//div[contains(@class,'approval-dropdown')]/button)[1]";

        private readonly string closeBtn = "//div[@class='second']/a";

        private readonly Dictionary<string, string> locators;

        private readonly Dictionary<string, string> locatorsCommon =

            new Dictionary<string, string>()
            {
                { "preview button", "//div[@class='buttons']//div[@class='first']//a//span[contains(@class,'ss-view')]"},
                //{ "back to editor button", "//button/span[contains(@class,'circle')]"}
            };

        private readonly Dictionary<string, string> locatorsTaf =

            new Dictionary<string, string>()
            {
                { "dropdown button", "(//div[contains(@class,'approval-dropdown')]/button)[2]"},
                { "dropdown menu item", "//div[contains(@class,'approval-dropdown')]//ul[contains(@class,'dropdown-menu')]//a[contains(text(),'{0}')]"},
                { "back to editor button", "//a/span[contains(@class,'circle')]"}
            };

        private readonly Dictionary<string, string> locatorsTafTest =

            new Dictionary<string, string>()
            {
                { "dropdown button", "//div[@class='buttons']//button[contains(@class,'dropdown-toggle')]"},
                { "dropdown menu item", "//div[@class='buttons']//button[contains(@class,'dropdown-toggle')] /..//ul[contains(@class,'dropdown-menu')]//a[contains(text(),'{0}')]"},
                { "back to editor button", "//button/span[contains(@class,'circle')]"}
            };

        private readonly Dictionary<string, string> tabIds = new Dictionary<string, string>()
            {
                { "Properties", "ss-settings"},
                { "Content", "ss-edit"},
                { "Location", "ss-compose"},
                { "Devices", "ss-smartphone"},
                { "Parameters", "ss-piechart"},
                { "Journey", "ss-merge"}
            };

        public EditorHeader(App app)
        {
            locators = SetLocatorsAuthoring(app, locatorsCommon, locatorsTaf, locatorsTafTest);
        }

        public bool IsDropdownMenuExpanded()
        {
            Element dropdown = new Element(GetXpath("dropdown button", locators));

            return dropdown.Exists() && dropdown.GetAttribute("aria-expanded") == "true";
        }

        public void ExpandDropdown()
        {
            if (!IsDropdownMenuExpanded())
            {
                new Element(GetXpath("dropdown button", locators)).Click();
            }
        }

        public bool IsButtonEnabled(string xPath) => !new Element(xPath).GetAttribute("class").Contains("disabled");

        public bool IsCloseButtonPresent() => new Element(closeBtn).Exists();

        public void ClickArticleTab(string tabName)
        {
            if (tabIds.ContainsKey(tabName))
            {
                Element tab = new Element(string.Format(headerTab, tabIds[tabName]));

                tab.ClickIfExists();
            }
        }

        public void ClickSaveButton() => new Element(saveBtn).ClickIfExists();

        public void ClickPreviewButton() => new Element(previewBtn).ClickIfExists();

        public void ClickHistoryButton() => new Element(historyBtn).ClickIfExists();
        
        public void ClickCloseButton() => new Element(closeBtn).ClickIfExists();

        public void ClickBackToEditorButton() => new Element(GetXpath("back to editor button", locators)).ClickIfExists();

        public void ClickDropdownMenuItem(string itemName) => new Element(string.Format(GetXpath("dropdown menu item", locators), itemName)).ClickIfExists();

        public void WaitTabIsActive(string tabName)
        {
            if (tabIds.ContainsKey(tabName))
            {
                Element tab = new Element(string.Format(activeTab, tabIds[tabName]));

                tab.WaitTillAppeared($"'{tabName}' article header tab", WaitConstants.PageLoadWaitInSec);
            }
        }

        public bool IsTabActive(string tabName)
        {
            bool isActive = false;

            if (tabIds.ContainsKey(tabName))
            {
                Element tab = new Element(string.Format(activeTab, tabIds[tabName]));

                isActive = tab.Exists();
            }

            return isActive;
        }

        public bool WaitSaveButtonIsActive(int waitInSeconds= WaitConstants.HalfMinuteInSec) =>
            UiWaitHelper.Wait(() => IsButtonEnabled(saveBtn), waitInSeconds);

        public bool WaitCloseButtonDisappeared(int waitInSeconds = WaitConstants.HalfMinuteInSec) =>
           UiWaitHelper.Wait(() => !IsCloseButtonPresent(), waitInSeconds);
    }
}
