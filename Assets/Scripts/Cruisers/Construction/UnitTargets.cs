using System.Collections.Generic;
using BattleCruisers.Buildables;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Construction
{
    // FELIX  Use :D
    public class UnitTargets : IUnitTargets
    {
        private readonly HashSet<ITarget> _ships;
        public IReadOnlyCollection<ITarget> Ships => _ships;

        private readonly HashSet<ITarget> _aircraft;
        public IReadOnlyCollection<ITarget> Aircraft => _aircraft;

        public UnitTargets(ICruiserUnitMonitor cruiserUnitMonitor)
        {
            Assert.IsNotNull(cruiserUnitMonitor);

            _ships = new HashSet<ITarget>();
            _aircraft = new HashSet<ITarget>();

            cruiserUnitMonitor.UnitCompleted += CruiserUnitMonitor_UnitCompleted;
            cruiserUnitMonitor.UnitDestroyed += CruiserUnitMonitor_UnitDestroyed;
        }

        private void CruiserUnitMonitor_UnitCompleted(object sender, UnitCompletedEventArgs e)
        {
            switch (e.CompletedUnit.TargetType)
            {
                case TargetType.Ships:
                    Assert.IsFalse(_ships.Contains(e.CompletedUnit));
                    _ships.Add(e.CompletedUnit);
                    break;

                case TargetType.Aircraft:
                    Assert.IsFalse(_aircraft.Contains(e.CompletedUnit));
                    _aircraft.Add(e.CompletedUnit);
                    break;
            }
        }

        // May occur for uncompleted unti.
        private void CruiserUnitMonitor_UnitDestroyed(object sender, UnitDestroyedEventArgs e)
        {
            switch (e.DestroyedUnit.TargetType)
            {
                case TargetType.Ships:
                    _ships.Remove(e.DestroyedUnit);
                    break;

                case TargetType.Aircraft:
                    _aircraft.Remove(e.DestroyedUnit);
                    break;
            }
        }
    }
}