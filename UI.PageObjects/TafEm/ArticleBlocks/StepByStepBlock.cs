using Taf.UI.Core.Constants;
using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Exceptions;
using Taf.UI.Core.Helpers;
using System.Collections.Generic;

namespace Taf.UI.PageObjects
{
    public class StepByStepBlock : ContentBlockBase
    {
        public StepByStepBlock(App app, bool isRedesign=false) : base(app)
        {
            this.app = app;

            this.isRedesign = isRedesign;

            locators = SetLocators(app, locatorsCommon, locatorsEmbed, locatorsTaf, 
                agentsRedesign:locatorsTafRedesign, isRedesign:isRedesign);
        }

        private readonly App app;

        private readonly bool isRedesign;

        private const string stepsContainer = "//div[contains(@class,'steps-container')]";

        private readonly Dictionary<string, string> locators;

        private readonly Dictionary<string, string> locatorsCommon =

            new Dictionary<string, string>()
            {
                
            };

        private readonly Dictionary<string, string> locatorsTaf =

            new Dictionary<string, string>()
            {
                { "view switcher button", "//div[contains(@class,'view-switcher')]/button/span[contains(@class,'{0}')]"},
                { "page x of y", "//div[@class='step-wrap']/p[contains(@class,'text')]/small"},
                { "list step", "//div[@class='steps-wrap']//div[contains(@class,'step-wrap')]"},
                { "list step text", "//div[contains(@class,'step-text')]"},
                { "image in list step", "//span[@class='img-wrap']/img"},
                { "swiper step", "//div[@class='steps-wrap']//div[contains(@class,'step-wrap')]"},
                { "swiper step text", "//div[contains(@class,'step-text-wrap')]"},
                { "image in swiper step", "//span[contains(@class,'img-wrap')]/img"},
                { "step title", "//div[contains(@class,'step-text-wrap')]/h3"},
                { "swiper prev button", "//div[contains(@class,'steps-navigation')]/button[contains(@class,'nav-left')]"},
                { "swiper next button", "//div[contains(@class,'steps-navigation')]/button[contains(@class,'nav-right')]"},
                { "step spinner", "//span[contains(@class,'spinner-border')]"}
            };

        private readonly Dictionary<string, string> locatorsEmbed =

            new Dictionary<string, string>()
            {
                { "is view switch active", "//div[@class='steps-view-switch']/label/span[contains(@class,'{0}')]/.."},
                { "view switcher button", "//div[@class='steps-view-switch-2']/label/span[contains(@class,'{0}')]"},
                { "list step", $"{stepsContainer}//li[@class='sp-b-step-list']"},
                { "list step text", "//div[contains(@class,'step-text')]"},
                { "image in list step", "//span[@class='img-wrap']/img"},
                { "swiper step", $"{stepsContainer}//div[contains(@class,'swiper-slide sp-b-step')]"},
                { "swiper step text", $"{stepsContainer}//div[contains(@class,'sp-b-steps-caption-static')]/div[contains(@class,'step-text')]"},
                { "image in swiper step", "//span[contains(@class,'img-wrap')]/img"},
                { "step title", $"{stepsContainer}//div[contains(@class,'sp-b-steps-caption-static')]//h4"},
                { "swiper prev button", $"{stepsContainer}//div[@slot='button-prev']"},
                { "swiper next button", $"{stepsContainer}//div[@slot='button-next']"},
                { "swiper page bullet", $"{stepsContainer}//div[contains(@class,'swiper-pagination-bullets')]/span"}
            };

        private readonly Dictionary<string, string> locatorsTafRedesign =

           new Dictionary<string, string>()
           {
                { "view switcher button", "//div[contains(@class,'step-block')]//button/i[@data-type='{0}']/.."},//new
                { "page x of y", "//div[contains(@class,'text-xs')]"},//new
                { "list step", "//div[contains(@class,'list-step')]"},//new
                { "list step text", "//p[contains(@class,'break-words')]"},//new
                { "swiper step", "//div[contains(@class,'step w-full')]"},//new
                { "swiper step active", "//div[contains(@class,'step w-full') and contains(@class,'active')]"},//new
                { "image in swiper step", "//img"},//new
                { "image in list step", "//img"},//new
                { "step title", "//h3"},//new
                { "swiper prev button", "//div[@class='step-by-step']//button/i[contains(@data-type, 'arrow-left')]"},//new
                { "swiper next button", "//div[@class='step-by-step']//button/i[contains(@data-type, 'arrow-right')]"},//new
                { "step spinner", "//span[contains(@class,'spinner-border')]"}
           };

