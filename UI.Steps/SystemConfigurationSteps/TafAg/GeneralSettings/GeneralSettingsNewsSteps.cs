using NLog;
using Taf.UI.Core.Configuration;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Exceptions;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models;
using Taf.UI.PageObjects;
using Taf.UI.PageObjects.Administration.SystemConfiguration.Taf;
using System.Collections.Generic;

namespace Taf.UI.Steps
{
    public class GeneralSettingsNewsSteps : SystemConfigCommonSteps //: BaseSteps
    {
        public GeneralSettingsNewsSteps(ILogger logger) : base(logger)
        {
            log = logger;

            featuresPage = new GeneralSettingsFeaturesPage();

            newsPage = new GeneralSettingsNewsPage();

            pageSteps = new PageSteps(log);
        }

        private readonly GeneralSettingsFeaturesPage featuresPage;

        private readonly GeneralSettingsNewsPage newsPage;

        private readonly PageSteps pageSteps; 

        private readonly ILogger log;

        private readonly Spinner spinner = new Spinner(App.Taf);

        public string SetPostIntoNewsFeed(string contentType, bool enable)
        {
            bool isEnabled = newsPage.IsSwitcherChecked(contentType);

            if (isEnabled != enable)
            {
                newsPage.ClickSwitcher(contentType);
            }

            string err = SaveSettings();

            LogHelper.LogInfo(log, $"Post news for '{contentType}' {(enable ? "enabled":"disabled")}");

            return err;
        }

        public void OpenPageViaDeepLink() => OpenDeepLink(App.Taf,  LinkConstants.NewsSettingsLink);

        public string CheckPage()
        {
            List<string> errors = new List<string>();

            //check page name
            string actualName = featuresPage.GetPageName();

            string expectedName = ConfigPagesConstants.GeneralSettings;

            string err = pageSteps.CheckPageName(actualName, expectedName);

            ErrorHelper.AddToErrorList(errors, err);

            //check feature buttons are present
            int featureButtonCount = featuresPage.GetFeatureButtonCount();

            if (featureButtonCount < 1)
            {
                err = $"Feature buttons are not present";

                ErrorHelper.AddToErrorList(errors, err);
            }

            //check save is present
            err = pageSteps.CheckPageElementsDisplayed(featuresPage.GetExpectedPageElements());

            ErrorHelper.AddToErrorList(errors, err);

            // check copyright
            err = pageSteps.CheckCopyrigt();

            ErrorHelper.AddToErrorList(errors, err);

            err = ErrorHelper.ConvertErrorsToString(errors, $"'General settings -> Features' page check failed: ");

            LogHelper.LogResult(log, "'General settings -> Features' page checked", err);

            return err;
        }
    }
}
