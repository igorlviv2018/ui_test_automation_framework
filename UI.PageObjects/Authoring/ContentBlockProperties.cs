using Taf.UI.Core.Element;

namespace Taf.UI.PageObjects
{
    public class ContentBlockProperties : BasePage
    {
        private const string titleInput = "//section[@class='instruments-bar']//input[@type='text']";

        private const string captionInput = "//section[@class='instruments-bar']//textarea";

        private const string doneButton = "//section[@class='instruments-bar']//button";

        private const string elementType = "//section[@class='instruments-bar']//*[@class='title']";

        public void SetTitle(string title)
        {
            new Element(titleInput).SetText(title);
        }

        public string GetTitle()
        {
            return new Element(titleInput).GetAttribute("value");
        }

        public void SetCaption(string caption)
        {
            new Element(captionInput).SetText(caption);
        }

        public string GetElementType()
        {
            return new Element(elementType).Text;
        }

        public void ClickDoneButton()
        {
            new Element(doneButton).ClickIfExists();
        }
    }
}
