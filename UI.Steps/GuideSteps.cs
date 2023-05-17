using NLog;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.PageObjects;
using System.Collections.Generic;

namespace Taf.UI.Steps
{
    public class GuideSteps : BaseSteps
    {
        private readonly GuideBlock guideBlock;

        private readonly NavigationBarSteps navigationBarSteps;

        private readonly DeviceSteps deviceSteps;

        public GuideSteps(ILogger logger) : base(App.Taf, logger)
        {
            guideBlock = new GuideBlock();

            navigationBarSteps = new NavigationBarSteps(log);

            deviceSteps = new DeviceSteps(App.Taf, log);
        }

        //public string CheckArticleIsOpened(string articleName)
        //{
        //    bool isLoaded = UiWaitHelper.Wait(() => guidePage.GetCaption() == articleName, WaitConstants.CheckElementExistInSec);

        //    return isLoaded ? string.Empty : $"Article (with caption: {articleName}) is not displayed!";
        //}

        //to fix
        public void CloseGuideBlock()
        {
            guideBlock.CloseGuide();

            //WaitGuideIsClosed();
        }

        public bool IsGuideOpened() => guideBlock.IsGuideDisplayed(); //fixed

        public string CheckGuideOpened() => guideBlock.IsGuideDisplayed() ? string.Empty : "Guide is not opened";

        //public bool IsGuideLoaded() =>
        //    UiWaitHelper.Wait(() => !guidePage.GetCaption().Contains("Loading...") && guidePage.GetCaption() != string.Empty, WaitConstants.CheckElementExistInSec);

        //public bool WaitGuideIsClosed() =>
        //    UiWaitHelper.Wait(() => guideBlock.IsGuideClosed(), WaitConstants.CheckElementExistInSec);

        public void OpenContentItemViaSearch(string guideName, UiContentType contentType, string device)
        {
            string err = navigationBarSteps.Find(guideName, contentType);


        }

        public string CheckGuideViewMode(GuideViewType viewMode)
        {
            bool isViewButtonActive = guideBlock.IsViewButtonActive(CommonHelper.GetGuideViewSwitcherSpanClass(viewMode));

            List<string> errors = new List<string>();

            string err = isViewButtonActive ? string.Empty : $"Expected view mode button is not active ({viewMode})";

            ErrorHelper.AddToErrorList(errors, err);

            if (viewMode == GuideViewType.Slider && !guideBlock.AreSwiperButtonsDisplayed())
            {
                err = "Swiper buttons are missing for Slider mode";

                ErrorHelper.AddToErrorList(errors, err);
            }

            if (viewMode == GuideViewType.List && !guideBlock.AreListItemsDisplayed())
            {
                err = "List items are missing for List mode";

                ErrorHelper.AddToErrorList(errors, err);
            }

            return ErrorHelper.ConvertErrorsToString(errors, "Guide view mode check: ");
        }

        public string OpenContentItemForRandomDeviceViaSearch(string guideName, UiContentType contentType)
        {
            string err = navigationBarSteps.Find(guideName, contentType);

            if (!string.IsNullOrEmpty(err))
            {
                return err;
            }

            string deviceName = deviceSteps.SelectRandomDeviceInDeviceSelector();

            if (string.IsNullOrEmpty(deviceName))
            {
                err = "Failed to select a random device in Device selector modal";
            }

            return err;
        }
    }
}