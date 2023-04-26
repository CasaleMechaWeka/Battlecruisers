using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using System;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction
{
    public class PvPUnitStartedEventArgs : EventArgs
    {
        public IPvPUnit StartedUnit { get; }

        public PvPUnitStartedEventArgs(IPvPUnit startedUnit)
        {
            StartedUnit = startedUnit;
        }
    }

    public class PvPUnitCompletedEventArgs : EventArgs
    {
        public IPvPUnit CompletedUnit { get; }

        public PvPUnitCompletedEventArgs(IPvPUnit completedUnit)
        {
            CompletedUnit = completedUnit;
        }
    }

    public class PvPUnitDestroyedEventArgs : EventArgs
    {
        public IPvPUnit DestroyedUnit { get; }

        public PvPUnitDestroyedEventArgs(IPvPUnit destroyedUnit)
        {
            DestroyedUnit = destroyedUnit;
        }
    }

    public interface IPvPCruiserUnitMonitor
    {
        /// <summary>
        /// Units that have been started and not destroyed.
        /// </summary>
        IReadOnlyCollection<IPvPUnit> AliveUnits { get; }

        event EventHandler<PvPUnitStartedEventArgs> UnitStarted;
        event EventHandler<PvPUnitCompletedEventArgs> UnitCompleted;
        event EventHandler<PvPUnitDestroyedEventArgs> UnitDestroyed;
    }
}