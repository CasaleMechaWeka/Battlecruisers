using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Tutorial.Steps.Providers;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.Providers
{
    public class LastBuildingStartedProviderTests
    {
        private LastBuildingStartedProvider _provider;
        private ICruiserController _cruiser;
        private IBuildable _building;

        [SetUp]
        public void SetuUp()
        {
            _cruiser = Substitute.For<ICruiserController>();
            _provider = new LastBuildingStartedProvider(_cruiser);

            _building = Substitute.For<IBuildable>();
        }

        [Test]
        public void NoBuildingsStarted()
        {
            Assert.AreEqual(0, _provider.FindHighlightables().Count);
            Assert.AreEqual(0, _provider.FindClickables().Count);
			Assert.IsNull(_provider.FindItem());
        }

        [Test]
        public void ABuildingStarted()
        {
            _cruiser.StartedConstruction += Raise.EventWith(new StartedConstructionEventArgs(_building));

            Assert.IsTrue(_provider.FindHighlightables().Contains(_building));
            Assert.IsTrue(_provider.FindClickables().Contains(_building));
            Assert.AreSame(_building, _provider.FindItem());
        }
    }
}
