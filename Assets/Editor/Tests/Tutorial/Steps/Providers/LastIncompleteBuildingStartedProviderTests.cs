using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Tests.Utils.Extensions;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Tutorial.Steps.Providers;
using BattleCruisers.UI;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.Providers
{
    public class LastIncompleteBuildingStartedProviderTests
    {
        private IItemProvider<IBuildable> _buildableProvider;
        private IItemProvider<IMaskHighlightable> _highlightableProvider;
        private IItemProvider<IClickableEmitter> _clickableProvider;
        private ICruiserBuildingMonitor _playerBuildingMonitor;
        private IBuilding _building1, _building2;

        [SetUp]
        public void SetuUp()
        {
            _playerBuildingMonitor = Substitute.For<ICruiserBuildingMonitor>();
            LastIncompleteBuildingStartedProvider provider = new LastIncompleteBuildingStartedProvider(_playerBuildingMonitor);

            _buildableProvider = provider;
            _highlightableProvider = provider;
            _clickableProvider = provider;

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
            _playerBuildingMonitor.EmitBuildingStarted(_building1);
            AssertItem(_building1);
        }

        [Test]
        public void Started_Completed()
        {
            _playerBuildingMonitor.EmitBuildingStarted(_building1);
            AssertItem(_building1);

            _playerBuildingMonitor.EmitBuildingCompleted(_building1);
            AssertNoItem();
        }

        [Test]
        public void Started_Started_Completed_Completed()
        {
            _playerBuildingMonitor.EmitBuildingStarted(_building1);
            AssertItem(_building1);

            _playerBuildingMonitor.EmitBuildingStarted(_building2);
            AssertItem(_building2);

            _playerBuildingMonitor.EmitBuildingCompleted(_building2);
            AssertItem(_building1);

            _playerBuildingMonitor.EmitBuildingCompleted(_building1);
            AssertNoItem();
        }

        private void AssertNoItem()
        {
            Assert.IsNull(_highlightableProvider.FindItem());
            Assert.IsNull(_clickableProvider.FindItem());
            Assert.IsNull(_buildableProvider.FindItem());            
        }

        private void AssertItem(IBuildable item)
        {
            Assert.AreSame(item, _highlightableProvider.FindItem());
            Assert.AreSame(item, _clickableProvider.FindItem());
            Assert.AreSame(item, _buildableProvider.FindItem());            
        }
    }
}
