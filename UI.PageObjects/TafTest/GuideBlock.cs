using Taf.UI.Core.Constants;
using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;
using System.Collections.Generic;

namespace Taf.UI.PageObjects
{
    public class GuideBlock : BasePage
    {
        //private readonly Dictionary<string, string> locators;

        private const string guideContentBlock = "//div[contains(@class,'topic-content')]//div[contains(@class,'guide container')]";

        private const string problemContentBlock = "//div[contains(@class,'topic-content')]//div[@id='problemContent']";

        private readonly string guideCaption = guideContentBlock + "h2";

        private readonly string viewSwitchButton = guideContentBlock + "//div[contains(@class,'switcher')]/button/span[contains(@class,'{0}')]/..";

        private readonly string closeGuideButton = guideContentBlock + "//div[contains(@class,'switcher')]/a[@class='close-guide']";

        private readonly string swiperButtonPrev = guideContentBlock + "//div[contains(@class,'swiper-button-prev')]";

        private readonly string swiperButtonNext = guideContentBlock + "//div[contains(@class,'swiper-button-next')]";

        private readonly string listViewItem = "//div[contains(@class,'guide-block')]//h3";

        //// ---------- Common locators --------------------------
        //private readonly Dictionary<string, string> locatorsCommon = new Dictionary<string, string>();

        //// ---------- SP Agents locators --------------------------
        //private readonly Dictionary<string, string> locatorsTaf =

        //    new Dictionary<string, string>()
        //    {
        //        { "guide caption", "//div[contains(@class,'modal-content')]//h5"},
        //        { "close button", "//header[@class='modal-header']//button[@class='close']"}
        //    };

        //// ---------- SP Embed locators --------------------------
        //private readonly Dictionary<string, string> locatorsEmbed =

        //    new Dictionary<string, string>()
        //    {
        //        { "guide caption", "//div[contains(@class,'modal-dialog')]//h3[contains(@class, 'modal-title')]" },
        //        { "close button", "//div[contains(@class,'modal-dialog')]//h3/../button[@class='close']"}
        //    };

        public GuideBlock()
        {
            //locators = SetLocators(app, locatorsCommon, locatorsEmbed, locatorsTaf);
        }

        public bool IsGuideDisplayed() => ElementsDisplayed(guideContentBlock);

        public bool IsProblemDisplayed() => ElementsDisplayed(problemContentBlock);

        //public bool IsGuideClosed()
        //{
        //    Element caption = new Element(GetXpath("guide caption", locators));

        //    return !caption.IsDisplayed();
        //}

        public string GetCaption() => new Element(guideCaption).Text;

        public void ClickListViewButton() => new Element(string.Format(viewSwitchButton, "ss-list")).ClickIfExists();

        public void ClickSlideViewButton() => new Element(string.Format(viewSwitchButton, "ss-picture")).ClickIfExists();

        public bool IsViewButtonActive(string spanClass) => new Element(string.Format(viewSwitchButton, spanClass)).GetAttribute("aria-pressed") == "true";

        public bool IsSlideViewButtonActive() => IsViewButtonActive("ss-picture");

        public bool IsListViewButtonActive() => IsViewButtonActive("ss-list");

        public bool AreSwiperButtonsDisplayed() => ElementsDisplayed(swiperButtonPrev, swiperButtonNext);

        public bool AreListItemsDisplayed() => ElementsDisplayed(listViewItem);

        public void CloseGuide() => new Element(closeGuideButton).ClickIfDisplayed();
    }
}