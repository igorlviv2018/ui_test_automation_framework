using System.Collections.Generic;

namespace Taf.UI.Core.Models
{
    public class DxFlowDecisionBlockData
    {
        public List<TafEmBranchData> AnswersData { get; set; } = new List<TafEmBranchData>();

        public TafEmBranchData SelectedAnswerData { get; set; }

        public int AnswerToSelectPosition { get; set; } = -1;

        public int AnswerToSelectBranchId { get; set; }

        public bool HasBranchesButtonView { get; set; } = true;
    }
}
