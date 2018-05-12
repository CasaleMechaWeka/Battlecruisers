using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Tutorial.Steps.Providers;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.Providers
{
    public class LastIncompleteBuildingStartedProviderTests
    {
        private IProvider<IBuildable> _buildableProvider;
        private IListProvider<IHighlightable> _highlightablesProvider;
        private IListProvider<IClickable> _clickablesProvider;
        private ICruiserController _cruiser;
        private IBuildable _building1, _building2;

        [SetUp]
        public void SetuUp()
        {
            _cruiser = Substitute.For<ICruiserController>();
            LastIncompleteBuildingStartedProvider provider = new LastIncompleteBuildingStartedProvider(_cruiser);

            _buildableProvider = provider;
            _highlightablesProvider = provider;
            _clickablesProvider = provider;

            _building1 = Substitute.For<IBuildable>();
            _building2 = Substitute.For<IBuildable>();
        }

        [Test]
        public void InitialState()
        {
            AssertNoItem();
        }

        [Test]
        public void Started()
        {
            _cruiser.StartedConstruction += Raise.EventWith(new StartedConstructionEventArgs(_building1));
            AssertItem(_building1);
        }

        [Test]
        public void Started_Completed()
        {
            _cruiser.StartedConstruction += Raise.EventWith(new StartedConstructionEventArgs(_building1));
            AssertItem(_building1);

            _cruiser.BuildingCompleted += Raise.EventWith(new CompletedConstructionEventArgs(_building1));
            AssertNoItem();
        }

        [Test]
        public void Started_Started_Completed_Completed()
        {
            _cruiser.StartedConstruction += Raise.EventWith(new StartedConstructionEventArgs(_building1));
            AssertItem(_building1);

            _cruiser.StartedConstruction += Raise.EventWith(new StartedConstructionEventArgs(_building2));
            AssertItem(_building2);

            _cruiser.BuildingCompleted += Raise.EventWith(new CompletedConstructionEventArgs(_building2));
            AssertItem(_building1);

            _cruiser.BuildingCompleted += Raise.EventWith(new CompletedConstructionEventArgs(_building1));
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
