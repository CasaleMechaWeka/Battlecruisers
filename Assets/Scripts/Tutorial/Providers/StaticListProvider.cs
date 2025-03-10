using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Providers
{
    public class StaticListProvider<TItem> : IListProvider<TItem>
    {
        private readonly IList<TItem> _items;

        public StaticListProvider(params TItem[] items)
        {
            Assert.IsNotNull(items);
            _items = items;
        }

        public IList<TItem> FindItems()
        {
            return _items;
        }
    }
}
