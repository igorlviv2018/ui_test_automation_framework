using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;
using System.Collections.Generic;

namespace Taf.UI.PageObjects
{
    public class ArticleEditLocationPage : BasePage
    {
        private readonly string collapseHeader = "//section/fieldset//div[contains(@class,'accordion-card')]/div[contains(@class,'card-header')]";

        private readonly string collapseHeaderSwitcher = "/label/input[@type='checkbox']";

        private readonly string collapseHeaderCaret = "/span[contains(@class,'icon-caret')]";

        private readonly string collapsePanel = "//div[contains(@class,'accordion-card')]//div[contains(@class,'card-body')]";

        private readonly string switcherXpath = "//span[contains(@class,'custom-switcher-indicator')]/../input";

        private readonly string switcherLabelXpath = "/..";

        private readonly string checkboxXpath = "//div[contains(@class,'publish-locations-wrap')]//input[@type='checkbox']";

        private readonly string checkboxLabelXpath = "/../label";

        private readonly string textAreaInCollapsePanel = "//textarea";

        private readonly Dictionary<string, string> locators;

        private readonly Dictionary<string, string> locatorsCommon =

            new Dictionary<string, string>()
            {
                { "collapse header caret", "/span[contains(@class,'icon-caret')]"},
                { "switcher in collapse", "//span[contains(@class,'custom-switcher-indicator')]/../input"},
                { "switcher label", "/.."},
                { "text area in collapse", "//textarea"}
            };

        private readonly Dictionary<string, string> locatorsTaf =

            new Dictionary<string, string>()
            {
                { "collapse header", "//section/fieldset//div[contains(@class,'accordion-card')]/div[contains(@class,'card-header')]"},
                { "collapse header switcher", "/label/input[@type='checkbox']"},
                { "collapse panel", "//div[contains(@class,'accordion-card')]//div[contains(@class,'card-body')]"}
            };

        private readonly Dictionary<string, string> locatorsTafTest =

            new Dictionary<string, string>()
            {
                { "collapse header", "//fieldset//div[contains(@class,'collapse-card')]/div[contains(@class,'card-header')]"},
                { "collapse header switcher", "//label/input[@type='checkbox']"},
                { "collapse panel", "//div[contains(@class,'collapse')]//div[contains(@class,'card-body')]"}
            };

        public ArticleEditLocationPage(App app)
        {
            locators = SetLocatorsAuthoring(app, locatorsCommon, locatorsTaf, locatorsTafTest);
        }

        public string CheckboxXpath(int collapsePosition, int checkboxPosition) =>
            IndexedXpath(IndexedXpath(GetXpath("collapse panel", locators), collapsePosition) + checkboxXpath, checkboxPosition);

        public string SwitcherXpath(int collapsePosition, int switcherPosition) =>
            IndexedXpath(IndexedXpath(GetXpath("collapse panel", locators), collapsePosition) + GetXpath("switcher in collapse", locators), switcherPosition);

        public string TextAreaXpath(int collapsePosition, int textAreaPosition) =>
            IndexedXpath(IndexedXpath(GetXpath("collapse panel", locators), collapsePosition) + GetXpath("text area in collapse", locators), textAreaPosition);

        // AGE-84

        public bool IsCollapsed(int collapsePosition) =>
            new Element(IndexedXpath(GetXpath("collapse header", locators), collapsePosition)).GetAttribute("aria-expanded") == "false";

        public bool IsCollapseEnabled(int collapsePosition) =>
            !new Element(IndexedXpath(GetXpath("collapse header", locators), collapsePosition)).GetAttribute("class").Contains("disabled");

        public bool IsCollapseHeaderSwitcherChecked(int collapsePosition) =>
            new Element(IndexedXpath(GetXpath("collapse header", locators), collapsePosition) + GetXpath("collapse header switcher", locators)).IsSelected;

        public void ExpandCollapse(int collapsePosition)
        {
            if (IsCollapsed(collapsePosition))
            {
                new Element(IndexedXpath(GetXpath("collapse header", locators), collapsePosition) + GetXpath("collapse header caret", locators)).ClickIfExists();
            }
        }

        public void ClickCollapseHeaderSwitcher(int collapsePosition)
        {
            if (IsCollapseEnabled(collapsePosition))
            {
                Element channelSwitcher = new Element(IndexedXpath(GetXpath("collapse header", locators), collapsePosition)
                    + GetXpath("collapse header switcher", locators)
                    + GetXpath("switcher label", locators));
                    //$"{collapseHeaderSwitcher}{switcherLabelXpath}");

                channelSwitcher.ClickIfExists();
            }
        }

        public void ClickCheckbox(int collapsePosition, int checkboxPosition) =>
            new Element(CheckboxXpath(collapsePosition, checkboxPosition) + checkboxLabelXpath).ClickIfExists();

        public bool IsCheckboxChecked(int collapsePosition, int checkboxPosition) =>
            new Element(CheckboxXpath(collapsePosition, checkboxPosition)).IsSelected;

        public void ClickSwitcher(int collapsePosition, int switcherPosition) => 
            new Element(SwitcherXpath(collapsePosition, switcherPosition) + GetXpath("switcher label", locators)).ClickIfExists();

        public bool IsSwitcherChecked(int collapsePosition, int switcherPosition) =>
            new Element(SwitcherXpath(collapsePosition, switcherPosition)).IsSelected;

        public void SetTextInTextArea(int collapsePosition, int textAreaPosition, string text) =>
            new Element(TextAreaXpath(collapsePosition, textAreaPosition)).SetText(text);
    }
}
