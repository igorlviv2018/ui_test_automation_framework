using Xunit;
using NLog;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Models.TestCase;
using Taf.UI.Steps.Authoring;
using System.Collections.Generic;
using Taf.UI.Steps;
using Taf.UI.Core.Models;
using Taf.UI.Core.Models.TafAuth;
using System.Threading;
using Taf.UI.PageObjects.Taf.DashboardPage;
using Taf.UI.Core.Element;
using Taf.UI.PageObjects.Authoring.CustomDevice;
using Taf.UI.Steps.AuthoringSteps.CustomDevice;
using Taf.UI.PageObjects.Authoring;
using Taf.UI.Steps.TafAdSteps;
using Taf.UI.Steps.TafAdSteps;

namespace Tests
{
    public class TafAdTests : TestBaseTaf, IClassFixture<TestFixture>
    {
        private readonly TestFixture fixture;

        private readonly StaticTestDataHelper testDataHelper;

        private readonly ArticleTableSteps articleTableSteps;

        private readonly BrowserSteps browserSteps;

        private readonly CustomDeviceCreateSteps deviceCreateSteps;

        private readonly LocationSettingsSteps locationSettingsSteps;

        private readonly SidebarSteps sideMenuSteps;

        //private readonly CommonAuthoringSteps commonAuthoringSteps;

        public TafAdTests(TestFixture fixture) : base(LogManager.GetLogger("TafAdUI"))
        {
            this.fixture = fixture;

            testDataHelper = new StaticTestDataHelper();

            articleTableSteps = new ArticleTableSteps(log);

            browserSteps = new BrowserSteps(log);

            deviceCreateSteps = new CustomDeviceCreateSteps(log);

            locationSettingsSteps = new LocationSettingsSteps(App.TafAd);

            sideMenuSteps = new SidebarSteps(log);

            //commonAuthoringSteps = new CommonAuthoringSteps(App.TafAd, log);
        }

        [Fact(DisplayName = "Create Phone journey")]
        public void CreateJourneyTest()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.UserLogin = "aqa.adm.ee.dev@gmail.com";

            testCase.ItemOriginalId = "2036";

            testCase.ArticleType = ArticleType.CustomArticle;

            testCase.TargetTestType = TestType.PublishToTaf;

            testCase.ArticleTestData = testDataHelper.TestDataArticle2036();

            LogHelper.LogTestStart(log, testCase.TestDescription);

            OpenAdvisor(testCase);

           // new JourneyTableSteps(log).SelectJourneys(new List<string>() { "Test3", "test_008" }, AuthoringTableTab.Active);

            //new JourneyTableSteps(log).SelectActionsMenuItem(AuthoringActionsMenuItem.Delete, AuthoringTableTab.Active);

            JourneyProperties prop = new JourneyProperties() { Description = "des", Title = "test_001", WorkNote = "test note" };

            new JourneyCreateSteps(log).CreateTestJourney(prop);

            new JourneyCreateSteps(log).SetLocationOptions();

            new JourneyCreateSteps(log).CreateJourneyProperties(prop);

            new CommonAuthoringSteps(App.TafAuth, log).PublishItem();

            new SideMenu().ClickMenuItem(AuthoringMenuItem.Devices);

            bool isLoaded = new SideMenu().WaitPageLoad(AuthoringMenuItem.Devices);

            deviceCreateSteps.OpenAddDeviceModal();

            string err = deviceCreateSteps.SelectRandomManufacturer();

            string err2 = deviceCreateSteps.SelectDeviceType(DeviceType.Router);

            string releaseNotes = "test release notes";

            PublishChannelsOptions publishChannelsOptions = locationSettingsSteps.SetTafPublishOptions(postToNewsFeed: true, includeArticleInSearch: true, releaseNotes: releaseNotes);

            CreateArticle(testCase, new ArticleProperties() { PublishChannelsOptions = publishChannelsOptions} );

            PublishArticle();

            browserSteps.OpenAppDeepLink(App.Taf);

            List<string> testArticles = new List<string> { testCase.ItemTitle };

            //Assert
            //Assert.True(string.IsNullOrEmpty(err), err);

            LogHelper.LogTestEnd(log, string.Empty, testCase.TestDescription);
        }

        [Fact(DisplayName = "Debug")]
        public void DebugTest()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.UserLogin = "aqa.sp.adm.prod@gmail.com";

            testCase.ItemOriginalId = "2036";

            testCase.ArticleType = ArticleType.CustomArticle;

            testCase.TargetTestType = TestType.PublishToTaf;

            testCase.ArticleTestData = testDataHelper.TestDataArticle2036();

            LogHelper.LogTestStart(log, testCase.TestDescription);

            OpenAuthoring(testCase);

            for (int i = 0; i < 25; i++)
            {   
                log.Info($"i={i}");

                browserSteps.OpenAppDeepLink(App.TafAuth);

                articleTableSteps.OpenArticle("113_test");

                // to refactor
                bool postToNews = i % 2 == 0;

                PublishChannelsOptions publishChannelsOptions = locationSettingsSteps.SetTafPublishOptions(postToNewsFeed: postToNews, includeArticleInSearch: true);

                //string err = commonAuthoringSteps.FillArticleLocation(publishChannelsOptions);

                PublishArticle();

                //Thread.Sleep(1000);

                browserSteps.OpenAppDeepLink(App.Taf);
            }

            LogHelper.LogTestEnd(log, string.Empty, testCase.TestDescription);
        }
    }
}
