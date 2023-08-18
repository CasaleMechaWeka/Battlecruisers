using BattleCruisers.AI.FactoryManagers;
using BattleCruisers.AI.ThreatMonitors;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI.FactoryManagers
{
    public class AircraftUnitChooserTests
    {
        private IUnitChooser _unitChooser;
        private IBuildableWrapper<IUnit> _defaultPlane, _lategamePlane, _antiAirPlane, _antiNavalPlane, _broadswordGunship;
        private IDroneManager _droneManager;
        private IThreatMonitor _airThreatMonitor, _navalThreatMonitor;

        [SetUp]
        public void SetuUp()
        {
            _defaultPlane = CreateUnit(numOfDrones: 2);
            _lategamePlane = CreateUnit(numOfDrones: 10);
            _antiAirPlane = CreateUnit(numOfDrones: 6);
            _antiNavalPlane = CreateUnit(numOfDrones: 6);
            _broadswordGunship = CreateUnit(numOfDrones: 18);

            _droneManager = Substitute.For<IDroneManager>();
            _droneManager.NumOfDrones = 12;

            _airThreatMonitor = Substitute.For<IThreatMonitor>();
            _navalThreatMonitor = Substitute.For<IThreatMonitor>();

            _unitChooser = new AircraftUnitChooser(
                _defaultPlane,
                _lategamePlane,
                _antiAirPlane,
                _antiNavalPlane,
                _broadswordGunship,

                _droneManager,
                _airThreatMonitor,
                _navalThreatMonitor,
                threatLevelThreshold: ThreatLevel.High);
        }

        private IBuildableWrapper<IUnit> CreateUnit(int numOfDrones)
        {
            IBuildableWrapper<IUnit> unit = Substitute.For<IBuildableWrapper<IUnit>>();
            unit.Buildable.Returns(Substitute.For<IUnit>());
            unit.Buildable.NumOfDronesRequired.Returns(numOfDrones);
            return unit;
        }

        [Test]
        public void Constructor_ChoosesUnit()
        {
            Assert.IsNotNull(_unitChooser.ChosenUnit);
        }

        [Test]
        public void NoThreats_ChoosesDefaultPlane()
        {
            _airThreatMonitor.CurrentThreatLevel.Returns(ThreatLevel.None);
            _navalThreatMonitor.CurrentThreatLevel.Returns(ThreatLevel.None);

            TriggerChoosing();

            Assert.AreSame(_defaultPlane, _unitChooser.ChosenUnit);
        }

        [Test]
        public void AirThreat_AboveThreshold_ChoosesAntiAirPlane()
        {
            _airThreatMonitor.CurrentThreatLevel.Returns(ThreatLevel.High);
            _navalThreatMonitor.CurrentThreatLevel.Returns(ThreatLevel.None);

            TriggerChoosing();

            Assert.AreSame(_antiAirPlane, _unitChooser.ChosenUnit);
        }

        [Test]
        public void AirThreat_BelowThreshold_ChoosesDefaultPlane()
        {
            _airThreatMonitor.CurrentThreatLevel.Returns(ThreatLevel.Low);
            _navalThreatMonitor.CurrentThreatLevel.Returns(ThreatLevel.None);

            TriggerChoosing();

            Assert.AreSame(_defaultPlane, _unitChooser.ChosenUnit);
        }

        [Test]
        public void NavalThreat_AboveThreshold_ChoosesAntiNavalPlane()
        {
            _airThreatMonitor.CurrentThreatLevel.Returns(ThreatLevel.None);
            _navalThreatMonitor.CurrentThreatLevel.Returns(ThreatLevel.High);

            TriggerChoosing();

            Assert.AreSame(_antiNavalPlane, _unitChooser.ChosenUnit);
        }

        [Test]
        public void NavalThreat_BelowThreshold_ChoosesDefaultPlane()
        {
            _airThreatMonitor.CurrentThreatLevel.Returns(ThreatLevel.None);
            _navalThreatMonitor.CurrentThreatLevel.Returns(ThreatLevel.Low);

            TriggerChoosing();

            Assert.AreSame(_defaultPlane, _unitChooser.ChosenUnit);
        }

        [Test]
        public void AirAndNavalThreats_AboveThreshold_ChoosesNonDefault()
        {
            _airThreatMonitor.CurrentThreatLevel.Returns(ThreatLevel.High);
            _navalThreatMonitor.CurrentThreatLevel.Returns(ThreatLevel.High);

            TriggerChoosing();

            Assert.AreNotSame(_defaultPlane, _unitChooser.ChosenUnit);
        }

        [Test]
        public void CannotAffordDesiredPlane_ChoosesDefault()
        {
            _droneManager.NumOfDrones = _defaultPlane.Buildable.NumOfDronesRequired;
            _airThreatMonitor.CurrentThreatLevel.Returns(ThreatLevel.None);
            _navalThreatMonitor.CurrentThreatLevel.Returns(ThreatLevel.High);

            TriggerChoosing();

            Assert.AreSame(_defaultPlane, _unitChooser.ChosenUnit);
        }

        [Test]
        public void CannotAffordDesiredPlane_CannotAffordDefault_ChoosesNull()
        {
            _droneManager.NumOfDrones = _defaultPlane.Buildable.NumOfDronesRequired - 1;
            _airThreatMonitor.CurrentThreatLevel.Returns(ThreatLevel.None);
            _navalThreatMonitor.CurrentThreatLevel.Returns(ThreatLevel.High);

            TriggerChoosing();

            Assert.IsNull(_unitChooser.ChosenUnit);
        }

        [Test]
        public void DroneNumChanged_TriggersChoosing()
        {
            _airThreatMonitor.CurrentThreatLevel.Returns(ThreatLevel.None);
            _navalThreatMonitor.CurrentThreatLevel.Returns(ThreatLevel.High);

            _droneManager.DroneNumChanged += Raise.EventWith(_droneManager, new DroneNumChangedEventArgs(newNumOfDrones: 72));

            Assert.AreSame(_antiNavalPlane, _unitChooser.ChosenUnit);
        }

        [Test]
        public void NavalThreatChanged_TriggersChoosing()
        {
            _airThreatMonitor.CurrentThreatLevel.Returns(ThreatLevel.None);
            _navalThreatMonitor.CurrentThreatLevel.Returns(ThreatLevel.High);

            _airThreatMonitor.ThreatLevelChanged += Raise.Event();

            Assert.AreSame(_antiNavalPlane, _unitChooser.ChosenUnit);
        }

        [Test]
        public void AirThreatChanged_TriggersChoosing()
        {
            _airThreatMonitor.CurrentThreatLevel.Returns(ThreatLevel.None);
            _navalThreatMonitor.CurrentThreatLevel.Returns(ThreatLevel.High);

            _navalThreatMonitor.ThreatLevelChanged += Raise.Event();

            Assert.AreSame(_antiNavalPlane, _unitChooser.ChosenUnit);
        }

        private void TriggerChoosing()
        {
            _airThreatMonitor.ThreatLevelChanged += Raise.Event();
        }
    }
}
