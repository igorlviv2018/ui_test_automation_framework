using NLog;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using System.Collections.Generic;

namespace Taf.UI.PageObjects
{
    public class Spinner: BasePage
    {
        // when content of extern connector is loading (caption is displayed immediately)
        //private string externalConnectorSpinner = "//div[contains(@class, 'content-loader')]/span[contains(@class,'spinner-border')]";

        private string topProgressBarRedesign = "//div[contains(@class, 'nuxt-loading')]";

        private readonly Dictionary<string, string> locators;

        private readonly bool isRedesign;

        private readonly Dictionary<string, string> locatorsCommon = 
            new Dictionary<string, string>()
            {
                { "page loading", "//div[@id='nuxt-loading']"}
            };

        private readonly Dictionary<string, string> locatorsTaf =

            new Dictionary<string, string>()
            {
                { "top progress bar", "//div[@class='nuxt-progress']"},
                { "article loading", "//div[@class='redirect-loading']/span[contains(@class,'spinner')]"},
                { "signin button spinner", "//button[@type='submit']/span[@role='status']"},
                { "search indicator", "//span[contains(@class, 'search-indicator spinner-grow')]"}
            };

        private readonly Dictionary<string, string> locatorsEmbed =

            new Dictionary<string, string>()
            {
                { "search indicator", "//div[contains(@class, 'search-indicator')]"},
                { "content loading", "//div[@class='sp-content-module']//div[contains(@class,'widget-loader')]"}
            };

        private readonly Dictionary<string, string> locatorsTafTest =

            new Dictionary<string, string>()
            {
                { "expanding parameter loading", "//span[contains(@class,'spinner-border')]"},
                { "journey step button click spinner", "//span[contains(@class,'spinner-border')]"}
            };

        private readonly Dictionary<string, string> locatorsTafRedesign =

           new Dictionary<string, string>()
           {
                { "top progress bar", "//div[@class='nuxt-progress']"},
                { "article loading", "//div[@class='redirect-loading']/span[contains(@class,'spinner')]"},
                { "signin button spinner", "//button[@type='submit']/span[@role='status']"},
                { "search indicator", "//div[contains(@class,'loader')]"}
           };

        public Spinner(App app, bool isRedesign=false)
        {
            //locators = SetLocators(app, locatorsCommon, locatorsEmbed, locatorsTaf, locatorsTafTest);
            this.isRedesign = isRedesign;

            locators = SetLocators(app, locatorsCommon, locatorsEmbed, locatorsTaf,
               agentsRedesign: locatorsTafRedesign, isRedesign: isRedesign);
        }

        public static bool WaitSpinnerToDisappear(string description, string spinnerXpath, bool throwException=true)
        {
            return new Element(spinnerXpath).WaitTillDisappeared($"{description}", WaitConstants.SpinnerToDisappearInSec, throwException);
        }

        public bool WaitSpinnerToAppear(SpinnerType spinnerType, int secondsToWait = WaitConstants.ImplicitWaitInSec)
        {
            string spinnerXpath = GetSpinnerXpath(spinnerType);

            return WaitSpinnerToAppear($"{spinnerType}", spinnerXpath, secondsToWait);
        }

        public bool WaitSpinnerToAppear(string spinnerDescription, string spinnerXpath, int secondsToWait= WaitConstants.ImplicitWaitInSec) =>
            new Element(spinnerXpath).WaitTillAppeared($"{spinnerDescription}", secondsToWait, throwException: false);

        public int WaitSpinnerToDisappear(SpinnerType spinnerType, int numOfSpinnersToWait=1)
        {
            string spinnerXpath = GetSpinnerXpath(spinnerType);

            int numOfSpinnersFound = 0;

            for (int i = 0; i < numOfSpinnersToWait; i++)
            {
                if (new Element(spinnerXpath).Exists(WaitConstants.MaxIntervalBetweenTopProgressBarsInMs, isTimeoutInMs: true))
                {
                    numOfSpinnersFound++;

                    WaitSpinnerToDisappear($"{spinnerType}", spinnerXpath);
                }
            }

            return numOfSpinnersFound;
        }

        public void WaitTopProgressBarToDisappear(int spinnerToAppearSecondsToWait = WaitConstants.ThreeSeconds)
        {
            if (isRedesign)
            {
                bool isDisplayed = new Element(topProgressBarRedesign).IsDisplayed(spinnerToAppearSecondsToWait);

                // to debug
                bool disappeared = UiWaitHelper.Wait(() => !new Element(topProgressBarRedesign).IsDisplayed(), spinnerToAppearSecondsToWait);
            }
            else
            {
                WaitSpinnerToAppear(SpinnerType.TopProgressBar, spinnerToAppearSecondsToWait);

                WaitSpinnerToDisappear(SpinnerType.TopProgressBar);
            }
        }

        public void WaitTopProgressBarToDisappearRedesign(int spinnerToAppearSecondsToWait = WaitConstants.ThreeSeconds)
        {
            bool isDisplayed = new Element(topProgressBarRedesign).IsDisplayed(spinnerToAppearSecondsToWait);

            // to debug
            bool disappeared = UiWaitHelper.Wait(() => !new Element(topProgressBarRedesign).IsDisplayed(), WaitConstants.ThreeSeconds);

            //WaitSpinnerToDisappear("Top progress bar", topProgressBarRedesign, throwException:false);
        }

        public void WaitPageLoadingToDisappear(int spinnerToAppearSecondsToWait = WaitConstants.ImplicitWaitInSec)
        {
            WaitSpinnerToAppear(SpinnerType.PageLoading, spinnerToAppearSecondsToWait);

            WaitSpinnerToDisappear(SpinnerType.PageLoading);
        }

        public string GetSpinnerXpath(SpinnerType spinnerType)
        {
            string spinnerXpath = "";

            if (spinnerType == SpinnerType.PageLoading)
            {
                spinnerXpath = GetXpath("page loading", locators); // pageLoading;
            }
            else if (spinnerType == SpinnerType.ArticleLoading)
            {
                spinnerXpath = GetXpath("article loading", locators); // articleLoading;
            }
            else if (spinnerType == SpinnerType.TopProgressBar)
            {
                spinnerXpath = GetXpath("top progress bar", locators); // topProgressBar;
            }
            else if (spinnerType == SpinnerType.SignInButton)
            {
                spinnerXpath = GetXpath("signin button spinner", locators); // signinButtonSpinner;
            }
            else if (spinnerType == SpinnerType.TafContentLoading)
            {
                spinnerXpath = GetXpath("content loading", locators);
            }
            else if (spinnerType == SpinnerType.SearchIndicator)
            {
                spinnerXpath = GetXpath("search indicator", locators); //searchIndicator;
            }
            else if (spinnerType == SpinnerType.JourneyEditorParameterExpanding)
            {
                spinnerXpath = GetXpath("expanding parameter loading", locators);
            }
            else if (spinnerType == SpinnerType.JourneyEditorPreviewStepButtonClick)
            {
                spinnerXpath = GetXpath("journey step button click spinner", locators);
            }

            return spinnerXpath;
        }
    }
}
