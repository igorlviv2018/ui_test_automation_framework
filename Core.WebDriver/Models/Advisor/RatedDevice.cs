using System.Collections.Generic;

namespace Taf.UI.Core.Models.TafAd
{
    public class RatedDevice
    {
        public string Manufacturer { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public int Rating { get; set; } = 0;

        public double TotalRating { get; set; } = 0;

        public List<double> RelativeRatings { get; set; } = new List<double>();

        public int Interval { get; set; } = -1; // interval parameters only

        public double IntervalRating { get; set; } = 0; // interval parameters only

        //public override string ToString() =>
        //    $"Title='{Title}', description='{Description}', publish date='{PublishDate}', expiration date='{ExpirationDate}', publish locations: {string.Join(',', PublishLocations)}";
    }
}
