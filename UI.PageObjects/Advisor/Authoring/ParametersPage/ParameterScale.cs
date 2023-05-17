using Taf.UI.Core.Element;
using System.Collections.Generic;

namespace Taf.UI.PageObjects.TafTest.Authoring
{
    public class ParameterScale : BasePage
    {
        private readonly string ratedItem = "//div[@class='points-wrap']/div[@class='point-wrap']"; // rating (infinity param) or interval number (interval)

        private readonly string itemValue = "/div[@class='point-value']"; // infinity, interval parameter

        private readonly string itemBrand = "//span[@class='point-brand']";

        private readonly string itemModel = "//span[contains(@class,'point-model')]";

        private readonly string intervalLabel = "//div[@class='scale-grid-line']//span[@class='text-truncate']";

        private string ItemAtPosition(int itemPosition) => IndexedXpath(ratedItem, itemPosition);

        public int GetRatedItemCount() => new Element(ratedItem).Count;

        public string GetItemRating(int itemPosition) => new Element(ItemAtPosition(itemPosition) + itemValue).InnerText;

        public string GetItemBrand(int itemPosition) => new Element(ItemAtPosition(itemPosition) + itemBrand).Text;

        public string GetItemModel(int itemPosition) => new Element(ItemAtPosition(itemPosition) + itemModel).Text;

        public List<string> GetIntervalLabels() => GetTextOfElements(intervalLabel);
    }
}
