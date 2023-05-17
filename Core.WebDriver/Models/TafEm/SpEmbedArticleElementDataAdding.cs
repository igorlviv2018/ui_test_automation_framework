using Taf.UI.Core.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Taf.UI.Core.Models
{
    public partial class TafEmArticleElement
    {
        public TafEmArticleElement CreateArticeElementAndAddToElementList(List<TafEmArticleElement> elementsInArticle, ArticleContentElementType type, 
            int position = 0, TafEmArticleElement parent = null, string title = "", string description = "", object data = null)
        {
            TafEmArticleElement articleElement = new TafEmArticleElement()
            {
                Title = title,

                Description = description,

                ElementType = type,

                ElementPosition = position,

                Data = data,

                Id = GenerateElementId(elementsInArticle)
            };

            elementsInArticle.Add(articleElement);

            if (parent != null)
            {
                articleElement.SetParent(parent);
            }

            return articleElement;
        }

        public int GenerateElementId(List<TafEmArticleElement> elementsAlreadyOnTree)
        {
            int id = 0;

            if (elementsAlreadyOnTree.Count != 0)
            {
                foreach (var element in elementsAlreadyOnTree)
                {
                    if (element.Id > id)
                    {
                        id = element.Id;
                    }
                }
            }

            //int max = elementsAlreadyOnTree.Max(e => e.Id);

            return ++id;
        }

        // ---- Custom article methods ----
        public TafEmArticleElement CreateRoot(List<TafEmArticleElement> articleElements, string trait = "")
        {
            TafEmArticleElement root = CreateArticeElementAndAddToElementList(articleElements, ArticleContentElementType.Root, 0, null,
                    "root", "");

            root.Trait = trait;

            return root;
        }

        public TafEmArticleElement AddText(List<TafEmArticleElement> articleElements, string title = "", string description = "", string parentTrait="")
        {
            TafEmArticleElement text = CreateArticeElementAndAddToElementList(articleElements, ArticleContentElementType.Text, 0,
                GetParent(articleElements, parentTrait), title, description, new TafEmTextBlockData());

            return text;
        }

        public TafEmArticleElement AddTextLinks(params string[] linkParams)
        {
            if (ElementType == ArticleContentElementType.Text)
            {
                AddLinksToTextData((TafEmTextBlockData)Data, linkParams);
            }

            return this;
        }

        public TafEmArticleElement AddAccordion(List<TafEmArticleElement> articleElements, string title="", string trait = "", bool addToRoot=false)
        {
            TafEmArticleElement parent = addToRoot    //debug
                ? articleElements.Where(e => e.ElementType == ArticleContentElementType.Root).FirstOrDefault()
                : this;

            TafEmArticleElement accord = CreateArticeElementAndAddToElementList(articleElements, ArticleContentElementType.Accordion, 0, parent, title);

            accord.Trait = trait;

            return accord;
        }

        public TafEmArticleElement AddCollapse(List<TafEmArticleElement> articleElements, string title = "", string accordTrait = "")
        {
            TafEmArticleElement accordToAddCollapseTo;

            if (string.IsNullOrEmpty(accordTrait))
            {
                accordToAddCollapseTo = articleElements
                    .Where(e => e.ElementType == ArticleContentElementType.Accordion).LastOrDefault();
            }
            else
            {
                accordToAddCollapseTo = articleElements
                        .Where(e => e.ElementType == ArticleContentElementType.Accordion && e.Trait == accordTrait).FirstOrDefault();
            }

            return CreateArticeElementAndAddToElementList(articleElements, ArticleContentElementType.Collapse, 0,
                accordToAddCollapseTo, title);
        }

        public TafEmArticleElement AddVideo(List<TafEmArticleElement> articleElements, string url, string title = "", string parentTrait = "")
        {
            TafEmArticleElement video = CreateArticeElementAndAddToElementList(articleElements, ArticleContentElementType.Video, 0, 
                GetParent(articleElements, parentTrait), title, "", new TafEmVideoBlockData(){ VideoUrl = url });

            return video;
        }

        public TafEmArticleElement AddImage(List<TafEmArticleElement> articleElements, string title = "", string filePath="", string parentTrait = "")
        {
            TafEmArticleElement image = CreateArticeElementAndAddToElementList(articleElements, ArticleContentElementType.Image, 0,
                GetParent(articleElements, parentTrait), title, "",
                data: new TafEmImageData()
                {
                    FilePath = filePath
                });

            return image;
        }

        public TafEmArticleElement AddStepByStep(List<TafEmArticleElement> articleElements, string title = "", string description = "", string parentTrait = "")
        {
            TafEmArticleElement stepByStep = CreateArticeElementAndAddToElementList(articleElements, ArticleContentElementType.StepByStep, 0,
                GetParent(articleElements, parentTrait), title, description, new TafEmStepByStepData());

            return stepByStep;
        }

        public TafEmArticleElement AddButtonsBlock(List<TafEmArticleElement> articleElements, string title="", string parentTrait = "")
        {
            TafEmArticleElement buttons = CreateArticeElementAndAddToElementList(articleElements, ArticleContentElementType.ButtonsBlock, 0,
                GetParent(articleElements, parentTrait), title, "", new TafEmButtonsBlockData());

            return buttons;
        }

        public TafEmArticleElement GetParent(List<TafEmArticleElement> articleElements, string parentTrait) => !string.IsNullOrEmpty(parentTrait)
                ? articleElements.Where(e => e.Trait == parentTrait).FirstOrDefault()
                : this;

        // ---- DF methods -----
        public TafEmArticleElement AddVideo(string url)
        {
            InitProcessData();

            ((DxFlowProcessBlockData)Data).AddVideoData(url);

            return this;
        }

        public TafEmArticleElement AddImage(string filePath, string originalFileName = "")
        {
            InitProcessData();

            ((DxFlowProcessBlockData)Data).AddImageData(filePath, originalFileName);

            return this;
        }

        public TafEmArticleElement AddStepByStepBlock()
        {
            InitProcessData();

            ((DxFlowProcessBlockData)Data).AddBlockData(new TafEmStepByStepData());

            return this;
        }

        public TafEmArticleElement AddStep(string title, string imageFilePath="", params string[] linkParams)
        {
            if (ElementType == ArticleContentElementType.StepByStep) // Custom article step-by-step
            {
                ((TafEmStepByStepData)Data).AddStepData(title, imageFilePath, linkParams);
            }
            else // step-by-step in a DF process block
            {
                InitProcessData();

                List<object> blockData = ((DxFlowProcessBlockData)Data).BlockData;

                if (blockData.Count == 0 || blockData.Last().GetType() != typeof(TafEmStepByStepData))
                {
                    ((DxFlowProcessBlockData)Data).AddBlockData(new TafEmStepByStepData());
                }
                
                ((TafEmStepByStepData)blockData.Last()).AddStepData(title, imageFilePath, linkParams);
            }

            return this;
        }

        public TafEmArticleElement AddButtonsBlock()
        {
            InitProcessData();

            ((DxFlowProcessBlockData)Data).AddBlockData(new TafEmButtonsBlockData());

            return this;
        }

        public TafEmArticleElement AddButton(ProcessButtonClickAction buttonClickAction, string label, string url = "", LinkButtonTarget linkButtonTarget=LinkButtonTarget.NewTab)
        {
            if (Data.GetType() == typeof(DxFlowProcessBlockData)) // DF process
            {
                InitProcessData();

                List<object> blockData = ((DxFlowProcessBlockData)Data).BlockData;

                if (blockData.Count == 0 || blockData.Last().GetType() != typeof(TafEmButtonsBlockData))
                {
                    ((DxFlowProcessBlockData)Data).AddBlockData(new TafEmButtonsBlockData());
                }

                ((TafEmButtonsBlockData)blockData.Last()).AddButton(buttonClickAction, label, url, linkButtonTarget);

            }

            if (Data.GetType() == typeof(TafEmButtonsBlockData)) // Custom article buttons block
            {
                ((TafEmButtonsBlockData)Data).AddButton(buttonClickAction, label, url, linkButtonTarget);
            }

            return this;
        }

        public TafEmArticleElement AddTextLink(string linkText, string uri)
        {
            InitProcessData();

            List<object> blockData = ((DxFlowProcessBlockData)Data).BlockData;

            if (blockData.Count == 0 || blockData.Last().GetType() != typeof(TafEmTextBlockData))
            {
                ((DxFlowProcessBlockData)Data).AddBlockData(new TafEmTextBlockData());
            }

            ((TafEmTextBlockData)blockData.Last()).AddLink(linkText, uri);

            return this;
        }

        public TafEmArticleElement AddText(params string[] linkParams)
        {
            InitProcessData();

            TafEmTextBlockData textData = new TafEmTextBlockData();

            AddLinksToTextData(textData, linkParams);

            ((DxFlowProcessBlockData)Data).AddBlockData(textData);

            return this;
        }

        public TafEmArticleElement AddTable()
        {
            InitProcessData();

            TafEmTableBlockData tableData = new TafEmTableBlockData();

            ((DxFlowProcessBlockData)Data).AddBlockData(tableData);

            return this;
        }

        public void AddLinksToTextData(TafEmTextBlockData textData, params string[] linkParams)
        {
            int paramNum = 0;

            string linkText = "";

            string uri;

            foreach (var linkParam in linkParams)
            {
                if (paramNum % 2 == 0)
                {
                    linkText = linkParam;
                }
                else
                {
                    uri = linkParam;

                    textData.AddLink(linkText, linkParam);
                }

                paramNum++;
            }
        }

        public void InitProcessData()
        {
            if (ElementType != ArticleContentElementType.Process && ElementType != ArticleContentElementType.Terminator)
            {
                return;
            }

            if (Data == null || Data.GetType() != typeof(DxFlowProcessBlockData))
            {
                Data = new DxFlowProcessBlockData();
            }
        }
    }
}
