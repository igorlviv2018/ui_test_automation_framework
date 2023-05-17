using NLog;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.PageObjects;
using Taf.WebDriver.Wrapper;
using System;

namespace Taf.UI.Steps
{
    public class BrowserSteps : BaseSteps
    {
        public BrowserSteps(ILogger logger) : base(App.Taf, logger)
        {
            secretsHelper = new SecretsHelper();

            spinner = new Spinner(App.Taf);
        }

        private readonly SecretsHelper secretsHelper;

        private readonly Spinner spinner;

        public void OpenDeepLink(App app, string linkPart, bool isRedesign=false)
        {
            Browser.Current.Navigate(new Uri(secretsHelper.GetTestEnvUrl(app, isRedesign) + linkPart));

            spinner.WaitPageLoadingToDisappear();

            LogHelper.LogInfo(log, $"Deep link '{linkPart}' opened");
        }

        public void OpenAppDeepLink(App app, bool isRedesign=false)
        {
            if (GetOpenApp() == app && IsAppStartPageOpen(app))
            {
                return;
            }

            OpenDeepLink(app, CommonHelper.GetRelativeAppLink(app), isRedesign);
        }

        public bool IsAnyAppOpened() => Browser.Current.GetWindowUrl().Contains("sp-agents.com");
    }
}
