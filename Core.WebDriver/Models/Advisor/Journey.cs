using System.Collections.Generic;

namespace Taf.UI.Core.Models.TafAd
{
    public class Journey
    {
        public string Title { get; set; } = string.Empty;

        public List<JourneyStep> Steps { get; set; } = new List<JourneyStep>();
    }
}
