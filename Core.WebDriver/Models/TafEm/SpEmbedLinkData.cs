namespace Taf.UI.Core.Models
{
    public class TafEmLinkData
    {
        public override string ToString() => $"Link text: {LinkText}, Uri: {LinkUri}";

        public string LinkText { get; set; }

        public string LinkUri { get; set; }
    }
}
