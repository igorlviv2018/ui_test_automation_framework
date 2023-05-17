using Xunit;
using NLog;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models.TestCase;

namespace Tests
{
    public class TafFlowTestsRedesign : TestBaseTaf, IClassFixture<TestFixture>
    {
        private readonly TestFixture fixture;

        private readonly StaticTestDataHelper testDataHelper;

        public TafFlowTestsRedesign(TestFixture fixture) : base(LogManager.GetLogger("TafArticleTestsUI"), isRedesign: true)
        {
            this.fixture = fixture;

            testDataHelper = new StaticTestDataHelper();
        }

        [Fact(DisplayName = "DF - Nested decisions (up to level 5) (Id=871) test")]
        [Trait("Category", "TafCheckFlow")]
        public void DFTest871()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ItemOriginalId = "871";

            testCase.ArticleTestData = testDataHelper.TestDataDF871();

            //Act, Assert
            CheckArticleTaf(testCase, fixture.TestEnvironment);
        }

        [Fact(DisplayName = "DF - Decision with 5 branches (Id=1029) test")]
        [Trait("Category", "TafCheckFlow")]
        public void DFTest1029()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ItemOriginalId = "1029";

            testCase.ArticleTestData = testDataHelper.TestDataDF1029();

            //Act, Assert
            CheckArticleTaf(testCase, fixture.TestEnvironment);
        }

        [Fact(DisplayName = "DF - Decision with images in branches (Id=2720) test")]
        [Trait("Category", "TafCheckFlow")]
        public void DFTest2720()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ItemOriginalId = "2720";

            testCase.ArticleTestData = testDataHelper.TestDataDF2720();

            //Act, Assert
            CheckArticleTaf(testCase, fixture.TestEnvironment);
        }

        [Fact(DisplayName = "DF - Decision with dropdown (one dropdown item with image) and Processes with buttons (Id=933) test")]
        [Trait("Category", "TafCheckFlow")]
        public void DFTest933()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ItemOriginalId = "933";

            testCase.ArticleTestData = testDataHelper.TestDataDF933();

            //Act, Assert
            CheckArticleTaf(testCase, fixture.TestEnvironment);
        }

        [Fact(DisplayName = "DF - External connector with buttons (in processes) and decision (Id=1030) test")]
        [Trait("Category", "TafCheckFlow")]
        public void DFTest1030()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ItemOriginalId = "1030";

            testCase.ArticleTestData = testDataHelper.TestDataDF1030();

            //Act, Assert
            CheckArticleTaf(testCase, fixture.TestEnvironment);
        }

        [Fact(DisplayName = "DF - External connector with with predefined process and step-by-step (Id=2045) test")]
        [Trait("Category", "TafCheckFlow")]
        public void DFTest2045()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ItemOriginalId = "2045";

            testCase.ArticleTestData = testDataHelper.TestDataDF2045();

            //Act, Assert
            CheckArticleTaf(testCase, fixture.TestEnvironment);
        }

        [Fact(DisplayName = "DF - Images and video (also in decision) (Id=1948) test")]
        [Trait("Category", "TafCheckFlow")]
        public void DFTest1948()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ItemOriginalId = "1948";

            testCase.ArticleTestData = testDataHelper.TestDataDF1948();

            //Act, Assert
            CheckArticleTaf(testCase, fixture.TestEnvironment);
        }

        [Fact(DisplayName = "DF - Internal connector to decision with images in buttons (Id=950) test")]
        [Trait("Category", "TafCheckFlow")]
        public void DFTest950()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ItemOriginalId = "950";

            testCase.ArticleTestData = testDataHelper.TestDataDF950();

            //Act, Assert
            CheckArticleTaf(testCase, fixture.TestEnvironment);
        }

        [Fact(DisplayName = "DF - Internal connector to process (no decisions in DF) (Id=936) test")]
        [Trait("Category", "TafCheckFlow")]
        public void DFTest936()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ItemOriginalId = "936";

            testCase.ArticleTestData = testDataHelper.TestDataDF936();

            //Act, Assert
            CheckArticleTaf(testCase, fixture.TestEnvironment);
        }

        [Fact(DisplayName = "DF - Internal connector to Terminator (Id=937) test")]
        [Trait("Category", "TafCheckFlow")]
        public void DFTest937()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ItemOriginalId = "937";

            testCase.ArticleTestData = testDataHelper.TestDataDF937();

            //Act, Assert
            CheckArticleTaf(testCase, fixture.TestEnvironment);
        }

        [Fact(DisplayName = "DF - Next/Previous step button moving to a decision (Id=1701) test")]
        [Trait("Category", "TafCheckFlow")]
        public void DFTest1701()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ItemOriginalId = "1701";

            testCase.ArticleTestData = testDataHelper.TestDataDF1701();

            //Act, Assert
            CheckArticleTaf(testCase, fixture.TestEnvironment);
        }

        [Fact(DisplayName = "DF - Next/Previous step button moving to proc with Next step button (Id=1712) test")]
        [Trait("Category", "TafCheckFlow")]
        public void DFTest1712()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ItemOriginalId = "1712";

            testCase.ArticleTestData = testDataHelper.TestDataDF1712();

            //Act, Assert
            CheckArticleTaf(testCase, fixture.TestEnvironment);
        }

        [Fact(DisplayName = "DF - Next/Previous step button in nested decision [level 2] (Id=1944) test")]
        [Trait("Category", "TafCheckFlow")]
        public void DFTest1944()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ItemOriginalId = "1944";

            testCase.ArticleTestData = testDataHelper.TestDataDF1944();

            //Act, Assert
            CheckArticleTaf(testCase, fixture.TestEnvironment);
        }

        [Fact(DisplayName = "DF - Next/Previous step button without effect (Id=1947) test")]
        [Trait("Category", "TafCheckFlow")]
        public void DFTest1947()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ItemOriginalId = "1947";

            testCase.ArticleTestData = testDataHelper.TestDataDF1947();

            //Act, Assert
            CheckArticleTaf(testCase, fixture.TestEnvironment);
        }

        //debug to del
        //[Fact(DisplayName = "Debug check DF - Predefined process outside and inside decision (Id=1048) test")]
        //[Trait("Category", "CreateDF")]
        //public void CheckFlow1048_debug()
        //{
        //    for (int i = 0; i < 6; i++)
        //    {
        //        DFTest1048();

        //        LogHelper.LogInfo(log, $"cycle ={i}");
        //    }
        //}

        [Fact(DisplayName = "DF - Predefined process outside and inside decision (Id=1048) test")]
        [Trait("Category", "TafCheckFlow")]
        public void DFTest1048()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ItemOriginalId = "1048";

            testCase.ArticleTestData = testDataHelper.TestDataDF1048();

            //Act, Assert
            CheckArticleTaf(testCase, fixture.TestEnvironment);
        }

        [Fact(DisplayName = "DF - Processes with buttons but no decisions (Id=836) test")]
        [Trait("Category", "TafCheckFlow")]
        public void DFTest836()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ItemOriginalId = "836";

            testCase.ArticleTestData = testDataHelper.TestDataDF836();

            //Act, Assert
            CheckArticleTaf(testCase, fixture.TestEnvironment);
        }

        [Fact(DisplayName = "DF - Restart flow button in nested decision [level 2] (Id=1711) test")]
        [Trait("Category", "TafCheckFlow")]
        public void DFTest1711()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ItemOriginalId = "1711";

            testCase.ArticleTestData = testDataHelper.TestDataDF1711();

            //Act, Assert
            CheckArticleTaf(testCase, fixture.TestEnvironment);
        }

        [Fact(DisplayName = "DF - Restart flow button in terminator (Id=001) test")]
        [Trait("Category", "TafCheckFlow")]
        public void DFTest001()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ItemOriginalId = "001";

            testCase.ArticleTestData = testDataHelper.TestDataDF001();

            //Act, Assert
            CheckArticleTaf(testCase, fixture.TestEnvironment);
        }

        [Fact(DisplayName = "DF - Step-by-step in process (Id=1991) test")]
        [Trait("Category", "TafCheckFlow")]
        public void DFTest1991()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ItemOriginalId = "1991";

            testCase.ArticleTestData = testDataHelper.TestDataDF1991();

            //Act, Assert
            CheckArticleTaf(testCase, fixture.TestEnvironment);
        }

        [Fact(DisplayName = "DF - Terminator with Previous step button (Id=1705) test")]
        [Trait("Category", "TafCheckFlow")]
        public void CreateFlow1705()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ItemOriginalId = "1705";

            testCase.ArticleTestData = testDataHelper.TestDataDF1705();

            //Act, Assert
            CheckArticleTaf(testCase, fixture.TestEnvironment);
        }
    }
}

