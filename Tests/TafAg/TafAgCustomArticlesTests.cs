using Xunit;
using NLog;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Models.TestCase;

namespace Tests
{
    public class TafCustomArticleTests : TestBaseTaf, IClassFixture<TestFixture>
    {
        private readonly TestFixture fixture;

        private readonly StaticTestDataHelper testDataHelper;

        public TafCustomArticleTests(TestFixture testFixture) : base(LogManager.GetLogger("TafArticleTestsUI"))
        {
            this.fixture = testFixture;

            testDataHelper = new StaticTestDataHelper();
        }

        [Fact(DisplayName = "CA - Image, video, step-by-step (no collapses) (Id=464) test")]
        [Trait("Category", "TafCheckCustomArticle")]
        public void CustomArticleTest871()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment, 2);

            testCase.ItemOriginalId = "464";

            testCase.ArticleType = ArticleType.CustomArticle;

            testCase.ArticleTestData = testDataHelper.TestDataArticle464();

            //Act, Assert
            CheckArticleTaf(testCase, fixture.TestEnvironment);
        }

        [Fact(DisplayName = "CA - Nested collapses with image, video, step-by-step, buttons block (Id=001) test")]
        [Trait("Category", "TafCheckCustomArticle")]
        public void CustomArticleTest001()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment, 2);

            testCase.ItemOriginalId = "001";

            testCase.ArticleType = ArticleType.CustomArticle;

            testCase.ArticleTestData = testDataHelper.TestDataArticle001();

            //Act, Assert
            CheckArticleTaf(testCase, fixture.TestEnvironment);
        }

        [Fact(DisplayName = "CA - Step-by-step in a collapse (Id=002) test")]
        [Trait("Category", "TafCheckCustomArticle")]
        public void CustomArticleTest002()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment, 2);

            testCase.ItemOriginalId = "002";

            testCase.ArticleType = ArticleType.CustomArticle;

            testCase.ArticleTestData = testDataHelper.TestDataArticle002();

            //Act, Assert
            CheckArticleTaf(testCase, fixture.TestEnvironment);
        }
    }
}