using NLog;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models;
using Taf.UI.PageObjects;
using Taf.WebDriver.Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Taf.UI.Steps
{
    public class BaseSteps
    {
        private readonly CurrentPage currentPage;

        private readonly YoutubePlayer youtubeBlock;

        private readonly ImageBlock imageBlock;

        protected readonly ILogger log;

        protected readonly App app;

        public BaseSteps(App app, ILogger logger)
        {
            this.app = app;

            log = logger;

            currentPage = new CurrentPage();

            youtubeBlock = new YoutubePlayer();

            imageBlock = new ImageBlock(app);
        }

        public void Refresh()
        {
            Browser.Current.Refresh();
        }

        public void RestartBrowser()
        {
            Browser.Current.Quit();

            Browser.Current.Init();

            Browser.Current.Maximize();
        }

        public App GetOpenApp()
        {
            string url = Browser.Current.GetWindowUrl();

            App openApp = CommonHelper.GetApp(url);

            return openApp;
        }

        public bool IsAppStartPageOpen(App app) => CommonHelper.IsAppAtStartPage(Browser.Current.GetWindowUrl(), app);

        public string CheckVideoBlock(string videoBlockXpath, TafEmVideoBlockData expectedData)
        {
            youtubeBlock.BaseXpath = videoBlockXpath;

            List<string> errors = new List<string>();

            if (!youtubeBlock.IsPlayerDisplayed())
            {
                errors.Add("Player is not displayed in Video block");
            }
            else
            {
                if (expectedData != null)
                {
                    string url = youtubeBlock.GetUrl();

                    url = Regex.Replace(url, @"&{0,1}t=\d+&{0,1}", "");

                    if (url != expectedData.VideoUrl)
                    {
                        errors.Add($"Actual URL is '{youtubeBlock.GetUrl()}' but expected URL is '{expectedData.VideoUrl}'");
                    }
                }

                string err = youtubeBlock.PlayVideo(CommonConstants.PlayVideoInSec);

                errors.Add(err);
            }

            return ErrorHelper.ConvertErrorsToString(errors);
        }

        public string CheckLinks(string linkXpath, List<TafEmLinkData> expectedLinks)
        {
            List<TafEmLinkData> actualLinks = currentPage.GetLinks(linkXpath);

            List<string> errors = new List<string>();

            if (actualLinks.Count != expectedLinks.Count)
            {
                errors.Add($"Actual links count: {actualLinks.Count}, expected: {expectedLinks.Count}");
            }
            else
            {
                errors.Add(DataHelper.CompareObjects(actualLinks, expectedLinks));
            }

            foreach (var expectedLink in expectedLinks) //to remove foreach?
            {
                if (!actualLinks.Where(e => e.LinkText == expectedLink.LinkText && e.LinkUri == expectedLink.LinkUri).Any())
                {
                    errors.Add($"Expected link ({expectedLink}) not found");
                }
            }

            foreach (var actualLink in actualLinks)
            {
                if (!expectedLinks.Where(e => e.LinkText == actualLink.LinkText && e.LinkUri == actualLink.LinkUri).Any())
                {
                    errors.Add($"Unexpected actual link ({actualLink}) found");
                }
            }

            int currentLink = 1;

            foreach (var actualLink in actualLinks)
            {
                currentPage.SaveWindowHandles();

                currentPage.ClickElementIfDispalyed($"({linkXpath})[{currentLink}]");

                string err = currentPage.CheckLink(actualLink.LinkUri, $"{actualLink} link: ");

                errors.Add(err);

                currentLink++;
            }

            return ErrorHelper.ConvertErrorsToString(errors);
        }

        public string CheckImage(string imageXpath)
        {
            List<string> errors = new List<string>();

            bool isImageDisplayed = imageBlock.IsImageDisplayed(imageXpath);

            if (!isImageDisplayed)
            {
                errors.Add($"Image is not displayed");
            }

            return ErrorHelper.ConvertErrorsToString(errors);
        }

        public string CheckImageTitle(string imageBlockXpath, string expectedImageTitle)
        {
            string actualImageTitle = imageBlock.GetImageTitle(imageBlockXpath);

            string error = string.Empty;

            if (actualImageTitle != expectedImageTitle)
            {
                error = $"Actual image title: {actualImageTitle} but expected: {expectedImageTitle}";
            }

            return error;
        }

        public void MarkAsTested(TafEmArticleElement element)
        {
            element.IsTested = true;
        }

        public void ScrollToView(string elementXpath)
        {
            currentPage.ScrollToView(elementXpath);
        }
    }
}
