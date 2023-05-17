using NLog;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.PageObjects;
using Taf.UI.PageObjects.Taf;
using Taf.UI.Steps.Base;

namespace Taf.UI.Steps
{
    public class SidebarSteps : SidebarBaseSteps
    {
        private readonly Sidebar sidebar;

        private readonly Spinner spinner;

        public SidebarSteps(ILogger logger, bool isRedesign=false) : base(logger)
        {
            sidebar = new Sidebar(isRedesign);

            spinner = new Spinner(App.Taf, isRedesign);
        }

        public void OpenPage(string menuItemName)
        {
            if (sidebar.GetCurrentMenuItem() != menuItemName.ToLower())
            {
                sidebar.ClickMenuItem(menuItemName);

                LogHelper.LogInfo(log, $"Side menu item '{menuItemName}' selected");

                spinner.WaitTopProgressBarToDisappear();
            }
        }
    }
}
