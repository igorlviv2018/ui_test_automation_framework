using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Taf.UI.PageObjects.Administration.SystemConfiguration.Taf
{
    public class GeneralSettingsGuidesPage : ConfigPageBase
    {
        private readonly string cardBlock = "//main/div[contains(@class, 'card')]";

        private readonly string menuItemByName = "//fieldset//ul[contains(@role,'menu')]//a[contains(text(),'{0}')]";

        private readonly string dropdownToggle = "//fieldset//button";

        private readonly string dropdownCurrentValue = "//fieldset//button/span";

        public Dictionary<string, string> GetExpectedPageElements() => new Dictionary<string, string>()
        {
            { "Save button", saveButton} 
        
        };

        public bool IsCardBlockDisplayed() => new Element(cardBlock).IsDisplayed();

        public void ClickDropdownToggle() => new Element(dropdownToggle).ClickIfExists();

        public void SelectMenuItem(string name) => new Element(string.Format(menuItemByName, name)).ClickIfExists();

        public bool IsDropdownExpanded() => new Element(dropdownToggle).GetAttribute("aria-expanded") == "true";

        public string GetDropdownCurrentValue() => new Element(dropdownCurrentValue).Text;
    }
}
