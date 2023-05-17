using NLog;
using Taf.UI.Steps;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Enums;
using Taf.UI.Steps.TafSteps;
using Taf.UI.Core.Models.TafAd;
using Taf.UI.Steps.JourneyPreviewSteps;
using Taf.UI.Steps.TafAdSteps;
using System.Collections.Generic;
using Xunit;
using System;

namespace Tests
{
    public class TestBaseTafAd : TestBaseTaf
    {
        private readonly AppsMenuSteps appsMenuSteps;

        private readonly BrowserSteps browserSteps;

        private readonly SidebarSteps sideMenuSteps;

        private readonly JourneysPageSteps journeysPageSteps;

        private readonly JourneyTableSteps journeysAdvisorTableSteps;

        private readonly JourneyPreviewSteps journeyPreviewSteps;

        private readonly JourneyRatingTableSteps journeyRatingTableSteps;

        private readonly TafAdTestDataHelper testDataHelper;

        private readonly List<TestJourney> testJourneys;

        private readonly App app;

        public TestBaseTafAd(ILogger logger, App app, TestFixture fixture) : base(logger)
        {
            appsMenuSteps = new AppsMenuSteps(log);

            browserSteps = new BrowserSteps(log);

            sideMenuSteps = new SidebarSteps(log);

            journeysPageSteps = new JourneysPageSteps(log);

            journeysAdvisorTableSteps = new JourneyTableSteps(log);

            journeyPreviewSteps = new JourneyPreviewSteps(log, app);

            journeyRatingTableSteps = new JourneyRatingTableSteps(log);
            
            testDataHelper = new TafAdTestDataHelper();

            testJourneys = testDataHelper.GetTestJourneysData(fixture.TestEnvironment);

            this.app = app;
        }

        public void OpenJourneyInTaf(string journeyTitle)
        {
            browserSteps.OpenAppDeepLink(App.Taf);

            sideMenuSteps.OpenPage("Help me choose");

            string err = journeysPageSteps.OpenJourney(journeyTitle);

            journeysPageSteps.WaitJourneyToLoad();

            Assert.True(string.IsNullOrEmpty(err), err);
        }

        public void CheckJourney(string title, JourneyCheckDepth checkDepth=JourneyCheckDepth.Minimum)
        {
            TestJourney testJourney = testDataHelper.GetTestJourneyData(title, testJourneys);

            Assert.True(testJourney != null, $"Journey '{title}': test data is not available");

            int numOfRandomCombinations = testDataHelper.GetTestCombinationsCount(testJourney.Steps, checkDepth);

            journeyPreviewSteps.CheckJourneyMultiple(testJourney.Steps, numOfRandomCombinations);

            //journeysPageSteps.CloseJourneyInTaf(); //rewrite
            if (app == App.Taf)
            {
                journeysPageSteps.CloseJourneyInTaf();
            }
            else
            { 
            
            }
        }

        public string CheckJourneyInfinityParameterTableInTaf(string title)
        {
            TestJourney testJourney = testDataHelper.GetTestJourneyData(title, testJourneys);

            Assert.True(testJourney != null, $"Journey '{title}': test data is not available");

            string err = journeyRatingTableSteps.CheckParameterCombinations(testDataHelper.GetAllUniqueInfinityParameters(testJourney.Steps));

            return err;
        }

        // (journey table and Editor)
        public void OpenJourneyInEditor(string journeyTitle)
        {
            browserSteps.OpenAppDeepLink(App.TafAd);

            string err = journeysAdvisorTableSteps.OpenJourney(journeyTitle);

            Assert.True(string.IsNullOrEmpty(err), err);
        }

        public void CheckOpeningJourneyPreviewFromTable()
        {
            string title = journeysAdvisorTableSteps.GetRandomJourneyTitle();

            if (string.IsNullOrEmpty(title))
            {
                string err = "Journey preview check failed: journey table is empty";

                LogHelper.LogError(log, err);

                throw new Exception(err);
            }
            else
            {
                LogHelper.LogInfo(log, $"Trying to open '{title}' journey preview using preview button in journey table");

                journeysAdvisorTableSteps.OpenJourneyPreview(title);
            }

            journeyPreviewSteps.CheckJourneyPreviewIsOpened();
        }
    }
}
