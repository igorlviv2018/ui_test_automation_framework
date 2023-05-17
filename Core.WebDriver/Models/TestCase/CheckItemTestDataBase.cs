using Taf.UI.Core.Enums;

namespace Taf.UI.Core.Models.TestCase
{
    public class CheckItemTestDataBase
    {
        public TestType TargetTestType { get; set; } = TestType.CreateCustomArticle; // type of test the created item (e.g. article) is intended for

        public string TestDescription { get; set; } = string.Empty;

        public string UserLogin { get; set; } = string.Empty;

        public User User { get; set; }

        public string ItemOriginalId { get; set; } = string.Empty;

        public string ItemCurrentId { get; set; } = string.Empty;

        public string ItemTitle { get; set; } = string.Empty;

        public bool IsItemCreated { get; set; } = false;

        public bool IsTestItemCreated() => IsItemCreated && !string.IsNullOrEmpty(ItemTitle);

        //public override string ToString() => $"{ItemTitle};{IsItemCreated};{ItemOriginalId};{ItemCurrentId};";
    }
}
