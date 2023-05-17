using NLog;
using Taf.UI.Steps;
using Taf.UI.Core.Helpers;
using System.Collections.Generic;
using Taf.UI.Core.Models;
using Taf.UI.Core.Enums;
using Taf.UI.Steps.Authoring;
using Taf.UI.Core.Models.TestCase;
using Xunit;

namespace Tests
{
    public class CreateArticleTests : TestBaseTaf, IClassFixture<TestFixture>
    {
        private readonly TestFixture fixture;

        private readonly LoginSteps loginSteps;

        private readonly BrowserSteps browserSteps;

        private readonly CommonAuthoringSteps authoringSteps;

        private readonly PublishingSummarySteps publishSummarySteps;

        private readonly StaticTestDataHelper testDataHelper;

        public CreateArticleTests(TestFixture fixture) : base(LogManager.GetLogger("TafAuthCreateFlowsUI"))
        {
            this.fixture = fixture;

            loginSteps = new LoginSteps(log);

            browserSteps = new BrowserSteps(log);

            authoringSteps = new CommonAuthoringSteps(App.TafAuth, log);

            publishSummarySteps = new PublishingSummarySteps(App.TafAuth);

            testDataHelper = new StaticTestDataHelper();
        }

        [Fact(DisplayName = "Create DF - Decision with 5 branches (Id=1029) test")]
        [Trait("Category", "CreateDF")]
        public void CreateFlow1029()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TargetTestType = TestType.CreateDiagnosticFlow;

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment); //loginSteps.GetUserEmail(fixture.TestEnvironment);

            testCase.ItemOriginalId = "1029";

            testCase.ArticleTestData = testDataHelper.TestDataDF1029();

