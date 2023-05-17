using NLog;
using Taf.UI.Steps;
using Taf.UI.Core.Enums;
using Taf.UI.Steps.Authoring;
using System.Collections.Generic;
using Taf.UI.Core.Helpers;
using Taf.UI.Steps.TafSteps;
using Taf.UI.Core.Models;
using Xunit;

namespace Tests
{
    public class TafAuthArticleOperationTests : TestBaseTaf, IClassFixture<TestFixture>
    {
        private readonly TestFixture fixture;

        private readonly LoginSteps loginSteps;

        private readonly AppsMenuSteps appsMenuSteps;

        private readonly BrowserSteps browserSteps;

        private readonly ArticleTableSteps articleTableSteps;

        private readonly ArticlesPageSteps agentsArticlesPageSteps;

        private readonly ProfileMenuSteps profileMenuSteps;

        private readonly string clientId;

        public TafAuthArticleOperationTests(TestFixture fixture) : base(LogManager.GetLogger("TafAuthArticleOperationTestsUI"))
        {
            this.fixture = fixture;

            loginSteps = new LoginSteps(log);

            appsMenuSteps = new AppsMenuSteps(log);

            browserSteps = new BrowserSteps(log);

            articleTableSteps = new ArticleTableSteps(log);

            agentsArticlesPageSteps = new ArticlesPageSteps(log);

            profileMenuSteps = new ProfileMenuSteps(log);

            clientId = "VF UK (AQA)";

            //clientId = fixture.DbHelper.GetClientIdByName(clientName);
        }

        [Fact(DisplayName = "Archive/restore articles(s) test")]
        public void ArchiveRestoreArticlesTest()
        {
            //Arrange
            string testDescription = XUnitHelper.FactDisplayName();

            LogHelper.LogTestStart(log, testDescription);

            //loginSteps.OpenApp(App.TafAuth, fixture.TestEnvironment);

            User user = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment, 6, clientId);

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            browserSteps.OpenAppDeepLink(App.TafAuth);

            List<string> testArticles = GetRandomNumberOfTestItems(TestType.ArchiveRestoreArticle);

            //Act
            PerformArticleOperation(testArticles, AuthoringActionsMenuItem.Archive, AuthoringTableTab.Active);

            err = articleTableSteps.CheckArticlesInTableTabsAfterArchiveOperation(testArticles);

            Assert.True(string.IsNullOrEmpty(err), err);

            err = agentsArticlesPageSteps.CheckArticlesNotPresentInArticlesTable(testArticles);

            Assert.True(string.IsNullOrEmpty(err), err);

            browserSteps.OpenAppDeepLink(App.TafAuth);

            PerformArticleOperation(testArticles, AuthoringActionsMenuItem.Restore, AuthoringTableTab.Archived);

            err = articleTableSteps.CheckArticlesInTableTabsAfterRestoreOperation(testArticles);

            Assert.True(string.IsNullOrEmpty(err), err);

            err = agentsArticlesPageSteps.CheckArticlesNotPresentInArticlesTable(testArticles);

            //Assert
            Assert.True(string.IsNullOrEmpty(err), err);

            LogHelper.LogTestEnd(log, err, testDescription);
        }

        [Fact(DisplayName = "Duplicate active articles(s) test")]
        public void DuplicateActiveArticlesTest()
        {
            //Arrange
            string testDescription = XUnitHelper.FactDisplayName();

            LogHelper.LogTestStart(log, testDescription);

            //loginSteps.OpenApp(App.TafAuth, fixture.TestEnvironment);

            User user = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment, 6, clientId);

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            browserSteps.OpenAppDeepLink(App.TafAuth);

            List<string> testArticles = GetRandomNumberOfTestItems(TestType.DuplicateActiveArticle);

            //Act
            PerformArticleOperation(testArticles, AuthoringActionsMenuItem.Duplicate, AuthoringTableTab.Active);

            err = articleTableSteps.CheckArticlesInTableTabsAfterDuplicateOperation(testArticles, AuthoringTableTab.Active);

            //Assert
            Assert.True(string.IsNullOrEmpty(err), err);

            LogHelper.LogTestEnd(log, err, testDescription);
        }

        [Fact(DisplayName = "Duplicate archived articles(s) test")]
        public void DuplicateArchivedArticlesTest()
        {
            //Arrange
            string testDescription = XUnitHelper.FactDisplayName();

            LogHelper.LogTestStart(log, testDescription);

            //loginSteps.OpenApp(App.TafAuth, fixture.TestEnvironment);

            User user = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment, 6, clientId);

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            browserSteps.OpenAppDeepLink(App.TafAuth);

            List<string> testArticles = GetRandomNumberOfTestItems(TestType.DuplicateArchivedArticle);

            PerformArticleOperation(testArticles, AuthoringActionsMenuItem.Archive, AuthoringTableTab.Active);

            //Act
            PerformArticleOperation(testArticles, AuthoringActionsMenuItem.Duplicate, AuthoringTableTab.Archived);

            err = articleTableSteps.CheckArticlesInTableTabsAfterDuplicateOperation(testArticles, AuthoringTableTab.Archived);

            //Assert
            Assert.True(string.IsNullOrEmpty(err), err);

            LogHelper.LogTestEnd(log, err, testDescription);
        }

        [Fact(DisplayName = "Delete article(s) (as Admin user) test")]
        public void DeleteArticlesTest()
        {
            //Arrange
            string testDescription = XUnitHelper.FactDisplayName();

            LogHelper.LogTestStart(log, testDescription);

            //loginSteps.OpenApp(App.TafAuth, fixture.TestEnvironment);

            profileMenuSteps.TrySignOut();

            //change to admin
            User user = UserHelper.GetAdminUser(fixture.TestUsers, fixture.TestEnvironment, 2, clientId);

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            browserSteps.OpenAppDeepLink(App.TafAuth);

            List<string> testArticles = GetRandomNumberOfTestItems(TestType.DeleteArticle);

            PerformArticleOperation(testArticles, AuthoringActionsMenuItem.Archive, AuthoringTableTab.Active);

            //Act
            PerformArticleOperation(testArticles, AuthoringActionsMenuItem.Delete, AuthoringTableTab.Archived);

            err = articleTableSteps.CheckArticlesInTableTabsAfterDeleteOperation(testArticles);

            Assert.True(string.IsNullOrEmpty(err), err);

            err = agentsArticlesPageSteps.CheckArticlesNotPresentInArticlesTable(testArticles);

            //Assert
            Assert.True(string.IsNullOrEmpty(err), err);

            LogHelper.LogTestEnd(log, err, testDescription);
        }

        [Fact(DisplayName = "Search Articles test")]
        public void SearchArticlesTest()
        {
            //Arrange
            string testDescription = XUnitHelper.FactDisplayName();

            LogHelper.LogTestStart(log, testDescription);

            //loginSteps.OpenApp(App.TafAuth, fixture.TestEnvironment);

            User user = UserHelper.GetContentAuthorUser(fixture.TestUsers, fixture.TestEnvironment, 6, clientId);

            string err = loginSteps.Login(user);

            Assert.True(string.IsNullOrEmpty(err), err);

            browserSteps.OpenAppDeepLink(App.TafAuth);

            //Act
            err = articleTableSteps.CheckArticlesTableSearch(new List<string>() { "000", "search_test_" }); // "000", rowsBeforeSearch);

            //Assert
            Assert.True(string.IsNullOrEmpty(err), err);

            LogHelper.LogTestEnd(log, err, testDescription);
        }
    }
}
