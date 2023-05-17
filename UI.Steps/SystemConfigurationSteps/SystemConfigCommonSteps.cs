using NLog;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.PageObjects;
using Taf.UI.PageObjects.Administration.SystemConfiguration;
using Taf.UI.PageObjects.Administration.SystemConfiguration.Taf;

namespace Taf.UI.Steps
{
    public class SystemConfigCommonSteps : BrowserSteps
    {
        public SystemConfigCommonSteps(ILogger logger, bool isRedesign=false) : base(logger)
        {
            sideMenu = new Sidebar();

            tabs = new TabBar(isRedesign);

            configPage = new ConfigPageBase(isRedesign);

            spinner = new Spinner(App.Taf, isRedesign);

            toastAlertSteps = new ToastAlertSteps();
        }

        private readonly Sidebar sideMenu;

        private readonly TabBar tabs;

        private readonly ConfigPageBase configPage;

        private readonly Spinner spinner;

        private readonly ToastAlertSteps toastAlertSteps;

        public void OpenConfigurationTab(string tabName)
        {
            if (!tabs.IsTabActive(tabName))
            {
                tabs.ClickTab(tabName);

                spinner.WaitTopProgressBarToDisappear(WaitConstants.TwoSeconds);
            }

            LogHelper.LogInfo(log, $"'{tabName}' tab opened");
        }

        public string CheckSideMenuPresent()
        {
            string err = sideMenu.IsSideMenuPresent()
                ? string.Empty
                : "Side menu is not present";

            LogHelper.LogResult(log, $"Checked presence of side menu", err);

            return err;
        }

        public string SaveSettings(int buttonPosition=1)
        {
            string err = string.Empty;

            if (configPage.IsSaveButtonEnabled(buttonPosition))
            {
                //configPage.SaveButtonScrollToView();

                configPage.ClickSaveButton(buttonPosition);

                // add alert message check - AGE-660
                err = toastAlertSteps.CheckAlertPopup(AlertStatus.Success, isRedesign: true);

                LogHelper.LogResult(log, $"Settings saved", err);
            }

            return err;
        }
    }
}
