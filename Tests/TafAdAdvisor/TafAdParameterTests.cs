using NLog;
using Taf.UI.Steps;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.Steps.TafAdSteps;
using Xunit;

namespace Tests
{
    public class TafAdParameterTests : TestBaseTaf, IClassFixture<TestFixture>
    {
        private readonly LoginSteps loginSteps;

        private readonly BrowserSteps browserSteps;
        
        private readonly JourneyTableSteps journeyTableSteps;

        private readonly JourneyParametersSteps journeyParametersSteps;

        private readonly EditorHeaderSteps editorHeaderSteps;

        public TafAdParameterTests() : base(LogManager.GetLogger("TafAdJourneyParameterTestsUI"))
        {
            loginSteps = new LoginSteps(log);

            browserSteps = new BrowserSteps(log);

            journeyTableSteps = new JourneyTableSteps(log);

            journeyParametersSteps = new JourneyParametersSteps(log);

            editorHeaderSteps = new EditorHeaderSteps(App.TafAd);

            //clientId = fixture.DbHelper.GetClientIdByName(clientName);
        }

        [Fact(DisplayName = "Create parameter test")]
        public void CreateParameterTest()
        {
            //Arrange
            LogHelper.LogTestStart(log, XUnitHelper.FactDisplayName());

            browserSteps.OpenAppDeepLink(App.Taf);

            string err = loginSteps.Login("aqa.adm.ee.dev@gmail.com");

            Assert.True(string.IsNullOrEmpty(err), err);

            browserSteps.OpenAppDeepLink(App.TafAd);

            journeyTableSteps.OpenJourney("test_008");

            editorHeaderSteps.OpenEditorTab("Parameters");

            err = journeyParametersSteps.CreateParameterProperties("test_1", JourneyParameterType.Infinity, "description");

            Assert.True(string.IsNullOrEmpty(err), err);

            err = new JourneyParametersSteps(log).CheckParameterProperties("test_1", JourneyParameterType.Infinity, "description");

            Assert.True(string.IsNullOrEmpty(err), err);

            err = journeyParametersSteps.EditParameterProperties("test_1", "test_1_updated", "description_updated");

            Assert.True(string.IsNullOrEmpty(err), err);

            err = new JourneyParametersSteps(log).CheckParameterProperties("test_1_updated", JourneyParameterType.Infinity, "description_updated");

            Assert.True(string.IsNullOrEmpty(err), err);

            err = journeyParametersSteps.DeleteParameter("test_1_updated");

            //Assert
            Assert.True(string.IsNullOrEmpty(err), err);

            LogHelper.LogTestEnd(log, err, XUnitHelper.FactDisplayName());
        }
    }
}
