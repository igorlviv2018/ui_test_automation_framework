using NLog;
using Taf.UI.Core.Element;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Exceptions;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models;
using Taf.UI.PageObjects;
using Taf.UI.PageObjects.Authoring;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Taf.UI.Core.Models.TestCase;
using System.Text.RegularExpressions;
using Taf.UI.Core.Models.TafAuth;

namespace Taf.UI.Steps.Authoring
{
    public class CommonAuthoringSteps : AuthoringBaseSteps
    {
        public CommonAuthoringSteps(App app, ILogger logger) : base(app, logger)
        {
            flowSteps = new DiagnosticFlowCreateSteps(log);

            customArticleSteps = new CustomArticleCreateSteps(log);

            editorHeaderSteps = new EditorHeaderSteps(app);

            locationSettingsSteps = new LocationSettingsSteps(app);
        }

        private readonly ArticlesPage articlesPage = new ArticlesPage();

        private readonly ArticleEditPropertiesPage propertiesPage = new ArticleEditPropertiesPage();

        private readonly DatePicker datePicker = new DatePicker();

        private readonly CreateArticleModal createArticleModal = new CreateArticleModal();

        private readonly EditorHeaderSteps editorHeaderSteps;

        private readonly LocationSettingsSteps locationSettingsSteps;

        private readonly DiagnosticFlowCreateSteps flowSteps;

        private readonly CustomArticleCreateSteps customArticleSteps;

        // debug - to del after debug
        public List<TafEmArticleElement> CloseTopLevelBlock(List<TafEmArticleElement> elements)
        {
            List<TafEmArticleElement> topLevels = new List<TafEmArticleElement>();

            foreach (var element in elements)
            {
                if (element.PathToElement.Count == 1)
                {
                    topLevels.Add(element);
                }
            }

            topLevels.Reverse();

            foreach (var element in topLevels)
            {
                Element removeButton = new Element(element.XPath + "/../a[@title='Remove']");

                removeButton.ScrollToView();

                removeButton.Click();

                Thread.Sleep(400); //debug
            }

            return topLevels;
        }

        public void SaveArticleId(CreateArticleTestCase testCase)
        {
            string urlWithId = propertiesPage.GetUrl();

            string id = new Regex(@"\d+").Match(urlWithId).Value;

            testCase.ItemCurrentId = id;
        }

        public List<string> GetOwnerList()
        {
            string currentOwner = propertiesPage.GetCurrentOwnerName();

            if (!propertiesPage.IsOwnerListOpened())
            {
                propertiesPage.ClearOwnerInput();

                propertiesPage.OpenOwnerList();
            }

            List<string> owners = propertiesPage.GetOwnerListItems();

            if (!string.IsNullOrEmpty(currentOwner))
            {
                propertiesPage.ClickOwnerListItem(currentOwner);
            }
            else
            {
                propertiesPage.CloseOwnerList();
            }

            return owners;
        }

        public ArticleProperties GetArticleProperties()
        {
            editorHeaderSteps.OpenEditorTab("Properties"); //replace str by enum

            ArticleProperties properties = new ArticleProperties()
            {
                Title = propertiesPage.GetTitle(),

                Description = propertiesPage.GetDescription(),

                WorkNote = propertiesPage.GetWorkNote(),

                CurrentOwner = propertiesPage.GetCurrentOwnerName(),

                OwnerList = GetOwnerList()
            };

            OpenScheduleSettings();

            properties.PublishDate = propertiesPage.GetPublishDate();

            properties.ExpirationDate = propertiesPage.GetExpirationDate();

            return properties;
        }

        public void SetArticleProperties(ArticleProperties properties)
        {
            editorHeaderSteps.OpenEditorTab("Properties"); //replace str by enum

            propertiesPage.SetTitle(properties.Title);

            propertiesPage.SetDescription(properties.Description);

            SetArticleScheduling(properties);
        }

        public void SetArticleScheduling(ArticleProperties properties)
        {
            OpenScheduleSettings();

            OpenPublishDatePicker();

            if (properties.PublishDate == "Immediately")
            {
                SetSpecialDate(DatePickerButton.Immediately);
            }
            else if (properties.PublishDate == "Pending")
            {
                SetSpecialDate(DatePickerButton.Pending);
            }
            else
            {
                SetDateInDatePicker(DateTime.ParseExact(properties.PublishDate, "d/M/yyyy", CultureInfo.InvariantCulture));
            }
        }

        public string GetPublishDate()
        {
            return propertiesPage.GetPublishDate();
        }

