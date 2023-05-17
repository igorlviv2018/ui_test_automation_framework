using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Taf.UI.Core.Helpers;
using Taf.WebDriver.Wrapper;
using Taf.UI.Core.Constants;
using System.Linq;
using System.Collections.Generic;
using Taf.UI.Core.Enums;
using System;
using System.Drawing;
using Taf.UI.Core.Exceptions;

namespace Taf.UI.Core.Element
{
    public class Element
    {
        protected By selector;

        public By GetSelector { get { return selector; } }

        public Element(By selector)
        {
            this.selector = selector;
        }

        public Element(string xpath)
        {
            selector = By.XPath(xpath);
        }

        public IWebElement ToIWebElement() => Browser.Current.FindElement(selector);

        public void Click() => Browser.Current.FindElement(selector).Click();

        public void SwitchToIFrame() => Browser.Current.GetWebDriver().SwitchTo().Frame(Browser.Current.FindElement(selector));

        public void SwitchToDefaultContent() => Browser.Current.GetWebDriver().SwitchTo().DefaultContent();

        public void ScrollToView()
        {
            if (Exists(WaitConstants.CheckElementExistInSec))
            {
                string script = "arguments[0].scrollIntoView(false);";

                ((IJavaScriptExecutor)Browser.Current.GetWebDriver()).ExecuteScript(script, Browser.Current.FindElement(selector));
            }
        }

        public void ScrollPageToBottom()
        {
            string script = "window.scrollTo(0, document.body.scrollHeight);";

            Browser.Current.ExecuteJavaScript(script);
        }

        public void ClickIfExists()
        {
            if (Exists(WaitConstants.ThreeSeconds))
            {
                Browser.Current.FindElement(selector).Click();
            }
        }

        public void ClickIfDisplayed()
        {
            if (IsDisplayed(WaitConstants.CheckElementExistInSec))
            {
                Browser.Current.FindElement(selector).Click();
            }
        }

        public void ClickUsingJs() // prefer not to use it
        {
            string script = "arguments[0].click();";

            ((IJavaScriptExecutor)Browser.Current.GetWebDriver()).ExecuteScript(script, Browser.Current.FindElement(selector));
        }

        public List<IWebElement> FindElements() => Browser.Current.FindElements(selector).ToList();

        public int Count
        {
            get 
            {
                return Exists() ? Browser.Current.FindElements(selector).Count() : 0;
            }
        }

        public string GetAttribute(string attributeName) =>
            Exists() ? Browser.Current.FindElement(selector).GetAttribute(attributeName) : string.Empty;

        public bool HasAttribute(string attributeName) => GetAttribute(attributeName) != null;

        public string GetAttributeJs(string attributeName)
        {
            if (!Exists())
            {
                return string.Empty;
            }

            IWebElement element = Browser.Current.FindElement(selector);

            var script = $"return arguments[0].getAttribute('{attributeName}');";

            return (string)((IJavaScriptExecutor)Browser.Current.GetWebDriver()).ExecuteScript(script, element);
        }

        public bool Exists()
        {
            Browser.Current.SetImplicitWait(0);
            
            bool exist = Browser.Current.FindElements(selector).Any();

            Browser.Current.SetImplicitWait(WaitConstants.ImplicitWaitInSec);

            return exist;
        }

        //public void DragAndDrop(Element to)
        //{
        //    IWebElement toElement = Browser.Current.FindElement(to.GetSelector); 

        //    Actions actions = new Actions(Browser.Current.GetWebDriver());

        //    actions.DragAndDrop(Browser.Current.FindElement(selector), toElement).Build().Perform();
        //}

        public bool Exists(int timeoutInSecOrMs, bool isTimeoutInMs=false)
        {
            Browser.Current.SetImplicitWait(0);

            int timeoutInMs = isTimeoutInMs ? timeoutInSecOrMs : 1000 * timeoutInSecOrMs;

            bool exist = UiWaitHelper.WaitInMs(Exists, timeoutInMs);

            Browser.Current.SetImplicitWait(WaitConstants.ImplicitWaitInSec);

            return exist;
        }

        public bool IsDisplayed() // debug - can SetImplicitWait be removed?
        {
            if (!Exists())
            {
                return false;
            }

            Browser.Current.SetImplicitWait(0);

            bool isDisplayed = Browser.Current.FindElement(selector).Displayed;

            Browser.Current.SetImplicitWait(WaitConstants.ImplicitWaitInSec);

            return isDisplayed;
        }

        public bool IsDisplayedSafe()
        {
            if (!Exists())
            {
                return false;
            }

            Browser.Current.SetImplicitWait(0);

            bool isDisplayed = false;

            try
            {
                isDisplayed = Browser.Current.FindElement(selector).Displayed;
            }
            catch (StaleElementReferenceException)
            { 
            
            }
            catch (NoSuchElementException)
            {

            }

            Browser.Current.SetImplicitWait(WaitConstants.ImplicitWaitInSec);

            return isDisplayed;
        }

        public bool IsDisplayed(int timeoutInSec)
        {
            Browser.Current.SetImplicitWait(0);

            bool isDisplayed = UiWaitHelper.Wait(() => IsDisplayed(), timeoutInSec);

            Browser.Current.SetImplicitWait(WaitConstants.ImplicitWaitInSec);

            return isDisplayed;
        }

        public bool WaitTillAppeared(string elementDescription, int timeoutInSec = WaitConstants.CheckElementExistInSec, bool throwException = true)
        {
            bool appeared = Exists(timeoutInSec);

            if (!appeared)
            {
                if (throwException)
                {
                    throw new ElementNotFoundException($"{elementDescription} did not appear within {timeoutInSec} sec!");
                }
            }

            return appeared;
        }

