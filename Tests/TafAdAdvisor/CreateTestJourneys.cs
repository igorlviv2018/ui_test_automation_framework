using Xunit;
using NLog;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Models.TestCase;
using System.Collections.Generic;

namespace Tests
{
    public class CreateTestJourneysTests : TestBaseTaf, IClassFixture<TestFixture>
    {
        private readonly TestFixture fixture;

        private readonly StaticTestDataHelper testDataHelper;

        public CreateTestJourneysTests(TestFixture fixture) : base(LogManager.GetLogger("TafAdCreateTestJourneysUI"))
        {
            this.fixture = fixture;

            testDataHelper = new StaticTestDataHelper();
        }

        [Fact(DisplayName ="Create test journeys for journey archive/restore test")]
        public void CreateJourneysForOperationTests()
        {
            CreateJourneyTestCase testCase = new CreateJourneyTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.UserLogin = "aqa.adm.ee.dev@gmail.com";

            testCase.ItemOriginalId = "2036";

            testCase.JourneyType = JourneyType.Phone;

            //testCase.ArticleTestData = testDataHelper.TestDataArticle2036(); // del

            // archive/restore journeys
            testCase.TargetTestType = TestType.ArchiveRestoreJourney;

            //CreateMultipleArticlesTest(testCase, 3);

            // duplicate active articles
            testCase.TargetTestType = TestType.DuplicateActiveArticle;

            //CreateMultipleArticlesTest(testCase, 1);

            // duplicate archived articles
            testCase.TargetTestType = TestType.DuplicateArchivedArticle;

            //CreateMultipleArticlesTest(testCase, 1);

            // delete articles
            testCase.TargetTestType = TestType.DeleteArticle;

            //CreateMultipleArticlesTest(testCase, 1);
        }

        [Fact(DisplayName = "Create test flows for article archive/restore test")]
        public void CreateCreateFlowsForOperationTests()
        {
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.UserLogin = "aqa.sp.adm.prod@gmail.com";

            testCase.ItemOriginalId = "2059";

            testCase.ArticleType = ArticleType.DiagnosticFlow;

            testCase.ArticleTestData = testDataHelper.TestDataDF2059();

            // archive/restore articles
            testCase.TargetTestType = TestType.ArchiveRestoreArticle;
            //CreateMultipleArticlesTest(testCase, 3);

            // duplicate active articles
            testCase.TargetTestType = TestType.DuplicateActiveArticle;

            CreateMultipleArticlesTest(testCase, 1);

            // duplicate archived articles
            testCase.TargetTestType = TestType.DuplicateArchivedArticle;

            CreateMultipleArticlesTest(testCase, 1);

            // delete articles
            testCase.TargetTestType = TestType.DeleteArticle;

            CreateMultipleArticlesTest(testCase, 1);
        }

        [Fact(DisplayName = "Create test articles (CAs & DFs) for article search tests")]
        public void CreateArticlesForSearchTests()
        {
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.UserLogin = "aqa.sp.adm.prod@gmail.com";

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
        }
    }
}