        private string stepByStepXpath;

        // to Remove stepByStepXpath
        public string BaseXpath
        {
            set { stepByStepXpath = value; }
        }

        private string SwiperPageBulletXpath(int pageNum) =>
            IndexedXpath(stepByStepXpath, GetXpath("swiper page bullet", locators), pageNum);

        private string ImageInListStepXpath(int stepNum) =>
            IndexedXpath(stepByStepXpath, GetXpath("list step", locators), stepNum) + GetXpath("image in list step", locators);

        private string SwiperStepXpath(int stepNum)
        {
            string swiperStepXpath = string.Empty;

            if (app == App.Taf)
            {
                swiperStepXpath = isRedesign
                    ? IndexedXpath(stepByStepXpath + GetXpath("swiper step", locators), stepNum)
                    : stepByStepXpath + GetXpath("swiper step", locators);
            }
            else if (app == App.Embed)
            {
                swiperStepXpath = IndexedXpath(stepByStepXpath + GetXpath("swiper step", locators), stepNum);
            }

            return swiperStepXpath;
        }

        private string SwiperActiveStepPageXofYXpath() => isRedesign
            ? stepByStepXpath + GetXpath("swiper step active", locators) + GetXpath("page x of y", locators)
            : GetXpath("page x of y", locators);

        private string ImageInSwiperStepXpath(int stepNum) =>
            SwiperStepXpath(stepNum) + GetXpath("image in swiper step", locators);

        private string TextInListStepXpath(int stepNum) =>
            IndexedXpath(stepByStepXpath, GetXpath("list step", locators), stepNum) + GetXpath("list step text", locators);

        public string TextLinksInSwiperStepXpath(int stepNum=1)
        {
            string linkXpath = string.Empty;

            if (app == App.Taf)
            {
                if (isRedesign)
                {
                    linkXpath = SwiperStepXpath(stepNum) + "//a";
                }
                else
                {
                    linkXpath = SwiperStepXpath(1) + "//a";
                }
            }
            else if (app == App.Embed)
            {
                linkXpath = stepByStepXpath + GetXpath("swiper step text", locators) + "//a";
            }

            return linkXpath;
        }

        public string TextLinksInListStepXpath(int stepNum) => TextInListStepXpath(stepNum) + "//a";

        public bool IsSwitchActive(StepByStepViewType type)
        {
            string switchType = type == StepByStepViewType.Slider ? "ss-picture" : "ss-list";

            if (isRedesign)
            {
                switchType = type == StepByStepViewType.Slider ? "image" : "list";
            }

            string activeIndicator = app == App.Taf
                ? GetXpath("view switcher button", locators)
                : GetXpath("is view switch active", locators); // SP Embed

            string switchXpath = string.Format(stepByStepXpath + activeIndicator, switchType);

            bool isActive = isRedesign
                ? new Element(switchXpath).GetAttribute("class").Contains("text-primary")
                : new Element(switchXpath).GetAttribute("class").Contains("active");

            return isActive;
        }

        public void ClickViewSwitch(StepByStepViewType type)
        {
            string switchType = type == StepByStepViewType.Slider ? "ss-picture" : "ss-list";

            if (isRedesign)
            {
                switchType = type == StepByStepViewType.Slider ? "image" : "list";
            }

            if (!IsSwitchActive(type))
            {
                new Element(string.Format(stepByStepXpath + GetXpath("view switcher button", locators), switchType)).Click();
            }

            if (!IsSwitchActive(type))
            {
                throw new ElementNotFoundException($"Failed to switch to {type} mode.");
            }
        }

        public int GetListStepCount() => new Element(stepByStepXpath + GetXpath("list step", locators)).Count;

        public int GetSwiperStepCount()
        {
            int stepCount = 0;

            if (app == App.Taf)
            {
                if (isRedesign)
                {
                    stepCount = new Element(stepByStepXpath + GetXpath("swiper step", locators)).Count;
                }
                else
                {
                    string pageXofY = new Element(GetXpath("page x of y", locators)).Text;

                    int.TryParse(CommonHelper.GetLastIntegerInString(pageXofY), out stepCount);
                }
            }
            else if (app == App.Embed)
            {
                stepCount = new Element(PrependBaseXpath(stepByStepXpath, GetXpath("swiper step", locators))).Count;
            }

            return stepCount;
        }

