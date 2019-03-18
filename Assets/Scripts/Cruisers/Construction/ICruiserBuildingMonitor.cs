using BattleCruisers.Buildables.Buildings;
using System;
using System.Collections.Generic;

namespace BattleCruisers.Cruisers.Construction
{
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

        event EventHandler<StartedBuildingConstructionEventArgs> BuildingStarted;
        event EventHandler<CompletedBuildingConstructionEventArgs> BuildingCompleted;
        event EventHandler<BuildingDestroyedEventArgs> BuildingDestroyed;
    }
}