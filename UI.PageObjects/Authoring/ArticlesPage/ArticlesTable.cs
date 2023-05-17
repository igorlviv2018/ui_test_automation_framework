using Taf.UI.Core.Element;
using Taf.UI.Core.Interfaces;
using Taf.UI.PageObjects.CommonPages.Authoring;
using System.Collections.Generic;

namespace Taf.UI.PageObjects.Authoring
{
    public class ArticlesTable : BaseTable, IItemsTable
    {
        private readonly string articleTitleByText = "//td[3]//div[contains(@class,'title-wrap')]/div[contains(@class,'title') and starts-with(text(),'{0}')]";

        private readonly string articleType = "//td[2]/div[contains(@class,'icon')]";

        private readonly string articleTitle = "//td[3]//div[contains(@class,'title-wrap')]/div[contains(@class,'title')]";

        private readonly string articleId = "//td[3]//div[contains(@class,'title-wrap')]/div[contains(@class,'secondary-text')]";

        private readonly string articleVersion = "//td[4]/div";

        private readonly string articleStatus = "//td[5]/div/span[contains(@class,'status')]";

        private readonly string articleOwner = "//td[6]";

        public void ClickItemTitle(string title)
        {
            Element articleTitleElement = new Element(string.Format(articleTitleByText, title));

            articleTitleElement.ClickIfExists();
        }

        public bool IsItemPresent(string title) => IsItemPresent(title, articleTitleByText);

        //public string GetArticleType(int rowPosition) => GetItemType(rowPosition, articleType);

        //public string GetArticleTitle(int rowPosition) => new Element(RowCellXpath(rowPosition, articleTitle)).Text;

        //public string GetArticleDescription(int rowPosition) => new Element(RowCellXpath(rowPosition, articleDescription)).Text;

        //public string GetArticleId(int rowPosition) => new Element(RowCellXpath(rowPosition, articleId)).Text;
        
        public List<string> GetTypeColumn() => GetAttributeOfElementsViaJs("class", articleType);

        public List<string> GetTitleColumn() => GetTextOfElementsViaJs(articleTitle);

        public List<string> GetArticleIdColumn() => GetTextOfElementsViaJs(articleId);

        public List<string> GetVersionColumn() => GetTextOfElementsViaJs(articleVersion);

        public List<string> GetItemStatusColumn() => GetTextOfElementsViaJs(articleStatus);

        public List<string> GetOwnerColumn() => GetTextOfElementsViaJs(articleOwner);
    }
}
