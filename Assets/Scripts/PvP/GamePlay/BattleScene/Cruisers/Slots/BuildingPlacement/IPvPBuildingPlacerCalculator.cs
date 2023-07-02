using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.BuildableOutline;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots.BuildingPlacement
{
    public interface IPvPBuildingPlacerCalculator
    {
        Quaternion FindBuildingRotation(IPvPSlot parentSlot);
        Vector2 FindHealthBarOffset(IPvPBuilding building, IPvPSlot parentSlot);
        Vector3 FindSpawnPosition(IPvPBuilding buildingToPlace, IPvPSlot parentSlot);
        Vector3 FindOutlineSpawnPosition(PvPBuildableOutlineController outlineToPlace, IPvPSlot parentSlot);
    }
}