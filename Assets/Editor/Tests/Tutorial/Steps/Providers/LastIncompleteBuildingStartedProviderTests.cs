using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Tests.Utils;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Tutorial.Steps.Providers;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.Providers
{
    public class LastIncompleteBuildingStartedProviderTests
    {
        private IItemProvider<IBuildable> _buildableProvider;
        private IListProvider<IHighlightable> _highlightablesProvider;
        private IListProvider<IClickableEmitter> _clickablesProvider;
        private ICruiserController _cruiser;
        private IBuilding _building1, _building2;

        [SetUp]
        public void SetuUp()
        {
            _cruiser = Substitute.For<ICruiserController>();
            LastIncompleteBuildingStartedProvider provider = new LastIncompleteBuildingStartedProvider(_cruiser);

            _buildableProvider = provider;
            _highlightablesProvider = provider;
            _clickablesProvider = provider;

            _building1 = Substitute.For<IBuilding>();
            _building2 = Substitute.For<IBuilding>();
        }

        [Test]
        public void InitialState()
        {
            AssertNoItem();
        }

        [Test]
        public void Started()
        {
            _cruiser.StartConstructingBuilding(_building1);
            AssertItem(_building1);
        }

        [Test]
        public void Started_Completed()
        {
            _cruiser.StartConstructingBuilding(_building1);
            AssertItem(_building1);

            _cruiser.CompleteConstructingBuliding(_building1);
            AssertNoItem();
        }

        [Test]
        public void Started_Started_Completed_Completed()
        {
            _cruiser.StartConstructingBuilding(_building1);
            AssertItem(_building1);

            _cruiser.StartConstructingBuilding(_building2);
            AssertItem(_building2);

            _cruiser.CompleteConstructingBuliding(_building2);
            AssertItem(_building1);

            _cruiser.CompleteConstructingBuliding(_building1);
            AssertNoItem();
        }

        private void AssertNoItem()
        {
            Assert.AreEqual(0, _highlightablesProvider.FindItems().Count);
            Assert.AreEqual(0, _clickablesProvider.FindItems().Count);
            Assert.IsNull(_buildableProvider.FindItem());            
        }

        private void AssertItem(IBuildable item)
        {
            Assert.IsTrue(_highlightablesProvider.FindItems().Contains(item));
            Assert.IsTrue(_clickablesProvider.FindItems().Contains(item));
            Assert.AreSame(item, _buildableProvider.FindItem());            
        }
    }
}
