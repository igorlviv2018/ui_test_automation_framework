namespace Taf.UI.PageObjects
{
    public class DashboardPage : BasePage
    {
        private readonly string homeMenuItemActive = "//a[contains(@class,'link-active')]//span[@class='ss-home']";

        private readonly string speedperformLogo = "//div[contains(@class,'navbar')]//a[contains(@class,'navbar-brand')]";

        // rewrite - determine elements to uniquely identify the page
        public bool IsAt() => ElementsPresent(speedperformLogo, homeMenuItemActive);

        // guess what elements are present for all clients
        public void WaitTillPageLoaded() => WaitPageLoad("Dashboard", homeMenuItemActive);
    }
}
