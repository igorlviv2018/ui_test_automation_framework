namespace Taf.UI.PageObjects
{
    public class TextBlock : BasePage
    {
        private readonly string linkXpath = "//a";

        public string TextLinksXpath(string baseXpath) => PrependBaseXpath(baseXpath, linkXpath);
    }
}
