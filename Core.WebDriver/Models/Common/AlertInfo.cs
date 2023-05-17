using Taf.UI.Core.Enums;

namespace Taf.UI.Core.Models
{
    public class AlertInfo
    {
        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public AlertStatus Status { get; set; } = AlertStatus.Failed;

        public bool IsDisplayed { get; set; } = false;
    }
}
