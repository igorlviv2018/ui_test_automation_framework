using NLog;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.PageObjects;
using System;
using System.Collections.Generic;

namespace Taf.UI.Core.Helpers
{
    public class PageSteps
    {
        public PageSteps(ILogger logger)
        {
            currentPage = new CurrentPage();

            spinner = new Spinner(App.Taf);

            log = logger;
        }

        private readonly CurrentPage currentPage;

        private readonly Spinner spinner;

        private readonly ILogger log;

        public string CheckCopyrigt()
        {
            string copyrightActual = currentPage.GetCopyrightText();

            string copyrightExpected = string.Format(CommonConstants.CopyrightText, DateTime.Now.Year);

            string err = copyrightActual != copyrightExpected
                ? $"Invalid copyright text: '{copyrightActual}' (but expected: '{copyrightExpected}')"
                : string.Empty;

            LogHelper.LogResult(log, "Copyright text checked", err);

            return err;
        }

        public string CheckCopyrigtOnPageRedesign()
        {
            string copyrightActual = currentPage.GetCopyrightTextOnPage();

            string copyrightExpected = string.Format(CommonConstants.CopyrightTextRedesign, DateTime.Now.Year);

            string err = copyrightActual != copyrightExpected
                ? $"Invalid copyright text: '{copyrightActual}' (but expected: '{copyrightExpected}')"
                : string.Empty;

            LogHelper.LogResult(log, "Copyright text checked", err);

            return err;
        }

        public string CheckPageElementsDisplayed(Dictionary<string, string> nameXpathPairs)
        {
            List<string> errors = new List<string>();

            foreach (var key in nameXpathPairs.Keys)
            {
                if (!currentPage.IsElementDisplayed(nameXpathPairs[key]))
                {
                    errors.Add($"{key} is not displayed");
                }
            }

            LogHelper.LogResult(log, "Page elements presence checked", ErrorHelper.ConvertErrorsToString(errors));

            return ErrorHelper.ConvertErrorsToString(errors);
        }

        public string CheckPageName(string actualName, string expectedName)
        {
            string err = actualName != expectedName
                ? $"Invalid actual page name: {actualName}, expected: {expectedName}"
                : string.Empty;

            LogHelper.LogResult(log, "Page name checked", err);

            return err;
        }
    }
}
