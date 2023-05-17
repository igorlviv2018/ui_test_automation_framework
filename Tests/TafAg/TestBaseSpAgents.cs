using Xunit;
using NLog;
using System.Collections.Generic;
using Taf.UI.Steps;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Models.TestCase;
using Taf.UI.Core.Models;
using Taf.UI.Steps.Authoring;
using System.Linq;
using Taf.UI.Steps.TafAdSteps;
using Taf.UI.Steps.TafSteps;
using Taf.UI.Core.Models.TafAuth;

namespace Tests
{
    public class TestBaseTaf
    {
        protected readonly ILogger log;

        protected readonly bool isRedesign;

        private readonly LoginSteps loginSteps;

        private readonly DxFlowSteps dxFlowSteps;

        private readonly CustomArticleSteps customArticleSteps;

        private readonly SidebarSteps sideMenuSteps;

        private readonly ArticleTableSteps articleTableSteps;

        private readonly JourneyTableSteps journeyTableSteps;

        private readonly ArticlesPageSteps articlesPageSteps;

        private readonly LocationSettingsSteps locationSettingsSteps;

        private readonly CommonAuthoringSteps authoringSteps;

        private readonly BrowserSteps browserSteps;

        private readonly TestCaseHelper testCaseHelper;

        private readonly List<CreateArticleTestCase> createArticleTestResults;

        private readonly List<CreateArticleTestCase> createFlowTestResults;

        public TestBaseTaf(ILogger logger, App app=App.Taf, bool isRedesign=false)
        {
            log = logger;

            this.isRedesign = isRedesign;

            loginSteps = new LoginSteps(log);

            dxFlowSteps = new DxFlowSteps(app, log, isRedesign);

            customArticleSteps = new CustomArticleSteps(app, log, isRedesign);

            sideMenuSteps = new SidebarSteps(log, isRedesign);

            articleTableSteps = new ArticleTableSteps(log);

            journeyTableSteps = new JourneyTableSteps(log);

            articlesPageSteps = new ArticlesPageSteps(log);

            locationSettingsSteps = new LocationSettingsSteps(App.TafAuth);

            authoringSteps = new CommonAuthoringSteps(App.TafAuth, log);

            browserSteps = new BrowserSteps(log);

            testCaseHelper = new TestCaseHelper();

            createArticleTestResults = CsvHelper.ReadCsv(CommonHelper.GetPathToCreatedItems(CommonHelper.CreatedItemsFileName(TestType.CreateCustomArticle)));

            createArticleTestResults.AddRange(CsvHelper.ReadCsv(CommonHelper.GetPathToCreatedItems(CommonHelper.CreatedItemsFileName(TestType.ArticleOperations))));

            createFlowTestResults = CsvHelper.ReadCsv(CommonHelper.GetPathToCreatedItems(CommonHelper.CreatedItemsFileName(TestType.CreateDiagnosticFlow)));
        }

        public void CheckArticleTaf(CreateArticleTestCase testCase, TestEnvironment testEnvironment)
        {
            //Arrange
            LogHelper.LogTestStart(log, testCase.TestDescription);

            dxFlowSteps.ClearTestedItemIds();

            List<CreateArticleTestCase> createItemTestResults = testCase.ArticleType == ArticleType.CustomArticle
                ? createArticleTestResults
                : createFlowTestResults;

            testCase = testCaseHelper.AddTestArticleCreationStatus(testCase, createItemTestResults);

            bool isTestArticleCreated = testCase.IsTestItemCreated();

            log.Info($"Test article (original id={testCase.ItemOriginalId}) is created (and published): {isTestArticleCreated}");

            Assert.True(isTestArticleCreated, $"Test article (original id={testCase.ItemOriginalId}) not created (or not published)");

            string err = loginSteps.Login(testCase.User, isRedesign: isRedesign);

            Assert.True(string.IsNullOrEmpty(err), err);

            browserSteps.OpenAppDeepLink(App.Taf, isRedesign);

            sideMenuSteps.LockSidebar();

            sideMenuSteps.OpenPage("Articles");

            err = isRedesign 
                ? articlesPageSteps.OpenArticleFromGrid(testCase.ItemTitle)
                : articlesPageSteps.OpenArticle(testCase.ItemTitle);

            LogHelper.Log(log, $"Test article/flow '{testCase.ItemTitle}' opened", err);

            Assert.True(string.IsNullOrEmpty(err), err);

            //Act
            err = CheckArticle(testCase);

            LogHelper.LogInfo(log, $"DF paths validated");

            LogHelper.LogTestEnd(log, err, testCase.TestDescription);

            //Assert
            Assert.True(string.IsNullOrEmpty(err), err);
        }

