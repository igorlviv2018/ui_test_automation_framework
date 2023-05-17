using NLog;
using Taf.Core.Models.Devices;
using Taf.UI.Core.ExtensionMethods;
using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models.Taf;
using Taf.UI.PageObjects;
using Taf.UI.PageObjects.Taf;
using Taf.UI.Steps.Base;
using Taf.UI.Steps.TafSteps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Taf.UI.Steps
{
    public class SidebarRecentItemsSteps : SidebarBaseSteps
    {
        private readonly DeviceSteps deviceSteps;

        private readonly ArticlesPageSteps articlesPageSteps;

        private readonly Sidebar sidebar;

        private readonly UiDeviceHelper uiDeviceHelper;

        private readonly Spinner spinner;

        private Queue<ContentItem> ExpectedRecentItems = new Queue<ContentItem>();

        public ContentItem LastItemOpenedFromRecentlyViewedList { get; set; } = null;

        private readonly bool isRedesign;

        public SidebarRecentItemsSteps(ILogger logger, bool isRedesign=false) : base(logger)
        {
            deviceSteps = new DeviceSteps(App.Taf, logger, isRedesign);

            articlesPageSteps = new ArticlesPageSteps(logger, isRedesign);

            sidebar = new Sidebar(isRedesign);

            uiDeviceHelper = new UiDeviceHelper(isRedesign);

            spinner = new Spinner(App.Taf, isRedesign);

            this.isRedesign = isRedesign;
        }

        public void OpenRandomItemUsingDeeplinkAndCheckRecentItemsList(ContentItem contentItem, int recentItemsListLength, int openingOrder=0)
        {
            OpenItemUsingDeeplink(contentItem, recentItemsListLength, openingOrder);

            string err = ValidateRecentlyViewedList();

            if (!string.IsNullOrEmpty(err))
            {
                throw new Exception(err);
            }
        }

        public void OpenItemUsingDeeplink(ContentItem contentItem, int recentItemsListLength, int openingOrder=0)
        {
            if (contentItem.ContentItemType == ContentItemType.Article)
            {
                articlesPageSteps.OpenArticleByIdUsingDeeplink(contentItem.Id, isRedesign);
            }
            else if (contentItem.ContentItemType == ContentItemType.Device)
            {
                deviceSteps.SelectDeviceUsingDeeplink(contentItem.Id);
            }

            string itemOrder = openingOrder > 0 ? openingOrder.ToString() + ": ": string.Empty;

            log.Info($"{itemOrder} {contentItem.ContentItemType} opened: '{contentItem.Title}', id: '{contentItem.Id}'");

            sidebar.WaitRecentItemLoading(contentItem);

            AddToExpectedRecentItems(contentItem, recentItemsListLength);
        }

        public void AddToExpectedRecentItems(ContentItem contentItem, int recentItemsListLength = CommonConstants.RecentItemsListDefaultCount)
        {
            if (contentItem.IsValid && !ExpectedRecentItems.Contains(contentItem))
            {
                ExpectedRecentItems.Enqueue(contentItem);
            }

            if (ExpectedRecentItems.Count > recentItemsListLength)
            {
                ExpectedRecentItems.Dequeue();
            }
        }

        public void RemoveFromExpectedRecentItems(ContentItem itemToRemove)
        {
            Queue<ContentItem> updatedQueue = new Queue<ContentItem>();

            foreach (var expItem in ExpectedRecentItems)
            {
                if (expItem != itemToRemove)
                {
                    updatedQueue.Enqueue(expItem);
                }
            }

            ExpectedRecentItems = updatedQueue;
        }

        public void OpenRandomItemsUsingDeeplinkAndCheckRecentItemsList(int itemsCount, int recentItemsListLength = CommonConstants.RecentItemsListDefaultCount)
        {
            ExpectedRecentItems.Clear();

            List<ContentItem> randomItems = GetRandomContentItems(itemsCount);

            if (randomItems.Count != itemsCount)
            {
                throw new Exception($"Check failed: expected items count is {randomItems.Count} but should be {itemsCount}");
            }

            for (int i = 0; i < itemsCount; i++)
            {
                OpenRandomItemUsingDeeplinkAndCheckRecentItemsList(randomItems[i], recentItemsListLength, openingOrder: i+1);
            }
        }

        public void OpenRandomItemsUsingDeeplink(int itemsCount, int recentItemsListLength = CommonConstants.RecentItemsListDefaultCount)
        {
            ExpectedRecentItems.Clear();

            List<ContentItem> randomItems = GetRandomContentItems(itemsCount);

            if (randomItems.Count != itemsCount)
            {
                throw new Exception($"Check failed: expected items count is {randomItems.Count} but should be {itemsCount}");
            }

            for (int i = 0; i < itemsCount; i++)
            {
                OpenItemUsingDeeplink(randomItems[i], recentItemsListLength, openingOrder: i + 1);
            }
        }

        public List<ContentItem> GetRandomContentItems(int itemsCount)
        {
            int articlesCount = DataHelper.GetRandomInteger(itemsCount);

            int devicesCount = itemsCount - articlesCount;

            List<MasterDevice> randomDevices = uiDeviceHelper.GetRandomExpectedDevices(devicesCount);

            List<ContentItem> randomItems = ConvertDevicesToContentItems(randomDevices);

            randomItems.AddRange(articlesPageSteps.GetRandomArticlesAsContentItems(articlesCount, isRedesign));

            randomItems.Shuffle();

            return randomItems;
        }

        public string ValidateRecentlyViewedList()
        {
            List<ContentItem> actualItems = GetActualRecentItems();

            string err = string.Empty;

            if (actualItems.Count != ExpectedRecentItems.Count)
            {
                string actualTitles = string.Join(", ", actualItems.Select(i => i.Title));

                string expectedTitles = string.Join(", ", ExpectedRecentItems.Select(i => i.Title).ToList());

                err = $"Count of recent items is invalid: {actualItems.Count} but expected - {ExpectedRecentItems.Count}. "
                    + $"Actual titles: {actualTitles}, Expected titles: {expectedTitles}.";

                LogHelper.LogError(log, err);

                return err;
            }

            List<ContentItem> expectedItems = ExpectedRecentItems.ToList();

            expectedItems.Reverse();

            for (int i = 0; i < actualItems.Count; i++)
            {
                if (actualItems[i] != expectedItems[i])
                {
                    err = $"{err}Position {i + 1} in 'Recently viewed' menu: actual - {actualItems[i]}, expected - {expectedItems[i]}; ";
                }
            }

            LogHelper.Log(log, "Recently viewed items list validated", err);

            return err;
        }

        public List<ContentItem> GetActualRecentItems()
        {
            List<string> actualItemIdsList = isRedesign ? sidebar.GetRecentItemIds() : sidebar.GetRecentDeviceIds();

            List<string> actualItemTitlesList = isRedesign ? sidebar.GetRecentItemsTitles() : sidebar.GetRecentDeviceNames();

            List<string> actualItemLinksList = isRedesign ? sidebar.GetRecentItemLinks() : sidebar.GetRecentDeviceIds();

            List<ContentItem> items = new List<ContentItem>();

            if (actualItemIdsList.Count != actualItemTitlesList.Count)
            {
                return items;
            }

            for (int i = 0; i < actualItemIdsList.Count; i++)
            {
                items.Add(new ContentItem()
                {
                    ContentItemType = CommonHelper.GetContentItemType(actualItemLinksList[i]),
                    Title = actualItemTitlesList[i],
                    Id = actualItemIdsList[i]
                });
            }

            return items;
        }

        public string ClearRecentItemList()
        {
            string err = string.Empty;

            int count = sidebar.GetRemoveButtonCount();

            while (count > 0)
            {
                sidebar.RemoveRecentItem();

                // wait recent device count decreased
                bool isCountDecreasedByOne = UiWaitHelper.Wait(() => count - sidebar.GetRemoveButtonCount() == 1, WaitConstants.ImplicitWaitInSec);

                if (!isCountDecreasedByOne)
                {
                    err = $"Clear recent item list (one by one): Item count did not decrease by 1 (expected: {count - 1}, actual: {sidebar.GetRemoveButtonCount()})";

                    break;
                }

                count = sidebar.GetRemoveButtonCount();
            }

            return err;
        }

        public void RemoveRandomRecentItem()
        {
            List<ContentItem> expectedRecentItems = ExpectedRecentItems.ToList();

            int recentItemsCount = expectedRecentItems.Count;

            LogHelper.Log(log, $"Expected recent items count: {recentItemsCount}");

            if (recentItemsCount > 0)
            {
                ContentItem randomItem = DataHelper.GetRandomElement(expectedRecentItems);

                string contentItemType = randomItem.ContentItemType.ToString().ToLower();

                sidebar.RemoveRecentItemById(contentItemType, randomItem.Id);

                LogHelper.Log(log, $"'{randomItem.ContentItemType}' removed from recently viewed list: '{randomItem.Title}', id='{randomItem.Id}'");

                RemoveFromExpectedRecentItems(randomItem);
            }
        }

        public void OpenRandomItemFromRecentlyViewedList()
        {
            List<ContentItem> expectedRecentItems = ExpectedRecentItems.ToList();

            int recentItemsCount = expectedRecentItems.Count;

            LogHelper.Log(log, $"Expected recent items count: {recentItemsCount}");

            if (recentItemsCount > 0)
            {
                ContentItem randomItem = DataHelper.GetRandomElement(expectedRecentItems);

                string contentItemType = randomItem.ContentItemType.ToString().ToLower();

                sidebar.OpenRecentItemById(contentItemType, randomItem.Id);

                spinner.WaitTopProgressBarToDisappear();

                LastItemOpenedFromRecentlyViewedList = randomItem;

                LogHelper.Log(log, $"'{randomItem.ContentItemType}' opened from recently viewed list: '{randomItem.Title}', id='{randomItem.Id}'");
            }
        }

        public string CheckItemPageIsOpened(ContentItem item)
        {
            bool isOpened = false;

            if (item.ContentItemType == ContentItemType.Article)
            {
                isOpened = articlesPageSteps.IsArticleOpened(item.Title);
            }
            else if (item.ContentItemType == ContentItemType.Device)
            {
                isOpened = deviceSteps.IsDevicePageOpened(item.DeviceModel);
            }

            string err = isOpened
                ? string.Empty
                : $"'{item.ContentItemType}' page not opened: '{item.Title}', id='{item.Id}'";

            LogHelper.LogResult(log, $"'{item.ContentItemType}' page opened: '{item.Title}', id='{item.Id}'", err);

            return err;
        }

        public string CheckItemPageIsOpened() => CheckItemPageIsOpened(LastItemOpenedFromRecentlyViewedList);

        public string CheckRecentlyViewedListItemIsActive(ContentItem item)
        {
            string contentItemType = item.ContentItemType.ToString().ToLower();

            bool isActive = UiWaitHelper.Wait(() => sidebar.IsRecentItemActive(contentItemType, item.Id), WaitConstants.ThreeSeconds);

            string err = isActive
                ? string.Empty
                : $"'{item.Title}' recently viewed list item (in sidebar) is not active: id='{item.Id}'";

            LogHelper.LogResult(log, $"'{item.Title}' recently viewed list item (in sidebar) is active: id='{item.Id}'", err);

            return err;
        }

        public string CheckRandomItemCanBeOpenedFromRecentlyViewedList()
        {
            OpenRandomItemFromRecentlyViewedList();

            string err = CheckRecentlyViewedListItemIsActive(LastItemOpenedFromRecentlyViewedList);

            err = ErrorHelper.AddSemicolon(err) + CheckItemPageIsOpened();

            return err;
        }

        //----- Helpers -----
        public List<ContentItem> ConvertDevicesToContentItems(List<MasterDevice> devices) =>
            devices.Select(d => new ContentItem()
            {
                ContentItemType = ContentItemType.Device,
                Title = GetExpectedRecentDeviceNameAndOs(d),
                DeviceModel = d.Model,
                Id = d.Id
            }).ToList();

        public string GetExpectedRecentDeviceNameAndOs(MasterDevice device)
        {
            string expectedDeviceOs = null;

            if (!(string.IsNullOrEmpty(device.OsName) && string.IsNullOrEmpty(device.OsVersion)))
            {
                expectedDeviceOs = $"{device.OsName} {device.OsVersion}".Trim();
            }

            string deviceNameAndOs = expectedDeviceOs != null && !device.Model.Contains(expectedDeviceOs)
                ? $"{device.Model} ({expectedDeviceOs})"
                : $"{device.Model}";

            deviceNameAndOs = isRedesign ? device.Manufacturer + " " + deviceNameAndOs : deviceNameAndOs;

            deviceNameAndOs = Regex.Replace(deviceNameAndOs, @"\s{2,}", " "); //replace multiple adjacent whitespaces by one space

            return deviceNameAndOs;
        }
    }
}
