using System.Collections.Generic;
using BattleCruisers.Tutorial.Providers;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Providers
{
    public class StaticListProviderTests
    {
        private IListProvider<int> _provider;
        private int _item1, _item2;

        [SetUp]
        public void SetuUp()
        {
            _item1 = 17;
            _item2 = 27;

            _provider = new StaticListProvider<int>(_item1, _item2);
        }

        [Test]
        public void FindItems()
        {
            IList<int> items = _provider.FindItems();

            Assert.IsTrue(items.Contains(_item1));
            Assert.IsTrue(items.Contains(_item2));
        }
    }
}
