using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Taf.UI.PageObjects.Administration.SystemConfiguration.Taf
{
    public class GeneralSettingsFeaturesPage : ConfigPageBase
    {
        private readonly string featueButton = "//main//button[@type='button' and contains(@class,'button-option')]";

        private readonly string featueButtonByName = "//main//button[@type='button' and contains(text(),'{0}')]";

        public Dictionary<string, string> GetExpectedPageElements() => new Dictionary<string, string>()
        {
            { "Save button", saveButton} 
        
        };

        public void ClickFeatureButton(string buttonName) => new Element(string.Format(featueButtonByName, buttonName)).ClickIfExists();

        public bool IsFeatureActivated(string buttonName) => new Element(string.Format(featueButtonByName, buttonName)).GetAttribute("class").Contains("active");

        public bool IsFeatureButtonDisplayed(string buttonName) => new Element(string.Format(featueButtonByName, buttonName)).IsDisplayed();

        public int GetFeatureButtonCount() => new Element(featueButton).Count;
    }
}
