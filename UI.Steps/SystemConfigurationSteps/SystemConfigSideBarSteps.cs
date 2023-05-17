using Microsoft.Extensions.Configuration;
using NLog;
using Taf.UI.Core.Configuration;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Exceptions;
using Taf.UI.Core.Helpers;
using Taf.UI.PageObjects;
using Taf.UI.PageObjects.Administration.SystemConfiguration;
using Taf.UI.Steps.Base;

namespace Taf.UI.Steps
{
    public class SystemConfigSideBarSteps : SidebarBaseSteps
    {
        public SystemConfigSideBarSteps(ILogger logger, bool isRedesign=false) : base(logger)
        {
            TestConfig = new TestConfiguration().ConfigRoot;

            sideMenu = new Sidebar(isRedesign);

            spinner = new Spinner(App.Taf, isRedesign);

            this.isRedesign = isRedesign;
        }

        private readonly IConfigurationRoot TestConfig;

        private readonly Sidebar sideMenu;

        private readonly TabBar tabs = new TabBar();

        private readonly Spinner spinner;

        private readonly bool isRedesign;

        public void ExpandApplicationBlock(App app)
        {
            string appName = CommonHelper.GetAppName(app);

            if (!sideMenu.IsApplicationBlockExpanded(appName))
            {
                sideMenu.ClickApplicationBlockExpandButton(appName);

                UiWaitHelper.Wait(() => sideMenu.IsApplicationBlockExpanded(appName), WaitConstants.TwoSeconds);
            }

            LogHelper.LogInfo(log, $"'{appName}' application block expanded (side bar)");
        }

        public void OpenConfigurationPage(App app, string pageName)
        {
            ExpandApplicationBlock(app);

            if (!sideMenu.IsSubmenuItemActive(app, pageName))
            {
                if (isRedesign)
                {
                    sideMenu.ClickSubmenuItemRedesign(app, pageName);
                }
                else
                {
                    sideMenu.ClickSubmenuItem(app, pageName);
                }
                
                spinner.WaitTopProgressBarToDisappear();
            }

            LogHelper.LogInfo(log, $"'{pageName}' page opened");
        }

        public void OpenConfigurationTab(string tabName)
        {
            if (!tabs.IsTabActive(tabName))
            {
                tabs.ClickTab(tabName);

                spinner.WaitTopProgressBarToDisappear(WaitConstants.TwoSeconds);
            }

            LogHelper.LogInfo(log, $"'{tabName}' tab opened");
        }
    }
}
