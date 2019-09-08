using BattleCruisers.Buildables.Buildings;
using UnityEngine;

namespace BattleCruisers.Cruisers.Slots.BuildingPlacement
{
    public interface IBuildingPlacerCalculator
    {
        Quaternion FindBuildingRotation(ISlot parentSlot);
        Vector3 FindHealthBarOffset(IBuilding building, ISlot parentSlot);
        Vector3 FindSpawnPosition(IBuilding buildingToPlace, ISlot parentSlot);
    }
}