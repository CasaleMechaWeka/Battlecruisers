using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Sorting
{
    public class PvPBuildableSorterFactory : IPvPBuildableSorterFactory
    {
        protected readonly IPvPBuildableKeyFactory _keyFactory;

        public PvPBuildableSorterFactory(IPvPBuildableKeyFactory keyFactory)
        {
            PvPHelper.AssertIsNotNull(keyFactory);

            _keyFactory = keyFactory;
        }

        public IPvPBuildableSorter<IPvPBuilding> CreateBuildingSorter()
        {
            return new PvPBuildingUnlockedLevelSorter(_keyFactory);
        }

        public IPvPBuildableSorter<IPvPUnit> CreateUnitSorter()
        {
            return new PvPUnitUnlockedLevelSorter(_keyFactory);
        }
    }
}
