using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Cruisers.Damage;
using BattleCruisers.Tests.Utils.Extensions;
using BattleCruisers.UI.Music;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using NUnit.Framework;
using System;

namespace BattleCruisers.Tests.UI.Music
{
    public class DangerMonitorTests
    {
        private IDangerMonitor _dangerMonitor;
        private IDeferrer _deferrer;
        private ICruiserController _playerCruiser, _aiCruiser;
        private IHealthThresholdMonitor _playerCruiserHealthMonitor, _aiCruiserHealthMonitor;
        private IBuilding _buildingCompleted;
        private IUnit _unitCompleted;
        private Action _deferredAction;
        private int _startCount, _endCount;

        [SetUp]
        public void TestSetup()
        {
            _deferrer = Substitute.For<IDeferrer>();
            _playerCruiser = Substitute.For<ICruiserController>();
            _aiCruiser = Substitute.For<ICruiserController>();
            _playerCruiserHealthMonitor = Substitute.For<IHealthThresholdMonitor>();
            _aiCruiserHealthMonitor = Substitute.For<IHealthThresholdMonitor>();

            _dangerMonitor = new DangerMonitor(_deferrer, _playerCruiser, _aiCruiser, _playerCruiserHealthMonitor, _aiCruiserHealthMonitor);

            _deferredAction = null;
            _deferrer.Defer(Arg.Do<Action>(action => _deferredAction = action), DangerMonitor.DANGER_LIFETIME_IN_S);

            _buildingCompleted = Substitute.For<IBuilding>();
            _unitCompleted = Substitute.For<IUnit>();

            _startCount = 0;
            _dangerMonitor.DangerStart += (sender, e) => _startCount++;

            _endCount = 0;
            _dangerMonitor.DangerEnd += (sender, e) => _endCount++;
        }

        [Test]
        public void PlayerCruiser_BuildingCompleted_Ultra_EmitsEvent()
        {
            // Danger start
            _buildingCompleted.Category.Returns(BuildingCategory.Ultra);
            _playerCruiser.BuildingMonitor.EmitBuildingCompleted(_buildingCompleted);
            Assert.AreEqual(1, _startCount);

            AssertDeferredDangerEnd();
        }

        [Test]
        public void PlayerCruiser_BuildingCompleted_Offensive_EmitsEvent()
        {
            // Danger start
            _buildingCompleted.Category.Returns(BuildingCategory.Offence);
            _playerCruiser.BuildingMonitor.EmitBuildingCompleted(_buildingCompleted);
            Assert.AreEqual(1, _startCount);

            AssertDeferredDangerEnd();
        }

        [Test]
        public void PlayerCruiser_BuildingCompleted_NotOffensiveOrUltra_DoesNotEmitEvent()
        {
            _buildingCompleted.Category.Returns(BuildingCategory.Defence);
            
            _playerCruiser.BuildingMonitor.EmitBuildingCompleted(_buildingCompleted);
            
            Assert.AreEqual(0, _startCount);
            Assert.IsNull(_deferredAction);
        }

        [Test]
        public void PlayerCruiser_UnitComlpeted_Ultra_EmitsEvent()
        {
            // Danger start
            _unitCompleted.IsUltra.Returns(true);
            _playerCruiser.UnitMonitor.EmitUnitComlpeted(_unitCompleted);
            Assert.AreEqual(1, _startCount);

            AssertDeferredDangerEnd();
        }

        [Test]
        public void PlayerCruiser_UnitComlpeted_NotUltra_DoesNotEmitEvent()
        {
            _unitCompleted.IsUltra.Returns(false);
            _playerCruiser.UnitMonitor.EmitUnitComlpeted(_unitCompleted);
            Assert.AreEqual(0, _startCount);
        }

        [Test]
        public void PlayerCruiser_DroppedBelowThreshold_CruisersStillAlive_EmitsEvent()
        {
            _playerCruiser.IsAlive.Returns(true);
            _aiCruiser.IsAlive.Returns(true);

            _playerCruiserHealthMonitor.DroppedBelowThreshold += Raise.Event();
            
            Assert.AreEqual(1, _startCount);
            Assert.IsNull(_deferredAction);
        }

