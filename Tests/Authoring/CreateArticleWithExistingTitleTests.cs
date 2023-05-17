using Xunit;
using NLog;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Models.TestCase;
using Taf.UI.Steps.Authoring;
using System.Collections.Generic;
using Taf.UI.Steps;

namespace Tests
{
    public class CreateArticleWithExistingTitleTests : TestBaseTaf, IClassFixture<TestFixture>
    {
        private readonly TestFixture fixture;

        private readonly StaticTestDataHelper testDataHelper;

        private readonly ArticleTableSteps articleTableSteps;

        public CreateArticleWithExistingTitleTests(TestFixture fixture) : base(LogManager.GetLogger("TafAuthCreateArticleWithSameTitleUI"))
        {
            this.fixture = fixture;

            testDataHelper = new StaticTestDataHelper();

            articleTableSteps = new ArticleTableSteps(log);
        }

        [Fact(DisplayName = "Create CA with the same title as archived article")]
        //[Trait("Category", "CreateDF")]
        public void CreateCustomArticleWithSameTitleAsArchivedArticleTest()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment, 2);

            testCase.ItemOriginalId = "2036";

            testCase.ArticleType = ArticleType.CustomArticle;

            testCase.TargetTestType = TestType.CreateArticleWithExistingTitle;

            testCase.ArticleTestData = testDataHelper.TestDataArticle2036();

            LogHelper.LogTestStart(log, testCase.TestDescription);

            OpenAuthoring(testCase);

            CreateArticle(testCase);

            List<string> testArticles = new List<string> { testCase.ItemTitle };

            //Act
            PerformArticleOperation(testArticles, AuthoringActionsMenuItem.Archive, AuthoringTableTab.Active);

            string err = articleTableSteps.CheckArticlesInTableTabsAfterArchiveOperation(testArticles);

            Assert.True(string.IsNullOrEmpty(err), err);

            articleTableSteps.SelectTab(AuthoringTableTab.Active);

            CreateArticle(testCase, testCase.ItemTitle);

            LogHelper.LogTestEnd(log, string.Empty, testCase.TestDescription);
        }

        [Fact(DisplayName = "Create CA with the same title as deleted article")]
        //[Trait("Category", "CreateDF")]
        public void CreateCustomArticleWithSameTitleAsDeletedArticleTest()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment, 2);

            testCase.ItemOriginalId = "2036";

            testCase.ArticleType = ArticleType.CustomArticle;

            testCase.TargetTestType = TestType.CreateArticleWithExistingTitle;

            testCase.ArticleTestData = testDataHelper.TestDataArticle2036();

            LogHelper.LogTestStart(log, testCase.TestDescription);

            OpenAuthoring(testCase);

            CreateArticle(testCase);

            List<string> testArticles = new List<string> { testCase.ItemTitle };

            //Act
            PerformArticleOperation(testArticles, AuthoringActionsMenuItem.Archive, AuthoringTableTab.Active);

            PerformArticleOperation(testArticles, AuthoringActionsMenuItem.Delete, AuthoringTableTab.Archived);

            //string err = articleTableSteps.CheckArticlesInTableTabsAfterArchiveOperation(testArticles);

            //Assert.True(string.IsNullOrEmpty(err), err);

            articleTableSteps.SelectTab(AuthoringTableTab.Active);

            CreateArticle(testCase, testCase.ItemTitle);

            LogHelper.LogTestEnd(log, string.Empty, testCase.TestDescription);
        }

        [Fact(DisplayName = "Create DF with the same title as archived flow")]
        //[Trait("Category", "CreateDF")]
        public void CreateFlowWithSameTitleAsArchivedArticleTest()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment, 2);

            testCase.ItemOriginalId = "2059";

            testCase.ArticleType = ArticleType.DiagnosticFlow;

            testCase.ArticleTestData = testDataHelper.TestDataDF2059();

            testCase.TargetTestType = TestType.CreateArticleWithExistingTitle;

            LogHelper.LogTestStart(log, testCase.TestDescription);

            OpenAuthoring(testCase);

            CreateArticle(testCase);

            List<string> testArticles = new List<string> { testCase.ItemTitle };

            //Act
            PerformArticleOperation(testArticles, AuthoringActionsMenuItem.Archive, AuthoringTableTab.Active);

            string err = articleTableSteps.CheckArticlesInTableTabsAfterArchiveOperation(testArticles);

            Assert.True(string.IsNullOrEmpty(err), err);

            articleTableSteps.SelectTab(AuthoringTableTab.Active);

            CreateArticle(testCase, testCase.ItemTitle);

            LogHelper.LogTestEnd(log, string.Empty, testCase.TestDescription);
        }

        [Fact(DisplayName = "Create DF with the same title as deleted flow")]
        public void CreateFlowWithSameTitleAsDeletedArticleTest()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment, 2);

            testCase.ItemOriginalId = "2059";

            testCase.ArticleType = ArticleType.DiagnosticFlow;

            testCase.ArticleTestData = testDataHelper.TestDataDF2059();

            testCase.TargetTestType = TestType.CreateArticleWithExistingTitle;

            LogHelper.LogTestStart(log, testCase.TestDescription);

            OpenAuthoring(testCase);

            CreateArticle(testCase);

            List<string> testArticles = new List<string> { testCase.ArticleTitle };

            //Act
            PerformArticleOperation(testArticles, AuthoringActionsMenuItem.Archive, AuthoringTableTab.Active);

            PerformArticleOperation(testArticles, AuthoringActionsMenuItem.Delete, AuthoringTableTab.Archived);

            // to del?
            //string err = articleTableSteps.CheckArticlesInTableTabsAfterDeleteOperation(testArticles);

            //Assert.True(string.IsNullOrEmpty(err), err);

            articleTableSteps.SelectTab(AuthoringTableTab.Active);

            CreateArticle(testCase, testCase.ArticleTitle);

            LogHelper.LogTestEnd(log, string.Empty, testCase.TestDescription);
        }
    }
}
