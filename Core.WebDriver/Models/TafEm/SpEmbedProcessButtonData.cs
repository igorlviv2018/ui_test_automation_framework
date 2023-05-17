using Taf.UI.Core.Enums;

namespace Taf.UI.Core.Models
{
    public class TafEmProcessButtonData
    {
        public ProcessButtonClickAction ClickAction { get; set; }

        public LinkButtonTarget LinkButtonTarget { get; set; }

        public string Label { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty; // Link buttons only
    }
}
