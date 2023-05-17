using System.Collections.Generic;

namespace Taf.UI.Core.Interfaces
{
    public interface IItemsTable
    {
        List<string> GetTitleColumn();

        bool IsItemPresent(string title);

        void ClickItemTitle(string title);

        public List<string> GetItemStatusColumn();

        public bool IsNoRecordsToShowDisplayed();
    }
}
