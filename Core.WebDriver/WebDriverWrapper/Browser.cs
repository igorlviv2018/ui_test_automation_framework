using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using Taf.UI.Core.Configuration;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.WebDriverWrapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Taf.WebDriver.Wrapper
{
    public class Browser
    {
        private IWebDriver driver;

        private readonly BrowserType browserType;

        private readonly TestConfiguration testConfig;

        private readonly string pathToWebdriver;

        [ThreadStatic]
        private static Browser current;

        public static Browser Current
        {
            get
            {
                return current;
            }
            set
            {
                current = value;
            }
        }

        public Browser(BrowserType browserType)
        {
            this.browserType = browserType;

            testConfig = new TestConfiguration();

            pathToWebdriver = SecretsHelper.ReadSecretValue(testConfig.ConfigRoot, "PATH_TO_WEBDRIVER");
        }

        public IEnumerable<IWebElement> FindElements(By selector)
        {
            return driver.FindElements(selector);
        }

        public IWebElement FindElement(By selector)
        {
            try
            {
                return driver.FindElement(selector);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Init()
        {
            if (driver == null)
            {
                CreateWebDriver();

                current = this;
            }
        }

        //public void Google()
        //{
        //    if (driver != null)
        //    {
        //        Navigate(new Uri("https://www.google.com"));
        //    }
        //}

        public object ExecuteJavaScript(string javaScript, params object[] args)
        {
            var js = (IJavaScriptExecutor)driver;

            return js.ExecuteScript(javaScript, args);
        }

        public void RestoreCookie(IReadOnlyCollection<Cookie> cookies)
        {
            foreach (var cookie in cookies)
            {
                driver.Manage().Cookies.AddCookie(cookie);
            }
        }

        public void Maximize() => driver.Manage().Window.Maximize();

        public void Navigate(Uri uri) => driver.Navigate().GoToUrl(uri);

        public void NavigateBack() => driver.Navigate().Back();

        public void Refresh() => driver.Navigate().Refresh();

        public void TakeScreenshot(string path)
        {
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();

            screenshot.SaveAsFile(path, ScreenshotImageFormat.Png);
        }

        public void SetImplicitWait(int seconds=WaitConstants.ImplicitWaitInSec)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(seconds);
        }

        public void SetPageLoadTimeout(int timeoutInSec=WaitConstants.PageLoadWaitInSec)
        {
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(timeoutInSec);
        }

        public void SwitchToWindowByHandle(string handle) => driver.SwitchTo().Window(handle);

        public string GetCurrentWindowHandle() => driver.CurrentWindowHandle;

        public List<string> GetWindowHandles() => driver.WindowHandles.ToList();

        public string GetWindowUrl() => driver.Url;

        public bool WaitExpectedUrl(string expUrl) =>
            UiWaitHelper.Wait(() => GetWindowUrl() == expUrl || GetWindowUrl() == $"{expUrl}/", 2);

        public IWebDriver GetWebDriver()
        {
            if (driver == null)
            {
                CreateWebDriver();
            }

            return driver;
        }

        public void Quit()
        {
            if (driver != null)
            {
                driver.Quit();

                driver = null;
            }
        }

        public void Close()
        {
            if (driver != null)
            {
                driver.Close();
            }
        }

        public BrowserType GetBrowserType() => browserType;

        private void CreateWebDriver()
        {
            var browserOptions = new BrowserOptions();

            switch (browserType)
            {
                case BrowserType.Chrome:
                    driver = new ChromeDriver(pathToWebdriver, browserOptions.GetChromeOptions());//, TimeSpan.FromMinutes(2)); //"C:\\_drivers",

                    //new DriverManager().SetUpDriver(new ChromeConfig(), "Latest");//, TimeSpan.FromMinutes(2));//debug - to del

                    //driver = new ChromeDriver();

                    break;

                case BrowserType.Firefox:
                    FirefoxDriverService service = PrepareFirefoxService();
                    driver = new FirefoxDriver(service, browserOptions.GetFirefoxOptions());
                    break;

                case BrowserType.InternetExplorer:
                    driver = new InternetExplorerDriver(pathToWebdriver, browserOptions.GetIEOptions(), TimeSpan.FromSeconds(120));
                    break;

                case BrowserType.Remote:

                    //service = PrepareFirefoxService();

                    driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), browserOptions.GetRemoteBrowserOptions());
                    
                    break;

                default:
                    throw new NotSupportedException($"The '{browserType}' browser is not supported");
            }

            SetImplicitWait();

            SetPageLoadTimeout();
        }

        public FirefoxDriverService PrepareFirefoxService()
        {
            FirefoxDriverService geckoService =
                FirefoxDriverService.CreateDefaultService(pathToWebdriver);

            geckoService.Host = "::1";

            return geckoService;
        }
    }
}
