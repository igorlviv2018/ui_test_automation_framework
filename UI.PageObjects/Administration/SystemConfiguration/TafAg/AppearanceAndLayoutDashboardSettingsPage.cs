using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Taf.UI.PageObjects.Administration.SystemConfiguration.Taf
{
    public class AppearanceAndLayoutDashboardSettingsPage : ConfigPageBase
    {
        private readonly string historySettingsBlockHeader = "//div[contains(@class,'card my')]/div[contains(text(),'History settings')]";

        private readonly string recentDevicesNumber = "//input[contains(@id,'recentDevices')]";

        private readonly string recentArticlesNumber = "//input[contains(@id,'recentArticles')]";

        public Dictionary<string, string> GetExpectedPageElements() => new Dictionary<string, string>()
        {
            { "Save button", saveButton} 
        
        };

        public void SetRecentDevicesNumber(int number) => new Element(recentDevicesNumber).SetText(number.ToString());

        public void SetRecentArticlesNumber(int number) => new Element(recentArticlesNumber).SetText(number.ToString());
    }
}
