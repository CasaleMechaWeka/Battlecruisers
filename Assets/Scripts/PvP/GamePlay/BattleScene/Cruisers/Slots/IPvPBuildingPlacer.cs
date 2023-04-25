using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Slots;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots.BuildingPlacement
{
    public interface IPvPBuildingPlacer
    {
        void PlaceBuilding(IPvPBuilding buildingToPlace, IPvPSlot parentSlot);
    }
}
