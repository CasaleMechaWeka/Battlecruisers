using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Drones
{
    public interface IPvPBuildingProvider
    {
        IPvPBuilding Building { get; }
    }
}
