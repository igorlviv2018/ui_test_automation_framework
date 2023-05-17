namespace Taf.UI.Core.Enums
{
    public enum AppsMenuItemType
    {
        AppLink,
        Heading,
        Separator,
        ClientName,
        Unknown
    }

    public enum AuthoringMenuItem
    {
        Articles,
        Devices
    }

    public enum AuthoringSubMenuItem
    {
        CustomArticles,
        DiagnosticFlows,
        Promos,
        InteractiveNavigationMaps,
        Smartwatch,
        Router,
        Tablet,
        Phone,
        Datacard
    }

    public enum AuthoringFlowBlockMenuItem
    {
        Copy,
        Remove,
        AddImage,
        RemoveImage,
        Buttons,
        Dropdown
    }

    public enum AuthoringActionsMenuItem
    {
        Archive,
        Restore,
        Duplicate,
        Move,
        Delete
    }

    public enum AuthoringTableTab
    {
        Active,
        Archived
    }

    public enum AuthoringArticlesTableColumnName
    {
        Type,
        Title,
        Status,
        Version,
        Owner,
        ReviewBy,
        Created,
        Modified
    }

    public enum ArticleType
    {
        None,
        CustomArticle,
        DiagnosticFlow,
        InteractiveNavigationMap
    }

    public enum ArticleStatus
    {
        None,
        Draft,
        Online,
        Archived,
        Expired
    }

    public enum AlertStatus
    {
        Success,
        Failed
    }

    public enum DatePickerButton
    {
        Pending,
        Immediately,
        Evergreen
    }

    public enum DeviceType
    {
        None,
        Datacard,
        Phone,
        Router,
        Smartwatch,
        Tablet
    }

    public enum ArticleContentElementType
    {
        Undefined,
        Text,
        RichText,
        StepByStep,
        StepInStepByStep,
        Video,
        Image,
        Accordion,
        Collapse,
        Group,
        Root,
        Table,
        ButtonsBlock,
        Process, //DF
        PredefinedProcess,
        Decision,
        Branch,
        InternalConnector,
        ExternalConnector,
        Terminator,
        CustomArticle
    }

    public enum JourneyType
    {
        None,
        Phone,
        Tablet
    }

    public enum JourneyStepType
    {
        None,
        Infinity,
        Interval,
        Specifications,
        Brands
    }

    public enum JourneyParameterType
    {
        Infinity,
        Interval
    }

    public enum JourneyCheckDepth
    {
        Minimum,
        Medium,
        Maximum
    }

    public enum AuthoringItemType
    {
        None,
        Article,
        Journey
    }

    public enum ContentItemType
    {
        None,
        Article,
        Device,
        Journey
    }

    public enum TafNewsOnItem
    {
        Device,
        Article
    }

    public enum TafNewsTableTab
    {
        All,
        Devices,
        Articles
    }

    public enum StepByStepViewType
    {
        List,
        Slider
    }

    public enum GuideViewType
    {
        List,
        Slider
    }

    public enum TestType
    {
        CreateCustomArticle,
        CreateDiagnosticFlow,
        CreateArticleWithExistingTitle,
        ArticleOperations,
        ArchiveRestoreArticle,
        DuplicateActiveArticle,
        DuplicateArchivedArticle,
        DeleteArticle,
        SearchArticles,
        RestoreArticleFromArchive,
        ArticleThatCannotBePublished,
        PublishToTaf,
        NewsConfiguration,

        CreateJourney,
        ArchiveRestoreJourney,
        DuplicateActiveJourney,
        DuplicateArchivedJourney,
        DeleteJourney
    }

    public enum ProcessButtonClickAction
    {
        None,
        NextStep,
        PreviousStep,
        RestartFlow,
        Link,
        JavaScript
    }

    public enum LinkButtonTarget
    {
        ModalWindow,
        NewTab,
        NewWindow
    }

    public enum SortOrder
    {
        None,
        Ascending,
        Descending
    }

    public enum YoutubePlayerState
    {
        Unstarted = -1,
        Ended,
        Playing,
        Paused,
        Buffering,
        VideoCued = 5
    }

    public enum YoutubePlayback
    {
        Play,
        Pause,
        Stop
    }
}
