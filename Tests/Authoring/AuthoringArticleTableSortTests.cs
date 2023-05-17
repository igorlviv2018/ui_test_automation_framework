using NLog;
using Taf.UI.Steps;
using Taf.UI.Core.Enums;
using Taf.UI.Steps.Authoring;
using System.Collections.Generic;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models.TafAuth;
using Taf.UI.Steps.TafSteps;
using Xunit;

namespace Tests
{
    public class TafAuthArticleTableSortTests : TestBaseTaf, IClassFixture<TestFixture>
    {
        private readonly TestFixture fixture;

        private readonly LoginSteps loginSteps;

        private readonly BrowserSteps browserSteps;

        private readonly ArticleTableSteps articleTableSteps;

        private readonly ArticlesPageSteps agentsArticlesPageSteps;

        public TafAuthArticleTableSortTests(TestFixture fixture) : base(LogManager.GetLogger("TafAuthArticleOperationTestsUI"))
        {
            this.fixture = fixture;

            loginSteps = new LoginSteps(log);

            browserSteps = new BrowserSteps(log);

            articleTableSteps = new ArticleTableSteps(log);

            agentsArticlesPageSteps = new ArticlesPageSteps(log);

            //clientId = fixture.DbHelper.GetClientIdByName(clientName);
        }

        [Fact(DisplayName = "Sort Articles test")]
        public void SortArticlesTest()
        {
            //Arrange
            string testDescription = XUnitHelper.FactDisplayName();

            LogHelper.LogTestStart(log, testDescription);

            browserSteps.OpenAppDeepLink(App.Taf);

            string err = loginSteps.Login("aqa.sp.adm.prod@gmail.com");

            Assert.True(string.IsNullOrEmpty(err), err);

            browserSteps.OpenAppDeepLink(App.TafAuth);

            articleTableSteps.SortByColumn(AuthoringArticlesTableColumnName.Title, SortOrder.Ascending);

            //Act
            List<ArticleTableRow> rowsAfterSorting = articleTableSteps.ReadArticlesTable();
            
            //to debug - consult devs
            err = articleTableSteps.CheckArticlesTableColumnSorting(AuthoringArticlesTableColumnName.Title, SortOrder.Ascending, rowsAfterSorting);

            Assert.True(string.IsNullOrEmpty(err), err);

            //Assert
            Assert.True(string.IsNullOrEmpty(err), err);

            LogHelper.LogTestEnd(log, err, testDescription);
        }
    }
}
