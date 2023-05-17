using Xunit;
using NLog;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Models.TestCase;
using System.Collections.Generic;
using Taf.UI.Steps;

namespace Tests
{
    public class CreateTestArticleTests : TestBaseTaf, IClassFixture<TestFixture>
    {
        private readonly TestFixture fixture;

        private readonly StaticTestDataHelper testDataHelper;

        private readonly ProfileMenuSteps profileMenuSteps;

        public CreateTestArticleTests(TestFixture fixture) : base(LogManager.GetLogger("TafAuthCreateTestArticleUI"))
        {
            this.fixture = fixture;

            testDataHelper = new StaticTestDataHelper();

            profileMenuSteps = new ProfileMenuSteps(log);
        }

        [Fact(DisplayName ="Create test custom articles for article archive/restore test")]
        public void CreateArticlesForOperationTests()
        {
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment, 4);

            testCase.ItemOriginalId = "2036";

            testCase.ArticleType = ArticleType.CustomArticle;

            testCase.ArticleTestData = testDataHelper.TestDataArticle2036();

            // archive/restore articles
            testCase.TargetTestType = TestType.ArchiveRestoreArticle;

            CreateMultipleArticlesTest(testCase, 3);

            // duplicate active articles
            testCase.TargetTestType = TestType.DuplicateActiveArticle;

            CreateMultipleArticlesTest(testCase, 1);

            // duplicate archived articles
            testCase.TargetTestType = TestType.DuplicateArchivedArticle;

            CreateMultipleArticlesTest(testCase, 1);

            // delete articles
            testCase.TargetTestType = TestType.DeleteArticle;

            CreateMultipleArticlesTest(testCase, 1);

            LogHelper.LogTestEnd(log, string.Empty, testCase.TestDescription);
        }

        [Fact(DisplayName = "Create test flows for article archive/restore test")]
        public void CreateCreateFlowsForOperationTests()
        {
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment, 4);

            testCase.ItemOriginalId = "2059";

            testCase.ArticleType = ArticleType.DiagnosticFlow;

            testCase.ArticleTestData = testDataHelper.TestDataDF2059();

            // archive/restore articles
            testCase.TargetTestType = TestType.ArchiveRestoreArticle;
            CreateMultipleArticlesTest(testCase, 3);

            // duplicate active articles
            testCase.TargetTestType = TestType.DuplicateActiveArticle;

            CreateMultipleArticlesTest(testCase, 1);

            // duplicate archived articles
            testCase.TargetTestType = TestType.DuplicateArchivedArticle;

            CreateMultipleArticlesTest(testCase, 1);

            // delete articles
            testCase.TargetTestType = TestType.DeleteArticle;

            CreateMultipleArticlesTest(testCase, 1);

            LogHelper.LogTestEnd(log, string.Empty, testCase.TestDescription);
        }

        [Fact(DisplayName = "Create test articles (CAs & DFs) for article search tests")]
        public void CreateArticlesForSearchTests()
        {
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment, 4);

            testCase.ItemOriginalId = "2036";

            testCase.ArticleType = ArticleType.CustomArticle;

            testCase.ArticleTestData = testDataHelper.TestDataArticle2036();

            // test articles for search tests
            testCase.TargetTestType = TestType.SearchArticles;

            List<string> articlesToCreateTitles = new List<string>()
            {
                "search_article_test_202001291525_neee",
                "search_article_test_202001291525_2",
                "search_article_test_#*$%"
            };

            CreateArticlesForAuthoringSearchTest(testCase, articlesToCreateTitles);

            testCase.ItemOriginalId = "2059";

            testCase.ArticleType = ArticleType.DiagnosticFlow;

            testCase.ArticleTestData = testDataHelper.TestDataDF2059();

            // test flows for search tests
            testCase.TargetTestType = TestType.SearchArticles;

            List<string> flowsToCreateTitles = new List<string>()
            {
                "search_article_test_202001291525_3",
                "search_article_test_qwerty_zxcvbn"
            };

            CreateArticlesForAuthoringSearchTest(testCase, flowsToCreateTitles);

            LogHelper.LogTestEnd(log, string.Empty, testCase.TestDescription);
        }

        [Fact(DisplayName = "Create test article for news configuration tests")]
        public void CreateArticlesForNewsConfigurationTests()
        {
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetAdminUser(fixture.TestUsers, fixture.TestEnvironment, 1, "VF UK (AQA)_2");

            //testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment, 4);

            testCase.ItemOriginalId = "2036";

            testCase.ArticleType = ArticleType.CustomArticle;

            testCase.ArticleTestData = testDataHelper.TestDataArticle2036();

            testCase.TargetTestType = TestType.NewsConfiguration;

            List<string> articlesToCreateTitles = new List<string>()
            {
                "news_settings_test"
            };

            LogHelper.LogTestStart(log, testCase.TestDescription);

            profileMenuSteps.TrySignOut();

            CreateArticleForNewsSettingsTest(testCase, articlesToCreateTitles);

            LogHelper.LogTestEnd(log, string.Empty, testCase.TestDescription);
        }
    }
}
