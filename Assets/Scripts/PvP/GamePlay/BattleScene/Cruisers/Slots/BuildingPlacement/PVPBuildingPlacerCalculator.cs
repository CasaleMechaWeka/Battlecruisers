using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.BuildableOutline;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots.BuildingPlacement
{
    public class PvPBuildingPlacerCalculator : IPvPBuildingPlacerCalculator
    {
        public Quaternion FindBuildingRotation(IPvPSlot parentSlot)
        {
            return parentSlot.Transform.Rotation;
        }

        public Vector3 FindSpawnPosition(IPvPBuilding buildingToPlace, IPvPSlot parentSlot)
        {
            float verticalChange = buildingToPlace.Position.y - buildingToPlace.PuzzleRootPoint.y;
            float horizontalChange = buildingToPlace.Position.x - buildingToPlace.PuzzleRootPoint.x;

            return parentSlot.BuildingPlacementPoint
                + (parentSlot.Transform.Up * verticalChange)
                + (parentSlot.Transform.Right * horizontalChange);
        }

        public Vector3 FindOutlineSpawnPosition(PvPBuildableOutlineController outlineToPlace, IPvPSlot parentSlot)
        {
            float verticalChange = outlineToPlace.Position.y - outlineToPlace.PuzzleRootPoint.y;
            float horizontalChange = outlineToPlace.Position.x - outlineToPlace.PuzzleRootPoint.x;

            return parentSlot.BuildingPlacementPoint
                + (parentSlot.Transform.Up * verticalChange)
                + (parentSlot.Transform.Right * horizontalChange);
        }

        public Vector2 FindHealthBarOffset(IPvPBuilding building, IPvPSlot parentSlot)
        {
            if (building.HealthBar.Offset.x == 0
                || !parentSlot.Transform.IsMirroredAcrossYAxis)
            {
                return building.HealthBar.Offset;
            }

            return
                new Vector2(
                    -building.HealthBar.Offset.x,
                    building.HealthBar.Offset.y);
        }
    }
}
