using Taf.UI.Core.Constants;
using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;
using System.Collections.Generic;

namespace Taf.UI.PageObjects
{
    public class GuideModal : BasePage
    {
        private readonly Dictionary<string, string> locators;

        // ---------- Common locators --------------------------
        private readonly Dictionary<string, string> locatorsCommon = new Dictionary<string, string>();

        // ---------- SP Agents locators --------------------------
        private readonly Dictionary<string, string> locatorsTaf =

            new Dictionary<string, string>()
            {
                { "guide caption", "//div[contains(@class,'modal-content')]//h5"},
                { "close button", "//header[@class='modal-header']//button[@class='close']"}
            };

        // ---------- SP Embed locators --------------------------
        private readonly Dictionary<string, string> locatorsEmbed =

            new Dictionary<string, string>()
            {
                { "guide caption", "//div[contains(@class,'modal-dialog')]//h3[contains(@class, 'modal-title')]" },
                { "close button", "//div[contains(@class,'modal-dialog')]//h3/../button[@class='close']"}
            };

        private readonly Dictionary<string, string> locatorsTafRedesign =

            new Dictionary<string, string>()
            {
                { "guide caption", "//div[contains(@id,'dialog-panel')]//div[contains(@class,'break-words')]"},
                { "close button", "//div[contains(@id,'dialog-panel')]//button[contains(@class,'close-button')]"}
            };

        public GuideModal(App app, bool isRedesign=false)
        {
            //locators = SetLocators(app, locatorsCommon, locatorsEmbed, locatorsTaf);
            locators = SetLocators(app, locatorsCommon, locatorsEmbed, locatorsTaf,
               agentsRedesign: locatorsTafRedesign, isRedesign: isRedesign);
        }

        public string GetCaption()
        {
            Element caption = new Element(GetXpath("guide caption", locators));

            return caption.IsDisplayed(WaitConstants.CheckElementExistInSec) ? caption.Text : string.Empty;
        }

        public bool IsDisplayed() => ElementsDisplayed(GetXpath("guide caption", locators));

        public bool IsGuideClosed() => !new Element(GetXpath("guide caption", locators)).IsDisplayed();

        public void CloseGuide() => new Element(GetXpath("close button", locators)).ClickIfDisplayed();
    }
}