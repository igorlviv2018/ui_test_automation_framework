using Taf.UI.Core.Constants;
using Taf.UI.Core.Element;
using System.Collections.Generic;

namespace Taf.UI.PageObjects.CommonPages.Authoring
{
    public class BaseTable : BasePage
    {
        //private readonly string tableColumnName = "//th[@role='columnheader']";

        private readonly string tableColumnName = "//th[@role='columnheader']/div[text()='{0}']/..";

        private readonly string tableTab = "//div[contains(@class,'header-tabs')]//a[contains(text(),'{0}')]";

        protected const string tableRow = "//tbody[@role='rowgroup']/tr";

        private readonly string rowCheckbox = "//td[1]//input[@type='checkbox']";

        private readonly string itemDescription = "//td[3]//div[contains(@class,'description')]";

        private readonly string noRecordsToShowAlert = $"{tableRow}//div[@role='alert']";

        protected const string actionsDropdown = "//div[contains(@class,'dropdown')]/button";

        protected readonly string actionsDropdownMenuItem = actionsDropdown + "/../ul[@role='menu']//a[@role='menuitem' and contains(text(),'{0}')]";

        //protected string ActionsDropdownMenuItemXpath(int position) => IndexedXpath(actionsDropdownMenuItem2, position);

        protected string ActionsDropdownMenuItemXpath(string menuItemName) => string.Format(actionsDropdownMenuItem, menuItemName);

        //protected string HeaderColumnName(int position) => IndexedXpath(tableColumnName, position);

        protected string HeaderColumnName(string columnName) => string.Format(tableColumnName, columnName);

        protected string RowAtPositionXpath(int position) => IndexedXpath(tableRow, position);

        protected string RowCellXpath(int rowPosition, string cellXpath) => RowAtPositionXpath(rowPosition) + cellXpath;

        public bool IsItemPresent(string title, string itemTitleByTextXpath) => new Element(string.Format(itemTitleByTextXpath, title)).Exists();

        //public string GetHeaderColumnSorting(int headerColumnPosition) => new Element(HeaderColumnName(headerColumnPosition)).GetAttribute("aria-sort");

        public string GetHeaderColumnSorting(string headerColumnName) => new Element(HeaderColumnName(headerColumnName)).GetAttribute("aria-sort");

        public void ClickHeaderColumnName(string headerColumnName) => new Element(HeaderColumnName(headerColumnName)).ClickIfExists();

        //--- Actions dropdown ---
        public bool IsActionsDropdownExpanded() => new Element(actionsDropdown).IsAriaExpanded();

        public void OpenActionsDropdown()
        {
            if (!IsActionsDropdownExpanded())
            {
                new Element(actionsDropdown).ClickIfExists();
            }
        }

        //public bool IsActionsMenuItemEnabled(int position) =>
        //    new Element(ActionsDropdownMenuItemXpath(position)).GetAttribute("aria-disabled") != "true";

        public bool IsActionsMenuItemEnabled(string menuItemName) =>
            new Element(ActionsDropdownMenuItemXpath(menuItemName)).GetAttribute("aria-disabled") != "true";

        public bool IsMenuItemPresent(string actionName) =>
            new Element(ActionsDropdownMenuItemXpath(actionName)).Exists(WaitConstants.TwoSeconds);

        public void SelectActionsMenuItem(string actionName)
        {
            if (IsActionsMenuItemEnabled(actionName))
            {
                new Element(ActionsDropdownMenuItemXpath(actionName)).ClickIfDisplayed();
            }
        }

        public void ClickTab(string tabName) => new Element(string.Format(tableTab, tabName)).ClickIfExists();

        public bool IsTabActive(string tabName) => new Element(string.Format(tableTab, tabName)).GetAttribute("class").Contains("active");

        public int GetRowCount() => IsNoRecordsToShowDisplayed() ? 0 : new Element(tableRow).Count;

        public bool IsNoRecordsToShowDisplayed() => new Element(noRecordsToShowAlert).Exists();

        public void CheckRowCheckbox(int rowPosition) => new Checkbox(RowCellXpath(rowPosition, rowCheckbox)).Check();

        public List<bool> GetItemCheckboxColumn() => GetStatusOfCheckboxes(rowCheckbox);

        public List<string> GetItemDescriptionColumn() => GetTextOfElementsViaJs(itemDescription);

        //public List<string> GetItemStatusColumn() => GetTextOfElementsViaJs("");//deb

        public List<string> GetColumn(string cellXpath) => GetTextOfElementsViaJs(cellXpath);

        public string GetCell(int rowPosition, string cellXpath) => new Element(RowCellXpath(rowPosition, cellXpath)).Text;
    }
}
