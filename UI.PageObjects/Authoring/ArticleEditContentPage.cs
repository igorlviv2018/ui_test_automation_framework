using Taf.UI.Core.Element;

namespace Taf.UI.PageObjects
{
    public class ArticleEditContentPage : BasePage
    {
        //private readonly string elementOnInstrumentPanel = "//section[@class='instruments-bar']//div[@class='instrument-block' and @title='{0}']";

        private readonly string elementOnInstrumentPanel = "//section[@class='instruments-bar']//div[@class='instrument-block']//div[@class='name' and contains(text(),'{0}')]";

        public void PlaceContentElement(string instrumentPanelElementName, string contentBlockToDropToXpath)
        {
            Element contentBlockToDropTo = new Element(contentBlockToDropToXpath);

            Element contentElementOnPanel = new Element(string.Format(elementOnInstrumentPanel, instrumentPanelElementName));

            if (contentElementOnPanel.Exists())
            {
                contentElementOnPanel.DragAndDropToBottomLeft(contentBlockToDropTo);
            }
        }
    }
}
