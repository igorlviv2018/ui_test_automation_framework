using NLog;
using Taf.UI.Core.Enums;
using Taf.UI.PageObjects.CommonPages;

namespace Taf.UI.Steps.Base
{
    public class SidebarBaseSteps : BaseSteps
    {
        public SidebarBaseSteps(ILogger logger) : base(App.Taf, logger)
        {
            sideMenu = new SidebarCommon();
        }

        private readonly SidebarCommon sideMenu;

        public void LockSidebar()
        {
            if (!sideMenu.IsMenuLocked())
            {
                sideMenu.ClickLockButton();
            }
        }
    }
}

