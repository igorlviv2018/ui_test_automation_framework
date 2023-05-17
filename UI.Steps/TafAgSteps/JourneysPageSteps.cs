using NLog;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.PageObjects;
using Taf.UI.PageObjects.Taf;
using Taf.UI.Steps.JourneyPreviewSteps;
using System.Collections.Generic;

namespace Taf.UI.Steps.TafSteps
{
    public class JourneysPageSteps : TafTableBaseSteps
    {
        private readonly JourneysTable journeysTable;

        private readonly JourneyRatingTableSteps journeyRatingTableSteps;

        private readonly Spinner spinner;

        public JourneysPageSteps(ILogger logger) : base(logger)
        {
            journeysTable = new JourneysTable();

            journeyRatingTableSteps = new JourneyRatingTableSteps(log);

            spinner = new Spinner(App.Taf);
        }

        public string OpenJourney(string title) => OpenItemTaf(title, journeysTable);

        public void CloseJourneyInTaf()
        {
            journeysTable.ClickCloseButton();

            journeyRatingTableSteps.WaitTableIsDisplayed();

            journeyRatingTableSteps.WaitTableToRefresh();

            journeysTable.ClickCloseButton();

            spinner.WaitSpinnerToDisappear(SpinnerType.TopProgressBar);
        }

        public void CloseJourneyInPreview()
        { 
            //rewrite

            //journeysTable.ClickCloseButton();

            spinner.WaitSpinnerToDisappear(SpinnerType.TopProgressBar);
        }

        public void WaitJourneyToLoad()
        {
            spinner.WaitSpinnerToAppear(SpinnerType.TopProgressBar, WaitConstants.OneSecond);

            spinner.WaitSpinnerToDisappear(SpinnerType.TopProgressBar);
        }

        //public List<ArticleTableRow> ReadArticlesTable()
        //{
        //    List<ArticleTableRow> rows = new List<ArticleTableRow>();

        //    int rowsCount = journeysTable.GetRowCount();

        //    if (rowsCount == 0)
        //    {
        //        return rows;
        //    }

        //    List<string> titles = journeysTable.GetTitleColumn();

        //    //List<string> descriptions = articlesTable.GetArticleDescriptionColumn();

        //    for (int i = 0; i < rowsCount; i++)
        //    {
        //        ArticleTableRow row = new ArticleTableRow()
        //        {
        //            //IsArticleSelected = checkboxStatus[i],
        //            //ArticleType = CommonHelper.GetArticleType(rawTypes[i]),
        //            ArticleTitle = titles[i],
        //            //ArticleDescription = descriptions[i],
        //            //ArticleId = CommonHelper.GetIntegerInString(rawIds[i]),
        //        };

        //        rows.Add(row);
        //    }

        //    return rows;
        //}

        public string CheckJourneysNotPresentInJourneysTable(List<string> articlesToTest) =>
            CheckItemsNotPresentInItemsTable(articlesToTest, "Help me choose", journeysTable);

        public string CheckJourneysPresentInJourneysTable(List<string> articlesToTest) =>
            CheckItemsPresentInItemsTable(articlesToTest, "Help me choose", journeysTable);
    }
}
