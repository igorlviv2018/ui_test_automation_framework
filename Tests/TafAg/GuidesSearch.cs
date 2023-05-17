using Taf.UI.PageObjects;
using Taf.UI.Steps;
using Taf.UI.Core.Enums;
using Xunit;
using NLog;

namespace Tests
{
    public class GuidesValidation : IClassFixture<TestFixture>
    {
        private readonly TestFixture fixture;

        private readonly ILogger log;

        private readonly NavigationBar navBar;

        private readonly GuideBlock guideBlock;

        private readonly string clientName;

        private readonly BrowserSteps browserSteps;

        public GuidesValidation(TestFixture fixture)
        {
            this.fixture = fixture;

            log = LogManager.GetLogger("TafGuidesSearch");

            clientName = fixture.TestConfig["ClientName"];

            navBar = new NavigationBar();

            guideBlock = new GuideBlock();

            browserSteps = new BrowserSteps(log);

            //clientId = fixture.DbHelper.GetClientIdByName(clientName);
        }

        [Fact]
        public void GuideSearchInGlobalSearch()
        {
            log.Info("Test running");

            LoginSteps loginSteps = new LoginSteps(log);

            browserSteps.OpenAppDeepLink(App.Taf);

            string err = loginSteps.Login("aqa.movistar.pe.adm.dev@gmail.com");

            Assert.True(string.IsNullOrEmpty(err), err);

            navBar.Search("cómo cargar");

            navBar.SelectSearchResultItem(UiContentType.Guide, "Cómo cargar la");

            DeviceSelectorPage deviceSelector = new DeviceSelectorPage(App.Taf);

            //Dictionary<string, string> res = deviceSelector.GetLocators(); //debug

            Assert.True(deviceSelector.IsDisplayed(), "Device selector not displayed!");
            
            deviceSelector.SearchDevices("xiaomi mi 8");

            err = deviceSelector.CheckExpectedDeviceFound("Xiaomi Mi 8 Lite", deviceSelector.GetFoundDevices());

            Assert.True(string.IsNullOrEmpty(err), err);

            deviceSelector.ChooseDevice("Xiaomi Mi 8 Lite");

            Assert.True(guideBlock.IsGuideDisplayed(), "Guide is not opened!");
        }
    }
}
