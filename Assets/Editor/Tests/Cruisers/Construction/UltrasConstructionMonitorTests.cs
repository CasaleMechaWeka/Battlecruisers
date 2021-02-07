using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Data.Static;
using BattleCruisers.Tests.Utils.Extensions;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Timers;
using NSubstitute;
using NUnit.Framework;
using System;

namespace BattleCruisers.Tests.Cruisers.Construction
{
    public class UltrasConstructionMonitorTests
    {
        private UltrasConstructionMonitor _monitor;
        private ICruiserController _cruiser;
        private IPrioritisedSoundPlayer _soundPlayer;
        private IDebouncer _debouncer;
        private IUnit _ultraUnit, _normalUnit;
        private IBuilding _ultraBuilding, _normalBuilding;
        private Action _debouncedAction;

        [SetUp]
        public void TestSetup()
        {
            _cruiser = Substitute.For<ICruiserController>();
            _soundPlayer = Substitute.For<IPrioritisedSoundPlayer>();
            _debouncer = Substitute.For<IDebouncer>();

            _monitor = new UltrasConstructionMonitor(_cruiser, _soundPlayer, _debouncer);

            _ultraUnit = Substitute.For<IUnit>();
            _ultraUnit.IsUltra.Returns(true);

            _normalUnit = Substitute.For<IUnit>();
            _normalUnit.IsUltra.Returns(false);

            _ultraBuilding = Substitute.For<IBuilding>();
            _ultraBuilding.Category.Returns(BuildingCategory.Ultra);

            _normalBuilding = Substitute.For<IBuilding>();
            _normalBuilding.Category.Returns(BuildingCategory.Defence);

            _debouncer.Debounce(Arg.Do<Action>(x => _debouncedAction = x));
        }

        [Test]
        public void UltraBuildingStarted_PlaysSound()
        {
            _cruiser.BuildingMonitor.EmitBuildingStarted(_ultraBuilding);

            Assert.IsNotNull(_debouncedAction);
            _debouncedAction.Invoke();
            _soundPlayer.Received().PlaySound(PrioritisedSoundKeys.Events.EnemyStartedUltra);
        }

        [Test]
        public void NormalBuildingStarted_DoesNotPlaySound()
        {
            _cruiser.BuildingMonitor.EmitBuildingStarted(_normalBuilding);
            Assert.IsNull(_debouncedAction);
        }

        [Test]
        public void UltraUnitStarted_PlaysSound()
        {
            _cruiser.UnitMonitor.EmitUnitStarted(_ultraUnit);

            Assert.IsNotNull(_debouncedAction);
            _debouncedAction.Invoke();
            _soundPlayer.Received().PlaySound(PrioritisedSoundKeys.Events.EnemyStartedUltra);
        }

        [Test]
        public void NormalUnitStarted_DoesNotPlaySound()
        {
            _cruiser.UnitMonitor.EmitUnitStarted(_normalUnit);
            Assert.IsNull(_debouncedAction);
        }

        [Test]
        public void Dispose_Unsubscribes()
        {
            _monitor.DisposeManagedState();

            _cruiser.BuildingMonitor.EmitBuildingStarted(_ultraBuilding);
            Assert.IsNull(_debouncedAction);

            _cruiser.UnitMonitor.EmitUnitStarted(_ultraUnit);
            Assert.IsNull(_debouncedAction);
        }
    }
}