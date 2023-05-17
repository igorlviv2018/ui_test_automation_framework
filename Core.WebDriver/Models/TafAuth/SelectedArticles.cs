using System.Collections.Generic;

namespace Taf.UI.Core.Models.TafAuth
{
    public class SelectedArticles
    {
        public int SelectedArticlesCount => SelectedArticlesTitles.Count;

        public string SelectArticlesError { get; set; } = string.Empty;

        public List<string> SelectedArticlesTitles { get; set; } = new List<string>();
    }
}
