using Taf.UI.Core.Enums;
using Taf.UI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Taf.UI.Core.Helpers
{
    public class DxFlowHelper
    {
        public List<int> TestedPathItemIds { get; set; } = new List<int>();

        public List<int> IdsOfProcessesWithClickedNextStepBtn { get; set; } = new List<int>();

        public Dictionary<int, int> ProcessedDecisons { get; set; } = new Dictionary<int, int>(); // <decisionId, branchId>

        public Dictionary<int, List<ProcessButtonClickAction>> TestedProcessButtons { get; set; } = new Dictionary<int, List<ProcessButtonClickAction>>();

        public List<int> AdditionallyTestedRestartFlowButtonProcessIds { get; set; } = new List<int>();

        private readonly DataHelper dataHelper = new DataHelper();

        public void ClearTestedItemIds()
        {
            TestedPathItemIds.Clear();

            IdsOfProcessesWithClickedNextStepBtn.Clear();

            ProcessedDecisons.Clear();

            TestedProcessButtons.Clear();

            AdditionallyTestedRestartFlowButtonProcessIds.Clear();
        }

        public void SaveIdOfProcessWithClickedNextStep(int processId)
        {
            if (!IdsOfProcessesWithClickedNextStepBtn.Contains(processId))
            {
                IdsOfProcessesWithClickedNextStepBtn.Add(processId);
            }
        }

        public void MarkPathItemAsTested(int pathItemId)
        {
            if (!TestedPathItemIds.Contains(pathItemId))
            {
                TestedPathItemIds.Add(pathItemId);
            }
        }

        public bool IsPathItemTested(int pathItemId) => TestedPathItemIds.Contains(pathItemId);

        /// <summary>
        /// Get expected path items after selecting an answer in a Decision or clicking 'Next step' button in a Process
        /// </summary>
        /// <param name="path"></param>
        /// <param name="currentPathPosition">position in an authoring path to start from</param>
        /// <returns>Dx flow expected path items</returns>
        public DxFlowPath GetExpectedPathItemsAfterMovingForward(List<TafEmArticleElement> path, int currentPathPosition, bool isFirstPath)
        {
            int itemToProcessPosition = GetItemToProcessPosition(path, currentPathPosition);

            int nextPosition = GetItemToProcessPosition(path, itemToProcessPosition + 1);

            if (currentPathPosition == 0 && itemToProcessPosition != path.Count - 1 && isFirstPath)
            {
                nextPosition = itemToProcessPosition;

                itemToProcessPosition = 0;
            }

            DxFlowPath flowPath = new DxFlowPath
            {
                ItemToProcessPosition = itemToProcessPosition,

                NextItemToProcessPosition = nextPosition //next position (also end of a subpath)
            };

            for (int i = 0; i <= nextPosition; i++)
            {
                DxFlowPathItemData item = GetExpectedPathItem(path, i);

                if (item.ItemType == PathItemType.Decision && i < nextPosition)
                {
                    item.IsProcessed = true; //mark any decision except the last one as 'processed'
                }

                if (item.ItemType != PathItemType.Undefined)
                {
                    flowPath.Items.Add(item);

                    if (itemToProcessPosition == 0 || i <= itemToProcessPosition)
                    {
                        flowPath.ItemsBeforeMoveForward.Add(item);
                    }
                }
            }

            SetLastDecisionAsUnprocessed(flowPath.ItemsBeforeMoveForward);

            return flowPath;
        }

        public List<DxFlowPathItemData> GetInitialPathItems(DxFlowPath flowPath)
        {
            List<DxFlowPathItemData> itemsBeforeMoveForward = flowPath.ItemsBeforeMoveForward;

            List<DxFlowPathItemData> initialItems = new List<DxFlowPathItemData>();

            foreach (var item in itemsBeforeMoveForward)
            {
                initialItems.Add(item);

                if (item.ItemType == PathItemType.Decision || item.ItemType == PathItemType.Process
                    && HasProcessButton(item, ProcessButtonClickAction.NextStep))
                {
                    break;
                }
            }

            SetLastDecisionAsUnprocessed(initialItems);

            return initialItems;
        }

        // to debug
        public List<DxFlowPathItemData> GetInitialPathItemsInExternalFlow(DxFlowPath flowPath)
        {
            List<DxFlowPathItemData> initialItems = new List<DxFlowPathItemData>();

            int externalConnectorPosition = GetExternalConnectorPosition(flowPath);

            if (externalConnectorPosition == -1)
            {
                return initialItems;
            }

            List<DxFlowPathItemData> items = flowPath.Items;

            for (int i = 0; i < items.Count; i++)
            {
                initialItems.Add(items[i]);

                if (i > externalConnectorPosition && (items[i].ItemType == PathItemType.Decision 
                    || items[i].ItemType == PathItemType.Process
                    && HasProcessButton(items[i], ProcessButtonClickAction.NextStep)))
                {
                    break;
                }
            }

            SetLastDecisionAsUnprocessed(initialItems);

            return initialItems;
        }

        public void SetLastDecisionAsUnprocessed(List<DxFlowPathItemData> pathItems)
        {
            if (pathItems.Count > 0 && pathItems.Last().ItemType == PathItemType.Decision)
            {
                DxFlowPathItemData item = pathItems.Last().Clone();

                item.IsProcessed = false;

                pathItems[^1] = item;
            }
        }

        public DxFlowProcessButtonPosition GetUntestedButtonPositionAfterMovingForward(DxFlowPath flowPath, ProcessButtonClickAction buttonClickAction)
        {
            List<DxFlowPathItemData> itemsBeforeMoveForward = flowPath.ItemsBeforeMoveForward;

            List<DxFlowPathItemData> itemsAfterMoveForward = flowPath.Items;

            int startPosition = itemsBeforeMoveForward.Count != itemsAfterMoveForward.Count
                ? itemsBeforeMoveForward.Count
                : 0;

            DxFlowProcessButtonPosition buttonPosition = new DxFlowProcessButtonPosition();

            for (int i = startPosition; i < itemsAfterMoveForward.Count; i++)
            {
                buttonPosition = UntestedButtonPosition(itemsAfterMoveForward[i], buttonClickAction, i + 1);

                if (IsProcessButtonPositionValid(buttonPosition))
                {
                    break;
                }
            }

            return buttonPosition;
        }

        public DxFlowProcessButtonPosition GetRestartFlowButtonToAdditionalyTestPosition(DxFlowPath flowPath, bool searchInExternalFlow=false)
        {
            List<DxFlowPathItemData> pathItems = flowPath.Items;

            int startPosition = 0;

            int endPosition = pathItems.Count - 1;

            int extConnectorPosition = GetExternalConnectorPosition(flowPath);

            if (extConnectorPosition != -1 && searchInExternalFlow)
            {
                startPosition = extConnectorPosition;
            }

            if (extConnectorPosition != -1 && !searchInExternalFlow)
            {
                endPosition = extConnectorPosition;
            }

            DxFlowProcessButtonPosition buttonPosition = new DxFlowProcessButtonPosition();

            if (extConnectorPosition == -1 && searchInExternalFlow)
            {
                return buttonPosition;
            }

            // search for Process with Restart flow btn that has a subsequent decision or a process with Next step button
            for (int i = startPosition; i <= endPosition; i++)
            {
                if (pathItems[i].ItemType == PathItemType.Process
                    && HasProcessButton(pathItems[i], ProcessButtonClickAction.RestartFlow)
                    && GetProcessWithNextStepBtnOrDecisionId(flowPath, i, endPosition) != -1)
                {
                    buttonPosition = GetButtonInProcessPosition(flowPath, pathItems[i].AssociatedArticleElementId, ProcessButtonClickAction.RestartFlow);

                    break;
                }
            }

            return buttonPosition;
        }

        public int GetExternalConnectorPosition(DxFlowPath renderedPath) =>
            renderedPath.Items.FindIndex(i => i.ItemType == PathItemType.ExternalConnector);

        public int GetExternalConnectorPosition(List<TafEmArticleElement> path) =>
            path.FindIndex(i => i.ElementType == ArticleContentElementType.ExternalConnector);

        public DxFlowPathItemData GetExpectedPathItem(List<TafEmArticleElement> path, int elementPosition)
        {
            TafEmArticleElement articleElement = elementPosition < path.Count
                ? path[elementPosition]
                : new TafEmArticleElement();

            DxFlowPathItemData pathItem = new DxFlowPathItemData()
            {
                AssociatedArticleElementId = articleElement.Id,
                Title = articleElement.Title,
                Description = articleElement.Description
            };

            if (articleElement.ElementType == ArticleContentElementType.Decision)
            {
                pathItem.ItemType = PathItemType.Decision;
            }
            else if (articleElement.ElementType == ArticleContentElementType.Process)
            {
                pathItem.ItemType = PathItemType.Process;
            }
            else if (articleElement.ElementType == ArticleContentElementType.PredefinedProcess)
            {
                pathItem.ItemType = PathItemType.PredefinedProcess;
            }
            else if (articleElement.ElementType == ArticleContentElementType.Terminator)
            {
                pathItem.ItemType = PathItemType.Terminator;
            }
            else if (articleElement.ElementType == ArticleContentElementType.ExternalConnector)
            {
                pathItem.ItemType = PathItemType.ExternalConnector;
            }

            pathItem.Data = articleElement.ElementType == ArticleContentElementType.Decision
                ? GetDecisionPathItemData(path, elementPosition)
                : path[elementPosition].Data;

            return pathItem;
        }

        public DxFlowDecisionBlockData GetDecisionPathItemData(List<TafEmArticleElement> path, int decisionPosition)
        {
            DxFlowDecisionBlockData decisionBlockData = new DxFlowDecisionBlockData();

            if (path[decisionPosition].ElementType == ArticleContentElementType.Decision)
            {
                if (decisionPosition + 1 < path.Count && path[decisionPosition + 1].ElementType == ArticleContentElementType.Branch)
                {
                    TafEmArticleElement branch = path[decisionPosition + 1];

                    TafEmBranchData branchData = dataHelper.GetElementData<TafEmBranchData>(branch);

                    decisionBlockData.SelectedAnswerData = branchData;

                    decisionBlockData.AnswerToSelectBranchId = branch.Id;

                    decisionBlockData.AnswerToSelectPosition = branch.ElementPosition;
                }

                decisionBlockData.AnswersData = GetDecisionAnswersData(path[decisionPosition]);

                TafEmDecisionData decisionData = dataHelper.GetElementData<TafEmDecisionData>(path[decisionPosition]);

                decisionBlockData.HasBranchesButtonView = decisionData.HasBranchesButtonView;
            }

            return decisionBlockData;
        }

        public int GetDecisionActiveBranchId(List<TafEmArticleElement> path, int decisionPosition)
        {
            int id = -1;

            if (decisionPosition + 1 < path.Count && path[decisionPosition + 1].ElementType == ArticleContentElementType.Branch)
            {
                id = path[decisionPosition + 1].Id;
            }

            return id;
        }

        public List<TafEmBranchData> GetDecisionAnswersData(TafEmArticleElement decision) =>
            decision.DecisionBranches.Select(b => (TafEmBranchData)b.Data).ToList();

        /// <summary>
        /// Get first position (in SP Authoring element path) of the decision that is either unprocessed (i.d. without a selected answer)
        /// or is processed but currently selected answer differs from the one to be selected next 
        /// </summary>
        /// <param name="path">path in a Dx Flow</param>
        /// <param name="currentPathPosition">position in a path to start from</param>
        /// <returns>position of a decision (that meet conditions) else or -1 if no next specific decision is found</returns>
        public int GetNextSpecificDecisionPosition(List<TafEmArticleElement> path, int currentPathPosition)
        {
            int nextDecisionPosition = -1;

            for (int i = currentPathPosition; i < path.Count; i++)
            {
                if (path[i].ElementType == ArticleContentElementType.Decision && !SkipDecision(path, i))
                {
                    nextDecisionPosition = i;

                    break;
                }
            }

            return nextDecisionPosition;
        }

        public int GetNextProcessWithUnclickedNextStepBtn(List<TafEmArticleElement> path, int currentPathPosition)
        {
            int nextProcessPosition = -1;

            for (int i = currentPathPosition; i < path.Count; i++)
            {
                int elementId = path[i].Id;

                if (path[i].ElementType == ArticleContentElementType.Process 
                    && HasProcessButton(path[i], ProcessButtonClickAction.NextStep)
                    && !IdsOfProcessesWithClickedNextStepBtn.Contains(elementId))
                {
                    nextProcessPosition = i;

                    break;
                }
            }

            return nextProcessPosition;
        }

        public int GetProcessWithNextStepBtnOrDecisionId(DxFlowPath flowPath, int startPosition, int endPosition)
        {
            int pathItemId = -1;

            List<DxFlowPathItemData> pathItems = flowPath.Items;

            for (int i = startPosition; i <= endPosition; i++)
            {
                if (pathItems[i].ItemType == PathItemType.Decision
                    || (pathItems[i].ItemType == PathItemType.Process
                        && HasProcessButton(pathItems[i], ProcessButtonClickAction.NextStep))
                    )
                {
                    pathItemId = pathItems[i].AssociatedArticleElementId;

                    break;
                }
            }

            return pathItemId;
        }

        public int SelectItemToProcessPosition(int decisionToProcessPosition, int processWithNextStepBtnPosition)
        {
            int itemToProcessPosition = -1;

            if (decisionToProcessPosition == -1 && processWithNextStepBtnPosition == -1)
            {
                itemToProcessPosition = -1;
            }
            else if (decisionToProcessPosition == -1 && processWithNextStepBtnPosition >= 0)
            {
                itemToProcessPosition = processWithNextStepBtnPosition;
            }
            else if (decisionToProcessPosition >= 0 && processWithNextStepBtnPosition == -1)
            {
                itemToProcessPosition = decisionToProcessPosition;
            }
            else if (decisionToProcessPosition >= 0 && processWithNextStepBtnPosition >= 0)
            {
                itemToProcessPosition = decisionToProcessPosition < processWithNextStepBtnPosition
                    ? decisionToProcessPosition
                    : processWithNextStepBtnPosition;
            }

            return itemToProcessPosition;
        }

        public int GetItemToProcessPosition(List<TafEmArticleElement> path, int startPosition)
        {
            if (startPosition == -1 || startPosition >= path.Count - 1)
            {
                return path.Count - 1;
            }

            int nextDecisionPosition = GetNextSpecificDecisionPosition(path, startPosition);

            int nextProcessWithUnclickedNextStepBtnPosition = GetNextProcessWithUnclickedNextStepBtn(path, startPosition);

            int itemToProcessPosition = SelectItemToProcessPosition(nextDecisionPosition, nextProcessWithUnclickedNextStepBtnPosition);

            return itemToProcessPosition != -1 ? itemToProcessPosition : path.Count - 1;
        }

        public bool HasProcessButton(TafEmArticleElement process, ProcessButtonClickAction clickAction)
        {
            bool hasProcessButton = false;

            if (process.ElementType == ArticleContentElementType.Process)
            {
                DxFlowProcessBlockData processData = dataHelper.GetElementData<DxFlowProcessBlockData>(process);

                foreach (var blockData in processData.BlockData)
                {
                    if (blockData.GetType() == typeof(TafEmButtonsBlockData) && HasButtonsBlockButton((TafEmButtonsBlockData)blockData, clickAction))
                    {
                        hasProcessButton = true;

                        break;
                    }
                }
            }

            return hasProcessButton;
        }

        public bool HasProcessButton(DxFlowPathItemData process, ProcessButtonClickAction clickAction) =>
        
            HasProcessButton(
                new TafEmArticleElement()
                {
                    ElementType = ArticleContentElementType.Process,
                    Data = process.Data
                },
                clickAction);

        //to del?
        public bool HasButtonsBlockButton(TafEmButtonsBlockData buttonsBlock, ProcessButtonClickAction clickAction) =>
            buttonsBlock.Buttons.Any(b => b.ClickAction == clickAction);

        public int ButtonPositionInButtonsBlock(TafEmButtonsBlockData buttonsBlock, ProcessButtonClickAction clickAction)
        {
            int position = buttonsBlock.Buttons.FindIndex(b => b.ClickAction == clickAction);

            return position != -1 ? position + 1 : -1;
        }

        public DxFlowProcessButtonPosition UntestedButtonPosition(DxFlowPathItemData pathItem, ProcessButtonClickAction clickAction, int processPosition)
        {
            DxFlowProcessButtonPosition buttonPosition = new DxFlowProcessButtonPosition();

            bool isProcessOrTerminator = pathItem.ItemType == PathItemType.Process || pathItem.ItemType == PathItemType.Terminator;

            if (isProcessOrTerminator && pathItem.Data.GetType() == typeof(DxFlowProcessBlockData))
            {
                DxFlowProcessBlockData processData = (DxFlowProcessBlockData)pathItem.Data;

                for (int i = 0; i < processData.BlockData.Count; i++)
                {
                    if (processData.BlockData[i].GetType() == typeof(TafEmButtonsBlockData))
                    {
                        TafEmButtonsBlockData buttonsBlock = (TafEmButtonsBlockData)processData.BlockData[i];

                        int btnPosition = ButtonPositionInButtonsBlock(buttonsBlock, clickAction);

                        int untestedBtnPosition = btnPosition != -1 && !IsButtonInProcessValidated(pathItem, clickAction)
                            ? btnPosition
                            : -1;

                        buttonPosition.ButtonPosition = untestedBtnPosition;

                        buttonPosition.ButtonsBlockPosition = i + 1;

                        buttonPosition.ProcessPosition = processPosition;

                        buttonPosition.ProcessId = pathItem.AssociatedArticleElementId;

                        if (untestedBtnPosition > -1)
                        {
                            buttonPosition.ButtonData = buttonsBlock.Buttons[untestedBtnPosition - 1];

                            break;
                        }
                    }
                }
            }

            return buttonPosition;
        }

        public void MarkButtonAsTested(int pathItemId, ProcessButtonClickAction clickAction)
        {
            if (TestedProcessButtons.ContainsKey(pathItemId))
            {
                TestedProcessButtons[pathItemId].Add(clickAction);
            }
            else
            {
                TestedProcessButtons[pathItemId] = new List<ProcessButtonClickAction> { clickAction };
            }
        }

        public void MarkRestartFlowButtonAsExtraTested(int processId)
        {
            if (!AdditionallyTestedRestartFlowButtonProcessIds.Contains(processId))
            {
                AdditionallyTestedRestartFlowButtonProcessIds.Add(processId);
            }
        }

        public bool IsButtonInProcessValidated(DxFlowPathItemData pathItem, ProcessButtonClickAction clickAction) =>
            TestedProcessButtons.ContainsKey(pathItem.AssociatedArticleElementId) 
            && TestedProcessButtons[pathItem.AssociatedArticleElementId].Contains(clickAction);

        public bool SkipDecision(List<TafEmArticleElement> path, int decisionPosition)
        {
            int branchId = decisionPosition + 1 < path.Count
                ? path[decisionPosition + 1].Id
                : -1;

            int decisionId = path[decisionPosition].Id;
            
            return !IsDecisionUnderInternalConnector(path, decisionPosition)
                && ProcessedDecisons.Contains(new KeyValuePair<int, int>(decisionId, branchId));
        }

        public bool IsDecisionUnderInternalConnector(List<TafEmArticleElement> path, int decisionPosition)
        {
            int internalConnectorPosition = path.FindIndex(e => e.ElementType == ArticleContentElementType.InternalConnector);

            return internalConnectorPosition != -1 && internalConnectorPosition < decisionPosition;
        }

        public int DecisionAnswerToSelectPosition(DxFlowPath renderedPath, int decisionId)
        {
            int renderedDecisionPos = GetRenderedPathItemPosition(renderedPath.Items, decisionId);

            return renderedDecisionPos != -1
                ? ((DxFlowDecisionBlockData)renderedPath.Items[renderedDecisionPos - 1].Data).AnswerToSelectPosition
                : renderedDecisionPos;
        }

        public string DecisionAnswerToSelect(DxFlowPath renderedPath, int decisionId)
        {
            int renderedDecisionPos = GetRenderedPathItemPosition(renderedPath.Items, decisionId);

            return renderedDecisionPos != -1
                ? ((DxFlowDecisionBlockData)renderedPath.Items[renderedDecisionPos - 1].Data).SelectedAnswerData.Answer
                : string.Empty;
        }

        public bool HasDecisionButtonView(DxFlowPath renderedPath, int decisionId)
        {
            int renderedDecisionPos = GetRenderedPathItemPosition(renderedPath.Items, decisionId);

            return renderedDecisionPos != -1
                && ((DxFlowDecisionBlockData)renderedPath.Items[renderedDecisionPos - 1].Data).HasBranchesButtonView;
        }

        /// <summary>
        /// Get path item position (counting from 1)
        /// </summary>
        /// <param name="pathItems">List of DxFlowPathItemData</param>
        /// <param name="associatedArticleElementId"></param>
        /// <returns>path item position (counting from 1)</returns>
        public int GetRenderedPathItemPosition(List<DxFlowPathItemData> pathItems, int associatedArticleElementId)
        {
            int position = pathItems.FindIndex(x => x.AssociatedArticleElementId == associatedArticleElementId);

            return position != -1 ? position + 1 : -1;
        }

        public DxFlowProcessButtonPosition GetButtonInProcessPosition(DxFlowPath renderedPath, int processId, ProcessButtonClickAction buttonAction)
        {
            int renderedProcessPos = GetRenderedPathItemPosition(renderedPath.Items, processId);

            DxFlowProcessButtonPosition buttonPos = new DxFlowProcessButtonPosition() { ProcessPosition = renderedProcessPos };

            if (renderedProcessPos == -1)
            {
                return buttonPos;
            }

            List<object> blockData = ((DxFlowProcessBlockData)renderedPath.Items[renderedProcessPos - 1].Data).BlockData;

            for(int i = 0; i < blockData.Count; i++)
            {
                if (blockData[i].GetType() == typeof(TafEmButtonsBlockData)
                    && ButtonPositionInButtonsBlock((TafEmButtonsBlockData)blockData[i], buttonAction) > -1)
                {
                    buttonPos.ButtonPosition = ButtonPositionInButtonsBlock((TafEmButtonsBlockData)blockData[i], buttonAction);

                    buttonPos.ButtonsBlockPosition = i + 1;

                    buttonPos.ProcessId = processId;

                    break;
                }
            }

            return buttonPos;
        }

        public bool IsProcessButtonPositionValid(DxFlowProcessButtonPosition buttonPosition) =>
            
            buttonPosition.ProcessPosition != -1
            && buttonPosition.ButtonsBlockPosition != -1
            && buttonPosition.ButtonPosition != -1;

        public bool HasDxFlowPathItemWithId(List<DxFlowPathItemData> pathItems, int pathItemId) =>
            
            pathItems.Any(i => i.AssociatedArticleElementId == pathItemId);

        public void ClearProcessedItemsIds()
        {
            IdsOfProcessesWithClickedNextStepBtn.Clear();
            
            ProcessedDecisons.Clear();
        }

        public void ClearProcessedExternalConnectorItemsIds(List<TafEmArticleElement> path)
        {
            List<int> extConnectorIds = new List<int>();

            int extConnectorPosition = GetExternalConnectorPosition(path);

            if (extConnectorPosition != -1)
            {
                for (int i = extConnectorPosition; i < path.Count; i++)
                {
                    extConnectorIds.Add(path[i].Id);
                }

                IdsOfProcessesWithClickedNextStepBtn.RemoveAll(id => extConnectorIds.Contains(id));

                foreach (var toDel in ProcessedDecisons.Where(kv => extConnectorIds.Contains(kv.Key)))
                {
                    ProcessedDecisons.Remove(toDel.Key);
                }
            }
        }
    }
}
