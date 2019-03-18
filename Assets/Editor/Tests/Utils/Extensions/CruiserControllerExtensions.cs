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
    }
}