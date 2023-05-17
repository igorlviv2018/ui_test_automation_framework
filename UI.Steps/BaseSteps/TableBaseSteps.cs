using NLog;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Interfaces;
using Taf.UI.PageObjects;
using Taf.UI.PageObjects.Authoring;
using Taf.UI.PageObjects.CommonPages.Authoring;
using System.Collections.Generic;
using System.Linq;

namespace Taf.UI.Steps
{
    public class TableBaseSteps : BaseSteps
    {
        public TableBaseSteps(App app, ILogger logger) : base(app, logger)
        {

        }

        private readonly ItemsTable itemsTable = new ItemsTable();

        private readonly ConfirmModal confirmModal = new ConfirmModal();

        private readonly Spinner spinner = new Spinner(App.Taf);

        private readonly SearchBlock searchBlock = new SearchBlock();

        private readonly ToastAlertSteps toastAlertSteps = new ToastAlertSteps();

        public string OpenItem(string title, IItemsTable itemsTable)
        {
            string err = string.Empty;

            if (itemsTable.IsItemPresent(title))
            {
                itemsTable.ClickItemTitle(title);

                SpinnerType spinnerType = app == App.Taf ? SpinnerType.TopProgressBar : SpinnerType.ArticleLoading;

                spinner.WaitSpinnerToDisappear(spinnerType);
            }
            else
            {
                err = $"Item (e.g article, journey) with '{title}' not present in the table";
            }

            return err;
        }

        public string OpenItemTaf(string title, ITafItemsTable itemsTable)
        {
            string err = string.Empty;

            if (itemsTable.IsItemPresent(title))
            {
                itemsTable.ClickItemTitle(title);

                spinner.WaitSpinnerToDisappear(SpinnerType.TopProgressBar);
            }
            else
            {
                err = $"Item (e.g article, journey) with '{title}' not present in the table";
            }

            return err;
        }

        public void SelectTab(AuthoringTableTab tab)
        {
            if (!itemsTable.IsTabActive(tab.ToString()))
            {
                itemsTable.ClickTab(tab.ToString());

                LogHelper.LogInfo(log, $"Tab '{tab}' (in items table) selected");

                spinner.WaitTopProgressBarToDisappear(WaitConstants.OneSecond);
            }
        }

        public void SelectActionsMenuItem(AuthoringActionsMenuItem menuItem, AuthoringTableTab tab)
        {
            if (tab == AuthoringTableTab.Active && menuItem != AuthoringActionsMenuItem.Delete
                || tab == AuthoringTableTab.Archived && menuItem != AuthoringActionsMenuItem.Archive)
            {
                itemsTable.OpenActionsDropdown();

                itemsTable.SelectActionsMenuItem(menuItem.ToString());

                LogHelper.LogInfo(log, $"Menu item '{menuItem}' selected (tab: {tab})");
            }
        }

        public string CheckMenuItemPresent(string menuItemName)
        {
            string err = string.Empty;

            if (!itemsTable.IsMenuItemPresent(menuItemName))
            {
                err = $"Dropdown '{menuItemName}' menu item not present";
            }

            LogHelper.LogResult(log, $"Menu item '{menuItemName}' present", err);

            return err;
        }

        public void WaitAlertsToDisappear() => toastAlertSteps.WaitAlertToDisappear();

        public string CheckAlert(AuthoringActionsMenuItem operation, AuthoringItemType itemType, int numOfArticles)
        {
            string expectedAlertMessage = GetExpectedAlertMessage(operation, itemType, numOfArticles);

            string err = toastAlertSteps.CheckAlertPopup(AlertStatus.Success, expectedAlertMessage);

            LogHelper.LogResult(log, $"'{operation}' operation completed", err);

            return ErrorHelper.AddPrefixToError(err, $"Operation '{operation}' alert check failed: ");
        }

        public string CheckConfirmModal(AuthoringActionsMenuItem operation, string itemName, int numberOfSelectedItems)
        {
            if (!confirmModal.WaitModalAppeared())
            {
                return "Operation confirm modal did not appear";
            }

            string ending = numberOfSelectedItems > 1 ? "s" : string.Empty;

            string expectedTitle = $"{operation} {itemName}{ending}?";

            string expectedMessage = string.Format(MessageConstants.AuthoringConfirmOperationModalMessage,
                operation.ToString().ToLower(), numberOfSelectedItems, itemName, ending);

            string actualTitle = confirmModal.GetTitle();

            string actualMessage = confirmModal.GetMessage();

            string err = string.Empty;

            if (actualTitle != expectedTitle)
            {
                err = $"Invalid title: {actualTitle} (expected: {expectedTitle});";
            }

            if (actualMessage != expectedMessage)
            {
                err = $"{err}Invalid message: {actualMessage} (expected: {expectedMessage});";
            }

            LogHelper.LogResult(log, $"Modal for '{operation}' operation checked", err);

            return ErrorHelper.AddPrefixToError(err, $"Confirm modal: ");
        }

