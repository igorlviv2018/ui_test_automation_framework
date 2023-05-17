using Taf.UI.Core.Constants;
using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using System.Collections.Generic;

namespace Taf.UI.PageObjects
{
    public class AccordionBlock : BasePage
    {
        public AccordionBlock(App app, bool isRedesign=false)
        {
            this.app = app;

            this.isRedesign = isRedesign;

            locators = SetLocators(app, locatorsCommon, locatorsEmbed, locatorsTaf,
               agentsRedesign: locatorsTafRedesign, isRedesign: isRedesign);
        }

        private readonly App app;

        private readonly bool isRedesign;

        public bool IsRecentCollapseExpandedFromClosedState { get; set; }

        private readonly Dictionary<string, string> locators;

        private readonly Dictionary<string, string> locatorsCommon =

            new Dictionary<string, string>()
            {
                //{ "collapse panel", "/div[@role='tabpanel']"}
            };

        private readonly Dictionary<string, string> locatorsTaf =

            new Dictionary<string, string>()
            {
                { "collapse", "/div[@class='accordion-block']/div[contains(@class,'collapse-content-block')]"},
                { "element in collapse", "/div[@role='tabpanel']/div[@class='card-body']/div[contains(@class,'content-blocks-preview')]/div[@class='my-3']"},
                { "collapse header", "/div[contains(@class,'card-header')]"},
                { "collapse panel", "/div[@role='tabpanel']"}
            };

        private readonly Dictionary<string, string> locatorsEmbed =

            new Dictionary<string, string>()
            {
                { "collapse", "/div[@class='sp-b-accordion']/div[contains(@class,'sp-b-collapse')]"},
                { "element in collapse", "/div[@role='tabpanel']/div[@class='panel-body']/div[@class='custom-article']/div[@class='size-xl']"},
                { "collapse header", "/div[contains(@class,'panel-heading')]"},
                { "collapse panel", "/div[@role='tabpanel']"}
            };

        private readonly Dictionary<string, string> locatorsTafRedesign =

            new Dictionary<string, string>()
            {
                { "collapse", "/div[@class='accordion-block']/div/div/div"},//new
                { "element in collapse", "/div[contains(@class,'rounded-bl')]/div[contains(@class,'article-preview')]/div[@class='my-3']"},//new
                { "collapse header", "/div[contains(@class,'rounded')]"},//new
                { "collapse panel", "/div[contains(@class,'rounded-bl')]"}//new
            };

        public string CollapseAtPosition(string parentXpath, int position) => IndexedXpath(parentXpath + GetXpath("collapse", locators), position);

        public string ElementInCollapseAtPosition(string collapseXpath, int position) => IndexedXpath(ElementInCollapse(collapseXpath), position);

        public string ElementInCollapse(string collapseXpath) => collapseXpath + GetXpath("element in collapse", locators);

        public string CollapseHeadingXpath(string collapseXpath) => collapseXpath + GetXpath("collapse header", locators);

        public string CollapsePanelXpath(string collapseXpath) => collapseXpath + GetXpath("collapse panel", locators);

        public string CollapseCaption(string collapseXpath) => CollapseHeadingXpath(collapseXpath) +  (isRedesign ? "/div/h3" : "/h3");

        public int GetAccordionCollapseCount(string collapseXpath) => isRedesign
            ? new Element($"{collapseXpath}/../../..//div[contains(@class,'my-2 w')]").Count
            : new Element($"{collapseXpath}/../div[contains(@class,'collapse')]").Count;

        public int GetElementsInCollapseCount(string collapseXpath) => new Element(ElementInCollapse(collapseXpath)).Count;

        public string GetCollapseTitle(string collapseXpath) => new Element(CollapseCaption(collapseXpath)).Text;

        public string OpenCollapse(string collapseXpath)
        {
            if (IsCollapsed(collapseXpath))
            {
                new Element(CollapseHeadingXpath(collapseXpath)).ClickIfExists();

                IsRecentCollapseExpandedFromClosedState = true;
            }
            else
            {
                IsRecentCollapseExpandedFromClosedState = false;
            }

            return IsCollapsePanelDisplayed(collapseXpath)
                ? string.Empty
                : "contents (panel) is not displayed";
        }

        public bool IsCollapsed(string collapseXpath)
        {
            bool isCollapsed = false;

            if (app == App.Taf)
            {
                isCollapsed = isRedesign
                    ? !new Element(CollapseHeadingXpath(collapseXpath)).GetAttribute("class").Contains("rounded-tr")
                    : new Element(CollapseHeadingXpath(collapseXpath)).GetAttribute("aria-expanded") == "false";
            }
            else if (app == App.Embed)
            { 
                isCollapsed = new Element(collapseXpath).GetAttribute("class").Contains("collapsed");
            }

            return isCollapsed;
        }

        public bool IsCollapsePanelDisplayed(string collapseXpath)
        {
            bool IsPanelDisplayed() => new Element(CollapsePanelXpath(collapseXpath)).IsDisplayed();

            return UiWaitHelper.Wait(IsPanelDisplayed, WaitConstants.CheckElementExistInSec);
        }
    }
}
