using Taf.UI.Core.Enums;
using Taf.UI.Core.Models.TestCase;
using System.Collections.Generic;
using System.Linq;

namespace Taf.UI.Core.Helpers
{
    public class TestCaseHelper
    {
        public CreateArticleTestCase AddTestArticleCreationStatus(CreateArticleTestCase verifyTestToRun, List<CreateArticleTestCase> createArticleResults)
        {
            bool readFromRecentResultsDataFile = string.IsNullOrEmpty(verifyTestToRun.ItemTitle) && !verifyTestToRun.IsItemCreated;

            if (readFromRecentResultsDataFile)
            {
                CreateArticleTestCase foundResult = createArticleResults.FirstOrDefault(x => x.ItemOriginalId == verifyTestToRun.ItemOriginalId);

                if (foundResult != null)
                {
                    verifyTestToRun.ItemTitle = foundResult.ItemTitle;

                    verifyTestToRun.IsItemCreated = foundResult.IsItemCreated;

                    verifyTestToRun.ItemCurrentId = foundResult.ItemCurrentId;
                }
            }

            return verifyTestToRun;
        }

        public string GetUniqueArticleTitle(CreateArticleTestCase testCase) => GetUniqueItemTitle(testCase.TargetTestType, testCase.ItemOriginalId);

        public string GetUniqueItemTitle(TestType testType, string itemOriginalId) => GetUniqueItemTitle(testType) + "_" + itemOriginalId;

        public string GetUniqueItemTitle(TestType testType) =>
            CommonHelper.GetArticleTitlePrefixByTestType(testType) + DataHelper.DateTimeString();
    }
}
