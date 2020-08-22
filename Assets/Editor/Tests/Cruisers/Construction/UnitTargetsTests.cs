using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using NSubstitute;
using NUnit.Framework;
using System.Linq;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Cruisers.Construction
{
    public class UnitTargetsTests
    {
        private IUnitTargets _unitTargets;
        private ICruiserUnitMonitor _cruiserUnitMonitor;
        private IUnit _ship, _aircraft, _building;

        [SetUp]
        public void TestSetup()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _cruiserUnitMonitor = Substitute.For<ICruiserUnitMonitor>();
            _unitTargets = new UnitTargets(_cruiserUnitMonitor);

            _ship = CreateUnit(TargetType.Ships);
            _aircraft = CreateUnit(TargetType.Aircraft);
            _building = CreateUnit(TargetType.Buildings);
        }

        private IUnit CreateUnit(TargetType targetType)
        {
            IUnit unit = Substitute.For<IUnit>();
            unit.TargetType.Returns(targetType);
            return unit;
        }

        [Test]
        public void InitialState()
        {
            Assert.IsNotNull(_unitTargets.Ships);
            Assert.IsNotNull(_unitTargets.Aircraft);
            Assert.AreEqual(0, _unitTargets.Ships.Count);
            Assert.AreEqual(0, _unitTargets.Aircraft.Count);
        }

        [Test]
        public void UnitStarted_Ship_AddsShip()
        {
            _cruiserUnitMonitor.UnitStarted += Raise.EventWith(new UnitStartedEventArgs(_ship));

            Assert.AreEqual(1, _unitTargets.Ships.Count);
            Assert.AreSame(_ship, _unitTargets.Ships.First());
        }

        [Test]
        public void UnitStarted_Aircraft_AddsAircraft()
        {
            _cruiserUnitMonitor.UnitStarted += Raise.EventWith(new UnitStartedEventArgs(_aircraft));

            Assert.AreEqual(1, _unitTargets.Aircraft.Count);
            Assert.AreSame(_aircraft, _unitTargets.Aircraft.First());
        }

        [Test]
        public void UnitStarted_Building_DoesNothing()
        {
            _cruiserUnitMonitor.UnitStarted += Raise.EventWith(new UnitStartedEventArgs(_building));

            Assert.AreEqual(0, _unitTargets.Ships.Count);
            Assert.AreEqual(0, _unitTargets.Aircraft.Count);
        }

        [Test]
        public void UnitStarted_Duplicate_Throws()
        {
            _cruiserUnitMonitor.UnitStarted += Raise.EventWith(new UnitStartedEventArgs(_ship));
            Assert.Throws<UnityAsserts.AssertionException>(() => _cruiserUnitMonitor.UnitStarted += Raise.EventWith(new UnitStartedEventArgs(_ship)));
        }

        [Test]
        public void UnitDestroyed_ExistingShip_Removes()
        {
            _cruiserUnitMonitor.UnitStarted += Raise.EventWith(new UnitStartedEventArgs(_ship));
            _cruiserUnitMonitor.UnitDestroyed += Raise.EventWith(new UnitDestroyedEventArgs(_ship));
            Assert.AreEqual(0, _unitTargets.Ships.Count);
        }

        [Test]
        public void UnitDestroyed_NonExistingShip_DoesNothing()
        {
            _cruiserUnitMonitor.UnitDestroyed += Raise.EventWith(new UnitDestroyedEventArgs(_ship));
            Assert.AreEqual(0, _unitTargets.Ships.Count);
        }

        [Test]
        public void UnitDestroyed_ExistingAircraft_Removes()
        {
            _cruiserUnitMonitor.UnitStarted += Raise.EventWith(new UnitStartedEventArgs(_aircraft));
            _cruiserUnitMonitor.UnitDestroyed += Raise.EventWith(new UnitDestroyedEventArgs(_aircraft));
            Assert.AreEqual(0, _unitTargets.Aircraft.Count);
        }

        [Test]
        public void UnitDestroyed_NonExistingAircraft_DoesNothing()
        {
            _cruiserUnitMonitor.UnitDestroyed += Raise.EventWith(new UnitDestroyedEventArgs(_aircraft));
            Assert.AreEqual(0, _unitTargets.Aircraft.Count);
        }

        [Test]
        public void UnitDestroyed_Building_DoesNothing()
        {
            _cruiserUnitMonitor.UnitDestroyed += Raise.EventWith(new UnitDestroyedEventArgs(_building));
            Assert.AreEqual(0, _unitTargets.Aircraft.Count);
            Assert.AreEqual(0, _unitTargets.Ships.Count);
        }
    }
}