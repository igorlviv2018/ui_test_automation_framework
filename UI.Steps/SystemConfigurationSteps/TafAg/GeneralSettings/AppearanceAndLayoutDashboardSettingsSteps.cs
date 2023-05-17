using NLog;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.PageObjects;
using Taf.UI.PageObjects.Administration.SystemConfiguration.Taf;
using System.Collections.Generic;

namespace Taf.UI.Steps
{
    public class AppearanceAndLayoutDashboardSettingsSteps : SystemConfigCommonSteps
    {
        public AppearanceAndLayoutDashboardSettingsSteps(ILogger logger) : base(logger)
        {
            log = logger;

            dashboardSettingsPage = new AppearanceAndLayoutDashboardSettingsPage();

            //configCommonSteps = new SystemConfigCommonSteps(log);

            pageSteps = new PageSteps(log);
        }

        private readonly AppearanceAndLayoutDashboardSettingsPage dashboardSettingsPage;

        //private readonly SystemConfigCommonSteps configCommonSteps;

        private readonly PageSteps pageSteps; 

        private readonly ILogger log;

        private readonly Spinner spinner = new Spinner(App.Taf);

        public string SetRecentItemsNumber(int numOfRecentItems, bool isArticles=false)
        {
            string err = string.Empty;

            if (numOfRecentItems >= 0 && numOfRecentItems <= 10)
            {
                if (isArticles)
                {
                    dashboardSettingsPage.SetRecentArticlesNumber(numOfRecentItems);
                }
                else
                {
                    dashboardSettingsPage.SetRecentDevicesNumber(numOfRecentItems);
                }
            }

            return err;
        }

        public string SetRecentDevicesNumber(int numOfRecentDevices) => SetRecentItemsNumber(numOfRecentDevices);

        public string ChangeRecentDevicesNumber(int numOfRecentDevices) => ChangeRecentItemsNumber(numOfRecentDevices);

        public void OpenPageViaDeepLink() => OpenDeepLink(App.Taf, LinkConstants.DashboardSettingsLink);

        public string ChangeRecentItemsNumber(int numOfRecentItems, bool isArticles=false)
        {
            OpenPageViaDeepLink();

            if (isArticles)
            {
                SetRecentArticlesNumber(numOfRecentItems);
            }
            else
            {
                SetRecentDevicesNumber(numOfRecentItems);
            }

            string err = SaveSettings();

            return err;
        }

        public string SetRecentArticlesNumber(int numOfRecentArticles) => SetRecentItemsNumber(numOfRecentArticles, isArticles: true);

        public string ChangeRecentArticlesNumber(int numOfRecentArticles) => ChangeRecentItemsNumber(numOfRecentArticles, isArticles: true);

        public string CheckPage()
        {
            List<string> errors = new List<string>();

            string err = "";

            ////check page name
            //string actualName = featuresPage.GetPageHeader();

            //string expectedName = ConfigPagesConstants.GeneralSettings;

            //string err = pageSteps.CheckPageName(actualName, expectedName);

            //ErrorHelper.AddToErrorList(errors, err);

            ////check feature buttons are present
            //int featureButtonCount = featuresPage.GetFeatureButtonCount();

            //if (featureButtonCount < 1)
            //{
            //    err = $"Feature buttons are not present";

            //    ErrorHelper.AddToErrorList(errors, err);
            //}

            ////check save is present
            //err = pageSteps.CheckPageElementsDisplayed(featuresPage.GetExpectedPageElements());

            //ErrorHelper.AddToErrorList(errors, err);

            //// check copyright
            //err = pageSteps.CheckCopyrigt();

            //ErrorHelper.AddToErrorList(errors, err);

            //err = ErrorHelper.ConvertErrorsToString(errors, $"'General settings -> Features' page check failed: ");

            //LogHelper.LogResult(log, "'General settings -> Features' page checked", err);

            return err;
        }
    }
}
