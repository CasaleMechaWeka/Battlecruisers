using BattleCruisers.Buildables.Buildings;
using UnityEngine;

namespace BattleCruisers.Cruisers.Slots.BuildingPlacement
{
    // FELIX  Interface, use, test
    public class BuildingPlacerCalculator : IBuildingPlacerCalculator
    {
        public Quaternion FindBuildingRotation(ISlot parentSlot)
        {
            return parentSlot.Transform.Rotation;
        }

        public Vector3 FindSpawnPosition(IBuilding buildingToPlace, ISlot parentSlot)
        {
            float verticalChange = buildingToPlace.Position.y - buildingToPlace.PuzzleRootPoint.y;
            float horizontalChange = buildingToPlace.Position.x - buildingToPlace.PuzzleRootPoint.x;

            return parentSlot.BuildingPlacementPoint
                + (parentSlot.Transform.Up * verticalChange)
                + (parentSlot.Transform.Right * horizontalChange);
        }

        public Vector2 FindHealthBarOffset(IBuilding building, ISlot parentSlot)
        {
            if (building.HealthBar.Offset.x == 0
                || !parentSlot.Transform.IsMirroredAcrossYAxis)
            {
                return building.HealthBar.Position;
            }

            return
                new Vector2(
                    -building.HealthBar.Offset.x,
                    building.HealthBar.Offset.y);
        }
    }
}
