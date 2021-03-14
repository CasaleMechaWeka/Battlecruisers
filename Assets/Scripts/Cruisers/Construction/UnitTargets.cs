using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Construction
{
    public class UnitTargets : IUnitTargets
    {
        private readonly HashSet<ITarget> _ships;
        public IReadOnlyCollection<ITarget> Ships => _ships;

        private readonly HashSet<ITarget> _aircraft;
        public IReadOnlyCollection<ITarget> Aircraft => _aircraft;

        // FELIX  Update tests :)
        private readonly HashSet<ITarget> _shipsAndAircraft;
        public IReadOnlyCollection<ITarget> ShipsAndAircraft => _shipsAndAircraft;

        public UnitTargets(ICruiserUnitMonitor cruiserUnitMonitor)
        {
            Assert.IsNotNull(cruiserUnitMonitor);

            _ships = new HashSet<ITarget>();
            _aircraft = new HashSet<ITarget>();
            _shipsAndAircraft = new HashSet<ITarget>();

            cruiserUnitMonitor.UnitStarted += CruiserUnitMonitor_UnitStarted;
            cruiserUnitMonitor.UnitDestroyed += CruiserUnitMonitor_UnitDestroyed;
        }

        private void CruiserUnitMonitor_UnitStarted(object sender, UnitStartedEventArgs e)
        {
            Logging.Log(Tags.UNIT_TARGETS, $"{e.StartedUnit}");

            switch (e.StartedUnit.TargetType)
            {
                case TargetType.Ships:
                    Assert.IsFalse(_ships.Contains(e.StartedUnit));
                    _ships.Add(e.StartedUnit);
                    break;

                case TargetType.Aircraft:
                    Assert.IsFalse(_aircraft.Contains(e.StartedUnit));
                    _aircraft.Add(e.StartedUnit);
                    break;
            }

            Assert.IsFalse(_shipsAndAircraft.Contains(e.StartedUnit));
            _shipsAndAircraft.Add(e.StartedUnit);
        }

        private void CruiserUnitMonitor_UnitDestroyed(object sender, UnitDestroyedEventArgs e)
        {
            Logging.Log(Tags.UNIT_TARGETS, $"{e.DestroyedUnit}  id: {e.DestroyedUnit?.GameObject?.GetInstanceID()}");

            switch (e.DestroyedUnit.TargetType)
            {
                case TargetType.Ships:
                    _ships.Remove(e.DestroyedUnit);
                    break;

                case TargetType.Aircraft:
                    _aircraft.Remove(e.DestroyedUnit);
                    break;
            }

            _shipsAndAircraft.Remove(e.DestroyedUnit);
        }
    }
}