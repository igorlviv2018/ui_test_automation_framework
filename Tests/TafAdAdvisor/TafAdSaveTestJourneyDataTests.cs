using NLog;
using Taf.UI.Steps;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using System.Collections.Generic;
using Taf.UI.Core.Models.TafAd;
using Taf.UI.Steps.TafAdSteps;
using Taf.UI.Core.Models;
using Xunit;

namespace Tests
{
    public class TafAdSaveTestJourneyDataTests : TestBaseTaf, IClassFixture<TestFixture>
    {
        private readonly LoginSteps loginSteps;

        private readonly JourneyEditorSteps journeyEditorSteps;

        private readonly BrowserSteps browserSteps;

        private readonly TafAdTestDataHelper TafAdHelper;

        private readonly TestFixture fixture;

        public TafAdSaveTestJourneyDataTests(TestFixture fixture) : base(LogManager.GetLogger("TafAdSaveTestJourneyDataTestsUI"))
        {
            loginSteps = new LoginSteps(log);

            journeyEditorSteps = new JourneyEditorSteps(log);

            browserSteps = new BrowserSteps(log);

            TafAdHelper = new TafAdTestDataHelper();

            this.fixture = fixture;

            //clientId = fixture.DbHelper.GetClientIdByName(clientName);
        }

        [Fact(DisplayName = "Save Test Journeys data to json test")]
        public void SaveTestJourneyDataToJson()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            //string err = loginSteps.Login("aqa.adm.ee.dev@gmail.com");

            User user = UserHelper.GetAdvisorAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            string err = loginSteps.Login(user);

            browserSteps.OpenAppDeepLink(App.TafAd);

            Assert.True(string.IsNullOrEmpty(err), err);

            //Act
            List<TestJourney> testJourneys = journeyEditorSteps.GetJourneyData(TafAdHelper.GetTestJourneyTitles());

            err = TafAdHelper.WriteToTestJourneysFile(testJourneys, fixture.TestEnvironment);

            Assert.True(string.IsNullOrEmpty(err), err);

            //Assert
            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());

            Assert.True(string.IsNullOrEmpty(err), err);
        }
    }
}
