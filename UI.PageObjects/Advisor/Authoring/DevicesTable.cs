using Taf.UI.Core.Element;
using Taf.UI.PageObjects.CommonPages.Authoring;

namespace Taf.UI.PageObjects.TafTest.Authoring
{
    public class DevicesTable : BaseTable
    {
        private readonly string advisorCheckbox = "//td[6]//input[@type='checkbox']";

        private readonly string manufacturer = "/td[3]";

        private readonly string model = "/td[4]";

        public void CheckCheckbox(int rowPosition) => new Checkbox(IndexedXpath(advisorCheckbox, rowPosition)).Check();

        public void GetManufacturer(int rowPosition) => new Element(RowCellXpath(rowPosition, manufacturer));

        public void GetModel(int rowPosition) => new Element(RowCellXpath(rowPosition, model));
    }
}
