using BattleCruisers.Buildables.Units;
using System;
using System.Collections.Generic;

namespace BattleCruisers.Cruisers.Construction
{
    public class UnitStartedEventArgs : EventArgs
    {
        public IUnit StartedUnit { get; }

        public UnitStartedEventArgs(IUnit startedUnit)
        {
            StartedUnit = startedUnit;
        }
    }

    public class UnitCompletedEventArgs : EventArgs
    {
        public IUnit CompletedUnit { get; }

        public UnitCompletedEventArgs(IUnit completedUnit)
        {
            CompletedUnit = completedUnit;
        }
    }

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

        event EventHandler<UnitStartedEventArgs> UnitStarted;
        event EventHandler<UnitCompletedEventArgs> UnitCompleted;
        event EventHandler<UnitDestroyedEventArgs> UnitDestroyed;
    }
}