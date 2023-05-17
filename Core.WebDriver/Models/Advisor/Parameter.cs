using System.Collections.Generic;

namespace Taf.UI.Core.Models.TafAd
{
    public class Parameter
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public List<RatedDevice> RatedDevices { get; set; } = new List<RatedDevice>();

        public List<string> IntervalLabels { get; set; } = new List<string>(); // interval parameters only

        public bool IsParameterEmpty() => string.IsNullOrEmpty(Title);

        //public override string ToString() =>
        //    $"Title='{Title}', description='{Description}', publish date='{PublishDate}', expiration date='{ExpirationDate}', publish locations: {string.Join(',', PublishLocations)}";
    }
}
