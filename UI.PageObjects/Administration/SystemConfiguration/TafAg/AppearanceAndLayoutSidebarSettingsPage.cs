using Taf.UI.Core.Element;
using System.Collections.Generic;

namespace Taf.UI.PageObjects.Administration.SystemConfiguration.Taf
{
    //============== Redesign only ===================
    public class AppearanceAndLayoutSidebarSettingsPage : ConfigPageBase
    {
        private readonly string recentItemsNumber = "//input[@type='number']";

        public AppearanceAndLayoutSidebarSettingsPage() : base(isRedesign: true)
        { 
        
        }

        public Dictionary<string, string> GetExpectedPageElements() => new Dictionary<string, string>()
        {
            { "Save button", saveButton} 
        
        };

        public void SetRecentItemsNumber(int number) => new Element(recentItemsNumber).SetText(number.ToString());
    }
}
