using NLog;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Interfaces;
using System.Collections.Generic;

namespace Taf.UI.Steps.TafSteps
{
    public class TafTableBaseSteps : TableBaseSteps
    {
        private readonly BrowserSteps browserSteps;

        private readonly SidebarSteps sideMenuSteps;

        public TafTableBaseSteps(ILogger logger) : base(App.Taf, logger)
        {
            sideMenuSteps = new SidebarSteps(logger);

            browserSteps = new BrowserSteps(logger);
        }

        //public List<ArticleTableRow> ReadArticlesTable()
        //{
        //    List<ArticleTableRow> rows = new List<ArticleTableRow>();

        //    int rowsCount = articlesTable.GetRowCount();

        //    if (rowsCount == 0)
        //    {
        //        return rows;
        //    }

        //    List<string> titles = articlesTable.GetTitleColumn();

        //    //List<string> descriptions = articlesTable.GetArticleDescriptionColumn();

        //    for (int i = 0; i < rowsCount; i++)
        //    {
        //        ArticleTableRow row = new ArticleTableRow()
        //        {
        //            ArticleTitle = titles[i],
        //        };

        //        rows.Add(row);
        //    }

        //    return rows;
        //}

        public string CheckItemsNotPresentInItemsTable(List<string> itemsToTest, string pageName, ITafItemsTable itemsTable)
        {
            browserSteps.OpenAppDeepLink(App.Taf);

            sideMenuSteps.OpenPage(pageName);

            List<string> titlesColumn = itemsTable.GetTitleColumn();

            string err = CheckItemsNotPresentInTable(itemsToTest, titlesColumn);

            return err;
        }

        public string CheckItemsPresentInItemsTable(List<string> articlesToTest, string pageName, ITafItemsTable itemsTable)
        {
            browserSteps.OpenAppDeepLink(App.Taf);

            sideMenuSteps.OpenPage(pageName);

            List<string> titlesColumn = itemsTable.GetTitleColumn();

            string err = CheckItemsPresentInTable(articlesToTest, titlesColumn);

            return err;
        }
    }
}
