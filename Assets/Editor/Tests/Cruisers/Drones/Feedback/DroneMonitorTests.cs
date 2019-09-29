using BattleCruisers.Buildables;
using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Effects.Drones;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Cruisers.Drones.Feedback
{
    public class DroneMonitorTests
    {
        private IDroneMonitor _droneMonitor;
        private IDroneFactory _droneFactory;
        private IDroneController _redDrone1, _redDrone2, _blueDrone1, _blueDrone2;

        [SetUp]
        public void TestSetup()
        {
            _droneFactory = Substitute.For<IDroneFactory>();
            _droneMonitor = new DroneMonitor(_droneFactory);

            _redDrone1 = Substitute.For<IDroneController>();
            _redDrone1.Faction.Returns(Faction.Reds);

            _redDrone2 = Substitute.For<IDroneController>();
            _redDrone2.Faction.Returns(Faction.Reds);

            _blueDrone1 = Substitute.For<IDroneController>();
            _blueDrone1.Faction.Returns(Faction.Blues);

            _blueDrone2 = Substitute.For<IDroneController>();
            _blueDrone2.Faction.Returns(Faction.Blues);

            _droneFactory.DroneCreated += Raise.EventWith(new DroneCreatedEventArgs(_redDrone1));
            _droneFactory.DroneCreated += Raise.EventWith(new DroneCreatedEventArgs(_redDrone2));
            _droneFactory.DroneCreated += Raise.EventWith(new DroneCreatedEventArgs(_blueDrone1));
            _droneFactory.DroneCreated += Raise.EventWith(new DroneCreatedEventArgs(_blueDrone2));

            UnityAsserts.Assert.raiseExceptions = true;
        }

        [Test]
        public void InitialState()
        {
            Assert.IsTrue(_droneMonitor.FactionToActiveDroneNum.ContainsKey(Faction.Blues));
            Assert.AreEqual(0, _droneMonitor.FactionToActiveDroneNum[Faction.Blues]);

            Assert.IsTrue(_droneMonitor.FactionToActiveDroneNum.ContainsKey(Faction.Reds));
            Assert.AreEqual(0, _droneMonitor.FactionToActiveDroneNum[Faction.Reds]);

            AssertDroneActivnessStatus(playerHasActiveDrone: false, aiHasActiveDrone: false);
        }

        [Test]
        public void DroneActivated()
        {
            _redDrone1.Activated += Raise.Event();
            AssertDroneNum(redDroneNum: 1, blueDroneNum: 0);
            AssertDroneActivnessStatus(playerHasActiveDrone: false, aiHasActiveDrone: true);
        }

        [Test]
        public void DroneDeactivated_NoPriorActivation_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _redDrone1.Deactivated += Raise.Event());
        }

        [Test]
        public void DroneDeactivated()
        {
            // Activation
            _redDrone1.Activated += Raise.Event();
            AssertDroneNum(redDroneNum: 1, blueDroneNum: 0);
            AssertDroneActivnessStatus(playerHasActiveDrone: false, aiHasActiveDrone: true);

            // Deactivation
            _redDrone1.Deactivated += Raise.Event();
            AssertDroneNum(redDroneNum: 0, blueDroneNum: 0);
            AssertDroneActivnessStatus(playerHasActiveDrone: false, aiHasActiveDrone: false);
        }

        [Test]
        public void Combination()
        {
            _redDrone1.Activated += Raise.Event();
            AssertDroneNum(redDroneNum: 1, blueDroneNum: 0);
            AssertDroneActivnessStatus(playerHasActiveDrone: false, aiHasActiveDrone: true);

            _blueDrone2.Activated += Raise.Event();
            AssertDroneNum(redDroneNum: 1, blueDroneNum: 1);
            AssertDroneActivnessStatus(playerHasActiveDrone: true, aiHasActiveDrone: true);

            _redDrone2.Activated += Raise.Event();
            AssertDroneNum(redDroneNum: 2, blueDroneNum: 1);
            AssertDroneActivnessStatus(playerHasActiveDrone: true, aiHasActiveDrone: true);

            _blueDrone2.Deactivated += Raise.Event();
            AssertDroneNum(redDroneNum: 2, blueDroneNum: 0);
            AssertDroneActivnessStatus(playerHasActiveDrone: false, aiHasActiveDrone: true);

            _redDrone1.Deactivated += Raise.Event();
            AssertDroneNum(redDroneNum: 1, blueDroneNum: 0);
            AssertDroneActivnessStatus(playerHasActiveDrone: false, aiHasActiveDrone: true);

            _blueDrone1.Activated += Raise.Event();
            AssertDroneNum(redDroneNum: 1, blueDroneNum: 1);
            AssertDroneActivnessStatus(playerHasActiveDrone: true, aiHasActiveDrone: true);

            _redDrone2.Deactivated += Raise.Event();
            AssertDroneNum(redDroneNum: 0, blueDroneNum: 1);
            AssertDroneActivnessStatus(playerHasActiveDrone: true, aiHasActiveDrone: false);

            _blueDrone2.Activated += Raise.Event();
            AssertDroneNum(redDroneNum: 0, blueDroneNum: 2);
            AssertDroneActivnessStatus(playerHasActiveDrone: true, aiHasActiveDrone: false);

            _blueDrone2.Deactivated += Raise.Event();
            AssertDroneNum(redDroneNum: 0, blueDroneNum: 1);
            AssertDroneActivnessStatus(playerHasActiveDrone: true, aiHasActiveDrone: false);

            _blueDrone1.Deactivated += Raise.Event();
            AssertDroneNum(redDroneNum: 0, blueDroneNum: 0);
            AssertDroneActivnessStatus(playerHasActiveDrone: false, aiHasActiveDrone: false);
        }

        private void AssertDroneNum(int redDroneNum, int blueDroneNum)
        {
            Assert.AreEqual(redDroneNum, _droneMonitor.FactionToActiveDroneNum[Faction.Reds]);
            Assert.AreEqual(blueDroneNum, _droneMonitor.FactionToActiveDroneNum[Faction.Blues]);
        }

        private void AssertDroneActivnessStatus(bool playerHasActiveDrone, bool aiHasActiveDrone)
        {
            Assert.AreEqual(playerHasActiveDrone, _droneMonitor.PlayerCruiserHasActiveDrones.Value);
            Assert.AreEqual(aiHasActiveDrone, _droneMonitor.AICruiserHasActiveDrones.Value);
        }
    }
}