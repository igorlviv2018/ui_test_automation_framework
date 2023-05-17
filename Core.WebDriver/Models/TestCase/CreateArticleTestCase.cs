using Taf.UI.Core.Enums;
using System.Collections.Generic;

namespace Taf.UI.Core.Models.TestCase
{
    public class CreateArticleTestCase : CheckItemTestDataBase
    {
        public TestEnvironment TestEnvironment { get; set; } = TestEnvironment.Dev;

        public string ArticleOriginalId { get; set; } = string.Empty;

        public string ArticleCurrentId { get; set; } = string.Empty;

        public List<TafEmArticleElement> ArticleTestData { get; set; } = new List<TafEmArticleElement>();

        public ArticleType ArticleType { get; set; } = ArticleType.DiagnosticFlow;

        public string ArticleTitle { get; set; }

        public override string ToString() => $"{ItemTitle};{IsItemCreated};{ItemOriginalId};{ItemCurrentId};";
    }
}
