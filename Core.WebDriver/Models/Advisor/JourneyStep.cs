using Taf.UI.Core.Enums;
using System.Collections.Generic;

namespace Taf.UI.Core.Models.TafAd
{
    public class JourneyStep
    {
        public JourneyStepType StepType { get; set; } = JourneyStepType.Infinity;

        public bool HasIdontMind { get; set; } = false;

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public List<Parameter> Parameters { get; set; } = new List<Parameter>(); // Infinity, interval steps only

        public List<string> ParametersToSelectTitles { get; set; } = new List<string>(); // Infinity, interval steps only

        public List<string> ParameterTitles { get; set; } = new List<string>(); // Infinity, interval steps only

        public string IntervalParameterTitle { get; set; } = string.Empty; // Interval steps only

        public List<int> IntervalsToSelect { get; set; } = new List<int>(); // interval steps only

        public List<bool> SelectedParameters { get; set; } = new List<bool>(); // interval steps only
    }
}
