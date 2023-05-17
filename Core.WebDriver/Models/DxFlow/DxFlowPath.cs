using Taf.UI.Core.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Taf.UI.Core.Models
{
    public class DxFlowPath
    {
        public List<DxFlowPathItemData> Items = new List<DxFlowPathItemData>();

        public List<DxFlowPathItemData> ItemsBeforeMoveForward = new List<DxFlowPathItemData>();

        public int ItemToProcessPosition { get; set; }

        public int NextItemToProcessPosition { get; set; } = -1;

        public override string ToString()
        {
            List<string> path = new List<string>();

            string elementName;

            string itemDescription;

            foreach (var item in Items)
            {
                elementName = string.IsNullOrEmpty(item.Title) ? "Unnamed" : item.Title;

                itemDescription = $"{item.ItemType}:{elementName}(Id={item.AssociatedArticleElementId})";

                if (item.ItemType == PathItemType.Decision)
                {
                    string selectedAnswer = ((DxFlowDecisionBlockData)item.Data).SelectedAnswerData.Answer;

                    itemDescription = !string.IsNullOrEmpty(selectedAnswer)
                        ? $"{itemDescription}: Selected answer:{selectedAnswer}"
                        : itemDescription;
                }

                path.Add(itemDescription);
            }

            return string.Join("->", path);
        }
    }
}
