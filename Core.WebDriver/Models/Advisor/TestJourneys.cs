using System.Collections.Generic;

namespace Taf.UI.Core.Models.TafAd
{
    public class TestJourneys
    {
        public string Title { get; set; } = string.Empty;

        public List<Journey> Steps { get; set; } = new List<Journey>();
    }
}