        public void CompleteOperation(AuthoringActionsMenuItem operation)
        {
            confirmModal.ClickButton(operation.ToString());

            //if (!confirmModal.WaitModalDisappeared()) //workaruond as sometimes button click is not handled
            //{
            //    confirmModal.ClickButton(operation.ToString());
            //    LogHelper.LogInfo(log, $"'{operation}' button clicked second time (as first click was not handled)");
            //}

            spinner.WaitTopProgressBarToDisappear(WaitConstants.TwoSeconds);
        }

        public string SelectItem(string title, List<string> titlesColumn) // int rowPosition)
        {
            string err = string.Empty;

            int rowPosition = FindItemRowPosition(title, titlesColumn);

            if (rowPosition > -1)
            {
                itemsTable.CheckRowCheckbox(rowPosition + 1);
            }
            else
            {
                err = $"Item with '{title}' not found in the table";
            }

            return err;
        }

        public string SelectItems(List<string> titles, List<string> titlesColumn)
        {
            List<string> errors = new List<string>();

            if (titles.Count > 0)
            {
                foreach (var title in titles)
                {
                    errors.Add(SelectItem(title, titlesColumn));
                }
            }
            else
            {
                errors.Add("No previously created test items found");
            }

            string err = ErrorHelper.ConvertErrorsToString(errors, "Selecting items failed: ");

            LogHelper.LogResult(log, $"Items selected (in table): {string.Join(", ", titles)}", err);

            return err;
        }

        public string SelectItems(List<string> titles, AuthoringTableTab tab, IItemsTable itemsTable)
        {
            SelectTab(tab);

            List<string> titlesColumn = itemsTable.GetTitleColumn();

            string err = SelectItems(titles, titlesColumn);

            return err;
        }

        /// <summary>
        /// Find Item row position in the table
        /// </summary>
        /// <param name="itemTitle"></param>
        /// <param name="allTitlesInTable">all Titles In Table</param>
        /// <returns>0-based position of the first occurrence if found, otherwise -1</returns>
        public int FindItemRowPosition(string itemTitle, List<string> titlesColumn) =>
            titlesColumn.FindIndex(x => x.StartsWith(itemTitle));

        public List<int> FindItemRowPositions(List<string> itemTitles, List<string> titlesColumn) =>
            itemTitles.Select(t => FindItemRowPosition(t, titlesColumn)).ToList();

        public bool IsItemPresent(string itemTitle, IItemsTable itemsTable) =>
            FindItemRowPosition(itemTitle, GetTitleColumn(itemsTable)) > -1;

        public void SortByColumn(AuthoringArticlesTableColumnName columnName, SortOrder sortOrder)
        {
            SortOrder currentOrder = CommonHelper.GetSortOrder(itemsTable.GetHeaderColumnSorting(columnName.ToString()));

            for (int i = 0; i < 2; i++)
            {
                if (currentOrder != sortOrder && (sortOrder != SortOrder.None))
                {
                    itemsTable.ClickHeaderColumnName(columnName.ToString());

                    currentOrder = CommonHelper.GetSortOrder(itemsTable.GetHeaderColumnSorting(columnName.ToString()));
                }
            }
        }

        public List<string> GetTitleColumn(IItemsTable itemsTable) => itemsTable.GetTitleColumn();

        public string CheckItemsPresenceInTab(AuthoringTableTab tab, List<string> titles, bool shouldBePresent, IItemsTable itemsTable)
        {
            SelectTab(tab);

            List<string> titlesColumn = GetTitleColumn(itemsTable);

            string err = shouldBePresent
                ? CheckItemsPresentInTable(titles, titlesColumn)
                : CheckItemsNotPresentInTable(titles, titlesColumn);

            return ErrorHelper.AddPrefixToError(err, $"{tab} items tab check failed: ");
        }

