using System;
using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.ThreatMonitors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.FactoryManagers
{
    /// <summary>
    /// Chooses the unit that best counters the current threat (as long as we 
    /// can afford that plane).
    /// 
    /// If user builds lots of:
    /// A) Planes => Build interceptors
    /// B) Lots of ships => Build gunships
    /// </summary>
    public class PvPAircraftUnitChooser : PvPUnitChooser
    {
        private readonly IPvPBuildableWrapper<IPvPUnit> _defaultPlane, _lategamePlane, _antiAirPlane, _antiNavalPlane;
        private readonly IPvPDroneManager _droneManager;
        private readonly IPvPThreatMonitor _airThreatMonitor, _navalThreatMonitor;
        private readonly PvPThreatLevel _threatLevelThreshold;

        public PvPAircraftUnitChooser(
            IPvPBuildableWrapper<IPvPUnit> defaultPlane,
            IPvPBuildableWrapper<IPvPUnit> lategamePlane,
            IPvPBuildableWrapper<IPvPUnit> antiAirPlane,
            IPvPBuildableWrapper<IPvPUnit> antiNavalPlane,
            IPvPDroneManager droneManager,
            IPvPThreatMonitor airThreatMonitor,
            IPvPThreatMonitor navalThreatMonitor,
            PvPThreatLevel threatLevelThreshold)
        {
            PvPHelper.AssertIsNotNull(defaultPlane, antiAirPlane, antiNavalPlane, droneManager, airThreatMonitor, navalThreatMonitor);

            _defaultPlane = defaultPlane;
            _lategamePlane = lategamePlane;
            _antiAirPlane = antiAirPlane;
            _antiNavalPlane = antiNavalPlane;
            _droneManager = droneManager;
            _airThreatMonitor = airThreatMonitor;
            _navalThreatMonitor = navalThreatMonitor;
            _threatLevelThreshold = threatLevelThreshold;

            _droneManager.DroneNumChanged += _droneManager_DroneNumChanged;
            _airThreatMonitor.ThreatLevelChanged += ThreatMonitor_ThreatLevelChanged;
            _navalThreatMonitor.ThreatLevelChanged += ThreatMonitor_ThreatLevelChanged;

            ChooseUnit();
        }

        private void _droneManager_DroneNumChanged(object sender, PvPDroneNumChangedEventArgs e)
        {
            ChooseUnit();
        }

        private void ThreatMonitor_ThreatLevelChanged(object sender, EventArgs e)
        {
            ChooseUnit();
        }

        private void ChooseUnit()
        {
            IPvPBuildableWrapper<IPvPUnit> desiredUnit = ChooseDesiredUnit();

            if (!CanAfforUnit(desiredUnit))
            {
                desiredUnit = CanAfforUnit(_defaultPlane) ? _defaultPlane : null;
            }

            ChosenUnit = desiredUnit;
        }

        private IPvPBuildableWrapper<IPvPUnit> ChooseDesiredUnit()
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
                return _lategamePlane;
            }
        }

        private bool RandBool()
        {
            return UnityEngine.Random.value > 0.5;
        }

        private bool CanAfforUnit(IPvPBuildableWrapper<IPvPUnit> unitWrapper)
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
