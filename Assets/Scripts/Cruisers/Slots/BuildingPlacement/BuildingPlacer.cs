using BattleCruisers.Buildables.Buildings;
using UnityEngine;

namespace BattleCruisers.Cruisers.Slots.BuildingPlacement
{
    public class BuildingPlacer : IBuildingPlacer
    {
        public void PlaceBuilding(IBuilding buildingToPlace, ISlot parentSlot)
        {
            buildingToPlace.Rotation = FindBuildingRotation(parentSlot);
            buildingToPlace.Position = FindSpawnPosition(buildingToPlace, parentSlot);
            buildingToPlace.HealthBar.Offset = FindHealthBarOffset(buildingToPlace, parentSlot);
        }

        private Quaternion FindBuildingRotation(ISlot parentSlot)
        {
            return parentSlot.Transform.Rotation;
        }

        private Vector3 FindSpawnPosition(IBuilding buildingToPlace, ISlot parentSlot)
        {
            float verticalChange = buildingToPlace.Position.y - buildingToPlace.PuzzleRootPoint.y;
            float horizontalChange = buildingToPlace.Position.x - buildingToPlace.PuzzleRootPoint.x;

            return parentSlot.BuildingPlacementPoint
                + (parentSlot.Transform.Up * verticalChange)
                + (parentSlot.Transform.Right * horizontalChange);
        }

        // FELIX  Update tests :)
        private Vector2 FindHealthBarOffset(IBuilding building, ISlot parentSlot)
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