        public string CheckItemsInTableTabsAfterArchiveOperation(List<string> titles, IItemsTable itemsTable)
        {
            List<string> errors = new List<string>();

            AuthoringTableTab tab = AuthoringTableTab.Active;

            string err = CheckItemsPresenceInTab(tab, titles, false, itemsTable);

            ErrorHelper.AddToErrorList(errors, err);

            tab = AuthoringTableTab.Archived;

            err = CheckItemsPresenceInTab(tab, titles, shouldBePresent: true, itemsTable);

            ErrorHelper.AddToErrorList(errors, err);

            err = CheckItemsStatus(tab, titles, ArticleStatus.Archived, itemsTable);

            ErrorHelper.AddToErrorList(errors, err);

            return ErrorHelper.ConvertErrorsToString(errors, $"Items check after 'Archive' operation failed: ");
        }

        public string CheckItemsInTableTabsAfterRestoreOperation(List<string> titles, IItemsTable itemsTable)
        {
            List<string> errors = new List<string>();

            AuthoringTableTab tab = AuthoringTableTab.Archived;

            string err = CheckItemsPresenceInTab(tab, titles, false, itemsTable);

            ErrorHelper.AddToErrorList(errors, err);

            tab = AuthoringTableTab.Active;

            err = CheckItemsPresenceInTab(tab, titles, shouldBePresent: true, itemsTable);

            ErrorHelper.AddToErrorList(errors, err);

            err = CheckItemsStatus(tab, titles, ArticleStatus.Draft, itemsTable);

            ErrorHelper.AddToErrorList(errors, err);

            return ErrorHelper.ConvertErrorsToString(errors, $"Items check after 'Restore' operation failed: ");
        }

        public string CheckItemsInTableTabsAfterDuplicateOperation(List<string> titles, AuthoringTableTab tab, IItemsTable itemsTable)
        {
            string err = CheckItemsPresenceInTab(tab, titles, shouldBePresent: true, itemsTable);

            List<string> errors = new List<string>();

            ErrorHelper.AddToErrorList(errors, err);

            //if (tab == AuthoringTableTab.Archived)
            //{
            //SelectTab(AuthoringTableTab.Active);

            //titleColumn = GetTitleColumn(itemsTable);
            //}

            List<string> duplicatedTestArticleTitles = GetDuplicatedTestItemsExpectedTitles(titles);

            err = CheckItemsPresenceInTab(tab, duplicatedTestArticleTitles, shouldBePresent: true, itemsTable);

            ErrorHelper.AddToErrorList(errors, err);

            err = CheckItemsStatus(AuthoringTableTab.Active, duplicatedTestArticleTitles, ArticleStatus.Draft, itemsTable);

            ErrorHelper.AddToErrorList(errors, err);

            return ErrorHelper.ConvertErrorsToString(errors, $"Articles check after 'Duplicate' operation failed: ");
        }

        public string CheckItemsInTableTabsAfterDeleteOperation(List<string> titles, IItemsTable itemsTable)
        {
            toastAlertSteps.WaitAlertToDisappear();

            List<string> errors = new List<string>();

            AuthoringTableTab tab = AuthoringTableTab.Archived;

            string err = CheckItemsPresenceInTab(tab, titles, shouldBePresent: false, itemsTable);

            ErrorHelper.AddToErrorList(errors, err);

            tab = AuthoringTableTab.Active;

            err = CheckItemsPresenceInTab(tab, titles, shouldBePresent: false, itemsTable);

            ErrorHelper.AddToErrorList(errors, err);

            return ErrorHelper.ConvertErrorsToString(errors, $"Articles check after 'Delete' operation failed: ");
        }

        public string CheckItemsStatus(AuthoringTableTab tab, List<string> titles, ArticleStatus expectedItemStatus, IItemsTable table)
        {
            List<ArticleStatus> itemStatusesToCheck = GetStatusesByTitle(titles, table);

            List<string> errors = new List<string>();

            for (int i = 0; i < titles.Count; i++)
            {
                if (itemStatusesToCheck[i] != expectedItemStatus)
                {
                    errors.Add($"{titles[i]} - {itemStatusesToCheck[i]}");
                }
            }

            string err = ErrorHelper.ConvertErrorsToString(errors, $"Items with invalid status found (tab: {tab}, expected status: {expectedItemStatus}): ");

            LogHelper.LogResult(log, $"Status of items checked", err);

            return err;
        }

        public List<string> GetDuplicatedTestItemsExpectedTitles(List<string> testItemTitles)
        {
            List<string> duplicatedItemTitles = testItemTitles
                .Select(x => $"Copy of {x}")
                .ToList();

            return duplicatedItemTitles;
        }

        public List<ArticleStatus> GetStatusesByTitle(List<string> titles, IItemsTable itemsTable)
        {
            List<ArticleStatus> statusColumn = itemsTable.GetItemStatusColumn().Select(s => CommonHelper.GetArticleStatus(s)).ToList();

            List<string> titlesColumn = GetTitleColumn(itemsTable);

            List<int> rowPositions = FindItemRowPositions(titles, titlesColumn);

            List<ArticleStatus> result = rowPositions.Where(p => p >= 0).Select(p => statusColumn[p]).ToList();

            return result;
        }

