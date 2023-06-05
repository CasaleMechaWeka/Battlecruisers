using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys
{
    public class PvPBuildableKeyFactory : IPvPBuildableKeyFactory
    {
        public PvPUnitKey CreateUnitKey(IPvPUnit unit)
        {
            return new PvPUnitKey(unit.Category, "PvP" + unit.PrefabName);
        }

        public PvPBuildingKey CreateBuildingKey(IPvPBuilding building)
        {
            return new PvPBuildingKey(building.Category, "PvP" + building.PrefabName);
        }
    }
}