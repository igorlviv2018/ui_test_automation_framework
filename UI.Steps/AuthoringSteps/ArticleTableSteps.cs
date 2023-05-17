using NLog;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.PageObjects.Authoring;
using Taf.UI.Core.Models.TafAuth;
using System.Collections.Generic;
using System.Linq;

namespace Taf.UI.Steps.Authoring
{
    public class ArticleTableSteps : TableBaseSteps
    {
        public ArticleTableSteps(ILogger logger) : base(App.Taf, logger)
        {

        }

        private readonly ArticlesTable articlesTable = new ArticlesTable();

        public string OpenArticle(string articleTitle) => OpenItem(articleTitle, articlesTable);

        public string SelectArticles(List<string> titles, AuthoringTableTab tab) => SelectItems(titles, tab, articlesTable);

        public List<string> GetRandomNumberOfTestArticles(List<string> availableTestArticleTitles)
        {
            List<string> articlesToSelect = new List<string>();

            string err = string.Empty;

            if (availableTestArticleTitles.Count > 0)
            {
                articlesToSelect = DataHelper.GetRandomSubList(availableTestArticleTitles);
            }
            else
            { 
                err = "No previously created test articles found";
            }

            LogHelper.LogResult(log, $"Articles randomly selected from available test articles: {string.Join(", ", articlesToSelect)}", err);

            return articlesToSelect;
        }

        public string CheckArticlesInTableTabsAfterArchiveOperation(List<string> titles) => CheckItemsInTableTabsAfterArchiveOperation(titles, articlesTable);

        public string CheckArticlesInTableTabsAfterRestoreOperation(List<string> titles) => CheckItemsInTableTabsAfterRestoreOperation(titles, articlesTable);

        public string CheckArticlesInTableTabsAfterDuplicateOperation(List<string> titles, AuthoringTableTab tab) =>
            CheckItemsInTableTabsAfterDuplicateOperation(titles, tab, articlesTable);

        public string CheckArticlesInTableTabsAfterDeleteOperation(List<string> titles) => CheckItemsInTableTabsAfterDeleteOperation(titles, articlesTable);

        //public List<string> GetDuplicatedTestArticlesExpectedTitles(List<string> testArticleTitles)
        //{
        //    List<string> duplicatedArticleTitles = testArticleTitles
        //        .Select(x => $"Copy of {x}")
        //        .ToList();

        //    return duplicatedArticleTitles;
        //}

        public int FindArticleRowPosition(string articleTitle, List<ArticleTableRow> rows) =>
            rows.FindIndex(x => x.ArticleTitle.StartsWith(articleTitle));

        /// <summary>
        /// Find Article row position in the table
        /// </summary>
        /// <param name="articleTitle"></param>
        /// <param name="titlesColumn"></param>
        /// <returns>0-based position of the first occurrence if found, otherwise -1</returns>
        public int FindArticleRowPosition(string articleTitle, List<string> titlesColumn) => FindItemRowPosition(articleTitle, titlesColumn);

        public List<string> GetTitleColumn() => articlesTable.GetTitleColumn();

        public List<ArticleTableRow> GetRowsByTitles(List<string> titles, List<ArticleTableRow> rows) =>
            rows.Where(r => titles.Any(t => r.ArticleTitle.StartsWith(t))).ToList();

        public List<ArticleTableRow> ReadArticlesTable()
        {
            List<ArticleTableRow> rows = new List<ArticleTableRow>();

            int rowsCount = articlesTable.GetRowCount();

            if (rowsCount == 0)
            {
                return rows;
            }

            List<bool> checkboxStatus = articlesTable.GetItemCheckboxColumn();

            List<string> rawTypes = articlesTable.GetTypeColumn();

            List<string> titles = articlesTable.GetTitleColumn();

            List<string> descriptions = articlesTable.GetItemDescriptionColumn();

            List<string> rawIds = articlesTable.GetArticleIdColumn();

            List<string> statuses = articlesTable.GetItemStatusColumn();

            List<string> versions = articlesTable.GetVersionColumn();

            List<string> owners = articlesTable.GetOwnerColumn();

            for (int i = 0; i < rowsCount; i++)
            {
                ArticleTableRow row = new ArticleTableRow()
                {
                    IsArticleSelected = checkboxStatus[i],
                    ArticleType = CommonHelper.GetArticleType(rawTypes[i]),
                    ArticleTitle = titles[i],
                    ArticleDescription = descriptions[i],
                    ArticleId = CommonHelper.GetIntegerInString(rawIds[i]),
                    ArticleStatus = CommonHelper.GetArticleStatus(statuses[i]),
                    ArticleVersion = versions[i],
                    ArticleOwner = owners[i]
                };

                rows.Add(row);
            }

            return rows;
        }

        public string CheckArticlesTableColumnSorting(AuthoringArticlesTableColumnName columnName, SortOrder sortOrder, List<ArticleTableRow> actualRowsAfterSorting)
        {
            string err = string.Empty;

            if (columnName == AuthoringArticlesTableColumnName.Title)
            {
                List<string> actualSortedColumn = actualRowsAfterSorting.Select(r => r.ArticleTitle).ToList();

                List<string> expectedSortedColumn = new List<string>();

                if (sortOrder == SortOrder.Ascending)
                {
                    expectedSortedColumn = actualRowsAfterSorting.OrderBy(x => x.ArticleTitle).Select(r => r.ArticleTitle).ToList();
                }
                else
                {
                    expectedSortedColumn = actualRowsAfterSorting.OrderByDescending(x => x.ArticleTitle).Select(r => r.ArticleTitle).ToList();
                }

                err = DataHelper.CompareObjects(actualSortedColumn, expectedSortedColumn);
            }

            LogHelper.LogResult(log, $"Checked articles sorting in table", err);

            return err;
        }

        public string CheckArticlesTableSearch(List<string> searchPhrases) => CheckItemsTableSearch(searchPhrases, articlesTable);
    }
}