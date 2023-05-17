using Taf.UI.Core.Enums;

namespace Taf.UI.Core.Models
{
    public class AppsMenuItem
    {
        public string Role { get; set; }

        public string Href { get; set; }

        public string Text { get; set; }

        public AppsMenuItemType Type { get; set; }

        public App App { get; set; }

        public bool IsEnabled { get; set; }
    }
}
