using System.Collections.Generic;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction
{
    public class PvPUnitTargets : IPvPUnitTargets
    {
        private readonly HashSet<IPvPTarget> _ships;
        public IReadOnlyCollection<IPvPTarget> Ships => _ships;

        private readonly HashSet<IPvPTarget> _aircraft;
        public IReadOnlyCollection<IPvPTarget> Aircraft => _aircraft;

        private readonly HashSet<IPvPTarget> _cruisers;
        public IReadOnlyCollection<IPvPTarget> Cruisers => _cruisers;

        private readonly HashSet<IPvPTarget> _shipsAndAircraft;
        public IReadOnlyCollection<IPvPTarget> ShipsAndAircraft => _shipsAndAircraft;

        public PvPUnitTargets(IPvPCruiserUnitMonitor cruiserUnitMonitor)
        {
            Assert.IsNotNull(cruiserUnitMonitor);

            _ships = new HashSet<IPvPTarget>();
            _aircraft = new HashSet<IPvPTarget>();
            _shipsAndAircraft = new HashSet<IPvPTarget>();
            _cruisers = new HashSet<IPvPTarget>();

            cruiserUnitMonitor.UnitStarted += CruiserUnitMonitor_UnitStarted;
            cruiserUnitMonitor.UnitDestroyed += CruiserUnitMonitor_UnitDestroyed;
        }

        private void CruiserUnitMonitor_UnitStarted(object sender, PvPUnitStartedEventArgs e)
        {
            Logging.Log(Tags.UNIT_TARGETS, $"{e.StartedUnit}");

            switch (e.StartedUnit.TargetType)
            {
                case PvPTargetType.Ships:
                    Assert.IsFalse(_ships.Contains(e.StartedUnit));
                    _ships.Add(e.StartedUnit);
                    AddToShipsAndAircraft(e);
                    break;

                case PvPTargetType.Aircraft:
                    Assert.IsFalse(_aircraft.Contains(e.StartedUnit));
                    _aircraft.Add(e.StartedUnit);
                    AddToShipsAndAircraft(e);
                    break;
            }
        }

        private void AddToShipsAndAircraft(PvPUnitStartedEventArgs e)
        {
            Assert.IsFalse(_shipsAndAircraft.Contains(e.StartedUnit));
            _shipsAndAircraft.Add(e.StartedUnit);
        }

        private void CruiserUnitMonitor_UnitDestroyed(object sender, PvPUnitDestroyedEventArgs e)
        {
            Logging.Log(Tags.UNIT_TARGETS, $"{e.DestroyedUnit}  id: {e.DestroyedUnit?.GameObject?.GetInstanceID()}");

            switch (e.DestroyedUnit.TargetType)
            {
                case PvPTargetType.Ships:
                    _ships.Remove(e.DestroyedUnit);
                    break;

                case PvPTargetType.Aircraft:
                    _aircraft.Remove(e.DestroyedUnit);
                    break;
            }

            _shipsAndAircraft.Remove(e.DestroyedUnit);
        }
    }
}