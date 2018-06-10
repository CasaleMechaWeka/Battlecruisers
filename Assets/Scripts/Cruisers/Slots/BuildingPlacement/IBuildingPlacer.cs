using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.Cruisers.Slots.BuildingPlacement
{
    public interface IBuildingPlacer
    {
        void PlaceBuilding(IBuilding buildingToPlace, ISlot parentSlot);
    }
}
