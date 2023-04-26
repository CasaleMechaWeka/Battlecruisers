using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using System;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction
{
    public class PvPBuildingStartedEventArgs : EventArgs
    {
        public IPvPBuilding StartedBuilding { get; }

        public PvPBuildingStartedEventArgs(IPvPBuilding startedBuilding)
        {
            StartedBuilding = startedBuilding;
        }
    }

    public class PvPBuildingCompletedEventArgs : EventArgs
    {
        public IPvPBuilding CompletedBuilding { get; }

        public PvPBuildingCompletedEventArgs(IPvPBuilding completedBuilding)
        {
            CompletedBuilding = completedBuilding;
        }
    }

    public class PvPBuildingDestroyedEventArgs : EventArgs
    {
        public IPvPBuilding DestroyedBuilding { get; }

        public PvPBuildingDestroyedEventArgs(IPvPBuilding destroyedBuilding)
        {
            DestroyedBuilding = destroyedBuilding;
        }
    }

    public interface IPvPCruiserBuildingMonitor
    {
        /// <summary>
        /// Buildings that have been started and not destroyed.
        /// </summary>
        IReadOnlyCollection<IPvPBuilding> AliveBuildings { get; }

        event EventHandler<PvPBuildingStartedEventArgs> BuildingStarted;
        event EventHandler<PvPBuildingCompletedEventArgs> BuildingCompleted;
        event EventHandler<PvPBuildingDestroyedEventArgs> BuildingDestroyed;
    }
}