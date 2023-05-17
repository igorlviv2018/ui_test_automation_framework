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
    public class GeneralSettingsGuidesSteps : SystemConfigCommonSteps //: BaseSteps
    {
        public GeneralSettingsGuidesSteps(ILogger logger) : base(logger)
        {
            log = logger;

            featuresPage = new GeneralSettingsFeaturesPage();

            guidesPage = new GeneralSettingsGuidesPage();

            pageSteps = new PageSteps(log);
        }

        private readonly GeneralSettingsFeaturesPage featuresPage;

        private readonly GeneralSettingsGuidesPage guidesPage;

        private readonly PageSteps pageSteps; 

        private readonly ILogger log;

        private readonly Spinner spinner = new Spinner(App.Taf);

        public string SetGuidesViewMode(GuideViewType guideViewType)
        {
            string err = string.Empty;

            string guidesViewMode = guideViewType.ToString();

            if (guidesPage.GetDropdownCurrentValue() != guidesViewMode)
            {
                guidesPage.ClickDropdownToggle();

                guidesPage.SelectMenuItem(guidesViewMode);

                err = SaveSettings();

                LogHelper.LogInfo(log, $"'{guidesViewMode}' menu item selected");
            }
            else
            {
                LogHelper.LogInfo(log, $"'{guidesViewMode}' menu item already selected");
            }

            return err;
        }

        public void OpenPageViaDeepLink() => OpenDeepLink(App.Taf, LinkConstants.GuidesSettingsLink);

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
