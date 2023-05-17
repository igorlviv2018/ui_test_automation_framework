using Taf.UI.Core.Element;
using Taf.UI.PageObjects.CommonPages.Authoring;
using System.Collections.Generic;

namespace Taf.UI.PageObjects.TafTest.Authoring
{
    public class ParametersTable : BaseTable
    {
        private readonly string expandToggle = "/td[1]//span[contains(@class,'icon-caret')]";

        private readonly string title = "/td[2]";

        private readonly string type = "/td[3]";

        private readonly string description = "/td[4]";

        private readonly string editButton = "/td[5]//button[contains(@class,'ss-edit')]";

        private readonly string deleteButton = "/td[5]//button[contains(@class,'ss-trash')]";

        public void ClickExpandToggle(int rowPosition) => new Element(RowCellXpath(rowPosition, expandToggle)).ClickIfExists();

        public bool IsParameterExpanded(int rowPosition) =>
            new Element(RowAtPositionXpath(rowPosition)).GetAttribute("class").Contains("has-details");//"rotate-270");

        public List<string> GetTitleColumn() => GetColumn(tableRow + title);

        public string GetTitle(int rowPosition) => GetCell(rowPosition, title);

        public string GetType(int rowPosition) => GetCell(rowPosition, type);

        public string GetDescription(int rowPosition) => GetCell(rowPosition, description);

        public void ClickEditButton(int rowPosition) => new Element(RowCellXpath(rowPosition, editButton)).ClickIfExists();

        public void ClickDeleteButton(int rowPosition) => new Element(RowCellXpath(rowPosition, deleteButton)).ClickIfExists();
    }
}