            //Act, Assert
            CreateArticleBaseTest(testCase);
        }

        [Fact(DisplayName = "Create DF - Decision with images in branches (Id=2720) test")]
        [Trait("Category", "CreateDF")]
        public void CreateFlow2720()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TargetTestType = TestType.CreateDiagnosticFlow;

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment); //loginSteps.GetUserEmail(fixture.TestEnvironment);

            testCase.ItemOriginalId = "2720";

            testCase.ArticleTestData = testDataHelper.TestDataDF2720();

            //Act, Assert
            CreateArticleBaseTest(testCase);
        }

        //[Trait("Category", "CreateDF")]
        //[Fact(DisplayName = "Debug Create DF - Decision with 5 branches (Id=1029) test")]
        //public void CreateFlow1029_debug()
        //{
        //    for (int i = 0; i < 50; i++)
        //    {
        //        CreateFlow1029();
        //        LogHelper.LogInfo(log, $"cycle ={i}");
        //    }
        //}

        [Fact(DisplayName = "Create DF - Decision with dropdown (one dropdown item with image) and Processes with buttons (Id=933) test")]
        [Trait("Category", "CreateDF")]
        public void CreateFlow933()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TargetTestType = TestType.CreateDiagnosticFlow;

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ItemOriginalId = "933";

            testCase.ArticleTestData = testDataHelper.TestDataDF933();

            //Act, Assert
            CreateArticleBaseTest(testCase);
        }

        [Fact(DisplayName = "Create DF - External connector with buttons (in processes) and decision (Id=1030) test")]
        [Trait("Category", "CreateDF")]
        public void DFTest1030()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TargetTestType = TestType.CreateDiagnosticFlow;

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ItemOriginalId = "1030";

            testCase.ArticleTestData = testDataHelper.TestDataDF1030(isAuthoring: true);

            //Act, Assert
            CreateArticleBaseTest(testCase);
        }

        [Fact(DisplayName = "Create DF - External connector with predefined process and step-by-step (Id=2045) test")]
        [Trait("Category", "CreateDF")]
        public void DFTest2045()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TargetTestType = TestType.CreateDiagnosticFlow;

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ItemOriginalId = "2045";

            testCase.ArticleTestData = testDataHelper.TestDataDF2045(isAuthoring: true);

            //Act, Assert
            CreateArticleBaseTest(testCase);

            fixture.ReopenBrowser(); //new2
        }

        [Fact(DisplayName = "Create DF - Images and video (also in decision) (Id=1948) test")]
        [Trait("Category", "CreateDF")]
        public void CreateFlow1948()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TargetTestType = TestType.CreateDiagnosticFlow;

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ItemOriginalId = "1948";

            testCase.ArticleTestData = testDataHelper.TestDataDF1948();

            //Act, Assert
            CreateArticleBaseTest(testCase);
        }

        [Fact(DisplayName = "Create DF - Internal connector pointing to decision with images in buttons (Id=950) test")]
        [Trait("Category", "CreateDF")]
        public void CreateFlow950()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TargetTestType = TestType.CreateDiagnosticFlow;

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ItemOriginalId = "950";

            testCase.ArticleTestData = testDataHelper.TestDataDF950();

            //Act, Assert
            CreateArticleBaseTest(testCase);

            fixture.ReopenBrowser(); //new2
        }

        [Fact(DisplayName = "Create DF - Internal connector to process (no decisions in DF) (Id=936) test")]
        [Trait("Category", "CreateDF")]
        public void CreateFlow936()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TargetTestType = TestType.CreateDiagnosticFlow;

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ItemOriginalId = "936";

            testCase.ArticleTestData = testDataHelper.TestDataDF936();

            //Act, Assert
            CreateArticleBaseTest(testCase);
        }

        [Fact(DisplayName = "Create DF - Internal connector to terminator")]
        [Trait("Category", "CreateDF")]
        public void CreateFlowInternalConnecorToTerminator()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TargetTestType = TestType.CreateDiagnosticFlow;

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ItemOriginalId = "937";

            testCase.ArticleTestData = testDataHelper.TestDataDF937();

            //Act, Assert
            CreateArticleBaseTest(testCase);
        }

        [Fact(DisplayName = "Create DF - Nested decisions (up to level 5) (Id=871) test")]
        [Trait("Category", "CreateDF")]
        public void CreateFlow871()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TargetTestType = TestType.CreateDiagnosticFlow;

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ItemOriginalId = "871";

            testCase.ArticleTestData = testDataHelper.TestDataDF871();

            //Act, Assert
            CreateArticleBaseTest(testCase);
        }

        [Fact(DisplayName = "Create DF - Next/Previous step button moving to a decision (Id=1701) test")]
        [Trait("Category", "CreateDF")]
        public void CreateFlow1701()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TargetTestType = TestType.CreateDiagnosticFlow;

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ArticleTestData = testDataHelper.TestDataDF1701();

            testCase.ItemOriginalId = "1701";

            //Act, Assert
            CreateArticleBaseTest(testCase);
        }
        
        [Fact(DisplayName = "Create DF - Next/Previous step button moving to proc with Next step button (Id=1712) test")]
        [Trait("Category", "CreateDF")]
        public void CreateFlow1712()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TargetTestType = TestType.CreateDiagnosticFlow;

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ArticleTestData = testDataHelper.TestDataDF1712();

            testCase.ItemOriginalId = "1712";

            //Act, Assert
            CreateArticleBaseTest(testCase);
        }

        [Fact(DisplayName = "Create DF - Next/Previous step button without effect (Id=1947) test")]
        [Trait("Category", "CreateDF")]
        public void CreateFlow1947()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TargetTestType = TestType.CreateDiagnosticFlow;

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ArticleTestData = testDataHelper.TestDataDF1947();

            testCase.ItemOriginalId = "1947";

            //Act, Assert
            CreateArticleBaseTest(testCase);

            fixture.ReopenBrowser(); //new2
        }

        [Fact(DisplayName = "Create DF - Next/Previous step button in nested decision [level 2] (Id=1944) test")]
        [Trait("Category", "CreateDF")]
        public void CreateFlow1944()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TargetTestType = TestType.CreateDiagnosticFlow;

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ArticleTestData = testDataHelper.TestDataDF1944();

            testCase.ItemOriginalId = "1944";

            //Act, Assert
            CreateArticleBaseTest(testCase);
        }

        [Fact(DisplayName = "Create DF - Predefined process outside and inside decision (Id=1048) test")]
        [Trait("Category", "CreateDF")]
        public void CreateFlow1048()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TargetTestType = TestType.CreateDiagnosticFlow;

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ArticleTestData = testDataHelper.TestDataDF1048();

            testCase.ItemOriginalId = "1048";

            //Act, Assert
            CreateArticleBaseTest(testCase);

            fixture.ReopenBrowser(); //new2
        }

        [Fact(DisplayName = "Create DF - Processes with buttons - no decisions (Id=836) test")]
        [Trait("Category", "CreateDF")]
        public void CreateFlow836()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TargetTestType = TestType.CreateDiagnosticFlow;

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ArticleTestData = testDataHelper.TestDataDF836();

            testCase.ItemOriginalId = "836";

            //Act, Assert
            CreateArticleBaseTest(testCase);
        }

        [Fact(DisplayName = "Create DF - Restart flow button in nested decision [level 2] (Id=1711) test")]
        [Trait("Category", "CreateDF")]
        public void CreateFlow1711()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TargetTestType = TestType.CreateDiagnosticFlow;

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ArticleTestData = testDataHelper.TestDataDF1711();

            testCase.ItemOriginalId = "1711";

            //Act, Assert
            CreateArticleBaseTest(testCase);
        }

        [Fact(DisplayName = "Create DF - Restart flow button in terminator (Id=001) test")]
        [Trait("Category", "CreateDF")]
        public void CreateFlow001()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TargetTestType = TestType.CreateDiagnosticFlow;

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ArticleTestData = testDataHelper.TestDataDF001();

            testCase.ItemOriginalId = "001";

            //Act, Assert
            CreateArticleBaseTest(testCase);
        }

        [Fact(DisplayName = "Create DF - Step-by-step in process (Id=1991) test")]
        [Trait("Category", "CreateDF")]
        public void CreateFlow1991()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TargetTestType = TestType.CreateDiagnosticFlow;

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ArticleTestData = testDataHelper.TestDataDF1991();

            testCase.ItemOriginalId = "1991";

            //Act, Assert
            CreateArticleBaseTest(testCase);
        }

        [Fact(DisplayName = "Create DF - Terminator with Previous step button (Id=1705) test")]
        [Trait("Category", "CreateDF")]
        public void CreateFlow1705()
        {
            //Arrange
            CreateArticleTestCase testCase = new CreateArticleTestCase();

            testCase.TargetTestType = TestType.CreateDiagnosticFlow;

            testCase.TestDescription = XUnitHelper.FactDisplayName();

            testCase.TestEnvironment = fixture.TestEnvironment;

            testCase.User = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            testCase.ArticleTestData = testDataHelper.TestDataDF1705();

            testCase.ItemOriginalId = "1705";

            //Act, Assert
            CreateArticleBaseTest(testCase);
        }

        [Fact(DisplayName = "Publish Location No Channels Alert")]
        [Trait("Category", "CreateDF")]
        public void PublishLocationNoChannelsAlert()
        {
            //Arrange
            string testDesc = "Publish Location No Channels Alert";

            log.Info($"*** {testDesc} started ***");

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            User contentAuthor = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            string err = loginSteps.Login(contentAuthor);

            Assert.True(string.IsNullOrEmpty(err), err);

            //Act
            browserSteps.OpenAppDeepLink(App.TafAuth);

            ArticleProperties articleProperties = new ArticleProperties()
            {
                ArticleType = ArticleType.CustomArticle,
                Title = "article_cannot_be_published_test_" + DataHelper.DateTimeString(),
                Description = "description",
                PublishDate = "Immediately",
                ExpirationDate = "Evergreen"
            };

            err = authoringSteps.CreateArticleProperties(articleProperties);

            LogHelper.Log(log, $"Flow created ({articleProperties})", err);

            Assert.True(string.IsNullOrEmpty(err), $"Failed to create article: {articleProperties.Title}!");

            bool isPublishSummaryModalOpened = publishSummarySteps.OpenPublishSummaryModal();

            Assert.True(isPublishSummaryModalOpened, "Publish summary modal is not displayed!");

            List<string> errors = new List<string>();

            err = publishSummarySteps.CheckNoChannelsWarning();

            LogHelper.Log(log, "No channels selected icon checked", err);

            ErrorHelper.AddToErrorList(errors, err);

            err = publishSummarySteps.CheckGoToLocationLinkInChannelPublishProperties();

            LogHelper.Log(log, "Go to Location link checked", err);

            ErrorHelper.AddToErrorList(errors, err);

            //Assert
            string allErrors = string.Join("; ", errors.ToArray());

            LogHelper.LogTestEnd(log, errors, testDesc);

            Assert.True(errors.Count == 0, allErrors);
        }

        [Fact(DisplayName = "Incomplete flow cannot be published")]
        [Trait("Category", "CreateDF")]
        public void IncompleteFlowCannotBePublished()
        {
            //Arrange
            string testDesc = "Incomplete flow cannot be published";

            log.Info($"*** {testDesc} started ***");

            //loginSteps.OpenApp(App.Taf, fixture.TestEnvironment);

            User contentAuthor = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment);

            string err = loginSteps.Login(contentAuthor);

            Assert.True(string.IsNullOrEmpty(err), err);

            //Act
            browserSteps.OpenAppDeepLink(App.TafAuth);

            ArticleProperties articleProperties = new ArticleProperties()
            {
                ArticleType = ArticleType.DiagnosticFlow,
                Title = "flow_cannot_be_published_test_" + DataHelper.DateTimeString(),
                Description = "description",
                PublishDate = "Immediately",
                ExpirationDate = "Evergreen"
            };

            err = authoringSteps.CreateArticleProperties(articleProperties);

            LogHelper.Log(log, $"Flow created ({articleProperties})", err);

            Assert.True(string.IsNullOrEmpty(err), $"Failed to create article: {articleProperties.Title} ({err})!");

            bool isPublishSummaryModalOpened = publishSummarySteps.OpenPublishSummaryModal();

            Assert.True(isPublishSummaryModalOpened, "Publish summary modal is not displayed!");

            List<string> errors = new List<string>();

            err = publishSummarySteps.CheckIncompleteFlowError();

            LogHelper.Log(log, "Incomplete flow icon checked", err);

            ErrorHelper.AddToErrorList(errors, err);

            err = publishSummarySteps.CheckGoToContentsLinkInFlowProperties();

            LogHelper.Log(log, "Go to Contents link checked", err);

            ErrorHelper.AddToErrorList(errors, err);

            err = publishSummarySteps.CheckExpectedButtonsPresent("Cancel");

            LogHelper.Log(log, "Cancel button presence checked", err);

            ErrorHelper.AddToErrorList(errors, err);

            err = publishSummarySteps.CheckCancelButton();

            LogHelper.Log(log, "Cancel button functionality checked", err);

            ErrorHelper.AddToErrorList(errors, err);

            //Assert
            string allErrors = string.Join("; ", errors.ToArray());

            LogHelper.LogTestEnd(log, errors, testDesc);

            Assert.True(errors.Count == 0, allErrors);
        }
    }
}
