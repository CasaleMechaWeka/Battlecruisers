using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using NSubstitute;

namespace BattleCruisers.Tests.Utils.Extensions
{
    public static class CruiserControllerExtensions
    {
        public static void StartConstructingBuilding(this ICruiserController cruiser, IBuilding buildingToStart)
        {
            cruiser.BuildingStarted += Raise.EventWith(new StartedBuildingConstructionEventArgs(buildingToStart));
        }

        // FELIX  Remove :)
        public static void CompleteConstructingBuliding(this ICruiserController cruiser, IBuilding buildingToComplete)
        {
			cruiser.BuildingCompleted += Raise.EventWith(new CompletedBuildingConstructionEventArgs(buildingToComplete));
        }
    }
}