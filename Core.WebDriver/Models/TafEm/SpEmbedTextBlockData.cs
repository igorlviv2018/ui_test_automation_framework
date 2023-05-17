using System.Collections.Generic;

namespace Taf.UI.Core.Models
{
    public class TafEmTextBlockData
    {
        public bool HasOrderedList { get; set; }

        public bool HasUnorderedList { get; set; }

        public string Text { get; set; } = "Plain text: ";

        public List<TafEmLinkData> Links { get; set; } = new List<TafEmLinkData>();

        public void AddLink(string linkText, string linkUri)
        {
            Links.Add(new TafEmLinkData() 
            { 
                LinkText = linkText,
                LinkUri = linkUri
            });
        }
    }
}
