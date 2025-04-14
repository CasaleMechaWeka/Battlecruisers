using System;
using BattleCruisers.AI.ThreatMonitors;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;

namespace BattleCruisers.AI.FactoryManagers
{
    /// <summary>
    /// Chooses the unit that best counters the current threat (as long as we 
    /// can afford that plane).
    /// 
    /// If user builds lots of:
    /// A) Planes => Build interceptors
    /// B) Lots of ships => Build gunships
    /// </summary>
    public class AircraftUnitChooser : UnitChooser
    {
        private readonly IBuildableWrapper<IUnit> _defaultPlane, _lategamePlane, _antiAirPlane, _antiNavalPlane, _broadswordGunship, _stratBomber;
        private readonly DroneManager _droneManager;
        private readonly BaseThreatMonitor _airThreatMonitor, _navalThreatMonitor;
        private readonly ThreatLevel _threatLevelThreshold;

        public AircraftUnitChooser(
            IBuildableWrapper<IUnit> defaultPlane,
            IBuildableWrapper<IUnit> lategamePlane,
            IBuildableWrapper<IUnit> antiAirPlane,
            IBuildableWrapper<IUnit> antiNavalPlane,
            IBuildableWrapper<IUnit> broadswordGunship,
            IBuildableWrapper<IUnit> stratBomber,
            DroneManager droneManager,
            BaseThreatMonitor airThreatMonitor,
            BaseThreatMonitor navalThreatMonitor,
            ThreatLevel threatLevelThreshold)
        {
            Helper.AssertIsNotNull(defaultPlane, antiAirPlane, antiNavalPlane, broadswordGunship, stratBomber, droneManager, airThreatMonitor, navalThreatMonitor);

            _defaultPlane = defaultPlane;
            _lategamePlane = lategamePlane;
            _antiAirPlane = antiAirPlane;
            _antiNavalPlane = antiNavalPlane;
            _broadswordGunship = broadswordGunship;
            _stratBomber = stratBomber;
            _droneManager = droneManager;
            _airThreatMonitor = airThreatMonitor;
            _navalThreatMonitor = navalThreatMonitor;
            _threatLevelThreshold = threatLevelThreshold;

            _droneManager.DroneNumChanged += _droneManager_DroneNumChanged;
            _airThreatMonitor.ThreatLevelChanged += ThreatMonitor_ThreatLevelChanged;
            _navalThreatMonitor.ThreatLevelChanged += ThreatMonitor_ThreatLevelChanged;

            ChooseUnit();
        }

        private void _droneManager_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
        {
            ChooseUnit();
        }

        private void ThreatMonitor_ThreatLevelChanged(object sender, EventArgs e)
        {
            ChooseUnit();
        }

        private void ChooseUnit()
        {
            IBuildableWrapper<IUnit> desiredUnit = ChooseDesiredUnit();

            if (!CanAfforUnit(desiredUnit))
            {
                desiredUnit = CanAfforUnit(_defaultPlane) ? _defaultPlane : null;
            }

            ChosenUnit = desiredUnit;
        }

        private IBuildableWrapper<IUnit> ChooseDesiredUnit()
        {
            if (_airThreatMonitor.CurrentThreatLevel >= _threatLevelThreshold
                && _navalThreatMonitor.CurrentThreatLevel >= _threatLevelThreshold)
            {
                return RandBool() ? _antiAirPlane : _antiNavalPlane;
            }
            else if (_airThreatMonitor.CurrentThreatLevel >= _threatLevelThreshold)
            {
                return _antiAirPlane;
            }
            else if (_navalThreatMonitor.CurrentThreatLevel >= _threatLevelThreshold)
            {
                return _antiNavalPlane;
            }
            else
            {
                int randInt = UnityEngine.Random.Range(0, 20);
                IBuildableWrapper<IUnit> lateGamePlane;

                if (randInt > 18)
                    lateGamePlane = _broadswordGunship;
                else if (randInt > 12)
                    lateGamePlane = _stratBomber;
                else
                    lateGamePlane = _lategamePlane;

                return lateGamePlane;
            }
        }

        private bool RandBool()
        {
            return UnityEngine.Random.value > 0.5;
        }

        private bool CanAfforUnit(IBuildableWrapper<IUnit> unitWrapper)
        {
            return unitWrapper.Buildable.NumOfDronesRequired <= _droneManager.NumOfDrones;
        }

        public override void DisposeManagedState()
        {
            _droneManager.DroneNumChanged -= _droneManager_DroneNumChanged;
            _airThreatMonitor.ThreatLevelChanged -= ThreatMonitor_ThreatLevelChanged;
            _navalThreatMonitor.ThreatLevelChanged -= ThreatMonitor_ThreatLevelChanged;
        }
    }
}