        public void CreateArticle(CreateArticleTestCase testCase, string articleTitle="")
        {
            //Arrange
            ArticleProperties articleProperties = new ArticleProperties()
            {
                ArticleType = testCase.ArticleType,
                Title = !string.IsNullOrEmpty(articleTitle) ? articleTitle : testCaseHelper.GetUniqueArticleTitle(testCase),
                Description = "description",
                PublishDate = "Immediately",
                ExpirationDate = "Evergreen", 
                PublishChannelsOptions = locationSettingsSteps.GetDefaultPublishChannelsOptions()
            };

            testCase.ItemTitle = articleProperties.Title;

            List<TafEmArticleElement> articleElementSequence = authoringSteps.GetArticleBlockSequence(testCase);

            //Act
            string err = authoringSteps.CreateArticleFull(articleProperties, articleElementSequence, testCase);

            Assert.True(string.IsNullOrEmpty(err), err);

            testCase.IsItemCreated = true;

            err = authoringSteps.PublishItem(); //publish - extract to separate method

            Assert.True(string.IsNullOrEmpty(err), err);

            authoringSteps.WaitItemsTableIsDisplayed();

            //LogHelper.LogTestEnd(log, err, testCase.TestDescription);

            ////Assert
            //Assert.True(string.IsNullOrEmpty(err), err);

            //CsvHelper.AppendToCsv(testCase, "d:\\createTest.csv");
        }

        public void CreateArticle(CreateArticleTestCase testCase, ArticleProperties articleProperties)
        {
            //Arrange
            articleProperties.ArticleType = testCase.ArticleType;

            if (string.IsNullOrEmpty(articleProperties.Title))
            {
                articleProperties.Title = testCaseHelper.GetUniqueArticleTitle(testCase);
            }

            articleProperties.Description = "description";

            articleProperties.PublishDate = "Immediately";

            articleProperties.ExpirationDate = "Evergreen";

            testCase.ItemTitle = articleProperties.Title;

            List<TafEmArticleElement> articleElementSequence = authoringSteps.GetArticleBlockSequence(testCase);

            //Act
            string err = authoringSteps.CreateArticleFull(articleProperties, articleElementSequence, testCase);

            Assert.True(string.IsNullOrEmpty(err), err);

            testCase.IsItemCreated = true;
        }

        public void PublishArticle()
        {
            string err = authoringSteps.PublishItem();

            Assert.True(string.IsNullOrEmpty(err), err);
        }

        public void CreateArticleBaseTest(CreateArticleTestCase testCase, string articleTitle = "")
        {
            LogHelper.LogTestStart(log, testCase.TestDescription);

            OpenAuthoring(testCase);

            CreateArticle(testCase, articleTitle);

            LogHelper.LogTestEnd(log, string.Empty, testCase.TestDescription);

            CsvHelper.AppendToCsv(testCase, CommonHelper.GetPathToCreatedItems(CommonHelper.CreatedItemsFileName(testCase.TargetTestType)));
        }

        public void CreateArticlesForAuthoringSearchTest(CreateArticleTestCase testCase, List<string> articleTitles)
        {
            LogHelper.LogTestStart(log, testCase.TestDescription);

            OpenAuthoring(testCase);

            List<string> titlesColumn = articleTableSteps.GetTitleColumn();

            foreach (var title in articleTitles)
            {
                bool isArticleInTable = articleTableSteps.FindArticleRowPosition(title, titlesColumn) != -1;

                if (!isArticleInTable)
                {
                    CreateArticle(testCase, title);
                }
            }
        }

        public void CreateArticleForNewsSettingsTest(CreateArticleTestCase testCase, List<string> articleTitles)
        {
            OpenAuthoring(testCase);

            List<string> titlesColumn = articleTableSteps.GetTitleColumn();

            PublishChannelsOptions publishChannelsOptions = locationSettingsSteps
                .SetTafPublishOptions(postToNewsFeed: true, includeArticleInSearch: true, releaseNotes: "news test");

            foreach (var title in articleTitles)
            {
                bool isArticleInTable = articleTableSteps.FindArticleRowPosition(title, titlesColumn) != -1;

                if (!isArticleInTable)
                {
                    //CreateArticle(testCase, title);
                    CreateArticle(testCase, new ArticleProperties() { Title = title, PublishChannelsOptions = publishChannelsOptions });

                    PublishArticle();
                }
            }
        }

