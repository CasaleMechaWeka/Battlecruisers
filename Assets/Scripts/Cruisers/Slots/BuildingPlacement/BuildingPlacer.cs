using System;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using UnityEngine;

namespace BattleCruisers.Cruisers.Slots.BuildingPlacement
{
    // FELIX  Test :)
    public class BuildingPlacer : IBuildingPlacer
    {
        public void PlaceBuilding(IBuilding buildingToPlace, ISlot parentSlot)
        {
            buildingToPlace.Rotation = FindBuildingRotation(parentSlot);
            buildingToPlace.Position = FindSpawnPosition(buildingToPlace, parentSlot);
        }

        private Quaternion FindBuildingRotation(ISlot parentSlot)
        {
            return parentSlot.Transform.rotation;
        }

        private Vector3 FindSpawnPosition(IBuilding buildingToPlace, ISlot parentSlot)
        {
            switch (parentSlot.Direction)
            {
                case Direction.Right:
                    float horizontalChange = (parentSlot.Size.x + buildingToPlace.Size.x) / 2 + (buildingToPlace.CustomOffsetProportion * buildingToPlace.Size.x);
                    return parentSlot.Transform.position + (parentSlot.Transform.right * horizontalChange);

                case Direction.Up:
                    float verticalChange = (parentSlot.Size.y + buildingToPlace.Size.y) / 2 + (buildingToPlace.CustomOffsetProportion * buildingToPlace.Size.y);
                    return parentSlot.Transform.position + (parentSlot.Transform.up * verticalChange);

                default:
                    throw new ArgumentException("Invalid slot direction");
            }
        }
    }
}
