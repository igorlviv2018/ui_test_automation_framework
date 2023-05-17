using OpenQA.Selenium;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.WebDriver.Wrapper;
using System;
using System.Threading;

namespace Taf.UI.PageObjects
{
    public class YoutubePlayer : BasePage
    {
        private readonly string largePlayBtn = "//button[contains(@class,'ytp-large-play-button')]";

        private readonly string smallPlayBtn = "//button[contains(@class,'ytp-play-button')]";

        private readonly string player = "//div[contains(@class,'html5-video-player')]";

        private string IframeXpath() => PrependBaseXpath(baseXpath, "//iframe");

        private string baseXpath;

        public string BaseXpath
        {
            set { baseXpath = value; }
        }

        public int GetDuration()
        {
            SwitchToIFrame(IframeXpath());

            IWebElement ytPlayer = new Element(player).ToIWebElement();

            object elapsedTimeInSec = ExecuteJavaScript("return arguments[0].getDuration();", ytPlayer);

            SwitchToDefaultContent();

            return Convert.ToInt32(elapsedTimeInSec);
        }

        public void ClickPlayButton()
        {
            SwitchToIFrame(IframeXpath());

            Element largeBtn = new Element(largePlayBtn);

            Element smallBtn = new Element(smallPlayBtn);

            new Element(player).ScrollToView();

            if (largeBtn.IsDisplayed(WaitConstants.OneSecond))
            {
                largeBtn.Click();
            }
            else if (smallBtn.IsDisplayed(WaitConstants.OneSecond))
            {
                smallBtn.Click();
            }

            SwitchToDefaultContent();
        }

        public string GetUrl()
        {
            SwitchToIFrame(IframeXpath());

            IWebElement ytPlayer = new Element(player).ToIWebElement();

            object url = ExecuteJavaScript("return arguments[0].getVideoUrl();", ytPlayer);

            SwitchToDefaultContent();

            return Convert.ToString(url);
        }

        public bool IsPlayerDisplayed()
        {
            Element frame = new Element(IframeXpath());

            Element ytPlayer = new Element(player);

            bool isDisplayed = false;

            if (frame.Exists())
            {
                frame.SwitchToIFrame();

                if (ytPlayer.Exists() && ytPlayer.IsDisplayed())
                {
                    isDisplayed = true;
                }

                frame.SwitchToDefaultContent();
            }

            return isDisplayed;
        }

        public YoutubePlayerState GetVideoState(bool switchToIFrame = true)
        {
            if (switchToIFrame)
            {
                SwitchToIFrame(IframeXpath());
            }

            IWebElement youtube = new Element(player).ToIWebElement();

            string state = Browser.Current.ExecuteJavaScript("return arguments[0].getPlayerState();", youtube).ToString();

            int.TryParse(state, out int stateAsInt);

            YoutubePlayerState playerState = (YoutubePlayerState)stateAsInt;

            if (switchToIFrame)
            {
                SwitchToDefaultContent();
            }

            return playerState;
        }

        /// <summary>
        /// Play video for specified amount of time and pause it
        /// </summary>
        /// <param name="secToPlay">time in sec</param>
        /// <returns>error message</returns>
        public string PlayVideo(int secToPlay)
        {
            string err = PlayViaUi(); //Start play via UI because of Chrome issue - start playing via iFrame API fails

            if (string.IsNullOrEmpty(err) && secToPlay < GetDuration())
            {
                Thread.Sleep(secToPlay * 1000);

                err += Playback(YoutubePlayback.Pause);
            }

            return err;
        }

        public string Playback(YoutubePlayback operation)
        {
            SwitchToIFrame(IframeXpath());

            IWebElement youtube = new Element(player).ToIWebElement();

            string javaScript = string.Empty;

            YoutubePlayerState expectedState = YoutubePlayerState.Buffering;

            if (operation == YoutubePlayback.Play)
            {
                javaScript = "return arguments[0].playVideo();";

                expectedState = YoutubePlayerState.Playing;
            }
            else if (operation == YoutubePlayback.Pause)
            {
                javaScript = "return arguments[0].pauseVideo();";

                expectedState = YoutubePlayerState.Paused;
            }
            else if (operation == YoutubePlayback.Stop)
            {
                javaScript = "return arguments[0].stopVideo();";

                expectedState = YoutubePlayerState.Paused;
            }

            ExecuteJavaScript(javaScript, youtube);

            bool isOperationSuccess = UiWaitHelper.Wait(() => GetVideoState(false) == expectedState, WaitConstants.VideoPlayerWaitInSec);

            string err = string.Empty;

            if (!isOperationSuccess)
            {
                err = $"{operation} operation failed during {WaitConstants.VideoPlayerWaitInSec} s (expected state '{expectedState}' was not reached)";
            }

            SwitchToDefaultContent();

            return err;
        }

        public string PlayViaUi()
        {
            if (GetVideoState() != YoutubePlayerState.Playing)
            {
                ClickPlayButton();
            }

            bool isOperationSuccess = UiWaitHelper.Wait(() => GetVideoState() == YoutubePlayerState.Playing, WaitConstants.VideoPlayerWaitInSec);

            return !isOperationSuccess
                ? $"{YoutubePlayback.Play} operation failed during {WaitConstants.VideoPlayerWaitInSec} s (expected state '{YoutubePlayerState.Playing}' was not reached)"
                : string.Empty;
        }
    }
}
