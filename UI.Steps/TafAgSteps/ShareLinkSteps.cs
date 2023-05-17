//using Taf.Core.Models.Settings.ContentSharing;
using Taf.UI.Core.Helpers;
using Taf.UI.PageObjects;
using System.Web;

namespace Taf.UI.Steps
{
    public class ShareLinkSteps// : BaseSteps
    {
        private readonly ShareLinkPage sharePage = new ShareLinkPage();

        //private readonly ApiHelper apiHelper = new ApiHelper();

        private readonly BrowserStorageHelper browserStorage = new BrowserStorageHelper();

        public void OpenPopup()
        {
            sharePage.ClickShareButton();

            sharePage.WaitLoading();
        }

        public void ClosePopup()
        {
            sharePage.ClickCloseButton();

            sharePage.WaitClosing();
        }

        public string CheckCaption()
        {
            string actualCaption = sharePage.GetCaption();

            string expectedCaption = "Share";

            return actualCaption != expectedCaption
                ? $"Share link popup caption is invalid - actual: '{actualCaption}', expected: '{expectedCaption}'"
                : string.Empty;
        }

        //public string CheckPublicLinkOnUrlTab(UrlTemplateType urlType)
        //{
        //    sharePage.OpenTab("ss-link");

        //    string actualLink = sharePage.GetPublicUrlInTextbox();

        //    string authToken = browserStorage.GetAuthToken();

        //    //string expectedLink = "http://www.test.com/devices/{device}";
        //    string expectedLink = apiHelper.GetExpectedContentSharingUrl(urlType, authToken);

        //    return actualLink != expectedLink
        //        ? $"Share link (on URL tab) is invalid - actual: {actualLink}, expected: {expectedLink}"
        //        : string.Empty;
        //}

        //debug
        //public string CheckPublicLinkButtonUrl(UrlTemplateType urlType)
        //{
        //    string authToken = browserStorage.GetAuthToken();

        //    string actualLink = sharePage.GetPublicLinkButtonUrl();

        //    actualLink = HttpUtility.UrlDecode(actualLink); //debug

        //    //string expectedLink = "http://www.test.com/guide"; //get from API ??
        //    string expectedLink = apiHelper.GetExpectedContentSharingUrl(urlType, authToken);

        //    return actualLink != expectedLink
        //        ? $"'Public link' button has invalid Url - actual: {actualLink}, expected: {expectedLink}"
        //        : string.Empty;
        //}

        //public string CheckShareLinkPopup(UrlTemplateType urlType)
        //{
        //    OpenPopup();

        //    string err = CheckPublicLinkOnUrlTab(urlType);

        //    ClosePopup();

        //    return err;
        //}
    }
}