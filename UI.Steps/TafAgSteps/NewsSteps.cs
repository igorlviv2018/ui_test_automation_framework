using NLog;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models.Taf;
using Taf.UI.PageObjects;
using Taf.UI.PageObjects.Taf;
using Taf.UI.PageObjects.Taf.DashboardPage;
using Taf.UI.PageObjects.Taf.NewsPage;
using System.Collections.Generic;
using System.Linq;

namespace Taf.UI.Steps
{
    public class NewsSteps : BaseSteps
    {
        private readonly DevicePage devicePage = new DevicePage();

        private readonly NewsPage newsPage = new NewsPage();

        private readonly NewsTable newsTable = new NewsTable();

        private readonly NewsBlock newsBlockOnDashboard = new NewsBlock();

        private readonly Sidebar sideMenu = new Sidebar();
        
        public NewsSteps(ILogger logger) : base(App.Taf, logger)
        { 
        
        }

        public void OpenDeviceFromNewsPage(string deviceId)
        {
            sideMenu.ClickMenuItem("News");

            newsPage.WaitPageLoad();

            if (!string.IsNullOrEmpty(deviceId))
            {
                newsPage.ClickDeviceNews(deviceId);

                devicePage.WaitPageLoad(deviceId);
            }
        }

        public void SelectTab(TafNewsTableTab tab)
        {
            string tabName = tab.ToString().ToLower();

            if (!newsTable.IsTabActive(tabName))
            {
                newsTable.ClickTab(tabName);

                LogHelper.LogInfo(log, $"Tab '{tab}' in 'News' table selected");
            }
        }

        /// <summary>
        /// Find News row position in the table
        /// </summary>
        /// <param name="itemTitle">article/device title</param>
        /// <param name="rows">news rows</param>
        /// <returns>0-based position of the first occurrence if found, otherwise -1</returns>
        public int FindNewsRowPosition(string itemTitle, List<NewsTableRow> rows) =>
            rows.FindIndex(x => x.ItemTitle.StartsWith(itemTitle));

        // to del as it is a slow reading method
        //public List<NewsTableRow> ReadNewsTable2()
        //{
        //    List<NewsTableRow> rows = new List<NewsTableRow>();
            
        //    int rowCount = newsTable.GetRowCount();

        //    for (int i = 0; i < rowCount; i++)
        //    {
        //        NewsTableRow row = new NewsTableRow()
        //        {
        //            NewsCaption = newsTable.GetCaption(i + 1),
        //            ItemTitle = newsTable.GetLinkText(i + 1),
        //            ReleaseNotes = newsTable.GetReleaseNotes(i + 1),
        //            Date = newsTable.GetDate(i + 1)
        //        };

        //        rows.Add(row);
        //    }

        //    return rows;
        //}

        public List<NewsTableRow> ReadNewsTable()
        {
            List<NewsTableRow> rows = new List<NewsTableRow>();

            int rowsCount = newsTable.GetRowCount();

            if (rowsCount == 0)
            {
                return rows;
            }

            List<string> newsCaptions = newsTable.GetNewsCaptionColumn();

            List<string> itemTitles = newsTable.GetLinkTextColumn();

            List<string> newsDates = newsTable.GetDateColumn();

            for (int i = 0; i < rowsCount; i++)
            {
                NewsTableRow row = new NewsTableRow()
                {
                    NewsCaption = newsCaptions[i],
                    ItemTitle = itemTitles[i],
                    Date = newsDates[i]
                };

                rows.Add(row);
            }

            return rows;
        }

        public string CheckNewsPresenceInTable(List<string> titles, List<NewsTableRow> rows, bool shouldBePresent)
        {
            List<string> actualTitles = rows.Select(r => r.ItemTitle).ToList();

            List<string> unexpectedItemTitles = shouldBePresent
                ? titles.Where(t => !actualTitles.Any(x => x.StartsWith(t))).ToList()
                : titles.Where(t => actualTitles.Any(x => x.StartsWith(t))).ToList();

            string err = string.Empty;

            if (unexpectedItemTitles.Count > 0)
            {
                err = string.Join(", ", unexpectedItemTitles);
            }

            string errPrefix = shouldBePresent
                ? "News are not present in table: "
                : "Found news that should not be present in table: ";

            err = ErrorHelper.AddPrefixToError(err, errPrefix);

            LogHelper.LogResult(log, $"Checked expected presence of news in table", err);

            return err;
        }

