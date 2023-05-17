using Taf.UI.Core.Element;

namespace Taf.UI.PageObjects
{
    public class ShareLinkPage : BasePage 
    {
        private const string baseXPath = "//div[contains(@class,'sharing-modal-body')]";

        private readonly string caption = $"{baseXPath}/h2";

        private readonly string shareButton = $"//span[@class='ss-share']";

        private readonly string publicLinkButton = $"//div[@class='public-link-wrap']/a[contains(@class,'public-link-btn')]";

        private readonly string tab = baseXPath + "//a[@role='tab']/span[contains(@class,'{0}')]";

        private readonly string emailInput = "//div[@role='tabpanel' and @aria-hidden='false']//input[@type='email']";

        private readonly string button = "//div[@role='tabpanel' and @aria-hidden='false']//button"; //Send or Copy button

        private readonly string buttonWithCaption = "//div[@role='tabpanel' and @aria-hidden='false']//button[contains(text(),'{0}')]"; //Send or Copy button

        private readonly string publicLinkTextbox = "//div[@role='tabpanel' and @aria-hidden='false']//input[@type='text']";

        private readonly string openLinkInNewWindow = "//div[@role='tabpanel' and @aria-hidden='false']//a";

        private readonly string closeButton = $"{baseXPath}/a[@class='close-btn']";

        public bool IsDisplayed()
        {
            return ElementsDisplayed(caption);
        }

        public string GetCaption() =>
            new Element(caption).Text;

        public string GetPublicLinkButtonUrl() =>
            new Element(publicLinkButton).GetAttribute("href");

        public void OpenTab(string id) =>
            new Element(string.Format(tab, id)).ClickIfExists();

        public void EnterEmail(string email)
        {
            Element emailTextbox = new Element(emailInput);

            emailTextbox.SetText(email);
        }

        public void SendPublicLinkEmail()
        {
            Element sendButton = new Element(button);

            sendButton.Click();
        }

        public string GetPublicUrlInTextbox()
        {
            Element url = new Element(publicLinkTextbox);

            return url.Text;
        }

        public void CopyPublicUrl()
        {
            Element copyButton = new Element(button);

            copyButton.Click();
        }

        public void OpenPublicLinkInNewWindow()
        {
            Element openInNewWindow = new Element(openLinkInNewWindow);

            openInNewWindow.Click();
        }

        public bool IsVisible()
        {
            bool isVisible = ElementsDisplayed(caption, button, closeButton);

            return isVisible;
        }

        public void ClickCloseButton() =>
            new Element(closeButton).ClickIfExists();

        public void ClickShareButton() =>
            new Element(shareButton).ClickIfExists();

        public void WaitLoading()
        {
            WaitPageLoad("Share link", caption, emailInput, button);
        }

        public void WaitClosing()
        {
            new Element(closeButton).WaitTillDisappeared("Share link popup close button");

            new Element(caption).WaitTillDisappeared("Share link popup caption ('Share')");
        }
    }
}