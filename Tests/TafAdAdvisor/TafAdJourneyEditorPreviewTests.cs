using NLog;
using Taf.UI.Steps;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models;
using Xunit;

namespace Tests
{
    public class TafAdJourneyEditorPreviewTests : TestBaseTafAd, IClassFixture<TestFixture>
    {
        private readonly LoginSteps loginSteps;

        private readonly TestFixture fixture;

        public TafAdJourneyEditorPreviewTests(TestFixture fixture) : base(LogManager.GetLogger("TafAdJourneyEditorPreviewTestsUI"), App.TafAd, fixture)
        {
            loginSteps = new LoginSteps(log);

            this.fixture = fixture;

            //clientId = fixture.DbHelper.GetClientIdByName(clientName);
        }

        [Fact(DisplayName = "SP Advisor editor preview: Journey with infinity, interval and brands parameters test")]
        public void JourneyWithInfinityParameterTest()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            User user = UserHelper.GetAdvisorAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            OpenJourneyInEditor("aqa_test_005");

            //Act
            CheckJourney("aqa_test_005");

            //Assert
            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());
        }

        [Fact(DisplayName = "SP Advisor editor preview: journey with single infinity parameter test")]
        public void JourneyWithSingleInfinityParameterTest()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            User user = UserHelper.GetAdvisorAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            OpenJourneyInEditor("aqa_test_001");

            //Act
            CheckJourney("aqa_test_001");

            //Assert
            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());
        }

        [Fact(DisplayName = "SP Advisor editor preview: journey with 4 infinity parameters test")]
        public void JourneyWithFourInfinityParametersTest()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            User user = UserHelper.GetAdvisorAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            OpenJourneyInEditor("aqa_test_002");

            //Act
            CheckJourney("aqa_test_002");

            //Assert
            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());
        }

        [Fact(DisplayName = "SP Advisor editor preview: journey with single interval parameter test")]
        public void JourneyWithSingleIntervalParameterTest()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            User user = UserHelper.GetAdvisorAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            OpenJourneyInEditor("aqa_test_003");

            //Act
            CheckJourney("aqa_test_003");

            //Assert
            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());
        }

        [Fact(DisplayName = "Deb: SP Advisor editor preview: journey with Brands parameter test ()")]
        public void JourneyWithBrandsParameterTestDeb()
        {
            //Arrange
            for (int i = 0; i < 12; i++)
            {
                JourneyWithBrandsParameterTest();

                log.Info($"cycle: {i}");
            }
        }

        [Fact(DisplayName = "SP Advisor editor preview: journey with Brands parameter test")]
        public void JourneyWithBrandsParameterTest()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            User user = UserHelper.GetAdvisorAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            OpenJourneyInEditor("aqa_test_004");

            //Act
            CheckJourney("aqa_test_004");

            //Assert
            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());
        }
    }
}
