using NLog;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Models.TestCase;
using Xunit;

namespace Tests
{
    public class TafCustomArticleTestsRedesign : TestBaseTaf, IClassFixture<TestFixture>
    {
        private readonly TestFixture fixture;

        private readonly StaticTestDataHelper testDataHelper;

        public TafCustomArticleTestsRedesign(TestFixture testFixture) : base(LogManager.GetLogger("TafArticleTestsUIRedesign"), isRedesign:true)
        {
            this.fixture = testFixture;

            testDataHelper = new StaticTestDataHelper();
        }

        [Fact(DisplayName = "Redesign: CA - Nested collapses with image, video, step-by-step, buttons block (Id=001) test")]
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

        [Fact(DisplayName = "Redesign: CA - Step-by-step in a collapse (Id=002) test")]
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

        [Fact(DisplayName = "Redesign: CA - Image, video, step-by-step (no collapses) (Id=464) test")]
        [Trait("Category", "TafCheckCustomArticle")]
        public void CustomArticleTest871Redesign()
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

        //[Fact(DisplayName = "Redesign: debug")]
        //[Trait("Category", "TafCheckCustomArticle")]
        //public void CustomArticleTest871RedesignDebug()
        //{
        //    //Arrange
        //    for (int i = 0; i < 10; i++)
        //    {
        //        CustomArticleTest871Redesign();

        //        log.Info($"cycle: {i}");
        //    }
        //}
    }
}