        public void OpenScheduleSettings()
        {
            if (!propertiesPage.IsScheduleOpened())
            {
                propertiesPage.ClickScheduleButton();
            }
        }

        public void OpenPublishDatePicker()
        {
            if (!propertiesPage.IsPublishDatePickerOpened())
            {
                propertiesPage.ClickPublishDateButton();
            }
        }

        public void OpenExpirationDatePicker()
        {
            if (!propertiesPage.IsExpirationDatePickerOpened())
            {
                propertiesPage.ClickExpirationDateButton();
            }
        }

        public void SetDateInDatePicker(DateTime date)
        {
            int monthToSelect = date.Month;

            int yearToSelect = date.Year;

            int dayToSelect = date.Day;

            string err = string.Empty;

            string tryToSetDateMsg = $"Trying to select date: {date}.";

            if (datePicker.IsMonthYearButtonDisplayed())
            {
                string monthYear = datePicker.GetMonthYear();

                int displayedMonth = DateTimeHelper.GetMonthNumber(monthYear);

                int displayedYear = DateTimeHelper.GetYear(monthYear);

                if (monthToSelect != displayedMonth || yearToSelect != displayedYear)
                {
                    datePicker.ClickMonthYearButton();

                    if (!datePicker.IsYearButtonDisplayed())
                    {
                        throw new ElementNotFoundException($"{tryToSetDateMsg}. Year button is not displayed (on a date picker)!");
                    }

                    int yearShift = yearToSelect - displayedYear;

                    if (yearShift != 0)
                    {
                        bool increase = yearShift > 0;
                        
                        for (int i = 0; i < Math.Abs(yearShift); i++)
                        {
                            bool oneYearShiftSuccess = increase ? datePicker.IncreaseYear() : datePicker.DecreaseYear();

                            if (!oneYearShiftSuccess)
                            {
                                err = increase ? $"{tryToSetDateMsg}. Year increase failed!" : $"{tryToSetDateMsg}. Year decrease failed!";
                                break;
                            }

                            int expectedDisplayedYear = increase ? displayedYear + i + 1 : displayedYear - (i + 1);

                            if (datePicker.GetYear() != expectedDisplayedYear.ToString())
                            {
                                err = $"{tryToSetDateMsg}. Displayed year is invalid after" + (increase ? "an increase!": "a decrease!");
                                break;
                            }
                        }

                        if (!string.IsNullOrEmpty(err))
                        {
                            throw new ElementNotFoundException(err);
                        }
                    }

                    if (monthToSelect - displayedMonth != 0)
                    {
                        string monthToBeSelected = DateTimeHelper.MonthName(date);

                        if (!datePicker.IsMonthCellEnabled(monthToBeSelected))
                        {
                            throw new ElementNotFoundException($"{tryToSetDateMsg}. Month ('{monthToBeSelected}') to be selected is disabled!");
                        }

                        datePicker.ClickMonthCell(DateTimeHelper.MonthName(date));
                    }
                }

                if (!datePicker.IsDayCellEnabled(dayToSelect))
                {
                    throw new ElementNotFoundException($"{tryToSetDateMsg}. Day ('{dayToSelect}') to be selected is disabled!");
                }

                datePicker.ClickDayCell(dayToSelect);
            }
        }

        public void SetSpecialDate(DatePickerButton button)
        {
            datePicker.ClickPickerButton(button);
        }

        public string CreateArticleProperties(ArticleProperties properties)
        {
            string errPrefix = $"Failed to create article ({properties}): ";

            string err = CheckTitleDescriptionNotEmpty(properties.Title, properties.Description);

            if (!string.IsNullOrEmpty(err))
            {
                return ErrorHelper.AddPrefixToError(err, errPrefix);
            }

            articlesPage.ClickCreateArticleButton();

            articlesPage.ClickArticleType(properties.ArticleType);

            createArticleModal.SetTitle(properties.Title);

            createArticleModal.SetDescription(properties.Description);

            createArticleModal.ClickCreateButton();

            err = CheckSpinnersForCreateItemModal(errPrefix);

            return ErrorHelper.AddPrefixToError(err, errPrefix);
        }

