using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Damage;
using BattleCruisers.Tests.Utils.Extensions;
using BattleCruisers.UI.Music;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Music
{
    public class DangerMonitorTests
    {
        private IDangerMonitor _dangerMonitor;
        private ICruiserController _playerCruiser, _aiCruiser;
        private IHealthThresholdMonitor _playerCruiserHealthMonitor, _aiCruiserHealthMonitor;
        private IBuilding _buildingCompleted;
        private IUnit _unitCompleted;
        private int _dangerEventCount;

        [SetUp]
        public void TestSetup()
        {
            _playerCruiser = Substitute.For<ICruiserController>();
            _aiCruiser = Substitute.For<ICruiserController>();
            _playerCruiserHealthMonitor = Substitute.For<IHealthThresholdMonitor>();
            _aiCruiserHealthMonitor = Substitute.For<IHealthThresholdMonitor>();

            _dangerMonitor = new DangerMonitor(_playerCruiser, _aiCruiser, _playerCruiserHealthMonitor, _aiCruiserHealthMonitor);

            _buildingCompleted = Substitute.For<IBuilding>();
            _unitCompleted = Substitute.For<IUnit>();

            _dangerEventCount = 0;
            _dangerMonitor.Danger += (sender, e) => _dangerEventCount++;
        }

        [Test]
        public void PlayerCruiser_BuildingCompleted_Ultra_EmitsEvent()
        {
            _buildingCompleted.Category.Returns(BuildingCategory.Ultra);
            _playerCruiser.BuildingMonitor.EmitBuildingCompleted(_buildingCompleted);
            Assert.AreEqual(1, _dangerEventCount);
        }

        [Test]
        public void PlayerCruiser_BuildingCompleted_Offensive_EmitsEvent()
        {
            _buildingCompleted.Category.Returns(BuildingCategory.Offence);
            _playerCruiser.BuildingMonitor.EmitBuildingCompleted(_buildingCompleted);
            Assert.AreEqual(1, _dangerEventCount);
        }

        [Test]
        public void PlayerCruiser_BuildingCompleted_NotOffensiveOrUltra_DoesNotEmitEvent()
        {
            _buildingCompleted.Category.Returns(BuildingCategory.Defence);
            _playerCruiser.BuildingMonitor.EmitBuildingCompleted(_buildingCompleted);
            Assert.AreEqual(0, _dangerEventCount);
        }

        [Test]
        public void PlayerCruiser_UnitComlpeted_Ultra_EmitsEvent()
        {
            _unitCompleted.IsUltra.Returns(true);
            _playerCruiser.UnitMonitor.CompleteConstructingBuliding(_unitCompleted);
            Assert.AreEqual(1, _dangerEventCount);
        }

        [Test]
        public void PlayerCruiser_UnitComlpeted_NotUltra_DoesNotEmitEvent()
        {
            _unitCompleted.IsUltra.Returns(false);
            _playerCruiser.UnitMonitor.CompleteConstructingBuliding(_unitCompleted);
            Assert.AreEqual(0, _dangerEventCount);
        }

        [Test]
        public void PlayerCruiser_HealthThresholdReached_CruisersStillAlive_EmitsEvent()
        {
            _playerCruiser.IsAlive.Returns(true);
            _aiCruiser.IsAlive.Returns(true);
            _playerCruiserHealthMonitor.ThresholdReached += Raise.Event();
            Assert.AreEqual(1, _dangerEventCount);
        }

        [Test]
        public void PlayerCruiser_HealthThresholdReached_ACruiserIsDestroyed_DoesNotEmit()
        {
            _playerCruiser.IsAlive.Returns(false);
            _aiCruiser.IsAlive.Returns(true);
            _playerCruiserHealthMonitor.ThresholdReached += Raise.Event();
            Assert.AreEqual(0, _dangerEventCount);
        }

        [Test]
        public void AICruiser_BuildingCompleted_Ultra_EmitsEvent()
        {
            _buildingCompleted.Category.Returns(BuildingCategory.Ultra);
            _aiCruiser.BuildingMonitor.EmitBuildingCompleted(_buildingCompleted);
            Assert.AreEqual(1, _dangerEventCount);
        }

        [Test]
        public void AICruiser_BuildingCompleted_Offensive_EmitsEvent()
        {
            _buildingCompleted.Category.Returns(BuildingCategory.Offence);
            _aiCruiser.BuildingMonitor.EmitBuildingCompleted(_buildingCompleted);
            Assert.AreEqual(1, _dangerEventCount);
        }

        [Test]
        public void AICruiser_BuildingCompleted_NotOffensiveOrUltra_DoesNotEmitEvent()
        {
            _buildingCompleted.Category.Returns(BuildingCategory.Defence);
            _aiCruiser.BuildingMonitor.EmitBuildingCompleted(_buildingCompleted);
            Assert.AreEqual(0, _dangerEventCount);
        }

        [Test]
        public void AICruiser_UnitComlpeted_Ultra_EmitsEvent()
        {
            _unitCompleted.IsUltra.Returns(true);
            _aiCruiser.UnitMonitor.UnitCompleted += Raise.EventWith(new CompletedUnitConstructionEventArgs(_unitCompleted));
            Assert.AreEqual(1, _dangerEventCount);
        }

        [Test]
        public void AICruiser_UnitComlpeted_NotUltra_DoesNotEmitEvent()
        {
            _unitCompleted.IsUltra.Returns(false);
            _aiCruiser.UnitMonitor.UnitCompleted += Raise.EventWith(new CompletedUnitConstructionEventArgs(_unitCompleted));
            Assert.AreEqual(0, _dangerEventCount);
        }

        [Test]
        public void AICruiser_HealthThresholdReached_CruisersStillAlive_EmitsEvent()
        {
            _playerCruiser.IsAlive.Returns(true);
            _aiCruiser.IsAlive.Returns(true);
            _aiCruiserHealthMonitor.ThresholdReached += Raise.Event();
            Assert.AreEqual(1, _dangerEventCount);
        }

        [Test]
        public void AICruiser_HealthThresholdReached_ACruiserIsDestroyed_DoesNotEmit()
        {
            _playerCruiser.IsAlive.Returns(true);
            _aiCruiser.IsAlive.Returns(false);
            _aiCruiserHealthMonitor.ThresholdReached += Raise.Event();
            Assert.AreEqual(0, _dangerEventCount);
        }
    }
}