        public string CheckNewsInTable(List<string> titles, TafNewsOnItem itemType, string expectedReleaseNotes, bool shouldBePresent)
        {
            List<string> errors = new List<string>();

            bool shouldBePresentInArticlesTab = shouldBePresent && itemType == TafNewsOnItem.Article;

            bool shouldBePresentInDevicesTab = shouldBePresent && itemType == TafNewsOnItem.Device;

            //bool shouldBePresentInAllTab = shouldBePresent;

            //SelectTab(TafNewsTableTab.Articles);

            string err = CheckNewsInTableTab(titles, itemType, expectedReleaseNotes, TafNewsTableTab.Articles, shouldBePresentInArticlesTab);

            errors.Add(err);

            //List<NewsTableRow> actualRows = ReadNewsTable();

            //string err = CheckNewsPresenceInTable(titles, actualRows, shouldBePresentInArticlesTab);

            //err = ErrorHelper.AddPrefixToError(err, $"News tab 'Articles': ");

            //errors.Add(err);

            //if (string.IsNullOrEmpty(err) && shouldBePresentInArticlesTab) // check news contents
            //{
            //    err = CheckNews(titles, itemType, expectedReleaseNotes, actualRows);

            //    err = ErrorHelper.AddPrefixToError(err, $"News tab 'Articles': ");

            //    ErrorHelper.AddToErrorList(errors, err);
            //}

            //SelectTab(TafNewsTableTab.Devices);

            err = CheckNewsInTableTab(titles, itemType, expectedReleaseNotes, TafNewsTableTab.Devices, shouldBePresentInDevicesTab);

            errors.Add(err);

            //actualRows = ReadNewsTable();

            //err = CheckNewsPresenceInTable(titles, actualRows, shouldBePresentInDevicesTab);

            //err = ErrorHelper.AddPrefixToError(err, $"News tab 'Devices': ");

            //errors.Add(err);

            //if (string.IsNullOrEmpty(err) && shouldBePresentInDevicesTab) // check news contents
            //{
            //    err = CheckNews(titles, itemType, expectedReleaseNotes, actualRows);

            //    err = ErrorHelper.AddPrefixToError(err, $"News tab 'Devices': ");

            //    ErrorHelper.AddToErrorList(errors, err);
            //}

            //SelectTab(TafNewsTableTab.All);

            err = CheckNewsInTableTab(titles, itemType, expectedReleaseNotes, TafNewsTableTab.All, shouldBePresent);

            errors.Add(err);

            //actualRows = ReadNewsTable();

            //err = CheckNewsPresenceInTable(titles, actualRows, shouldBePresent);

            //err = ErrorHelper.AddPrefixToError(err, $"News tab 'All': ");

            //errors.Add(err);

            //if (string.IsNullOrEmpty(err) && shouldBePresent) // check news contents
            //{
            //    err = CheckNews(titles, itemType, expectedReleaseNotes, actualRows);

            //    err = ErrorHelper.AddPrefixToError(err, $"News tab 'All': ");

            //    ErrorHelper.AddToErrorList(errors, err);
            //}

            err = ErrorHelper.ConvertErrorsToString(errors);

            LogHelper.LogResult(log, $"Checked expected news in table", err);

            return err;
        }

        public string CheckNewsInTableTab(List<string> titles, TafNewsOnItem itemType, string expectedReleaseNotes, TafNewsTableTab tab, bool shouldBePresentInTab)
        {
            bool isTabPresent = newsTable.IsTabPresent(tab.ToString().ToLower());

            if (!isTabPresent)
            {
                return shouldBePresentInTab ? $"News tab '{tab}': news are not present: {string.Join(", ", titles)}": string.Empty;
            }

            SelectTab(tab);

            List<NewsTableRow> actualRows = ReadNewsTable();

            string err = CheckNewsPresenceInTable(titles, actualRows, shouldBePresentInTab);

            err = ErrorHelper.AddPrefixToError(err, $"News tab '{tab}': ");

            List<string> errors = new List<string>() { err };

            if (string.IsNullOrEmpty(err) && shouldBePresentInTab) // check news contents
            {
                err = CheckNews(titles, itemType, expectedReleaseNotes, actualRows);

                err = ErrorHelper.AddPrefixToError(err, $"News tab '{tab}': ");

                ErrorHelper.AddToErrorList(errors, err);
            }

            return ErrorHelper.ConvertErrorsToString(errors);
        }

