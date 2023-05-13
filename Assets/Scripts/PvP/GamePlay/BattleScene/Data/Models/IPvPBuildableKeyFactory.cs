using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys
{
    public interface IPvPBuildableKeyFactory
    {
        PvPBuildingKey CreateBuildingKey(IPvPBuilding building);
        PvPUnitKey CreateUnitKey(IPvPUnit unit);
    }
}