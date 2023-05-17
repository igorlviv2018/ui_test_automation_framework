using NLog;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Models.TestCase;
using Taf.UI.Steps.Authoring;
using System.Collections.Generic;
using Taf.UI.Steps;
using Taf.UI.Core.Models;
using Taf.UI.Core.Models.TafAuth;
using Xunit;

namespace Tests
{
    public class TafNewsTests : TestBaseTaf, IClassFixture<TestFixture>
    {
        private readonly TestFixture fixture;

        private readonly StaticTestDataHelper testDataHelper;

        private readonly ArticleTableSteps articleTableSteps;

        private readonly BrowserSteps browserSteps;

        private readonly NewsSteps newsSteps;

        private readonly LocationSettingsSteps locationSettingsSteps;

        private readonly SidebarSteps sideMenuSteps;

        private readonly CommonAuthoringSteps commonAuthoringSteps;

        public TafNewsTests(TestFixture fixture) : base(LogManager.GetLogger("TafNewsTestsUI"))
        {
            this.fixture = fixture;

            testDataHelper = new StaticTestDataHelper();

            articleTableSteps = new ArticleTableSteps(log);

            browserSteps = new BrowserSteps(log);

            newsSteps = new NewsSteps(log);

            locationSettingsSteps = new LocationSettingsSteps(App.TafAuth);

            sideMenuSteps = new SidebarSteps(log);

            commonAuthoringSteps = new CommonAuthoringSteps(App.TafAuth, log);
        }

        [Fact(DisplayName = "Check news feed in SP Agents after publishing")]
        [Trait("Category", "TafCheckNews")]
        public void CheckNewsFeedAfterPublishingTest()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment, 5);

            testCase.ItemOriginalId = "2036";

            testCase.ArticleType = ArticleType.CustomArticle;

            testCase.TargetTestType = TestType.PublishToTaf;

            testCase.ArticleTestData = testDataHelper.TestDataArticle2036();

            LogHelper.LogTestStart(log, testCase.TestDescription);

            OpenAuthoring(testCase);

            string releaseNotes = "test release notes";

            PublishChannelsOptions publishChannelsOptions = locationSettingsSteps.SetTafPublishOptions(postToNewsFeed: true, includeArticleInSearch: true, releaseNotes: releaseNotes);

            CreateArticle(testCase, new ArticleProperties() { PublishChannelsOptions = publishChannelsOptions} );

            PublishArticle();

            browserSteps.OpenAppDeepLink(App.Taf);

            List<string> testArticles = new List<string> { testCase.ItemTitle };

            string err = newsSteps.CheckNewsInDashboard(testArticles, TafNewsOnItem.Article, releaseNotes, shouldBePresent: true); //true);

            Assert.True(string.IsNullOrEmpty(err), err);

            sideMenuSteps.OpenPage("News");

            err = newsSteps.CheckNewsInTable(testArticles, TafNewsOnItem.Article, releaseNotes, shouldBePresent: true);

            Assert.True(string.IsNullOrEmpty(err), err);

            //Act
            browserSteps.OpenAppDeepLink(App.TafAuth);

            articleTableSteps.OpenArticle(testCase.ItemTitle);

            publishChannelsOptions = locationSettingsSteps.SetTafPublishOptions(postToNewsFeed: false, includeArticleInSearch: true);

            err = commonAuthoringSteps.FillArticleLocation(publishChannelsOptions);

            Assert.True(string.IsNullOrEmpty(err), err);

            PublishArticle(); // publish again

            browserSteps.OpenAppDeepLink(App.Taf);

            err = newsSteps.CheckNewsInDashboard(testArticles, TafNewsOnItem.Article, releaseNotes, shouldBePresent: false);

            Assert.True(string.IsNullOrEmpty(err), err);

            sideMenuSteps.OpenPage("News");

            err = newsSteps.CheckNewsInTable(testArticles, TafNewsOnItem.Article, releaseNotes, shouldBePresent: false);

            //Assert
            Assert.True(string.IsNullOrEmpty(err), err);

            LogHelper.LogTestEnd(log, string.Empty, testCase.TestDescription);
        }

        [Fact(DisplayName = "Check news disappear after article archived")]
        [Trait("Category", "TafCheckNews")]
        public void CheckNewsDisappearAfterArticleArchivedTest()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment, 5);

            testCase.ItemOriginalId = "2036";

            testCase.ArticleType = ArticleType.CustomArticle;

            testCase.TargetTestType = TestType.PublishToTaf;

            testCase.ArticleTestData = testDataHelper.TestDataArticle2036();

            LogHelper.LogTestStart(log, testCase.TestDescription);

            OpenAuthoring(testCase);

            //Dictionary<string, string> res = new AddDeviceModal().GetAvailableManufacturers();

            string releaseNotes = "test release notes";

            PublishChannelsOptions publishChannelsOptions = locationSettingsSteps.SetTafPublishOptions(postToNewsFeed: true, includeArticleInSearch: true, releaseNotes: releaseNotes);

            CreateArticle(testCase, new ArticleProperties() { PublishChannelsOptions = publishChannelsOptions });

            PublishArticle();

            List<string> testArticles = new List<string> { testCase.ItemTitle };

            PerformArticleOperation(testArticles, AuthoringActionsMenuItem.Archive, AuthoringTableTab.Active);

            browserSteps.OpenAppDeepLink(App.Taf);

            string err = newsSteps.CheckNewsInDashboard(testArticles, TafNewsOnItem.Article, releaseNotes, shouldBePresent: false);

            Assert.True(string.IsNullOrEmpty(err), err);

            sideMenuSteps.OpenPage("News");

            err = newsSteps.CheckNewsInTable(testArticles, TafNewsOnItem.Article, releaseNotes, shouldBePresent: false);

            //Assert
            Assert.True(string.IsNullOrEmpty(err), err);

            LogHelper.LogTestEnd(log, string.Empty, testCase.TestDescription);
        }
    }
}
