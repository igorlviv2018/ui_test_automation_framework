using Xunit;
using NLog;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Models.TestCase;

namespace Tests
{
    public class CreateCustomArticleTests : TestBaseTaf, IClassFixture<TestFixture>
    {
        private readonly TestFixture fixture;

        private readonly StaticTestDataHelper testDataHelper;

        public CreateCustomArticleTests(TestFixture fixture) : base(LogManager.GetLogger("TafAuthCreateArticleUI"))
        {
            this.fixture = fixture;

            testDataHelper = new StaticTestDataHelper();
        }

        [Fact(DisplayName ="Create CA - Image, video, step-by-step (no collapses)")]
        [Trait("Category", "CreateCustomArticle")]
        //[Trait("Category", "TafAuthAll")]
        public void CreateArticle464()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TargetTestType = TestType.CreateCustomArticle;

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment, 3);

            testCase.ItemOriginalId = "464";

            testCase.ArticleType = ArticleType.CustomArticle;

            testCase.ArticleTestData = testDataHelper.TestDataArticle464();

            //Act, Assert
            CreateArticleBaseTest(testCase);
        }

        [Fact(DisplayName = "Create CA - Nested collapses with image, video, step-by-step, buttons block")]
        [Trait("Category", "CreateCustomArticle")]
        //[Trait("Category", "TafAuthAll")]
        public void CreateArticle001()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TargetTestType = TestType.CreateCustomArticle;

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment, 3);

            testCase.ItemOriginalId = "001";

            testCase.ArticleType = ArticleType.CustomArticle;

            testCase.ArticleTestData = testDataHelper.TestDataArticle001();

            //Act, Assert
            CreateArticleBaseTest(testCase);
        }

        [Fact(DisplayName = "Create CA - Step-by-step in a collapse")]
        [Trait("Category", "CreateCustomArticle")]
        //[Trait("Category", "TafAuthAll")]
        public void CreateArticle002()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TargetTestType = TestType.CreateCustomArticle;

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment, 3);

            testCase.ItemOriginalId = "002";

            testCase.ArticleType = ArticleType.CustomArticle;

            testCase.ArticleTestData = testDataHelper.TestDataArticle002();

            //Act, Assert
            CreateArticleBaseTest(testCase);
        }
    }
}
