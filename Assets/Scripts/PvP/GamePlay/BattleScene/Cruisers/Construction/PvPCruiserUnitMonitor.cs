using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction
{
    public class PvPCruiserUnitMonitor : IPvPCruiserUnitMonitor, IPvPManagedDisposable
    {
        private readonly IPvPCruiserBuildingMonitor _buildingMonitor;

        private readonly HashSet<IPvPUnit> _aliveUnits;
        public IReadOnlyCollection<IPvPUnit> AliveUnits => _aliveUnits;

        public event EventHandler<PvPUnitStartedEventArgs> UnitStarted;
        public event EventHandler<PvPUnitCompletedEventArgs> UnitCompleted;
        public event EventHandler<PvPUnitDestroyedEventArgs> UnitDestroyed;

        public PvPCruiserUnitMonitor(IPvPCruiserBuildingMonitor buildingMonitor)
        {
            Assert.IsNotNull(buildingMonitor);

            _buildingMonitor = buildingMonitor;
            _buildingMonitor.BuildingCompleted += _buildingMonitor_BuildingCompleted;

            _aliveUnits = new HashSet<IPvPUnit>();
        }

        private void _buildingMonitor_BuildingCompleted(object sender, PvPBuildingCompletedEventArgs e)
        {
            IPvPFactory factory = e.CompletedBuilding as IPvPFactory;

            if (factory != null)
            {
                factory.UnitStarted += Factory_StartedBuildingUnit;
                factory.UnitCompleted += Factory_CompletedBuildingUnit;
                factory.Destroyed += Factory_Destroyed;
            }
        }

        private void Factory_StartedBuildingUnit(object sender, PvPUnitStartedEventArgs e)
        {
            Logging.Log(Tags.UNIT_MONITOR, $"_aliveUnits.Count: {_aliveUnits.Count}  e.StartedUnit: {e.StartedUnit}");

            Assert.IsFalse(_aliveUnits.Contains(e.StartedUnit));
            _aliveUnits.Add(e.StartedUnit);

            e.StartedUnit.Destroyed += Unit_Destroyed;

            UnitStarted?.Invoke(this, e);
        }

        private void Factory_CompletedBuildingUnit(object sender, PvPUnitCompletedEventArgs e)
        {
            UnitCompleted?.Invoke(this, e);
        }

        private void Unit_Destroyed(object sender, PvPDestroyedEventArgs e)
        {
            Logging.Log(Tags.UNIT_MONITOR, $"_aliveUnits.Count: {_aliveUnits.Count}  e.DestroyedTarget: {e.DestroyedTarget}");

            IPvPUnit destroyedUnit = e.DestroyedTarget.Parse<IPvPUnit>();
            destroyedUnit.Destroyed -= Unit_Destroyed;

            Assert.IsTrue(_aliveUnits.Contains(destroyedUnit));
            _aliveUnits.Remove(destroyedUnit);

            UnitDestroyed?.Invoke(this, new PvPUnitDestroyedEventArgs(destroyedUnit));
        }

        private void Factory_Destroyed(object sender, PvPDestroyedEventArgs e)
        {
            IPvPFactory factory = e.DestroyedTarget.Parse<IPvPFactory>();
            factory.UnitStarted -= Factory_StartedBuildingUnit;
            factory.UnitCompleted -= Factory_CompletedBuildingUnit;
            factory.Destroyed -= Factory_Destroyed;
        }

        public void DisposeManagedState()
        {
            _buildingMonitor.BuildingCompleted -= _buildingMonitor_BuildingCompleted;
        }
    }
}