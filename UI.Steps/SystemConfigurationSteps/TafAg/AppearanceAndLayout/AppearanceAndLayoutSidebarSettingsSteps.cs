using NLog;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.PageObjects;
using Taf.UI.PageObjects.Administration.SystemConfiguration.Taf;
using System.Collections.Generic;

namespace Taf.UI.Steps
{
    //======== Redesign only =================
    public class AppearanceAndLayoutSidebarSettingsSteps : SystemConfigCommonSteps
    {
        public AppearanceAndLayoutSidebarSettingsSteps(ILogger logger) : base(logger, isRedesign: true)
        {
            sidebarPage = new AppearanceAndLayoutSidebarSettingsPage();
        }

        private readonly AppearanceAndLayoutSidebarSettingsPage sidebarPage;

        private readonly Spinner spinner = new Spinner(App.Taf);

        public string SetRecentItemsNumber(int numOfRecentItems)
        {
            string err = string.Empty;

            if (numOfRecentItems >= 0 && numOfRecentItems <= 10)
            {
                sidebarPage.SetRecentItemsNumber(numOfRecentItems);
            }

            return err;
        }

        public void OpenPageViaDeepLink() => OpenDeepLink(App.Taf, LinkConstants.SidebarSettingsLink, isRedesign: true);

        public string ChangeRecentItemsNumber(int numOfRecentItems)
        {
            OpenPageViaDeepLink();

            SetRecentItemsNumber(numOfRecentItems);

            string err = SaveSettings();

            return err;
        }

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
