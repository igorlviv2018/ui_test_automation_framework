using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using System.Collections.Generic;

namespace Taf.UI.Core.Models
{
    public class ArticlePath
    {
        public List<TafEmArticleElement> Path { get; set; } = new List<TafEmArticleElement>();

        public TafEmArticleElement Element { get; set; } = new TafEmArticleElement();

        private readonly DataHelper dataHelper = new DataHelper();

        public override string ToString()
        {
            List<string> path = new List<string>();

            string elementName;

            string shortType;

            foreach (var pathElement in Path)
            {
                if (CommonHelper.IsElementVisible(pathElement))
                {
                    if (pathElement.ElementType == ArticleContentElementType.Branch)
                    {
                        elementName = dataHelper.GetElementData<TafEmBranchData>(pathElement).Answer;
                    }
                    else
                    {
                        elementName = string.IsNullOrEmpty(pathElement.Title) ? "*" : pathElement.Title;
                    }

                    shortType = CommonHelper.GetShortType(pathElement.ElementType);

                    path.Add($"{shortType}: {elementName}(Id={pathElement.Id}, Pos={pathElement.ElementPosition})");
                }
            }

            return string.Join("->", path);
        }
    }
}
