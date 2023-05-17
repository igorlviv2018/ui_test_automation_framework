using Taf.UI.Core.Enums;
using System;

namespace Taf.UI.Core.Models.Taf
{
    public class ContentItem
    {
        public ContentItemType ContentItemType { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Link { get; set; } = string.Empty;

        public string Id { get; set; } = string.Empty;

        public string DeviceModel { get; set; } = string.Empty;

        public bool IsValid { get { return !(string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Id)); } }

        public static bool operator ==(ContentItem a1, ContentItem a2)
        {
            if (ReferenceEquals(a1, a2))
            {
                return true;
            }

            if ((object)a1 == null || (object)a2 == null)
            {
                return false;
            }

            return a1.Title.StartsWith(a2.Title) && a1.Id == a2.Id;
        }

        public static bool operator !=(ContentItem a1, ContentItem a2) => !(a1 == a2);

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ContentItem))
            {
                return false;
            }
            else
            {
                return this == (ContentItem)obj;
            }
        }

        public override string ToString() => $"Item: '{Title}', id='{Id}'";
    }
}
