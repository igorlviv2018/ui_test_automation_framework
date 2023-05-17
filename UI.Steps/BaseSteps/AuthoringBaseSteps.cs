using Taf.UI.Core.Constants;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.PageObjects;
using Taf.UI.PageObjects.Authoring;
using System.Collections.Generic;
using NLog;
using Taf.UI.PageObjects.TafAd.Authoring;
using Taf.UI.PageObjects.CommonPages.Authoring;

namespace Taf.UI.Steps.Authoring
{
    public class AuthoringBaseSteps : BaseSteps
    {
        public AuthoringBaseSteps(App app, ILogger logger) : base(app, logger)
        {
            publishingSummarySteps = new PublishingSummarySteps(app);

            locationSettingsSteps = new LocationSettingsSteps(app);

            toastAlertSteps = new ToastAlertSteps();
        }

        private readonly ArticlesPage articlesPage = new ArticlesPage();

        private readonly JourneysPage journeysPage = new JourneysPage();

        private readonly SearchBlock searchBlock = new SearchBlock();

        private readonly CreateArticleModal createArticleModal = new CreateArticleModal();

        private readonly CreateJourneyModal createJourneyModal = new CreateJourneyModal();

        private readonly Spinner spinner = new Spinner(App.Taf);

        private readonly LocationSettingsSteps locationSettingsSteps;

        private readonly PublishingSummarySteps publishingSummarySteps;

        private readonly ToastAlertSteps toastAlertSteps;

        public string PublishItem()
        {
            string err = string.Empty;

            LogHelper.LogInfo(log, $"Publishing item (article/journey)");

            if (!publishingSummarySteps.OpenPublishSummaryModal())
            {
                err = "Publish summary dialog did not appear; ";
            };

            err += publishingSummarySteps.Publish();

            toastAlertSteps.WaitAlertToDisappear();

            LogHelper.LogResult(log, "Item published", err);

            return err;
        }

        public bool WaitItemsTableIsDisplayed()
        {
            bool isDisplayed = UiWaitHelper.Wait(() => searchBlock.IsSearchInputVisible(), WaitConstants.FifteenSeconds);

            return isDisplayed;
        }

        protected string CheckTitleDescriptionNotEmpty(string title, string description)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrEmpty(title))
            {
                errors.Add("title is empty");
            }

            if (string.IsNullOrEmpty(description))
            {
                errors.Add("description is empty");
            }

            return ErrorHelper.ConvertErrorsToString(errors);
        }

        protected string CheckSpinnersForCreateItemModal(string errPrefix)
        {
            spinner.WaitSpinnerToDisappear(SpinnerType.TopProgressBar);

            bool isSpinnerDisappeared = createArticleModal.IsSpinnerInCreateButtonDisappeared();

            List<string> errors = new List<string>();

            if (!isSpinnerDisappeared)
            {
                errors.Add($"spinner in 'Create' button did not disappear (within {WaitConstants.SpinnerToDisappearInSec} s)");
            }

            string err = toastAlertSteps.CheckAlertPopup(AlertStatus.Success);

            if (!string.IsNullOrEmpty(err))
            {
                errors.Add(err);

                return ErrorHelper.ConvertErrorsToString(errors, errPrefix);
            }

            isSpinnerDisappeared = createArticleModal.IsSpinnerInModalDisappeared();

            if (!isSpinnerDisappeared)
            {
                errors.Add($"spinner in 'Create' modal window did not disappear (within {WaitConstants.SpinnerToDisappearInSec} s)");
            }

            return ErrorHelper.ConvertErrorsToString(errors, errPrefix);
        }
    }
}

