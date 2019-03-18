using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Cruisers.Construction
{
    public class UltrasConstructionMonitorTests
    {
        private UltrasConstructionMonitor _monitor;
        private ICruiserController _cruiser;
        private IPrioritisedSoundPlayer _soundPlayer;
        private IUnit _ultraUnit, _normalUnit;
        private IBuilding _ultraBuilding, _normalBuilding;

        [SetUp]
        public void TestSetup()
        {
            _cruiser = Substitute.For<ICruiserController>();
            _soundPlayer = Substitute.For<IPrioritisedSoundPlayer>();

            _monitor = new UltrasConstructionMonitor(_cruiser, _soundPlayer);

            _ultraUnit = Substitute.For<IUnit>();
            _ultraUnit.IsUltra.Returns(true);

            _normalUnit = Substitute.For<IUnit>();
            _normalUnit.IsUltra.Returns(false);

            _ultraBuilding = Substitute.For<IBuilding>();
            _ultraBuilding.Category.Returns(BuildingCategory.Ultra);

            _normalBuilding = Substitute.For<IBuilding>();
            _normalBuilding.Category.Returns(BuildingCategory.Defence);
        }

        [Test]
        public void UltraBuildingStarted_PlaysSound()
        {
            _cruiser.BuildingStarted += Raise.EventWith(new StartedBuildingConstructionEventArgs(_ultraBuilding));
            _soundPlayer.Received().PlaySound(PrioritisedSoundKeys.Events.EnemyStartedUltra);
        }

        [Test]
        public void NormalBuildingStarted_DoesNotPlaySound()
        {
            _cruiser.BuildingStarted += Raise.EventWith(new StartedBuildingConstructionEventArgs(_normalBuilding));
            _soundPlayer.DidNotReceiveWithAnyArgs().PlaySound(null);
        }

        [Test]
        public void UltraUnitStarted_PlaysSound()
        {
            _cruiser.UnitMonitor.UnitStarted += Raise.EventWith(new StartedUnitConstructionEventArgs(_ultraUnit));
            _soundPlayer.Received().PlaySound(PrioritisedSoundKeys.Events.EnemyStartedUltra);
        }

        [Test]
        public void NormalUnitStarted_DoesNotPlaySound()
        {
            _cruiser.UnitMonitor.UnitStarted += Raise.EventWith(new StartedUnitConstructionEventArgs(_normalUnit));
            _soundPlayer.DidNotReceiveWithAnyArgs().PlaySound(null);
        }

        [Test]
        public void Dispose_Unsubscribes()
        {
            _monitor.DisposeManagedState();

            _cruiser.UnitMonitor.UnitStarted += Raise.EventWith(new StartedUnitConstructionEventArgs(_ultraUnit));
            _soundPlayer.DidNotReceiveWithAnyArgs().PlaySound(null);

            _cruiser.BuildingStarted += Raise.EventWith(new StartedBuildingConstructionEventArgs(_ultraBuilding));
            _soundPlayer.DidNotReceiveWithAnyArgs().PlaySound(null);
        }
    }
}