using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using NSubstitute;

namespace BattleCruisers.Tests.Utils.Extensions
{
    public static class CruiserBuildingMonitorExtensions
    {
        public static void EmitBuildingStarted(this ICruiserBuildingMonitor buildingMonitor, IBuilding buildingToStart)
        {
            buildingMonitor.BuildingStarted += Raise.EventWith(new StartedBuildingConstructionEventArgs(buildingToStart));
        }

        public static void EmitBuildingCompleted(this ICruiserBuildingMonitor buildingMonitor, IBuilding buildingToComplete)
        {
			buildingMonitor.BuildingCompleted += Raise.EventWith(new CompletedBuildingConstructionEventArgs(buildingToComplete));
        }
    }
}