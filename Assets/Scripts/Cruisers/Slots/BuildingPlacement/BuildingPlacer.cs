using System;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using UnityEngine;

namespace BattleCruisers.Cruisers.Slots.BuildingPlacement
{
    // FELIX  UPdate tests :)
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
                // FELIX  Can collapse these 2 switches?  Are identical???
                case Direction.Right:
                    {
                        // Naval factory
                        // FELIX  Also handle y change
                        float horizontalChange = buildingToPlace.Position.x - buildingToPlace.PuzzleRootPoint.x;
                        return parentSlot.BuildingPlacementPoint + (parentSlot.Transform.Right * horizontalChange);
                    }

                case Direction.Up:
                    {
                        // All other buildings
                        float verticalChange = buildingToPlace.Position.y - buildingToPlace.PuzzleRootPoint.y;
                        float horizontalChange = buildingToPlace.Position.x - buildingToPlace.PuzzleRootPoint.x;
                        return parentSlot.BuildingPlacementPoint
                            + (parentSlot.Transform.Up * verticalChange)
                            + (parentSlot.Transform.Right * horizontalChange);
                    }

                default:
                    throw new ArgumentException("Invalid slot direction");
            }
        }
    }
}