        public bool WaitTillDisappeared(string elementDescription, int timeoutInSec=WaitConstants.CheckElementExistInSec, bool throwException=true)
        {
            Browser.Current.SetImplicitWait(0);

            bool disappeared = UiWaitHelper.Wait(() => !IsDisplayed(), timeoutInSec);

            Browser.Current.SetImplicitWait(WaitConstants.ImplicitWaitInSec);

            if (!disappeared && throwException)
            {
                throw new ElementNotFoundException($"{elementDescription} did not disappear within {timeoutInSec} sec!");
            }

            return disappeared;
        }

        public void ClickWithActions()
        {
            Actions actions = new Actions(Browser.Current.GetWebDriver());

            actions.MoveToElement(Browser.Current.FindElement(selector)).Click().Perform();
        }

        public void SelectText(int startPosition, int selectionLength)
        {
            Actions actions = new Actions(Browser.Current.GetWebDriver());

            actions.SendKeys(Keys.Home).Perform();

            for (int i = 0; i < startPosition; i++)
            {
                actions.SendKeys(Keys.ArrowRight);
            }

            actions.KeyDown(Keys.LeftShift);

            for (int i = 0; i < selectionLength; i++)
            {
                actions.SendKeys(Keys.ArrowRight);
            }

            actions.KeyUp(Keys.LeftShift);

            actions.Build().Perform();
        }

        public Size GetViewPortSize()
        {
            int width = Convert.ToInt32(((IJavaScriptExecutor)Browser.Current.GetWebDriver())
                .ExecuteScript("return window.innerWidth"));

            int height = Convert.ToInt32(((IJavaScriptExecutor)Browser.Current.GetWebDriver())
                .ExecuteScript("return window.innerHeight"));

            return new Size()
            {
                Width = width,
                Height = height
            };
        }

        /// <summary>
        /// Drag and drop to bottom left corner of the destination element
        /// </summary>
        /// <param name="to">destination element</param>
        public void DragAndDropToBottomLeft(Element to)
        {
            IWebElement toElement = Browser.Current.FindElement(to.GetSelector);

            Actions actions = new Actions(Browser.Current.GetWebDriver());

            Size size = toElement.Size;

            int xCoord = (int)(0.1*size.Width);

            int yCoord = (int)(0.9*size.Height);

            to.ScrollToView();

            actions.ClickAndHold(Browser.Current.FindElement(selector))
                .MoveToElement(toElement, xCoord, yCoord)
                .MoveByOffset(-1, -1)
                .Release()
                .Build()
                .Perform();
        }

        public Size Size => Browser.Current.FindElement(selector).Size;

        public void DragAndDropToTopLeft(Element to)
        {
            IWebElement toElement = Browser.Current.FindElement(to.GetSelector);

            Actions actions = new Actions(Browser.Current.GetWebDriver());

            Size size = toElement.Size;

            int xCoord = (int)(0.1 * size.Width);

            int yCoord = (int)(0.15 * size.Height);

            actions.ClickAndHold(Browser.Current.FindElement(selector))
                .MoveToElement(toElement, xCoord, yCoord)
                .MoveByOffset(-1, -1)
                .Release()
                .Perform();
        }

        public void SetText(string text, bool clearWithCtrlADel=false)
        {
            if (Exists(WaitConstants.CheckElementExistInSec))
            {
                IWebElement element = Browser.Current.FindElement(selector);

                if (clearWithCtrlADel)
                {
                    element.SendKeys(Keys.Control + "a");

                    element.SendKeys(Keys.Delete);
                }
                else
                {
                    element.Clear();
                }

                element.SendKeys(text);
            }
        }

        public void SendKeys(string text) => Browser.Current.FindElement(selector).SendKeys(text);

        public string Text
        {
            get
            {
                string text = string.Empty;

                if (Exists(WaitConstants.CheckElementExistInSec))
                {
                    IWebElement element = Browser.Current.FindElement(selector);

                    text = !string.IsNullOrEmpty(element.Text)
                        ? element.Text
                        : string.Empty; //element.GetAttribute("value");
                }

                return text;
            }
        }
        
        public string InnerText
        {
            get
            {
                string innerText = Exists(WaitConstants.HalfSecondInMs, isTimeoutInMs: true)
                    ? Browser.Current.FindElement(selector).GetAttribute("innerText")
                    : string.Empty;

                return innerText;
            }
        }

        public string InputText
        {
            get
            {
                string inputText = Exists(WaitConstants.HalfSecondInMs, isTimeoutInMs: true)
                    ? Browser.Current.FindElement(selector).GetAttribute("value")
                    : string.Empty;

                return inputText;
            }
        }

        public bool IsSelected
        {
            get { return Browser.Current.FindElement(selector).Selected; }
        }

        public bool IsImageVisible()
        {
            if (!Exists())
            {
                return false;
            }

            IWebElement image = Browser.Current.FindElement(selector);

            var script = Browser.Current.GetBrowserType() == BrowserType.InternetExplorer
                ? "return arguments[0].complete"
                : "return (typeof arguments[0].naturalWidth !=\"undefined\" && "+
                  "arguments[0].naturalWidth > 0)" ;

            bool isVisible() => (bool)((IJavaScriptExecutor)Browser.Current.GetWebDriver()).ExecuteScript(script, image);

            return UiWaitHelper.Wait(isVisible, WaitConstants.CheckElementExistInSec);
        }

        public bool IsAriaExpanded() => GetAttribute("aria-expanded") == "true";

        public void SelectFromDropdown(string value)
        {
            if (Exists(WaitConstants.OneSecond))
            {
                new SelectElement(Browser.Current.FindElement(selector)).SelectByValue(value);
            }
        }
    }
}
