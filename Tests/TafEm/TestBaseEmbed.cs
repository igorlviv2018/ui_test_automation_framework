using Xunit;
using NLog;
using System.Collections.Generic;
using Taf.UI.Steps;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Models.TestCase;

namespace Tests
{
    public class TestBaseEmbed
    {
        private readonly ILogger log;

        private readonly DxFlowSteps dxFlowSteps;

        private readonly CustomArticleSteps customArticleSteps;

        private readonly TestCaseHelper testCaseHelper;

        private readonly List<CreateArticleTestCase> createArticleTestResults;

        public TestBaseEmbed(ILogger logger)
        {
            log = logger;

            dxFlowSteps = new DxFlowSteps(App.Embed, log);

            customArticleSteps = new CustomArticleSteps(App.Embed, log);

            testCaseHelper = new TestCaseHelper();

            createArticleTestResults = CsvHelper.ReadCsv(CommonHelper.GetPathToCreatedItems(CommonHelper.CreatedItemsFileName(TestType.CreateCustomArticle)));

            createArticleTestResults.AddRange(CsvHelper.ReadCsv(CommonHelper.GetPathToCreatedItems(CommonHelper.CreatedItemsFileName(TestType.CreateDiagnosticFlow))));
        }

        public void CheckArticleEmbed(CreateArticleTestCase testCase)
        {
            //Arrange
            LogHelper.LogTestStart(log, testCase.TestDescription);

            testCase = testCaseHelper.AddTestArticleCreationStatus(testCase, createArticleTestResults);

            bool isTestArticleCreated = testCase.IsTestItemCreated();

            LogHelper.LogInfo(log, $"Test article (original id={testCase.ArticleOriginalId}) is created (and published): {isTestArticleCreated}");

            Assert.True(isTestArticleCreated, $"Test article (original id={testCase.ArticleOriginalId}) not created (or not published)");

            customArticleSteps.OpenEmbedAppAndShowContent(testCase.ArticleCurrentId);

            LogHelper.LogInfo(log, $"Embed app opened");

            LogHelper.LogInfo(log, $"Test article '{testCase.ArticleTitle}' opened");

            //Act
            string err = dxFlowSteps.CheckAllPaths(testCase.ArticleTestData);

            LogHelper.LogInfo(log, $"Article validated");

            LogHelper.LogTestEnd(log, err, testCase.TestDescription);

            //Assert
            Assert.True(string.IsNullOrEmpty(err), err);
        }
    }
}
