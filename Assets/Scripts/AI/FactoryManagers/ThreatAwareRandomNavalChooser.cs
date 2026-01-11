using System.Collections.Generic;
using System.Linq;
using BattleCruisers.AI.ThreatMonitors;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.FactoryManagers
{
    /// <summary>
    /// Random naval chooser with simple threat awareness:
    /// - FlakTurtle is only considered if AIR threat is at least Low.
    /// - If BOTH air and naval threat are present, Frigate gets extra weight.
    /// - Otherwise picks random affordable ship.
    /// </summary>
    public class ThreatAwareRandomNavalChooser : UnitChooser
    {
        private readonly IList<IBuildableWrapper<IUnit>> _units;
        private readonly DroneManager _droneManager;
        private readonly BaseThreatMonitor _airThreat;
        private readonly BaseThreatMonitor _navalThreat;

        public ThreatAwareRandomNavalChooser(
            IList<IBuildableWrapper<IUnit>> units,
            DroneManager droneManager,
            BaseThreatMonitor airThreatMonitor,
            BaseThreatMonitor navalThreatMonitor)
        {
            Helper.AssertIsNotNull(units, droneManager, airThreatMonitor, navalThreatMonitor);
            Assert.IsTrue(units.Count != 0);

            _units = units;
            _droneManager = droneManager;
            _airThreat = airThreatMonitor;
            _navalThreat = navalThreatMonitor;

            _droneManager.DroneNumChanged += _droneManager_DroneNumChanged;
            _airThreat.ThreatLevelChanged += _threatChanged;
            _navalThreat.ThreatLevelChanged += _threatChanged;

            ChooseUnit();
        }

        private void _droneManager_DroneNumChanged(object sender, DroneNumChangedEventArgs e) => ChooseUnit();
        private void _threatChanged(object sender, System.EventArgs e) => ChooseUnit();

        private void ChooseUnit()
        {
            var affordable = _units.Where(u => u != null && u.Buildable.NumOfDronesRequired <= _droneManager.NumOfDrones).ToList();
            if (affordable.Count == 0)
            {
                ChosenUnit = null;
                return;
            }

            bool airPresent = _airThreat.CurrentThreatLevel != ThreatLevel.None;
            bool navalPresent = _navalThreat.CurrentThreatLevel != ThreatLevel.None;

            // Filter out FlakTurtle unless air threat exists
            var filtered = affordable.Where(u =>
            {
                string name = u.Buildable.PrefabName;
                if (!airPresent && name.Contains("FlakTurtle")) return false;
                return true;
            }).ToList();

            if (filtered.Count == 0)
                filtered = affordable; // fallback

            // Bias Frigate if both air and naval threats exist
            var weighted = new List<IBuildableWrapper<IUnit>>(filtered);
            if (airPresent && navalPresent)
            {
                foreach (var u in filtered)
                    if (u.Buildable.PrefabName.Contains("Frigate"))
                        weighted.Add(u); // duplicate once for small bias
            }

            int idx = UnityEngine.Random.Range(0, weighted.Count);
            ChosenUnit = weighted[idx];
        }

        public override void OnUnitBuilt() => ChooseUnit();

        public override void DisposeManagedState()
        {
            _droneManager.DroneNumChanged -= _droneManager_DroneNumChanged;
            _airThreat.ThreatLevelChanged -= _threatChanged;
            _navalThreat.ThreatLevelChanged -= _threatChanged;
        }
    }
}


