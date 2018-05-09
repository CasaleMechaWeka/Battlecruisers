using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Providers
{
    // FELIX  Test :)
    public class StaticListProvider<TItem> : IListProvider<TItem>
    {
        private readonly IList<TItem> _items;

        public StaticListProvider(params TItem[] items)
        {
            Assert.IsNotNull(items);
            Assert.IsTrue(items.Length > 0);

            _items = items;
        }

        public IList<TItem> FindItems()
        {
            return _items;
        }
    }
}
