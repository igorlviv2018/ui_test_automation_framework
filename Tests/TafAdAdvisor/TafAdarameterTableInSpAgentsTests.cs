using NLog;
using Taf.UI.Steps;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.Steps.TafSteps;
using Taf.UI.Steps.JourneyPreviewSteps;
using Xunit;

namespace Tests
{
    public class TafAdParameterTableInTafTests : TestBaseTafAd, IClassFixture<TestFixture>
    {
        private readonly LoginSteps loginSteps;

        private readonly AppsMenuSteps appsMenuSteps;

        private readonly BrowserSteps browserSteps;

        private readonly JourneysPageSteps journeysPageSteps;

        private readonly JourneyRatingTableSteps journeyRatingTableSteps;

        private readonly SidebarSteps sideMenuSteps;

        public TafAdParameterTableInTafTests(TestFixture fixture) : base(LogManager.GetLogger("TafAdParameterTableInTafTestsUI"), App.Taf, fixture)
        {
            loginSteps = new LoginSteps(log);

            appsMenuSteps = new AppsMenuSteps(log);

            browserSteps = new BrowserSteps(log);

            journeysPageSteps = new JourneysPageSteps(log);

            journeyRatingTableSteps = new JourneyRatingTableSteps(log);

            sideMenuSteps = new SidebarSteps(log);

            //clientId = fixture.DbHelper.GetClientIdByName(clientName);
        }

        // ===== to del ======
        [Fact(DisplayName = "Journey table with 2 infinity parameters test")]
        public void TableWithTwoParametersTest()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //loginSteps.OpenApp(App.Taf, "Dev");

            string err = loginSteps.Login("aqa.adm.ee.dev@gmail.com");

            //FileHelper.WriteToJsonFile(rad, Path.Combine(CommonHelper.GetTestDataFolderPath(), "journeys.json"));

            Assert.True(string.IsNullOrEmpty(err), err);

            browserSteps.OpenAppDeepLink(App.Taf);

            sideMenuSteps.OpenPage("Help me choose");

            journeysPageSteps.OpenJourney("test_005");

            journeysPageSteps.WaitJourneyToLoad();

            //Act
            err = CheckJourneyInfinityParameterTableInTaf("aqa_test_005");

            //err = journeyRatingTableSteps.CheckParameterCombinations(TafAdHelper.GetAllUniqueInfinityParameters(steps));

            //Assert
            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());

            Assert.True(string.IsNullOrEmpty(err), err);
        }
    }
}
