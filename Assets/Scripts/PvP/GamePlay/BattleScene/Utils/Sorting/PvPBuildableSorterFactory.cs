using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Sorting
{
    public class PvPBuildableSorterFactory : IPvPBuildableSorterFactory
    {
        protected readonly IStaticData _staticData;
        protected readonly IPvPBuildableKeyFactory _keyFactory;

        public PvPBuildableSorterFactory(IStaticData staticData, IPvPBuildableKeyFactory keyFactory)
        {
            PvPHelper.AssertIsNotNull(staticData, keyFactory);

            _staticData = staticData;
            _keyFactory = keyFactory;
        }

        public IPvPBuildableSorter<IPvPBuilding> CreateBuildingSorter()
        {
            return new PvPBuildingUnlockedLevelSorter(_staticData, _keyFactory);
        }

        public IPvPBuildableSorter<IPvPUnit> CreateUnitSorter()
        {
            return new PvPUnitUnlockedLevelSorter(_staticData, _keyFactory);
        }
    }
}