        public void OpenAuthoring(CreateArticleTestCase testCase)
        {
            string err = loginSteps.Login(testCase.User);

            Assert.True(string.IsNullOrEmpty(err), err);

            browserSteps.OpenAppDeepLink(App.TafAuth);
        }

        public void OpenAdvisor(CreateArticleTestCase testCase)
        {
            string err = loginSteps.Login(testCase.UserLogin);

            Assert.True(string.IsNullOrEmpty(err), err);

            browserSteps.OpenAppDeepLink(App.TafAd);
        }

        public void CreateMultipleArticlesTest(CreateArticleTestCase testCase, int numberOfArticlesToCreate)
        {
            for (int i = 0; i < numberOfArticlesToCreate; i++)
            {
                CreateArticleBaseTest(testCase);
            }
        }

        private string CheckArticle(CreateArticleTestCase testCase)
        {
            string err = string.Empty;

            if (testCase.ArticleType == ArticleType.CustomArticle)
            {
                err = customArticleSteps.CheckCustomArticle(testCase.ArticleTestData);
            }
            else if (testCase.ArticleType == ArticleType.DiagnosticFlow)
            {
                err = dxFlowSteps.CheckAllPaths(testCase.ArticleTestData);
            }

            return err;
        }

        public List<string> GetAvailableTestItemTitles(TestType verifyTestToRunType)
        {
            List<string> availableArticleTitles = GetAvailableTestItems(verifyTestToRunType)
                .Select(x => x.ItemTitle)
                .ToList();

            return availableArticleTitles;
        }

        public List<CreateArticleTestCase> GetAvailableTestItems(TestType verifyTestToRunType)
        {
            string titlePrefix = CommonHelper.GetArticleTitlePrefixByTestType(verifyTestToRunType);

            List<CreateArticleTestCase> availableItemTitles = createArticleTestResults
                .Where(x => x.ItemTitle.StartsWith(titlePrefix))
                .ToList();

            return availableItemTitles;
        }

        public List<string> GetRandomNumberOfTestItems(TestType testType)
        {
            List<string> availableTestItems = GetAvailableTestItemTitles(testType);

            List<string> randomTestItemTitles = articleTableSteps.GetRandomNumberOfTestArticles(availableTestItems);

            return randomTestItemTitles;
        }

        public void PerformOperation(List<string> itemsToTest, AuthoringItemType itemType, AuthoringActionsMenuItem operationType, AuthoringTableTab tab)
        {
            articleTableSteps.SelectTab(tab);

            string err;

            if (itemType == AuthoringItemType.Article)
            {
                err = articleTableSteps.SelectArticles(itemsToTest, tab);
            }
            else
            {
                err = journeyTableSteps.SelectJourneys(itemsToTest, tab);
            }

            Assert.True(string.IsNullOrEmpty(err), err);

            err = articleTableSteps.CheckMenuItemPresent(operationType.ToString());

            Assert.True(string.IsNullOrEmpty(err), err);

            articleTableSteps.WaitAlertsToDisappear();

            articleTableSteps.SelectActionsMenuItem(operationType, tab);

            string itemName = itemType == AuthoringItemType.Article ? "article" : "advisor journey";

            err = articleTableSteps.CheckConfirmModal(operationType, itemName, itemsToTest.Count);

            Assert.True(string.IsNullOrEmpty(err), err);

            articleTableSteps.CompleteOperation(operationType);

            err = articleTableSteps.CheckAlert(operationType, itemType, itemsToTest.Count);

            Assert.True(string.IsNullOrEmpty(err), err);
        }

        public void PerformArticleOperation(List<string> articlesToTest, AuthoringActionsMenuItem operationType, AuthoringTableTab tab) =>
            PerformOperation(articlesToTest, AuthoringItemType.Article, operationType, tab);

        public void PerformJourneyOperation(List<string> articlesToTest, AuthoringActionsMenuItem operationType, AuthoringTableTab tab) =>
           PerformOperation(articlesToTest, AuthoringItemType.Journey, operationType, tab);
    }
}
