using System;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using UnityEngine;

namespace BattleCruisers.Cruisers.Slots.BuildingPlacement
{
    public class BuildingPlacer : IBuildingPlacer
    {
        public void PlaceBuilding(IBuilding buildingToPlace, ISlot parentSlot)
        {
            buildingToPlace.Rotation = FindBuildingRotation(parentSlot);
            buildingToPlace.Position = FindSpawnPosition(buildingToPlace, parentSlot);
        }

        private Quaternion FindBuildingRotation(ISlot parentSlot)
        {
            return parentSlot.Transform.Rotation;
        }

        private Vector3 FindSpawnPosition(IBuilding buildingToPlace, ISlot parentSlot)
        {
            switch (parentSlot.Direction)
            {
                case Direction.Right:
                    // Naval factory
                    float horizontalChange = buildingToPlace.Size.x / 2;
                    return parentSlot.BuildingPlacementPoint + (parentSlot.Transform.Right * horizontalChange);

                case Direction.Up:
                    // All other buildings
                    float verticalChange = buildingToPlace.Size.y / 2;
                    return parentSlot.BuildingPlacementPoint + (parentSlot.Transform.Up * verticalChange);

                default:
                    throw new ArgumentException("Invalid slot direction");
            }
        }
    }
}
