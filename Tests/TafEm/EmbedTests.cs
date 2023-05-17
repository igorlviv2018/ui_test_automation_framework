using NLog;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models.TestCase;
using Xunit;

namespace Tests
{
    public class TafEmTests : TestBaseEmbed, IClassFixture<TestFixture>
    {
        private readonly TestFixture fixture;

        //private readonly ILogger log;

        private readonly StaticTestDataHelper testDataHelper;

        public TafEmTests(TestFixture fixture) : base(LogManager.GetLogger("TafEmArticleTestsUI"))
        {
            this.fixture = fixture;

            //log = LogManager.GetLogger("TafEmArticleTestsUI");

            testDataHelper = new StaticTestDataHelper();

            //clientName = fixture.TestConfig["ClientName"];
        }

        //[Fact(DisplayName = "Custom article (Id=840) test")]
        //public void EmbedCustomArticleTest()
        //{
        //    string testDesc = "Custom article (Id=840) test";

        //    log.Info($"*** {testDesc} started ***");

        //    TafEmSteps.OpenEmbedAppAndShowContent("1593");//"935"); //"840"

        //    //List<TafEmArticleElement> articl = testDataHelper.TestArticleDataId840_new();

        //    List<TafEmArticleElement> articl = testDataHelper.TestArticleDataId840_new_upd();

        //    //List<TafEmArticleElement> res = articl[0].FindArticlePaths2(articl);

        //    string err = TafEmSteps.CheckCustomArticle(articl);

        //    log.Info($"Logged in successful");

        //    LogHelper.LogTestEnd(log, err, testDesc);

        //    //Assert
        //    Assert.True(string.IsNullOrEmpty(err), err);
        //}

        [Fact(DisplayName = "DF - Decision with 5 branches (Id=1029) test")]
        public void DFTest1029()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.ArticleOriginalId = "1029";

            testCase.ArticleTestData = testDataHelper.TestDataDF1029();

            //Act, Assert
            CheckArticleEmbed(testCase);
        }

        [Fact(DisplayName = "DF - Decision with dropdown (one dropdown item with image) and Processes with buttons (Id=933) test")]
        public void DFTest933()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.ArticleOriginalId = "933";

            testCase.ArticleTestData = testDataHelper.TestDataDF933();

            //Act, Assert
            CheckArticleEmbed(testCase);
        }

        [Fact(DisplayName = "DF - External connector with buttons (in processes) and decision (Id=1030) test")]
        public void DFTest1030()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.ArticleOriginalId = "1030";

            testCase.ArticleTestData = testDataHelper.TestDataDF1030();

            //Act, Assert
            CheckArticleEmbed(testCase);
        }

        [Fact(DisplayName = "DF - External connector with with predefined process and step-by-step (Id=2045) test")]
        public void DFTest2045()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.ArticleOriginalId = "2045";

            testCase.ArticleTestData = testDataHelper.TestDataDF2045();

            //Act, Assert
            CheckArticleEmbed(testCase);
        }

        [Fact(DisplayName = "DF - Images and video (also in decision) (Id=1948) test")]
        public void DFTest1948()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.ArticleOriginalId = "1948";

            testCase.ArticleTestData = testDataHelper.TestDataDF1948();

            //Act, Assert
            CheckArticleEmbed(testCase);
        }

        [Fact(DisplayName = "DF - Internal connector to decision (Id=950) test")]
        public void DFTest950()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.ArticleOriginalId = "950";

            testCase.ArticleTestData = testDataHelper.TestDataDF950();

            //Act, Assert
            CheckArticleEmbed(testCase);
        }

        [Fact(DisplayName = "DF - Internal connector to process (no decisions in DF) (Id=936) test")]
        public void DFTest936()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.ArticleOriginalId = "936";

            testCase.ArticleTestData = testDataHelper.TestDataDF936();

            //Act, Assert
            CheckArticleEmbed(testCase);
        }

        [Fact(DisplayName = "DF - Internal connector to Terminator (Id=937) test")]
        public void DFTest937()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.ArticleOriginalId = "937";

            testCase.ArticleTestData = testDataHelper.TestDataDF937();

            //Act, Assert
            CheckArticleEmbed(testCase);
        }

        [Fact(DisplayName = "DF - Nested decisions (up to level 5) (Id=871) test")]
        public void DFTest871()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.ArticleOriginalId = "871";

            testCase.ArticleTestData = testDataHelper.TestDataDF871();

            //Act, Assert
            CheckArticleEmbed(testCase);
        }

        [Fact(DisplayName = "DF - Next/Previous step button in nested decision [level 2] (Id=1944) test")]
        public void DFTest1944()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.ArticleOriginalId = "1944";

            testCase.ArticleTestData = testDataHelper.TestDataDF1944();

            //Act, Assert
            CheckArticleEmbed(testCase);
        }

        [Fact(DisplayName = "DF - Next/Previous step button moving to a decision (Id=1701) test")]
        public void DFTest1701()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.ArticleOriginalId = "1701";

            testCase.ArticleTestData = testDataHelper.TestDataDF1701();

            //Act, Assert
            CheckArticleEmbed(testCase);
        }

        [Fact(DisplayName = "DF - Next/Previous step button moving to proc with Next step button (Id=1712) test")]
        public void DFTest1712()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.ArticleOriginalId = "1712";

            testCase.ArticleTestData = testDataHelper.TestDataDF1712();

            //Act, Assert
            CheckArticleEmbed(testCase);
        }

        [Fact(DisplayName = "DF - Next/Previous step button without effect (Id=1947) test")]
        public void DFTest1947()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.ArticleOriginalId = "1947";

            testCase.ArticleTestData = testDataHelper.TestDataDF1947();

            //Act, Assert
            CheckArticleEmbed(testCase);
        }

        [Fact(DisplayName = "DF - Predefined process outside and inside decision (Id=1048) test")]
        public void DFTest1048()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.ArticleOriginalId = "1048";

            testCase.ArticleTestData = testDataHelper.TestDataDF1048();

            //Act, Assert
            CheckArticleEmbed(testCase);
        }

        [Fact(DisplayName = "DF - Processes with buttons but no decisions (Id=836) test")]
        public void DFTest836()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.ArticleOriginalId = "836";

            testCase.ArticleTestData = testDataHelper.TestDataDF836();

            //Act, Assert
            CheckArticleEmbed(testCase);
        }

        [Fact(DisplayName = "DF - Restart flow button in nested decision [level 2] (Id=1711) test")]
        public void DFTest1711()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.ArticleOriginalId = "1711";

            testCase.ArticleTestData = testDataHelper.TestDataDF1711();

            //Act, Assert
            CheckArticleEmbed(testCase);
        }

        [Fact(DisplayName = "DF - Restart flow button in terminator (Id=001) test")]
        public void DFTest001()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.ArticleOriginalId = "001";

            testCase.ArticleTestData = testDataHelper.TestDataDF001();

            //Act, Assert
            CheckArticleEmbed(testCase);
        }

        [Fact(DisplayName = "DF - Step-by-step in process (Id=1991) test")]
        public void DFTest1991()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.ArticleOriginalId = "1991";

            testCase.ArticleTestData = testDataHelper.TestDataDF1991();

            //Act, Assert
            CheckArticleEmbed(testCase);
        }

        [Fact(DisplayName = "DF - Terminator with Previous step button (Id=1705) test")]
        public void CreateFlow1705()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.ArticleTestData = testDataHelper.TestDataDF1705();

            testCase.ArticleOriginalId = "1705";

            //Act, Assert
            CheckArticleEmbed(testCase);
        }
    }
}

