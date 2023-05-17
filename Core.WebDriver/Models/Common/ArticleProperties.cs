using Taf.UI.Core.Enums;
using Taf.UI.Core.Models.TafAuth;
using System.Collections.Generic;

namespace Taf.UI.Core.Models
{
    public class ArticleProperties
    {
        public ArticleType ArticleType { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string WorkNote { get; set; } = string.Empty;

        public string CurrentOwner { get; set; }

        public List<string> OwnerList { get; set; }

        public string PublishDate { get; set; } = "Pending";

        public string ExpirationDate { get; set; } = "Evergreen";

        public List<App> PublishLocations { get; set; } = new List<App>();

        public PublishChannelsOptions PublishChannelsOptions { get; set; } = new PublishChannelsOptions(); 

        public override string ToString() =>
            $"Title='{Title}', description='{Description}', publish date='{PublishDate}', expiration date='{ExpirationDate}', publish locations: {string.Join(',', PublishLocations)}";
    }
}