        public string CheckItemsNotPresentInTable(List<string> titles, List<string> titlesColumn)
        {
            List<string> foundUnexpectedItems = titles.Where(t => titlesColumn.Any(x => x.StartsWith(t))).ToList();

            string err = string.Empty;

            if (foundUnexpectedItems.Count > 0)
            {
                err = string.Join(", ", foundUnexpectedItems);
            }

            err = ErrorHelper.AddPrefixToError(err, "Found item(s) that should not be present in table: ");

            LogHelper.LogResult(log, $"Checked absence of items in table", err);

            return err;
        }

        public string CheckItemsPresentInTable(List<string> titles, List<string> titlesColumn)
        {
            List<string> missingItems = titles.Where(t => !titlesColumn.Any(x => x.StartsWith(t))).ToList();

            string err = string.Empty;

            if (missingItems.Count > 0)
            {
                err = string.Join(", ", missingItems);
            }

            err = ErrorHelper.AddPrefixToError(err, "Items are not present in table: ");

            LogHelper.LogResult(log, $"Checked presense of items in table", err);

            return err;
        }

        public string SearchInItemsTable(string searchText)
        {
            string err = ClearSearchInput();

            searchBlock.SearchItems(searchText);

            LogHelper.LogInfo(log, $"Search items by text: {searchText}");

            return err;
        }

        public string ClearSearchInput()
        {
            string err = string.Empty;

            if (searchBlock.IsClearSearchButtonVisible())
            {
                searchBlock.ClickCrossButtonInSearchInput();

                bool hasCrossButtonDisappeared = UiWaitHelper.Wait(() => !searchBlock.IsClearSearchButtonVisible(), WaitConstants.CheckElementDisappearedInSec);

                if (!hasCrossButtonDisappeared)
                {
                    err = "Search input: Cross button did not disappear after clicking";
                }

                LogHelper.LogResult(log, $"Search input cleared", err);
            }

            return err;
        }

        public string CheckItemsTableSearch(string searchText, List<string> titlesBeforeSearch, IItemsTable itemsTable)
        {
            string err = string.Empty;

            List<string> actualItemsTitles = GetTitleColumn(itemsTable);

            if (actualItemsTitles.Count == 0)
            {
                err = itemsTable.IsNoRecordsToShowDisplayed() ? string.Empty : "No records to show alert is not displayed in empty table";
            }

            List<string> expectedItemsTitles = titlesBeforeSearch
                .Where(r => r.Contains(searchText))
                .ToList();

            err += DataHelper.CompareListsIgnoreOrder(actualItemsTitles, expectedItemsTitles);

            err = ErrorHelper.AddPrefixToError(err, $"Search phrase '{searchText}': ");

            err = ErrorHelper.AddPostfixToError(err, $" (actual items: {string.Join(", ", actualItemsTitles)}, expected: {string.Join(", ", expectedItemsTitles)})");

            LogHelper.LogResult(log, $"Checked items search in table using '{searchText}' search phrase", err);

            return ErrorHelper.AddPrefixToError(err, $"Search using '{searchText}' search phrase: ");
        }

        public string CheckItemsTableSearch(List<string> searchPhrases, IItemsTable itemsTable)
        {
            string err = string.Empty;

            foreach (var searchPhrase in searchPhrases)
            {
                err = ClearSearchInput();

                if (!string.IsNullOrEmpty(err))
                {
                    break;
                }

                List<string> titlesBeforeSearch = GetTitleColumn(itemsTable);

                err = SearchInItemsTable(searchPhrase);

                if (!string.IsNullOrEmpty(err))
                {
                    break;
                }

                err = CheckItemsTableSearch(searchPhrase, titlesBeforeSearch, itemsTable);

                if (!string.IsNullOrEmpty(err))
                {
                    break;
                }
            }

            return err;
        }

        private string GetExpectedAlertMessage(AuthoringActionsMenuItem operation, AuthoringItemType itemType, int numOfItems)
        {
            string itemName = itemType == AuthoringItemType.Article ? "article" : "advisor journey";

            string expectedAlertMessage = numOfItems > 1
                ? $"{numOfItems} {itemName}s were {operation.ToString().ToLower()}d"
                : $"The {itemName} was {operation.ToString().ToLower()}d";

            return expectedAlertMessage;
        }
    }
}
