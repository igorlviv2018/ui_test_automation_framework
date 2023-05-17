using NLog;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.PageObjects;

namespace Taf.UI.Steps
{
    public class AppsMenuSteps : BaseSteps
    {
        private readonly AppsMenu appsMenu;

        private readonly Spinner spinner;

        public AppsMenuSteps(ILogger logger) : base(App.Taf, logger)
        {
            appsMenu = new AppsMenu();

            spinner = new Spinner(App.Taf);
        }

        //public void OpenApp(AppLinkType app)
        //{
        //    if (GetOpenApp() == app && IsAppStartPageOpen(app))
        //    {
        //        return;
        //    }

        //    appsMenu.OpenAppsMenu();

        //    appsMenu.SelectMenuItem(CommonHelper.GetRelativeAppLink(app));

        //    LogHelper.LogInfo(log, $"Switched to '{app}' app");

        //    spinner.WaitSpinnerToDisappear(SpinnerType.TopProgressBar, 2);
        //}

        public string ClientName()
        {
            appsMenu.OpenAppsMenu();

            return appsMenu.GetClientName();
        }
    }
}
