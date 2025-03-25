using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Sorting
{
    public class PvPBuildableSorterFactory : IPvPBuildableSorterFactory
    {
        public IPvPBuildableSorter<IPvPBuilding> CreateBuildingSorter()
        {
            return new PvPBuildingUnlockedLevelSorter();
        }

        public IPvPBuildableSorter<IPvPUnit> CreateUnitSorter()
        {
            return new PvPUnitUnlockedLevelSorter();
        }
    }
}
