using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.BuildableOutline;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots.BuildingPlacement
{
    public interface IPvPBuildingPlacer
    {
        void PlaceBuilding(IPvPBuilding buildingToPlace, IPvPSlot parentSlot);
        void PlaceOutline(PvPBuildableOutlineController outline, IPvPSlot parentSlot);
    }
}