        public string CreateArticleFull(ArticleProperties properties, List<TafEmArticleElement> articleElementSequence, CreateArticleTestCase testCase)
        {
            LogHelper.LogInfo(log, $"Creating article: {properties}");

            string err = CreateArticleProperties(properties);

            LogHelper.LogResult(log, "Filled and saved article Properties", err);

            if (!string.IsNullOrEmpty(err))
            {
                return err;
            }

            SaveArticleId(testCase); // add article id

            err = FillArticleScheduling(properties);

            LogHelper.LogResult(log, "Filled and saved Scheduling settings", err);

            if (!string.IsNullOrEmpty(err))
            {
                return err;
            }

            err = FillArticleContent(articleElementSequence, properties.ArticleType);

            LogHelper.LogResult(log, $"Created {properties.ArticleType} content", err);

            if (!string.IsNullOrEmpty(err))
            {
                return err;
            }

            err = FillArticleLocation(properties.PublishChannelsOptions);

            LogHelper.LogResult(log, $"Filled and saved Location settings", err);

            if (!string.IsNullOrEmpty(err))
            {
                return err;
            }

            LogHelper.LogInfo(log, $"Article ('{properties.Title}') created");

            return err;
        }

        public string FillArticleScheduling(ArticleProperties properties)
        {
            editorHeaderSteps.OpenEditorTab("Properties");

            SetArticleScheduling(properties);
            
            return editorHeaderSteps.EditorSave();
        }

        public string FillArticleContent(List<TafEmArticleElement> articleElementSequence, ArticleType articleType)
        {
            editorHeaderSteps.OpenEditorTab("Content");

            string err = string.Empty;

            if (articleType == ArticleType.CustomArticle)
            {
                err = customArticleSteps.CreateArticleContent(articleElementSequence);
            }
            else if (articleType == ArticleType.DiagnosticFlow)
            {
                err = flowSteps.CreateFlowContent(articleElementSequence);
            }

            err += editorHeaderSteps.EditorSave();

            return err;
        }

        public string FillArticleLocation(PublishChannelsOptions publishChannelsOptions)
        {
            editorHeaderSteps.OpenEditorTab("Location");

            string err = locationSettingsSteps.SetPublishChannelsOptions(publishChannelsOptions);

            err += editorHeaderSteps.EditorSave();

            return err;
        }

        public List<TafEmArticleElement> GetArticleBlockSequence(CreateArticleTestCase testCase)
        {
            List<TafEmArticleElement> blockSequence = new List<TafEmArticleElement>();

            if (testCase.ArticleType == ArticleType.CustomArticle)
            {
                blockSequence = customArticleSteps.GetCustomArticleBlockSequence(testCase.ArticleTestData);
            }
            else if (testCase.ArticleType == ArticleType.DiagnosticFlow)
            {
                blockSequence = flowSteps.GetFlowBlockSequence(testCase.ArticleTestData);
            }

            return blockSequence;
        }

        //public void CheckArticlesMenuHasSubItems()//bool clientHasSpRetail=false)
        //{
        //    if (!homeSideMenu.IsMenuItemActive(AuthoringMenuItem.Articles))
        //    {
        //        homeSideMenu.ClickMenuItem(AuthoringMenuItem.Articles);

        //        homeSideMenu.WaitMenuItemActive(AuthoringMenuItem.Articles);
        //    }

        //    string err = "";

        //    List<AuthoringSubMenuItem> requiredArticlesMenuItems = new List<AuthoringSubMenuItem>() 
        //    { 
        //        AuthoringSubMenuItem.CustomArticles,
        //        AuthoringSubMenuItem.DiagnosticFlows,
        //        AuthoringSubMenuItem.InteractiveNavigationMaps
        //    };

        //    bool clientHasSpRetail = apiHelper.GetClientApps(179).Contains("SpRetail");

        //    if (clientHasSpRetail)
        //    {
        //        requiredArticlesMenuItems.Add(AuthoringSubMenuItem.Promos);
        //    }

        //    foreach (var item in requiredArticlesMenuItems)
        //    {
        //        if (!homeSideMenu.IsSubMenuItemDisplayed(AuthoringMenuItem.Articles, item))
        //        {
        //            err += $"Required submenu item (Articles->{item}) is not present; ";
        //        }
        //    }

        //    //List<AuthoringSubMenuItem> requiredDeviceMenuItems = new List<AuthoringSubMenuItem>()
        //    //{
        //    //    AuthoringSubMenuItem.CustomArticles,
        //    //    AuthoringSubMenuItem.DiagnosticFlows,
        //    //    AuthoringSubMenuItem.InteractiveNavigationMaps
        //    //};

        //    if (!string.IsNullOrEmpty(err))
        //    {
        //        throw new ElementNotFoundException($"Articles menu (Home page side bar): {err}");
        //    }
        //}
    }
}

