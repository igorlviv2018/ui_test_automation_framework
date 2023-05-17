using Taf.UI.Core.Element;

namespace Taf.UI.PageObjects.Authoring.ContentBlocks
{
    public class VideoBlock : BasePage
    {
        private readonly string videoUrlInput = "/div[@role='group']/input";

        private readonly string videoAddButton = "/div[@role='group']/div/button";

        public void SetVideoUrl(string blockXpath, string url)
        {
            new Element($"{blockXpath}{videoUrlInput}").SetText(url);
        }

        public void ClickVideoAdd(string blockXpath)
        {
            new Element($"{blockXpath}{videoAddButton}").ClickIfExists();
        }

        public bool IsVideoUrlValid(string blockXpath)
        {
            bool isUrlInvalid = new Element($"{blockXpath}{videoUrlInput}").GetAttribute("aria-invalid") == "true";

            return !isUrlInvalid;
        }
    }
}
