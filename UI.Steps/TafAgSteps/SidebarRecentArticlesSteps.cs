using NLog;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models.Taf;
using Taf.UI.PageObjects.Taf;
using Taf.UI.Steps.TafSteps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Taf.UI.Steps
{
    // to delete - after Redesign is in prod
    public class SidebarRecentArticlesSteps// : BaseSteps
    {
        private readonly ArticlesPageSteps articlesPageSteps;

        private readonly Sidebar sidebar;

        private readonly ILogger log;

        private Queue<Article> ExpectedRecentArticles = new Queue<Article>();

        public SidebarRecentArticlesSteps(ILogger logger, bool isRedesign=false)
        {
            log = logger;

            articlesPageSteps = new ArticlesPageSteps(logger, isRedesign);

            sidebar = new Sidebar(isRedesign);
        }

        public void SelectRandomArticlesAndCheckRecentArticlesList(int randomArticlesCount, int recentArticlesListLength, bool isRedesign=false)
        {
            List<Article> randomArticles = articlesPageSteps.GetRandomArticlesList(randomArticlesCount, isRedesign);

            foreach (var randomArticle in randomArticles)
            {
                articlesPageSteps.OpenArticleUsingDeeplink(randomArticle.Link, isRedesign);

                log.Info($"Article opened: '{randomArticle.Title}', id: '{randomArticle.Id}'");

                AddToExpectedRecentArticles(randomArticle, recentArticlesListLength);

                string err = ValidateRecentArticlesList();

                if (!string.IsNullOrEmpty(err))
                {
                    throw new Exception(err);
                }
            }
        }

        public List<Article> SelectRandomArticles(int randomArticlesCount, int recentArticlesListLength, bool isRedesign=false)
        {
            List<Article> randomArticles = articlesPageSteps.GetRandomArticlesList(randomArticlesCount, isRedesign);

            foreach (var randomArticle in randomArticles)
            {
                articlesPageSteps.OpenArticleUsingDeeplink(randomArticle.Link, isRedesign);

                log.Info($"Article opened: '{randomArticle.Title}', id: '{randomArticle.Id}'");

                AddToExpectedRecentArticles(randomArticle, recentArticlesListLength);
            }

            return randomArticles;
        }

        public void AddToExpectedRecentArticles(Article article, int recentArticlesListLength = CommonConstants.RecentArticlesListDefaultMaxCount)
        {
            if (string.IsNullOrEmpty(article.Title) || string.IsNullOrEmpty(article.Link))
            {
                return;
            }

            if (!ExpectedRecentArticles.Where(a => a.Title == article.Title && a.Link == article.Link).Any())
            {
                ExpectedRecentArticles.Enqueue(article);
            }

            if (ExpectedRecentArticles.Count > recentArticlesListLength)
            {
                ExpectedRecentArticles.Dequeue();
            }
        }

        public void RemoveFromExpectedRecentArticles(Article articleToRemove)
        {
            Queue<Article> updatedQueue = new Queue<Article>();

            foreach (var article in ExpectedRecentArticles)
            {
                if (article != articleToRemove)
                {
                    updatedQueue.Enqueue(article);
                }
            }

            ExpectedRecentArticles = updatedQueue;
        }

        public void RemoveRandomArticleFromRecentArticlesList()
        {
            List<Article> articles = GetActualRecentArticles();

            Article articleToRemove = DataHelper.GetRandomElement(articles);

            if (articleToRemove == null)
            {
                return;
            }

            RemoveFromExpectedRecentArticles(articleToRemove);

            sidebar.RemoveRecentItemById(articleToRemove.Id);

            LogHelper.LogInfo(log, $"Article: {articleToRemove.Title}, id={articleToRemove.Id} removed from recent article list");
        }

        public string ValidateRecentArticlesList()
        {
            List<Article> actualArticlesList = GetActualRecentArticles();

            actualArticlesList.Reverse();

            List<Article> expectedArticlesList = ExpectedRecentArticles.ToList();

            string err;

            if (actualArticlesList.Count != expectedArticlesList.Count)
            {
                string actualArticles = string.Join(", ", actualArticlesList.Select(a => a.Title).ToList());

                string expectedArticles = string.Join(", ", expectedArticlesList.Select(a => a.Title).ToList());

                err = $"Count of recent articles is invalid: {actualArticlesList.Count} but expected - {expectedArticlesList.Count}. "
                    + $"Actual articles: {actualArticles}, Expected articles: {expectedArticles}.";

                LogHelper.LogError(log, err);

                return err;
            }

            List<string> errors = new List<string>();

            int actualArticlesCount = actualArticlesList.Count;

            for (int i = 0; i < actualArticlesCount; i++)
            {
                if (actualArticlesList[i] != expectedArticlesList[i])
                {
                    err = $"Position {actualArticlesCount - i} in 'Recent articles' menu: actual - {actualArticlesList[i]}, expected - {expectedArticlesList[i]}";

                    errors.Add(err);
                }
            }

            err = ErrorHelper.ConvertErrorsToString(errors);

            LogHelper.LogResult(log, "Recent articles list validated", err);

            return err;
        }

        public List<Article> GetActualRecentArticles()
        {
            int recentArticleCount = sidebar.GetRecentItemCount();

            List<string> titles = sidebar.GetRecentArticleTitles();

            List<string> links = sidebar.GetRecentArticleLinks();

            List<Article> articles = new List<Article>();

            for (int i = 0; i < recentArticleCount; i++)
            {
                articles.Add(new Article() { Title = titles[i], Link = DataHelper.GetArticleLinkPart(links[i]), Id = DataHelper.GetArticleId(links[i]) });
            }

            return articles;
        }

        public string ClearRecentArticleList()
        {
            ClearExpectedRecentArticlesList();

            string err = string.Empty;

            int count = sidebar.GetRemoveArticleButtonCount();

            while (count > 0)
            {
                sidebar.RemoveRecentArticle();

                // wait recent device count decreased
                bool isCountDecreasedByOne = UiWaitHelper.Wait(() => count - sidebar.GetRemoveArticleButtonCount() == 1, WaitConstants.ImplicitWaitInSec);

                if (!isCountDecreasedByOne)
                {
                    err = $"Item count did not decrease by 1 (expected: {count - 1 }, actual: {sidebar.GetRemoveArticleButtonCount()})";

                    break;
                }

                count = sidebar.GetRemoveArticleButtonCount();
            }

            return err;
        }

        public void ClearExpectedRecentArticlesList() => ExpectedRecentArticles.Clear();
    }
}