        [Test]
        public void PlayerCruiser_DroppedBelowThreshold_ACruiserIsDestroyed_DoesNotEmit()
        {
            _playerCruiser.IsAlive.Returns(false);
            _aiCruiser.IsAlive.Returns(true);

            _playerCruiserHealthMonitor.DroppedBelowThreshold += Raise.Event();
            
            Assert.AreEqual(0, _startCount);
        }

        [Test]
        public void PlayerCruiser_RoseAboveThreshold()
        {
            _playerCruiserHealthMonitor.RoseAboveThreshold += Raise.Event();
            Assert.AreEqual(1, _endCount);
        }

        [Test]
        public void AICruiser_BuildingCompleted_Ultra_EmitsEvent()
        {
            // Danger start
            _buildingCompleted.Category.Returns(BuildingCategory.Ultra);
            _aiCruiser.BuildingMonitor.EmitBuildingCompleted(_buildingCompleted);
            Assert.AreEqual(1, _startCount);

            AssertDeferredDangerEnd();
        }

        [Test]
        public void AICruiser_BuildingCompleted_Offensive_EmitsEvent()
        {
            // Danger start
            _buildingCompleted.Category.Returns(BuildingCategory.Offence);
            _aiCruiser.BuildingMonitor.EmitBuildingCompleted(_buildingCompleted);
            Assert.AreEqual(1, _startCount);

            AssertDeferredDangerEnd();
        }

        [Test]
        public void AICruiser_BuildingCompleted_NotOffensiveOrUltra_DoesNotEmitEvent()
        {
            _buildingCompleted.Category.Returns(BuildingCategory.Defence);
            _aiCruiser.BuildingMonitor.EmitBuildingCompleted(_buildingCompleted);
            Assert.AreEqual(0, _startCount);
        }

        [Test]
        public void AICruiser_UnitComlpeted_Ultra_EmitsEvent()
        {
            // Danger start
            _unitCompleted.IsUltra.Returns(true);
            _aiCruiser.UnitMonitor.UnitCompleted += Raise.EventWith(new UnitCompletedEventArgs(_unitCompleted));
            Assert.AreEqual(1, _startCount);

            AssertDeferredDangerEnd();
        }

        [Test]
        public void AICruiser_UnitComlpeted_NotUltra_DoesNotEmitEvent()
        {
            _unitCompleted.IsUltra.Returns(false);
            _aiCruiser.UnitMonitor.UnitCompleted += Raise.EventWith(new UnitCompletedEventArgs(_unitCompleted));
            Assert.AreEqual(0, _startCount);
        }

        [Test]
        public void AICruiser_DroppedBelowThreshold_CruisersStillAlive_EmitsEvent()
        {
            _playerCruiser.IsAlive.Returns(true);
            _aiCruiser.IsAlive.Returns(true);

            _aiCruiserHealthMonitor.DroppedBelowThreshold += Raise.Event();
            
            Assert.AreEqual(1, _startCount);
            Assert.IsNull(_deferredAction);
        }

        [Test]
        public void AICruiser_DroppedBelowThreshold_ACruiserIsDestroyed_DoesNotEmit()
        {
            _playerCruiser.IsAlive.Returns(true);
            _aiCruiser.IsAlive.Returns(false);

            _aiCruiserHealthMonitor.DroppedBelowThreshold += Raise.Event();
            
            Assert.AreEqual(0, _startCount);
        }

        [Test]
        public void AICruiser_RoseAboveThreshold()
        {
            _aiCruiserHealthMonitor.RoseAboveThreshold += Raise.Event();
            Assert.AreEqual(1, _endCount);
        }

        private void AssertDeferredDangerEnd()
        {
            Assert.AreEqual(0, _endCount);
            Assert.IsNotNull(_deferredAction);
            _deferredAction.Invoke();
            Assert.AreEqual(1, _endCount);
        }
    }
}