using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Providers
{
    /// <summary>
    /// Allows a reference to the provider to be used BEFORE the actual items
    /// the provider will return are available.
    /// </summary>
    public interface IListProvider<TItem>
    {
        IList<TItem> FindItems();
    }
}
