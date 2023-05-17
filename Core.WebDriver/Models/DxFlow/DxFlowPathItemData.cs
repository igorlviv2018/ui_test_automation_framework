using Taf.UI.Core.Enums;

namespace Taf.UI.Core.Models
{
    public class DxFlowPathItemData
    {
        public PathItemType ItemType { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int AssociatedArticleElementId { get; set; } = -1;

        public object Data { get; set; }

        public bool IsProcessed { get; set; } = false;

        public DxFlowPathItemData Clone()
        {
            return new DxFlowPathItemData()
            {
                ItemType = ItemType,
                Title = Title,
                Description = Description,
                AssociatedArticleElementId = AssociatedArticleElementId,
                Data = Data,
                IsProcessed = IsProcessed
            };
        }
    }
}
