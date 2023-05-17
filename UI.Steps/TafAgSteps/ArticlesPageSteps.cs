using NLog;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models.Taf;
using Taf.UI.Core.Models.TafAuth;
using Taf.UI.PageObjects;
using Taf.UI.PageObjects.Taf;
using System;
using System.Collections.Generic;

namespace Taf.UI.Steps.TafSteps
{
    public class ArticlesPageSteps : TafTableBaseSteps
    {
        private readonly ArticlesTable articlesTable;

        private readonly ArticlesGrid articlesGrid;

        private readonly ArticlePage articlePage;

        private readonly SidebarSteps sidebarSteps;

        private readonly BrowserSteps browserSteps;

        private readonly Spinner spinner;

        private readonly Random random;

        public ArticlesPageSteps(ILogger logger, bool isRedesign=false) : base(logger)
        {
            articlesTable = new ArticlesTable();

            articlesGrid = new ArticlesGrid();

            articlePage = new ArticlePage();

            sidebarSteps = new SidebarSteps(logger, isRedesign);

            browserSteps = new BrowserSteps(logger);

            spinner = new Spinner(App.Taf, isRedesign);

            random = new Random();
        }

        public string OpenArticle(string title) => OpenItemTaf(title, articlesTable);

        //redesign
        public string OpenArticleFromGrid(string title)
        {
            string err = string.Empty;

            if (articlesGrid.IsItemPresent(title))
            {
                articlesGrid.ClickItemTitle(title);

                spinner.WaitTopProgressBarToDisappearRedesign();
            }
            else
            {
                err = $"Item (e.g article, journey) with '{title}' not present in the table";
            }

            return err;
        }

        //redesign
        public bool IsArticleOpened(string title) => articlePage.GetTitle() == title;

        public void CloseArticle()
        {
            articlesTable.ClickCloseButton();

            spinner.WaitSpinnerToDisappear(SpinnerType.TopProgressBar);
        }

        public List<ArticleTableRow> ReadArticlesTable()
        {
            List<ArticleTableRow> rows = new List<ArticleTableRow>();

            int rowsCount = articlesTable.GetRowCount();

            if (rowsCount == 0)
            {
                return rows;
            }

            List<string> titles = articlesTable.GetTitleColumn();

            //List<string> descriptions = articlesTable.GetArticleDescriptionColumn();

            for (int i = 0; i < rowsCount; i++)
            {
                ArticleTableRow row = new ArticleTableRow()
                {
                    //IsArticleSelected = checkboxStatus[i],
                    //ArticleType = CommonHelper.GetArticleType(rawTypes[i]),
                    ArticleTitle = titles[i],
                    //ArticleDescription = descriptions[i],
                    //ArticleId = CommonHelper.GetIntegerInString(rawIds[i]),
                    //ArticleStatus = statuses[i],
                    //ArticleVersion = versions[i],
                    //ArticleOwner = owners[i]
                };

                rows.Add(row);
            }

            return rows;
        }

        public string CheckArticlesNotPresentInArticlesTable(List<string> articlesToTest)
        {
            browserSteps.OpenAppDeepLink(App.Taf);

            sidebarSteps.OpenPage("Articles");

            List<string> titlesColumn = articlesTable.GetTitleColumn();

            string err = CheckItemsNotPresentInTable(articlesToTest, titlesColumn);

            return err;
        }

        public List<Article> GetRandomArticlesList(int numOfArticles, bool isRedesign=false)
        {
            sidebarSteps.OpenPage("Articles");

            List<string> titlesColumn = isRedesign 
                ? articlesGrid.GetFullTitlesInGrid() : articlesTable.GetTitleColumn();

            List<string> linkColumn = isRedesign
                ? articlesGrid.GetLinksInGrid() : articlesTable.GetLinkColumn();

            List<Article> articles = new List<Article>();

            if (titlesColumn.Count != linkColumn.Count)
            {
                return articles;
            }

            int articlesTotal = titlesColumn.Count;

            for (int i = 0; i < numOfArticles; i++)
            {
                int index = random.Next(articlesTotal);

                articles.Add(new Article() { Title = titlesColumn[index], Link = linkColumn[index], Id = DataHelper.GetArticleId(linkColumn[index]) });
            }

            return articles;
        }

        public List<ContentItem> GetRandomArticlesAsContentItems(int numOfArticles, bool isRedesign = false)
        {
            sidebarSteps.OpenPage("Articles");

            List<string> titlesColumn = isRedesign
                ? articlesGrid.GetFullTitlesInGrid() : articlesTable.GetTitleColumn();

            List<string> linkColumn = isRedesign
                ? articlesGrid.GetLinksInGrid() : articlesTable.GetLinkColumn();

            List<ContentItem> articles = new List<ContentItem>();

            if (titlesColumn.Count != linkColumn.Count)
            {
                return articles;
            }

            int articlesTotal = titlesColumn.Count;

            for (int i = 0; i < numOfArticles; i++)
            {
                int index = random.Next(articlesTotal);

                articles.Add(new ContentItem() 
                {
                    ContentItemType = ContentItemType.Article, 
                    Title = titlesColumn[index],
                    Link = linkColumn[index],
                    Id = DataHelper.GetArticleId(linkColumn[index])
                });
            }

            return articles;
        }

        public void OpenArticleUsingDeeplink(string articleLink, bool isRedesign=false) => browserSteps.OpenDeepLink(App.Taf, articleLink, isRedesign);

        public void OpenArticleByIdUsingDeeplink(string articleId, bool isRedesign=false) => 
            browserSteps.OpenDeepLink(App.Taf, string.Format(LinkConstants.TafArticleLink, articleId), isRedesign);
    }
}
