using BattleCruisers.Buildables.Buildings;
using System;
using System.Collections.Generic;

namespace BattleCruisers.Cruisers.Construction
{
    public class BuildingStartedEventArgs : EventArgs
    {
        public IBuilding StartedBuilding { get; }

        public BuildingStartedEventArgs(IBuilding startedBuilding)
        {
            StartedBuilding = startedBuilding;
        }
    }

    public class BuildingCompletedEventArgs : EventArgs
    {
        public IBuilding CompletedBuilding { get; }

        public BuildingCompletedEventArgs(IBuilding completedBuilding)
        {
            CompletedBuilding = completedBuilding;
        }
    }

    public class BuildingDestroyedEventArgs : EventArgs
    {
        public IBuilding DestroyedBuilding { get; }

        public BuildingDestroyedEventArgs(IBuilding destroyedBuilding)
        {
            DestroyedBuilding = destroyedBuilding;
        }
    }

    public interface ICruiserBuildingMonitor
    {
        /// <summary>
        /// Buildings that have been completed but not destroyed.
        /// </summary>
        IReadOnlyCollection<IBuilding> AliveBuildings { get; }

        event EventHandler<BuildingStartedEventArgs> BuildingStarted;
        event EventHandler<BuildingCompletedEventArgs> BuildingCompleted;
        event EventHandler<BuildingDestroyedEventArgs> BuildingDestroyed;
    }
}