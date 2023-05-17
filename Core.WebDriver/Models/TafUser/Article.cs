namespace Taf.UI.Core.Models.Taf
{
    public class Article
    {
        public string Title { get; set; } = string.Empty;

        public string Link { get; set; } = string.Empty;

        public string Id { get; set; } = string.Empty;

        public static bool operator ==(Article a1, Article a2)
        {
            if (ReferenceEquals(a1, a2))
            {
                return true;
            }

            if ((object)a1 == null || (object)a2 == null)
            {
                return false;
            }

            return a1.Title == a2.Title && a1.Id == a2.Id;
        }

        public static bool operator !=(Article a1, Article a2) => !(a1 == a2);

        public override string ToString() => $"Article: {Title}, id={Id}";
    }
}
