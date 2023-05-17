using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;
using System.Collections.Generic;

namespace Taf.UI.PageObjects
{
    public class DxFlowPredefinedProcessBlock : ContentBlockBase
    {
        private readonly Dictionary<string, string> locators;

        private readonly Dictionary<string, string> locatorsCommon =

            new Dictionary<string, string>()
            {
                { "show button", "//button[contains(@class,'btn')]"}
            };

        private readonly Dictionary<string, string> locatorsTaf =

            new Dictionary<string, string>()
            {

            };

        private readonly Dictionary<string, string> locatorsEmbed =

            new Dictionary<string, string>()
            {

            };

        public DxFlowPredefinedProcessBlock(App app) : base(app)
        {
            locators = SetLocators(app, locatorsCommon, locatorsEmbed, locatorsTaf);
        }

        private string ShowButtonXpath(int pathItemNum) =>
            DxFlowPathItemAtPositionXpath(pathItemNum) + GetXpath("show button", locators);

        public void ClickButton(int pathItemNum) => new Element(ShowButtonXpath(pathItemNum)).ClickIfDisplayed();

        public bool IsButtonPressed(int pathItemNum) // SP Agents only
        {
            Element showBtn = new Element(ShowButtonXpath(pathItemNum));

            return showBtn.Exists() && showBtn.GetAttribute("aria-pressed") == "true";
        }
    }
}
