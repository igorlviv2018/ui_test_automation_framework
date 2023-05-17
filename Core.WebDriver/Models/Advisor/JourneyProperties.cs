using Taf.UI.Core.Enums;
using System.Collections.Generic;

namespace Taf.UI.Core.Models
{
    public class JourneyProperties
    {
        public JourneyType JourneyType { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string WorkNote { get; set; } = string.Empty;

        public string CurrentOwner { get; set; }

        public List<string> OwnerList { get; set; }

        public List<App> PublishLocations { get; set; } = new List<App>();

        //public PublishChannelsOptions PublishChannelsOptions { get; set; } = new PublishChannelsOptions(); 

        //public override string ToString() =>
        //    $"Title='{Title}', description='{Description}', publish date='{PublishDate}', expiration date='{ExpirationDate}', publish locations: {string.Join(',', PublishLocations)}";
    }
}