        public string GetSwiperStepTitle() => new Element(stepByStepXpath + GetXpath("step title", locators)).Text;

        public string GetSwiperStepTitle(int stepNum) => new Element(SwiperStepXpath(stepNum) + GetXpath("step title", locators)).Text;

        public string GetListStepTitle(int stepNum) => new Element(IndexedXpath(stepByStepXpath + GetXpath("step title", locators), stepNum)).Text;

        public int GetSwiperPageBulletsCount() =>
            new Element(PrependBaseXpath(stepByStepXpath, GetXpath("swiper page bullet", locators))).Count;

        public int GetSwiperActivePageNumber()
        {
            int activePageNumber = -1;

            if (app == App.Taf)
            {
                Element xOfY = new Element(SwiperActiveStepPageXofYXpath());

                xOfY.ScrollToView();

                string pageXofY = xOfY.Text;

                int.TryParse(CommonHelper.GetFirstIntegerInString(pageXofY), out activePageNumber);
            }
            else if (app == App.Embed)
            {
                for (int i = 1; i <= GetSwiperPageBulletsCount(); i++)
                {
                    Element pageBullet = new Element(SwiperPageBulletXpath(i));

                    if (pageBullet.GetAttribute("class").Contains("bullet-active-main"))
                    {
                        activePageNumber = i;

                        break;
                    }
                }
            }

            return activePageNumber;
        }

        public int GetSwiperActivePageNumberRedesign(int stepNum)
        {
            int activePageNumber = -1;

            if (app == App.Taf)
            {
                Element xOfY = new Element(SwiperStepXpath(stepNum) + GetXpath("page x of y", locators));

                xOfY.ScrollToView();

                string pageXofY = xOfY.Text;

                int.TryParse(CommonHelper.GetFirstIntegerInString(pageXofY), out activePageNumber);
            }
            else if (app == App.Embed)
            {
                for (int i = 1; i <= GetSwiperPageBulletsCount(); i++)
                {
                    Element pageBullet = new Element(SwiperPageBulletXpath(i));

                    if (pageBullet.GetAttribute("class").Contains("bullet-active-main"))
                    {
                        activePageNumber = i;

                        break;
                    }
                }
            }

            return activePageNumber;
        }

        public bool IsSwiperStepActive(int stepNum) => new Element(SwiperStepXpath(stepNum)).GetAttribute("class").Contains("active");

        public void ClickSwiperPrevButton() => new Element(PrependBaseXpath(stepByStepXpath, GetXpath("swiper prev button", locators))).Click();

        public void ClickSwiperNextButton() => new Element(PrependBaseXpath(stepByStepXpath, GetXpath("swiper next button", locators))).Click();

        public bool IsSwiperPrevButtonActive() => IsSwiperButtonActive(true);

        public bool IsSwiperNextButtonActive() => IsSwiperButtonActive(false);

        public bool IsSwiperButtonActive(bool isPrev)
        {
            string buttonDescription = isPrev ? "swiper prev button" : "swiper next button";

            Element button = new Element(PrependBaseXpath(stepByStepXpath, GetXpath(buttonDescription, locators)));

            bool isButtonActive = false;

            if (app == App.Taf)
            {
                isButtonActive = !button.GetAttribute("class").Contains("disabled");
            }
            else if (app == App.Embed)
            {
                isButtonActive = button.GetAttribute("aria-disabled").Contains("false");
            }

            return isButtonActive;
        }

        public bool IsSwiperPrevButtonDisplayed() =>
            new Element(PrependBaseXpath(stepByStepXpath, GetXpath("swiper prev button", locators))).IsDisplayed();

        public bool IsSwiperNextButtonDisplayed() =>
            new Element(PrependBaseXpath(stepByStepXpath, GetXpath("swiper next button", locators))).IsDisplayed();

        public bool IsImageInSwiperStepDisplayed(int stepNumber) => new Element(ImageInSwiperStepXpath(stepNumber)).IsImageVisible();

        public bool IsImageInListStepDisplayed(int stepNumber) => new Element(ImageInListStepXpath(stepNumber)).IsImageVisible();

        public void WaitSpinnerToAppear() =>
            new Spinner(app).WaitSpinnerToAppear("step load spinner", GetXpath("step spinner", locators), WaitConstants.OneSecond);

        public void WaitSpinnerToDisappear() =>
            Spinner.WaitSpinnerToDisappear("step load spinner", GetXpath("step spinner", locators), false);
    }
}
