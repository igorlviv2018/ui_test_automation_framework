using NLog;
using Taf.UI.Steps;
using Taf.UI.Core.Enums;
using System.Collections.Generic;
using Taf.UI.Core.Helpers;
using Taf.UI.Steps.TafSteps;
using Taf.UI.Steps.TafAdSteps;
using Taf.UI.Steps.TafAdSteps;
using Taf.UI.Core.Models;
using Xunit;

namespace Tests
{
    public class TafAdJourneyOperationTests : TestBaseTafAd, IClassFixture<TestFixture>
    {
        private readonly TestFixture fixture;

        private readonly LoginSteps loginSteps;

        private readonly AppsMenuSteps appsMenuSteps;

        private readonly BrowserSteps browserSteps;
        
        private readonly JourneyTableSteps journeyTableSteps;

        private readonly JourneysPageSteps agentsJourneysPageSteps;

        private readonly JourneyCreateSteps journeyCreateSteps;

        private readonly string clientId;

        public TafAdJourneyOperationTests(TestFixture fixture) : base(LogManager.GetLogger("TafAdJourneyOperationTestsUI"), App.TafAd, fixture)
        {
            this.fixture = fixture;

            loginSteps = new LoginSteps(log);

            appsMenuSteps = new AppsMenuSteps(log);

            browserSteps = new BrowserSteps(log);

            journeyTableSteps = new JourneyTableSteps(log);

            agentsJourneysPageSteps = new JourneysPageSteps(log);

            journeyCreateSteps = new JourneyCreateSteps(log);

            clientId = "EE UK (AQA)";

            //clientId = fixture.DbHelper.GetClientIdByName(clientName);
        }

        [Fact(DisplayName = "Archive/restore journeys(s) test")]
        public void ArchiveRestoreJourneysTest()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            User user = UserHelper.GetAdminUser(fixture.TestUsers, fixture.TestEnvironment, 1, clientId);

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            browserSteps.OpenAppDeepLink(App.TafAd);

            List<string> testJourneys = new List<string> { "archive_restore_journey_001", "archive_restore_journey_002" };

            journeyCreateSteps.PrepareTestJourneys(testJourneys);

            //Act
            PerformJourneyOperation(testJourneys, AuthoringActionsMenuItem.Archive, AuthoringTableTab.Active);

            err = journeyTableSteps.CheckJourneysInTableTabsAfterArchiveOperation(testJourneys);

            Assert.True(string.IsNullOrEmpty(err), err);

            err = agentsJourneysPageSteps.CheckJourneysNotPresentInJourneysTable(testJourneys);

            Assert.True(string.IsNullOrEmpty(err), $"SP Agents check after journey 'archive' operation failed: {err}");

            browserSteps.OpenAppDeepLink(App.TafAd);

            PerformJourneyOperation(testJourneys, AuthoringActionsMenuItem.Restore, AuthoringTableTab.Archived);

            err = journeyTableSteps.CheckJourneysInTableTabsAfterRestoreOperation(testJourneys);

            //Assert
            Assert.True(string.IsNullOrEmpty(err), err);

            //err = agentsArticlesPageSteps.CheckArticlesNotPresentInArticlesTable(testArticles);

            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());
        }

        [Fact(DisplayName = "Duplicate active journey(s) test")]
        public void DuplicateActiveJourneysTest()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            User user = UserHelper.GetAdminUser(fixture.TestUsers, fixture.TestEnvironment, 1, clientId);

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            browserSteps.OpenAppDeepLink(App.TafAd);

            List<string> testJourneys = new List<string> { "duplicate_journey_001" };

            journeyCreateSteps.PrepareTestJourneys(testJourneys);

            //Act
            PerformJourneyOperation(testJourneys, AuthoringActionsMenuItem.Duplicate, AuthoringTableTab.Active);

            err = journeyTableSteps.CheckJourneysInTableTabsAfterDuplicateOperation(testJourneys, AuthoringTableTab.Active);

            //Assert
            Assert.True(string.IsNullOrEmpty(err), err);

            //Cleanup
            List<string> duplicatedJourneysTitles = journeyTableSteps.GetDuplicatedTestItemsExpectedTitles(testJourneys);

            PerformJourneyOperation(duplicatedJourneysTitles, AuthoringActionsMenuItem.Archive, AuthoringTableTab.Active);

            PerformJourneyOperation(duplicatedJourneysTitles, AuthoringActionsMenuItem.Delete, AuthoringTableTab.Archived);

            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());
        }

        [Fact(DisplayName = "Delete journey(s) test")]
        public void DeleteJourneysTest()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            User user = UserHelper.GetAdminUser(fixture.TestUsers, fixture.TestEnvironment, 1, clientId);

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            browserSteps.OpenAppDeepLink(App.TafAd);

            List<string> testJourneys = new List<string> { "delete_journey_001" };

            journeyCreateSteps.PrepareTestJourneys(testJourneys);

            PerformJourneyOperation(testJourneys, AuthoringActionsMenuItem.Archive, AuthoringTableTab.Active);

            //Act
            PerformJourneyOperation(testJourneys, AuthoringActionsMenuItem.Delete, AuthoringTableTab.Archived);

            err = journeyTableSteps.CheckJourneysInTableTabsAfterDeleteOperation(testJourneys);

            Assert.True(string.IsNullOrEmpty(err), err);

            err = agentsJourneysPageSteps.CheckJourneysNotPresentInJourneysTable(testJourneys);

            //Assert
            Assert.True(string.IsNullOrEmpty(err), $"SP Agents check after journey 'delete' operation failed: {err}");

            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());
        }

        [Fact(DisplayName = "Search Journeys test")]
        public void SearchJourneysTest()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            User user = UserHelper.GetAdminUser(fixture.TestUsers, fixture.TestEnvironment, 1, clientId);

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            browserSteps.OpenAppDeepLink(App.TafAd);

            List<string> testJourneys = new List<string> { "search_journey_001", "search_journey_#&%", "search_journey_02_#&%", "search_test_###" };

            journeyCreateSteps.PrepareTestJourneys(testJourneys);

            //Act
            List<string> searchPhrases = new List<string>() { "journey_00", "_#&%", "test_#", "no_matching_text_", "ney_02" };

            err = journeyTableSteps.CheckJourneysTableSearch(searchPhrases);

            //Assert
            Assert.True(string.IsNullOrEmpty(err), err);

            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());
        }

        [Fact(DisplayName = "Open journey preview from journey table test")]
        public void PreviewJourneyFromJourneyTableTest()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            User user = UserHelper.GetAdminUser(fixture.TestUsers, fixture.TestEnvironment, 1, clientId);

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            browserSteps.OpenAppDeepLink(App.TafAd);

            //Act
            CheckOpeningJourneyPreviewFromTable();

            //Assert
            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());
        }
    }
}
