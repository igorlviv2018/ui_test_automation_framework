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

namespace Tests
{
    public class CreateCustomDeviceTests : TestBaseTaf, IClassFixture<TestFixture>
    {
        private readonly TestFixture fixture;

        private readonly StaticTestDataHelper testDataHelper;

        private readonly ArticleTableSteps articleTableSteps;

        private readonly AppsMenuSteps appsMenuSteps;

        private readonly BrowserSteps browserSteps;

        private readonly CustomDeviceCreateSteps deviceCreateSteps;

        private readonly LocationSettingsSteps locationSettingsSteps;

        private readonly SidebarSteps sideMenuSteps;

        private readonly CommonAuthoringSteps commonAuthoringSteps;

        public CreateCustomDeviceTests(TestFixture fixture) : base(LogManager.GetLogger("TafAuthCreateCustomDeviceUI"))
        {
            this.fixture = fixture;

            testDataHelper = new StaticTestDataHelper();

            articleTableSteps = new ArticleTableSteps(log);

            appsMenuSteps = new AppsMenuSteps(log);

            browserSteps = new BrowserSteps(log);

            deviceCreateSteps = new CustomDeviceCreateSteps(log);

            locationSettingsSteps = new LocationSettingsSteps(App.TafAuth);

            sideMenuSteps = new SidebarSteps(log);

            commonAuthoringSteps = new CommonAuthoringSteps(App.TafAuth, log);
        }

        [Fact(DisplayName = "Create Phone device")]
        public void CheckPhoneDeviceTest()
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

            List<string> testArticles = new List<string> { testCase.ArticleTitle };

            

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

                string err = commonAuthoringSteps.FillArticleLocation(publishChannelsOptions);

                PublishArticle();

                //Thread.Sleep(1000);

                browserSteps.OpenAppDeepLink(App.Taf);
            }

            LogHelper.LogTestEnd(log, string.Empty, testCase.TestDescription);
        }

        //[Fact(DisplayName = "Debug2")]
        //public void DebugTest2()
        //{
        //    //Arrange
        //    for (int i = 0; i < 7; i++)
        //    {
        //        CheckNewsDisappearAfterArticleArchivedTest();

        //        log.Info($"i={i}");
        //    }

        //    //LogHelper.LogTestEnd(log, string.Empty, testCase.TestDescription);
        //}
    }
}
