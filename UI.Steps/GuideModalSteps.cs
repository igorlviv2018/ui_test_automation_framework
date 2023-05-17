using NLog;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.PageObjects;

namespace Taf.UI.Steps
{
    public class GuideModalSteps : BaseSteps
    {
        private readonly GuideModal guidePage;

        private readonly NavigationBarSteps navigationBarSteps;

        private readonly DeviceSteps deviceSteps;

        public GuideModalSteps(App app, ILogger logger, bool isRedesign=false) : base(app, logger)
        {
            guidePage = new GuideModal(app, isRedesign);

            navigationBarSteps = new NavigationBarSteps(log);

            deviceSteps = new DeviceSteps(App.Taf, log, isRedesign);
        }

        public string CheckArticleIsOpened(string articleName)
        {
            bool isLoaded = UiWaitHelper.Wait(() => guidePage.GetCaption() == articleName, WaitConstants.CheckElementExistInSec);

            return isLoaded ? string.Empty : $"Article (with caption: {articleName}) is not displayed!";
        }

        public void CloseGuideModal()
        {
            guidePage.CloseGuide();

            WaitGuideIsClosed();
        }

        public bool IsGuideModalOpened() => guidePage.IsDisplayed();

        public bool IsGuideLoaded() =>
            UiWaitHelper.Wait(() => !guidePage.GetCaption().Contains("Loading...") && guidePage.GetCaption() != string.Empty, WaitConstants.CheckElementExistInSec);

        public bool WaitGuideIsClosed() =>
            UiWaitHelper.Wait(() => guidePage.IsGuideClosed(), WaitConstants.CheckElementExistInSec);

        public void OpenContentItemViaSearch(string guideName, UiContentType contentType, string device)
        {
            string err = navigationBarSteps.Find(guideName, contentType);


        }

        public string OpenContentItemForRandomDeviceViaSearch(string guideName, UiContentType contentType)
        {
            string err = navigationBarSteps.Find(guideName, contentType);

            if (!string.IsNullOrEmpty(err))
            {
                return err;
            }

            err = deviceSteps.SelectRandomDeviceInDeviceSelector();

            return "";
        }
    }
}