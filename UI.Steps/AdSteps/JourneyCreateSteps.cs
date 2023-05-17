using NLog;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models;
using Taf.UI.PageObjects;
using Taf.UI.PageObjects.TafAd.Authoring;
using Taf.UI.Steps.Authoring;
using Taf.UI.Steps.TafAdSteps;
using System.Collections.Generic;

namespace Taf.UI.Steps.TafAdSteps
{
    public class JourneyCreateSteps : AuthoringBaseSteps
    {
        public JourneyCreateSteps(ILogger logger) : base(App.TafAuth, logger)
        {
            editorHeaderSteps = new EditorHeaderSteps(App.TafAd);

            journeyParametersSteps = new JourneyParametersSteps(log);

            journeyTableSteps = new JourneyTableSteps(log);
        }

        private readonly JourneysPage journeysPage = new JourneysPage();

        private readonly CreateArticleModal createArticleModal = new CreateArticleModal();

        private readonly CreateJourneyModal createJourneyModal = new CreateJourneyModal();

        private readonly Spinner spinner = new Spinner(App.Taf);

        private readonly EditorHeaderSteps editorHeaderSteps;

        private readonly JourneyParametersSteps journeyParametersSteps;

        private readonly JourneyTableSteps journeyTableSteps;

        private readonly LocationSettingsSteps locationSettingsSteps = new LocationSettingsSteps(App.TafAd);

        private readonly ToastAlertSteps toastAlertSteps = new ToastAlertSteps();

        public string CreateJourneyProperties(JourneyProperties properties)
        {
            string errPrefix = $"Failed to create Journey ({properties}): ";

            string err = CheckTitleDescriptionNotEmpty(properties.Title, properties.Description);

            if (!string.IsNullOrEmpty(err))
            {
                return ErrorHelper.AddPrefixToError(err, errPrefix);
            }

            journeysPage.ClickCreateJourneyButton();

            //to do - select type

            createJourneyModal.SetTitle(properties.Title);

            createJourneyModal.SetDescription(properties.Description);

            createJourneyModal.ClickCreateButton();

            err = CheckSpinnersForCreateItemModal(errPrefix);

            return ErrorHelper.AddPrefixToError(err, errPrefix);
        }

        public string CreateTestJourney(JourneyProperties properties)
        {
            LogHelper.LogInfo(log, $"Creating journey: {properties}");

            string err = CreateJourneyProperties(properties);

            LogHelper.LogResult(log, "Filled and saved journey Properties", err);

            if (!string.IsNullOrEmpty(err))
            {
                LogHelper.LogError(log, err);

                return err;
            }

            editorHeaderSteps.OpenEditorTab("Parameters");

            err = journeyParametersSteps.CreateParameterProperties("infinity", JourneyParameterType.Infinity, "test parameter");

            if (!string.IsNullOrEmpty(err))
            {
                LogHelper.LogError(log, err);

                return err;
            }

            //editorHeaderSteps.OpenEditorTab("Location");

            editorHeaderSteps.CloseEditor();

            LogHelper.LogInfo(log, $"Journey ('{properties.Title}') created");

            return err;
        }

        public string CreateTestJourney(TestType testType) =>
            CreateTestJourney(new JourneyProperties()
                {
                    Title = new TestCaseHelper().GetUniqueItemTitle(testType),
                    Description = "test journey"
                });

        public string CreateTestJourney(TestType testType, string affix) =>
           CreateTestJourney(new JourneyProperties()
           {
               Title = CommonHelper.GetArticleTitlePrefixByTestType(testType) + "_00" + affix,
               Description = "test journey"
           });

        public string CreateTestJourney(string title) =>
           CreateTestJourney(new JourneyProperties()
           {
               Title = title,
               Description = "test journey"
           });

        /// <summary>
        /// Create test journeys with specified titles if they do not exist (if exist - skip creating)
        /// </summary>
        /// <param name="titles"></param>
        public void PrepareTestJourneys(List<string> titles)
        {
            foreach (var title in titles)
            {
                if (!journeyTableSteps.IsJourneyPresent(title))
                {
                    CreateTestJourney(title);
                }
            }
        }

        public string SetLocationOptions() => locationSettingsSteps.SetPublishChannelsOptions(locationSettingsSteps.GetDefaultPublishChannelsOptionsTafAd());
    }
}


