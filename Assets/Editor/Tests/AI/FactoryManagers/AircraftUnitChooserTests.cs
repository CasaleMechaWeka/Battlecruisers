using BattleCruisers.AI.FactoryManagers;
using BattleCruisers.AI.ThreatMonitors;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Drones;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.AI.FactoryManagers
{
    public class AircraftUnitChooserTests
	{
		private IUnitChooser _unitChooser;
		private IBuildableWrapper<IUnit> _defaultPlane, _antiAirPlane, _antiNavalPlane;
        private IDroneManager _droneManager;
        private IThreatMonitor _airThreatMonitor, _navalThreatMonitor;

		[SetUp]
		public void SetuUp()
		{
			UnityAsserts.Assert.raiseExceptions = true;

			_defaultPlane = CreateUnit(numOfDrones: 2);
			_antiAirPlane = CreateUnit(numOfDrones: 6);
			_antiNavalPlane = CreateUnit(numOfDrones: 6);

			_droneManager = Substitute.For<IDroneManager>();
			_droneManager.NumOfDrones.Returns(12);

            _airThreatMonitor = Substitute.For<IThreatMonitor>();
            _navalThreatMonitor = Substitute.For<IThreatMonitor>();

            _unitChooser = new AircraftUnitChooser(
                _defaultPlane,
                _antiAirPlane,
                _antiNavalPlane,
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
	}
}
