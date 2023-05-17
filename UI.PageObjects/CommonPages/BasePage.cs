using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Exceptions;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models;
using Taf.WebDriver.Wrapper;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Taf.UI.PageObjects
{
    public abstract class BasePage
    {
        //[ThreadStatic]
        //protected static Browser browser;

        private readonly By copyright = By.CssSelector("div[class*='text-muted'] > small");

        private readonly string copyrightOnPage = "//main/div[last()]";

        private string CurrentWindowHandle { get; set; }

        private List<string> WindowHandlesBeforeOperation { get; set; }

        public string IndexedXpath(string xPath, int index) => $"({xPath})[{index}]";

        public string IndexedXpath(string baseXpath, string xPath, int index) => $"({baseXpath}{xPath})[{index}]";

        public string PrependBaseXpath(string baseXpath, string xPath) => $"{baseXpath}{xPath}";

        public Dictionary<string, string> SetLocators(App app, Dictionary<string, string> common, Dictionary<string, string> embed, Dictionary<string, string> agents, 
            Dictionary<string, string> advisor=null,
            Dictionary<string, string> agentsRedesign=null,
            bool isRedesign=false
            )
        {
            Dictionary<string, string> dictToMerge = new Dictionary<string, string>();

            if (app == App.Taf)
            {
                dictToMerge = isRedesign ? agentsRedesign : agents;
            }
            else if (app == App.Embed)
            {
                dictToMerge = embed;
            }
            else if (app == App.TafTest)
            {
                dictToMerge = advisor;
            }

            return common.Union(dictToMerge)
                .GroupBy(p => p.Key)
                .ToDictionary(pair => pair.Key, pair => pair.First().Value);
        }

        public Dictionary<string, string> SetLocatorsAuthoring(App app, Dictionary<string, string> common, Dictionary<string, string> authoring, Dictionary<string, string> advisor)
        {
            Dictionary<string, string> dictToMerge = new Dictionary<string, string>();

            if (app == App.Taf)
            {
                dictToMerge = authoring;
            }
            else if (app == App.TafTest)
            {
                dictToMerge = advisor;
            }

            return common.Union(dictToMerge)
                .GroupBy(p => p.Key)
                .ToDictionary(pair => pair.Key, pair => pair.First().Value);
        }

        public Dictionary<string, string> SetLocatorsAdvisor(App app, Dictionary<string, string> common, Dictionary<string, string> agents, Dictionary<string, string> advisor)
        {
            Dictionary<string, string> dictToMerge = new Dictionary<string, string>();

            if (app == App.Taf)
            {
                dictToMerge = agents;
            }
            else if (app == App.TafTest)
            {
                dictToMerge = advisor;
            }

            return common.Union(dictToMerge)
                .GroupBy(p => p.Key)
                .ToDictionary(pair => pair.Key, pair => pair.First().Value);
        }

        public string GetXpath(string locatorName, Dictionary<string, string> locators) =>
            locators.ContainsKey(locatorName)
                    ? locators[locatorName]
                    : string.Empty;

        //public void OpenGoogle()
        //{
        //    Browser.Current.Navigate(new Uri("https://www.google.com"));
        //    Thread.Sleep(3000);
        //}

        public object ExecuteJavaScript(string javaScript, params object[] args) => Browser.Current.ExecuteJavaScript(javaScript, args);

        public void SwitchToDefaultContent() => new Element("").SwitchToDefaultContent();

        public void SwitchToIFrame(string iFrameXpath) => new Element(iFrameXpath).SwitchToIFrame();

        public string CheckUrl(string expectedUrl)
        {
            bool isUrlCorrect = Browser.Current.WaitExpectedUrl(expectedUrl);

            string err = string.Empty;

            if (!isUrlCorrect)
            {
                string actualUrl = Browser.Current.GetWindowUrl();

                err = $"Expected URL - {expectedUrl} but actual - {actualUrl}";
            }

            return err;
        }

        public void SaveWindowHandles()
        {
            CurrentWindowHandle = Browser.Current.GetCurrentWindowHandle();

            WindowHandlesBeforeOperation = Browser.Current.GetWindowHandles();
        }

        public string SwitchToRecentlyOpenedWindow()
        {
            List<string> windowHandles = Browser.Current.GetWindowHandles();

            var newHandles = windowHandles.Where(h => !WindowHandlesBeforeOperation.Contains(h)).ToList();

            if (newHandles.Count > 0)
            {
                Browser.Current.SwitchToWindowByHandle(newHandles[0]);

                return string.Empty;
            }

            return "No recently opend tab/window found";
        }

        public void CloseRecentlyOpenedWindow()
        {
            string err = SwitchToRecentlyOpenedWindow();

            if (string.IsNullOrEmpty(err))
            {
                Browser.Current.Close();
            }
        }

        public string CheckLink(string expectedUrl, string errPrefix)
        {
            string err = SwitchToRecentlyOpenedWindow();

            bool isSwitchSuccess = string.IsNullOrEmpty(err);

            if (!isSwitchSuccess)
            {
                return ErrorHelper.AddPrefixToError(err, errPrefix);
            }

            err = CheckUrl(expectedUrl);

            err = ErrorHelper.AddPrefixToError(err, errPrefix);

            if (isSwitchSuccess)
            {
                CloseRecentlyOpenedWindow();

                Browser.Current.SwitchToWindowByHandle(CurrentWindowHandle);
            }

            return err;
        }

        public List<TafLinkData> GetLinks(string linkCommonXpath)
        {
            Element link = new Element(linkCommonXpath);

            List<TafLinkData> linkData = new List<TafLinkData>();

            if (!link.Exists(WaitConstants.HalfSecondInMs, isTimeoutInMs: true))
            {
                return linkData;
            }

            string href;

            foreach (var element in link.FindElements())
            {
                href = element.GetAttribute("href");

                if (!string.IsNullOrEmpty(href))
                {
                    linkData.Add(new TafLinkData()
                    {
                        LinkText = element.Text,
                        LinkUri = href.TrimEnd('/')
                    });
                }
            }

            return linkData;
        }

        public bool ElementsPresent(params By[] selectors)
        {
            foreach (By selector in selectors)
            {
                if (!new Element(selector).Exists(WaitConstants.CheckElementExistInSec))
                {
                    return false;
                }
            }

            return true;
        }

        public bool ElementsPresent(params string[] xPaths)
        {
            foreach (string xPath in xPaths)
            {
                if (!new Element(xPath).Exists(WaitConstants.OneSecond))
                {
                    return false;
                }
            }

            return true;
        }

        public bool ElementsDisplayed(params string[] xPaths)
        {
            foreach (string xPath in xPaths)
            {
                if (!new Element(xPath).IsDisplayed(WaitConstants.ImplicitWaitInSec))
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsElementDisplayed(string xPath) => new Element(xPath).IsDisplayed(WaitConstants.ImplicitWaitInSec);

        public string GetCopyrightText() => new Element(copyright).Text;

        //redesign
        public string GetCopyrightTextOnPage() => new Element(copyrightOnPage).Text;

        public void ClickElementIfDispalyed(string xPath) => new Element(xPath).ClickIfDisplayed();

        public List<string> GetTextOfElements(string xPath) => new Element(xPath).FindElements().Select(item => item.Text).ToList();

        public List<string> GetInnerTextOfElements(string xPath) => new Element(xPath).FindElements().Select(item => item.GetAttribute("innerText")).ToList();

        public List<string> GetAttributeOfElements(string attributeName, string xPath) =>
            new Element(xPath).FindElements().Select(e => e.GetAttribute(attributeName)).ToList();

        // a faster version (can be used in column reading in tables e.g.)
        public List<string> GetTextOfElementsViaJs(string xPath) => new Element(xPath).FindElements().Select(e => GetTextViaJs(e)).ToList();

        public List<string> GetAttributeOfElementsViaJs(string attributeName, string xPath) =>
            new Element(xPath).FindElements().Select(e => GetAttributeViaJs(e, attributeName)).ToList();

        public List<bool> GetStatusOfCheckboxes(string xPath) => new Element(xPath).FindElements().Select(e => e.Selected).ToList();

        public string GetTextViaJs(IWebElement webElement)
        {
            string script = "return arguments[0].innerText;";

            return (string)((IJavaScriptExecutor)Browser.Current.GetWebDriver()).ExecuteScript(script, webElement);
        }

        public string GetAttributeViaJs(IWebElement element, string attributeName)
        {
            var script = $"return arguments[0].getAttribute('{attributeName}');";

            return (string)((IJavaScriptExecutor)Browser.Current.GetWebDriver()).ExecuteScript(script, element);
        }

        public bool GetCheckedViaJs(IWebElement element)
        {
            var script = $"return arguments[0].checked;";

            return (bool)((IJavaScriptExecutor)Browser.Current.GetWebDriver()).ExecuteScript(script, element);
        }

        public List<string> GetDeviceListIds(string xPath) =>
            new Element(xPath).FindElements().Select(item => item.GetAttribute("href").Split("/").Last()).ToList();

        public void ClickElementWithTextIfExists(string xPath, string text)
        {
            Element elementWithText = new Element(string.Format(xPath, text));

            elementWithText.ClickIfExists();
        }

        public bool IsImageDisplayed(string imageXpath) => new Element(imageXpath).IsImageVisible();

        public void WaitPageLoad(string pageName, params string[] xPaths)
        {
            if (!ElementsDisplayed(xPaths))
            {
                throw new PageNotLoadedException(
                    $"'{pageName}' page not loaded within {WaitConstants.HalfMinuteInSec} seconds!");
            }
        }

        public void WaitPageLoad(string pageName, params By[] xPaths)
        {
            if (!ElementsPresent(xPaths))
            {
                throw new PageNotLoadedException(
                    $"'{pageName}' page not loaded within {WaitConstants.CheckElementExistInSec} seconds!");
            }
        }

        public void ScrollToView(string xPath) => new Element(xPath).ScrollToView();

        public Size GetElementSize(string xPath) => new Element(xPath).Size;

        public Size GetViewPortSize() => new Element("").GetViewPortSize();

        public bool IsModalDisplayed(string modalXpath) => new Element(modalXpath).IsDisplayedSafe();

        public bool WaitModalDisappeared(string modalXpath) =>
            UiWaitHelper.Wait(() => !IsModalDisplayed(modalXpath), WaitConstants.CheckElementDisappearedInSec);

        public bool WaitModalAppeared(string modalXpath) =>
            UiWaitHelper.Wait(() => IsModalDisplayed(modalXpath), WaitConstants.CheckElementDisappearedInSec);

        public void SetImage(string imageInputXpath, string imageFilePath)
        {
            Element fileInput = new Element(imageInputXpath);

            if (Browser.Current.GetBrowserType() == BrowserType.Remote)
            {
                ((RemoteWebDriver)Browser.Current.GetWebDriver()).FileDetector = new LocalFileDetector();
            }

            if (fileInput.Exists())
            {
                fileInput.SendKeys(imageFilePath);
            }
        }
    }
}
