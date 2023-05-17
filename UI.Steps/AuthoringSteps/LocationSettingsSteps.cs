using Taf.UI.Core.Enums;
using Taf.UI.Core.Helpers;
using Taf.UI.Core.Models.TafAuth;
using Taf.UI.PageObjects;
using System.Collections.Generic;

namespace Taf.UI.Steps
{
    public class LocationSettingsSteps
    {
        public LocationSettingsSteps(App app)
        {
            locationTab = new ArticleEditLocationPage(app);

            this.app = app;
        }

        private readonly ArticleEditLocationPage locationTab;

        private readonly App app;

        public string EnableTafEmChannel()
        {
            string err = string.Empty;

            if (locationTab.IsCollapseEnabled((int)App.Embed + 1))
            {
                if (!locationTab.IsCollapseHeaderSwitcherChecked((int)App.Embed + 1))
                {
                    locationTab.ClickCollapseHeaderSwitcher((int)App.Embed + 1);
                }
            }
            else
            {
                err = "SP Embed configuration is disabled";
            }

            return err;
        }

        public string SetTafChannelOptions(TafPublishOptions publishOptions)
        {
            string err = string.Empty;

            const int advisorCollapsePosition = 1;

            const int authoringCollapsePosition = 2;

            int collapsePosition = app == App.TafAuth ? authoringCollapsePosition : advisorCollapsePosition;

            if (locationTab.IsCollapseEnabled(collapsePosition))
            {
                // enable channel (SP Agents)
                if (!locationTab.IsCollapseHeaderSwitcherChecked(collapsePosition))
                {
                    locationTab.ClickCollapseHeaderSwitcher(collapsePosition);
                }

                locationTab.ExpandCollapse(collapsePosition);

                // set 'Post article info in news feed when published' option
                if (locationTab.IsSwitcherChecked(collapsePosition, 1) != publishOptions.PostToNewsFeed)
                {
                    locationTab.ClickSwitcher(collapsePosition, 1);
                }

                // set release note - SP Authoring only
                if (app == App.TafAuth && publishOptions.PostToNewsFeed && publishOptions.ReleaseNote.Length > 0)
                {
                    locationTab.SetTextInTextArea(collapsePosition, textAreaPosition: 1, publishOptions.ReleaseNote);
                }

                // enable 'Include article in search function' option
                if (locationTab.IsSwitcherChecked(collapsePosition, 2) != publishOptions.IncludeArticleInSearch)
                {
                    locationTab.ClickSwitcher(collapsePosition, 2); // remov hardcode
                }

                // set 'Articles' location option - SP Authoring only
                if (app == App.TafAuth && locationTab.IsCheckboxChecked(collapsePosition, 1) != publishOptions.IsArticlesLocationSelected)
                {
                    locationTab.ClickCheckbox(collapsePosition, 1);
                }
            }
            else
            {
                err = "SP Agents configuration is disabled";
            }

            return err;
        }

        public string SetPublishChannelsOptions(PublishChannelsOptions publishChannelsOptions)
        {
            string err;

            List<string> errors = new List<string>();

            if (publishChannelsOptions.TafPublishOptions.IsChannelEnabled)
            {
                err = SetTafChannelOptions(publishChannelsOptions.TafPublishOptions);

                errors.Add(err);
            }

            if (publishChannelsOptions.IsEmbedChannelEnabled)
            {
                err = EnableTafEmChannel();

                errors.Add(err);
            }

            return ErrorHelper.ConvertErrorsToString(errors);
        }

        public PublishChannelsOptions GetDefaultPublishChannelsOptions() => new PublishChannelsOptions()
        {
            IsEmbedChannelEnabled = true,

            TafPublishOptions = new TafPublishOptions()
            {
                IsChannelEnabled = true,
                IncludeArticleInSearch = true,
                IsArticlesLocationSelected = true
            }
        };

        public PublishChannelsOptions GetDefaultPublishChannelsOptionsTafAd() => new PublishChannelsOptions()
        {
            IsEmbedChannelEnabled = false,

            TafPublishOptions = new TafPublishOptions()
            {
                IsChannelEnabled = true,
                IncludeArticleInSearch = true,
                IsArticlesLocationSelected = true
            }
        };

        public PublishChannelsOptions SetTafPublishOptions(bool postToNewsFeed, bool includeArticleInSearch, string releaseNotes="", bool isChannelEnabled=true) =>
            new PublishChannelsOptions()
            {
                IsEmbedChannelEnabled = true, // move to separate method

                TafPublishOptions = new TafPublishOptions()
                {
                    IsChannelEnabled = isChannelEnabled,
                    IncludeArticleInSearch = includeArticleInSearch,
                    IsArticlesLocationSelected = true,
                    PostToNewsFeed = postToNewsFeed,
                    ReleaseNote = releaseNotes
                }
            };
    }
}
