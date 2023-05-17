using System.Collections.Generic;

namespace Taf.UI.Core.Interfaces
{// to del?
    public interface ITafItemsTable
    {
        List<string> GetTitleColumn();
        
        bool IsItemPresent(string title);

        void ClickItemTitle(string title);
    }
}
