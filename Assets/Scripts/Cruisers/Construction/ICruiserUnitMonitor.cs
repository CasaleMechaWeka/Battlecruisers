using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using System;
using System.Collections.Generic;

namespace BattleCruisers.Cruisers.Construction
{
    public class UnitDestroyedEventArgs : EventArgs
    {
        public IUnit DestroyedUnit { get; }

        public UnitDestroyedEventArgs(IUnit destroyedUnit)
        {
            DestroyedUnit = destroyedUnit;
        }
    }

    public interface ICruiserUnitMonitor
    {
        /// <summary>
        /// Units that have been completed but not destroyed.
        /// </summary>
        IReadOnlyCollection<IUnit> AliveUnits { get; }

        event EventHandler<StartedUnitConstructionEventArgs> UnitStarted;
        event EventHandler<CompletedUnitConstructionEventArgs> UnitCompleted;
        event EventHandler<UnitDestroyedEventArgs> UnitDestroyed;
    }
}