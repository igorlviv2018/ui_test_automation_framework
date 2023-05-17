using NLog;
using Taf.UI.Steps;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models;
using Xunit;

namespace Tests
{
    public class TafAdJourneyInTafTests : TestBaseTafAd, IClassFixture<TestFixture>
    {
        private readonly LoginSteps loginSteps;

        private readonly TestFixture fixture;

        public TafAdJourneyInTafTests(TestFixture fixture) : base(LogManager.GetLogger("TafAdJourneyInTafTestsUI"), App.Taf, fixture)
        {
            loginSteps = new LoginSteps(log);

            this.fixture = fixture;

            //clientId = fixture.DbHelper.GetClientIdByName(clientName);
        }

        [Fact(DisplayName = "TafAg: journey with infinity, interval and brands parameters test")]
        public void JourneyWithInfinityParameterTest()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            User user = UserHelper.GetAdvisorAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            OpenJourneyInTaf("aqa_test_005");

            //Act
            CheckJourney("aqa_test_005");

            //Assert
            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());
        }

        [Fact(DisplayName = "TafAg: journey with single infinity parameter test")]
        public void JourneyWithSingleInfinityParameterTest()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            User user = UserHelper.GetAdvisorAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            OpenJourneyInTaf("aqa_test_001");

            //Act
            CheckJourney("aqa_test_001");

            //Assert
            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());
        }

        [Fact(DisplayName = "TafAg: journey with 4 infinity parameters test")]
        public void JourneyWithFourInfinityParametersTest()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            User user = UserHelper.GetAdvisorAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            OpenJourneyInTaf("aqa_test_002");

            //Act
            CheckJourney("aqa_test_002");

            //Assert
            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());
        }

        [Fact(DisplayName = "TafAg: journey with single interval parameter test")]
        public void JourneyWithSingleIntervalParameterTest()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            User user = UserHelper.GetAdvisorAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            OpenJourneyInTaf("aqa_test_003");

            //Act
            CheckJourney("aqa_test_003");

            //Assert
            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());
        }

        [Fact(DisplayName = "TafAg: journey with Brands parameter test")]
        public void JourneyWithBrandsParameterTest()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            User user = UserHelper.GetAdvisorAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            OpenJourneyInTaf("aqa_test_004");

            //Act
            CheckJourney("aqa_test_004");

            //Assert
            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());
        }

        [Fact(DisplayName = "TafAg: journey parameter table with 4 infinity parameters test")]
        public void TableWithTwoParametersTest()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            User user = UserHelper.GetAdvisorAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            OpenJourneyInTaf("aqa_test_002");

            //Act
            err = CheckJourneyInfinityParameterTableInTaf("aqa_test_002");

            //Assert
            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());

            Assert.True(string.IsNullOrEmpty(err), err);
        }
    }
}