        public string CheckNewsInDashboard(List<string> titles, TafNewsOnItem itemType, string expectedReleaseNotes, bool shouldBePresent)
        {
            List<string> errors = new List<string>();

            string err;

            if (shouldBePresent)
            {
                foreach (var title in titles)
                {
                    if (newsBlockOnDashboard.IsNewsPresent(title))
                    {
                        // check caption
                        string actualCaption = newsBlockOnDashboard.GetNewsCaption(title);

                        string expectedCaption = string.Format(MessageConstants.TafNewsCaption, itemType.ToString().ToLower());

                        if (!actualCaption.StartsWith(expectedCaption))
                        {
                            err = $"Invalid actual caption: {actualCaption} (expected the caption to start with: {expectedCaption})";

                            ErrorHelper.AddToErrorList(errors, err, $"News about '{title}': ");
                        }

                        // release notes
                        string actualReleaseNotes = newsBlockOnDashboard.GetReleaseNotes(title);

                        if (actualReleaseNotes != expectedReleaseNotes)
                        {
                            err = $"Invalid release notes: {actualReleaseNotes} (expected: {expectedReleaseNotes})";

                            ErrorHelper.AddToErrorList(errors, err, $"News about '{title}': ");
                        }
                    }
                    else
                    {
                        err = $"News about '{title}' {itemType} not present on the Dashboard";

                        errors.Add(err);
                    }
                }
            }

            if (!shouldBePresent)
            {
                foreach (var title in titles)
                {
                    if (newsBlockOnDashboard.IsNewsPresent(title))
                    {
                        err = $"News about '{title}' {itemType} present on the Dashboard (it should not be present)";

                        errors.Add(err);
                    }
                }
            }

            return ErrorHelper.ConvertErrorsToString(errors, "News check on dashboard (Home page) failed: ");
        }

        public string CheckNews(string itemTitle, TafNewsOnItem itemType, string expectedReleaseNotes, List<NewsTableRow> actualNewsRows)
        {
            string err = string.Empty;

            List<string> errors = new List<string>();

            int actualNewsRowNumber = FindNewsRowPosition(itemTitle, actualNewsRows);

            if (actualNewsRowNumber > -1)
            {
                NewsTableRow actualNewsRow = actualNewsRows[actualNewsRowNumber];

                // check caption
                string expectedCaption = string.Format(MessageConstants.TafNewsCaption, itemType.ToString().ToLower());

                if (actualNewsRow.NewsCaption != expectedCaption)
                {
                    errors.Add($"Invalid news caption: {actualNewsRow.NewsCaption}, expected: {expectedCaption}");
                }

                // check release notes
                string actualReleaseNotes = newsTable.GetReleaseNotes(actualNewsRowNumber + 1);

                if (actualReleaseNotes != expectedReleaseNotes)
                {
                    errors.Add($"Invalid release notes: {actualReleaseNotes}, expected: {expectedReleaseNotes}");
                }

                // check link? open item from news
                // -------

            }
            else
            {
                errors.Add("not present in news table");
            }

            err = ErrorHelper.ConvertErrorsToString(errors, $"News about '{itemTitle}' {itemType.ToString().ToLower()}: ");

            LogHelper.LogResult(log, $"Checked expected news content in news table", err);

            return err;
        }

        public string CheckNews(List<string> itemTitles, TafNewsOnItem itemType, string expectedReleaseNotes, List<NewsTableRow> actualNewsRows)
        {
            string err;

            List<string> errors = new List<string>();

            foreach (var title in itemTitles)
            {
                err = CheckNews(title, itemType, expectedReleaseNotes, actualNewsRows);

                ErrorHelper.AddToErrorList(errors, err);
            }

            return ErrorHelper.ConvertErrorsToString(errors);
        }

        //public string CheckArticlesPresentInTable(List<string> titles, List<ArticleTableRow> rows)
        //{
        //    List<string> actualTitles = rows.Select(r => r.ArticleTitle).ToList();

        //    List<string> missingArticles = titles.Where(t => !actualTitles.Any(x => x.StartsWith(t))).ToList();

        //    string err = string.Empty;

        //    if (missingArticles.Count > 0)
        //    {
        //        err = string.Join(", ", missingArticles);
        //    }

        //    err = ErrorHelper.AddPrefixToError(err, "Articles are not present in table: ");

        //    LogHelper.LogResult(log, $"Checked presense of articles in table", err);

        //    return err;
        //}
    }
}
