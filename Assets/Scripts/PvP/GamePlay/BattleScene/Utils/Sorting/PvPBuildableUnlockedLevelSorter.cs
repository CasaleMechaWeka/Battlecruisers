using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Sorting
{
    public abstract class PvPBuildableUnlockedLevelSorter
    {
        protected readonly IStaticData _staticData;
        protected readonly IPvPBuildableKeyFactory _keyFactory;

        protected PvPBuildableUnlockedLevelSorter(IStaticData staticData, IPvPBuildableKeyFactory keyFactory)
        {
            PvPHelper.AssertIsNotNull(staticData, keyFactory);

            _staticData = staticData;
            _keyFactory = keyFactory;
        }
    }
}