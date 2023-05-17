using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Taf.UI.Core.Models
{
    public partial class TafEmArticleElement
    {
        private readonly DataHelper dataHelper = new DataHelper();

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Id { get; set; } = -1;

        public ArticleContentElementType ElementType { get; set; }

        public int ElementPosition { get; set; } = 1; //postion of a nested in any layout element, starting from 1, by defaul 1

        public string Trait { get; set; } = string.Empty; // can be used for distinguishing an accordion in a short test data description

        public bool IsTested { get; set; } = false;

        public List<int> ChildrenIds { get; set; } = new List<int>();

        public TafEmArticleElement Parent { get; set; } = null;

        public TafEmArticleElement IntConnectorConnectionPoint { get; set; } = null;

        public object Data { get; set; }

        public List<TafEmArticleElement> DecisionBranches { get; set; } = new List<TafEmArticleElement>();

        public List<TafEmArticleElement> PathToElement { get; set; } = new List<TafEmArticleElement>();

        public List<TafEmArticleElement> TreeTraversalSequence { get; set; } = new List<TafEmArticleElement>();

        public List<int> ProcessedInternalConnectorIds { get; set; } = new List<int>();

        //public int CollapseCount { get; set; } = 0;

        public string XPath { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{ElementType} (title={Title}), id={Id}";
        }

        public void SetParent(TafEmArticleElement parent)
        {
            if (Id != -1 && !parent.ChildrenIds.Contains(Id))
            {
                parent.ChildrenIds.Add(Id);
            }
        }

        // BFS algorithm is used
        public IEnumerable<ArticlePath> FindArticlePaths(List<TafEmArticleElement> articleElements)
        {
            TreeTraversalSequence.Clear();

            ProcessedInternalConnectorIds.Clear();

            var queue = new Queue<ArticlePath>();

            TafEmArticleElement root = articleElements[0];

            queue.Enqueue(new ArticlePath() { Element = root, Path = new List<TafEmArticleElement>() { root } });

            int position;

            while (queue.Any())
            {
                var node = queue.Dequeue();

                if (node.Element.ElementType == ArticleContentElementType.InternalConnector)
                {
                    int internalConnectorId = node.Element.Id;

                    if (ProcessedInternalConnectorIds.Contains(internalConnectorId))
                    {
                        yield return node;

                        continue;
                    }
                    else
                    {
                        ProcessedInternalConnectorIds.Add(internalConnectorId);
                    }
                }

                if (node.Element.ChildrenIds.Any())
                {
                    position = 1;

                    foreach (var childId in node.Element.ChildrenIds)
                    {
                        ArticlePath articlePath = new ArticlePath();

                        articlePath.Element = dataHelper.GetElementById(childId, articleElements);

                        articlePath.Element.ElementPosition = position;

                        if (!node.Path.Any(x => x.ElementType == ArticleContentElementType.InternalConnector)
                            && !node.Path.Any(x => x.ElementType == ArticleContentElementType.ExternalConnector))
                        {
                            articlePath.Element.PathToElement = node.Path;

                            TreeTraversalSequence.Add(articlePath.Element);
                        }

                        if (node.Element.ElementType != ArticleContentElementType.InternalConnector)
                        {
                            articlePath.Element.Parent = node.Element;
                        }

                        articlePath.Path.AddRange(node.Path);

                        articlePath.Path.Add(articlePath.Element);

                        queue.Enqueue(articlePath);

                        position++;
                    }
                }
                else
                {
                    yield return node;
                }
            }
        }
    }
}
