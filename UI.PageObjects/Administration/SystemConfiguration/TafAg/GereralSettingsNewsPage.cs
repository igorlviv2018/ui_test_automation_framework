using Taf.UI.Core.Element;
using System.Collections.Generic;

namespace Taf.UI.PageObjects.Administration.SystemConfiguration.Taf
{
    public class GeneralSettingsNewsPage : ConfigPageBase
    {
        private readonly string cardBlock = "//main/div[contains(@class, 'card')]";

        private readonly string switcherLabel = "/..";

        private readonly string switcherByName = "//main//span[contains(@class,'custom-switcher-label') and contains(text(),'{0}')]/../input";

        public Dictionary<string, string> GetExpectedPageElements() => new Dictionary<string, string>()
        {
            { "Save button", saveButton}
        };

        public bool IsCardBlockDisplayed() => new Element(cardBlock).IsDisplayed();

        public void ClickSwitcher(string contentType) =>
            new Element(string.Format(switcherByName, contentType) + switcherLabel).ClickIfExists();

        public bool IsSwitcherChecked(string contentType) =>
            new Element(string.Format(switcherByName, contentType)).IsSelected;
    }
}
