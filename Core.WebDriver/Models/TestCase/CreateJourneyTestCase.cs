using Taf.UI.Core.Enums;
using System.Collections.Generic;

namespace Taf.UI.Core.Models.TestCase
{
    public class CreateJourneyTestCase : CheckItemTestDataBase
    {
        public string ArticleOriginalId { get; set; } = string.Empty;

        public string ArticleCurrentId { get; set; } = string.Empty;

        //public List<TafEmArticleElement> ArticleTestData { get; set; } = new List<TafEmArticleElement>();

        public JourneyType JourneyType { get; set; } = JourneyType.Phone;

        public string JourneyTitle { get; set; }

        public override string ToString() => $"{JourneyTitle};{IsItemCreated};{ArticleOriginalId};{ArticleCurrentId};";
    }
}
