using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Tutorial.Steps.Providers;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.Providers
{
    public class LastBuildingStartedProviderTests
    {
        private IProvider<IBuildable> _buildableProvider;
        private IListProvider<IHighlightable> _highlightablesProvider;
        private IListProvider<IClickable> _clickablesProvider;
        private ICruiserController _cruiser;
        private IBuildable _building;

        [SetUp]
        public void SetuUp()
        {
            _cruiser = Substitute.For<ICruiserController>();
            LastBuildingStartedProvider provider = new LastBuildingStartedProvider(_cruiser);

            _buildableProvider = provider;
            _highlightablesProvider = provider;
            // FELIX
            //_clickablesProvider = provider;

            _building = Substitute.For<IBuildable>();
        }

        [Test]
        public void NoBuildingsStarted()
        {
            Assert.AreEqual(0, _highlightablesProvider.FindItems().Count);
            Assert.AreEqual(0, _clickablesProvider.FindItems().Count);
			Assert.IsNull(_buildableProvider.FindItem());
        }

        [Test]
        public void ABuildingStarted()
        {
            _cruiser.StartedConstruction += Raise.EventWith(new StartedConstructionEventArgs(_building));

            Assert.IsTrue(_highlightablesProvider.FindItems().Contains(_building));
            Assert.IsTrue(_clickablesProvider.FindItems().Contains(_building));
            Assert.AreSame(_building, _buildableProvider.FindItem());
        }
    }
}
