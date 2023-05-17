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
    public class GeneralSettingsFeaturesSteps //: BaseSteps
    {
        public GeneralSettingsFeaturesSteps(ILogger logger) // : base(app, logger)
        {
            log = logger;

            featuresPage = new GeneralSettingsFeaturesPage();

            pageSteps = new PageSteps(log);
        }

        private readonly GeneralSettingsFeaturesPage featuresPage;

        private readonly PageSteps pageSteps; 

        private readonly ILogger log;

        private readonly Spinner spinner = new Spinner(App.Taf);

        public string ActivateFeature(string feature)
        {
            string err = string.Empty;

            if (!featuresPage.IsFeatureButtonDisplayed(feature))
            {
                err = $"Feature '{feature}' button not present";

                LogHelper.LogError(log, err);
            }

            //check page name
            if (featuresPage.IsFeatureActivated(feature))
            {
                //featuresPage.ClickFeatureButton(feature);

                LogHelper.LogInfo(log, $"Feature '{feature}' already activated");
            }
            else
            {
                featuresPage.ClickFeatureButton(feature);

                LogHelper.LogInfo(log, $"Feature '{feature}' activated");
            }

            return err;
        }

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
