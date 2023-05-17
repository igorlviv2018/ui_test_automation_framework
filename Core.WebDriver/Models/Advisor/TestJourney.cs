using System.Collections.Generic;

namespace Taf.UI.Core.Models.TafAd
{
    public class TestJourney
    {
        public string Title { get; set; } = string.Empty;

        public List<JourneyStep> Steps { get; set; } = new List<JourneyStep>();

        public List<string> UsedParameterTitles { get; set; } = new List<string>(); //infinity & interval

        public bool IsJourneyEmpty() => Title == string.Empty;
    }
}
