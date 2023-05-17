using Taf.UI.Core.Constants;
using Taf.UI.Core.Element;
using Taf.UI.PageObjects.CommonPages.Authoring;

namespace Taf.UI.PageObjects.TafTest.Taf
{
    public class RatingsTable : BaseTable
    {
        private readonly string journeyTitleByText = "//div[contains(@class,'list-block')]//div[@class='title' and starts-with(text(),'{0}')]";

        private readonly string manufacturer = "/td[2]";

        private readonly string model = "/td[3]";

        private readonly string index = "/td[4]";

        private readonly string weightValue = "/td[5]//div[@class='weights-scale']/div[@class='weight-item']/span";

        private readonly string table = "//table";

        public string GetParameterRating(int rowPosition, int parameterPosition) =>
             new Element(IndexedXpath(RowCellXpath(rowPosition, weightValue), parameterPosition)).GetAttribute("innerHTML");

        public string GetManufacturer(int rowPosition) => new Element(RowCellXpath(rowPosition, manufacturer)).Text;

        public string GetModel(int rowPosition) => new Element(RowCellXpath(rowPosition, model)).Text;

        public string GetDeviceIndex(int rowPosition) => new Element(RowCellXpath(rowPosition, index)).Text;

        public bool IsTableDisplayed() => new Element(table).IsDisplayed(WaitConstants.HalfMinuteInSec);

        public bool IsTableBusy() => new Element(table).GetAttribute("aria-busy").Contains("true");

        public bool IsAttributeAriaBusyNull() => new Element(table).GetAttribute("aria-busy") == null;

        public bool IsTableRefreshed() => new Element(table).GetAttribute("aria-busy").Contains("false");
    }
}
