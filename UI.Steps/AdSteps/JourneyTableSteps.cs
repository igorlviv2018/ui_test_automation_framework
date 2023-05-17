using NLog;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.PageObjects.Authoring;
using Taf.UI.Core.Models.TafAuth;
using System.Collections.Generic;
using System.Linq;

namespace Taf.UI.Steps.TafAdSteps
{
    public class JourneyTableSteps : TableBaseSteps
    {
        private readonly JourneysTable journeysTable;

        public JourneyTableSteps(ILogger logger) : base(App.Taf, logger)
        {
            journeysTable = new JourneysTable();
        }

        public string OpenJourney(string title) => OpenItem(title, journeysTable);

        public void OpenJourneyPreview(string journeyTitle)
        {
            int rowPosition = FindItemRowPosition(journeyTitle, GetTitleColumn(journeysTable));

            if (rowPosition > -1)
            {
                journeysTable.ClickJourneyPreviewButton(rowPosition);
            }
        }

        public string GetRandomJourneyTitle()
        {
            List<string> titles = GetTitleColumn(journeysTable);

            string randomTitle = string.Empty;

            if (titles.Count > 0)
            {
                randomTitle = DataHelper.GetRandomElement(titles);
            }

            return randomTitle;
        }

        public string SelectJourneys(List<string> titles, AuthoringTableTab tab) => SelectItems(titles, tab, journeysTable);

        public bool IsJourneyPresent(string title) => IsItemPresent(title, journeysTable);

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

        public string CheckJourneysTableSearch(List<string> searchPhrases) => CheckItemsTableSearch(searchPhrases, journeysTable);

        public string CheckJourneysInTableTabsAfterArchiveOperation(List<string> titles) => CheckItemsInTableTabsAfterArchiveOperation(titles, journeysTable);

        public string CheckJourneysInTableTabsAfterRestoreOperation(List<string> titles) => CheckItemsInTableTabsAfterRestoreOperation(titles, journeysTable);

        public string CheckJourneysInTableTabsAfterDuplicateOperation(List<string> titles, AuthoringTableTab tab) =>
            CheckItemsInTableTabsAfterDuplicateOperation(titles, tab, journeysTable);

        public string CheckJourneysInTableTabsAfterDeleteOperation(List<string> titles) => CheckItemsInTableTabsAfterDeleteOperation(titles, journeysTable);

        public List<ArticleTableRow> GetRowsByTitles(List<string> titles, List<ArticleTableRow> rows) =>
            rows.Where(r => titles.Any(t => r.ArticleTitle.StartsWith(t))).ToList();
    }
}