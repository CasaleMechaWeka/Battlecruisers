using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Sorting
{
    public abstract class PvPBuildableUnlockedLevelSorter
    {
        protected readonly IPvPBuildableKeyFactory _keyFactory;

        protected PvPBuildableUnlockedLevelSorter(IPvPBuildableKeyFactory keyFactory)
        {
            PvPHelper.AssertIsNotNull(keyFactory);

            _keyFactory = keyFactory;
        }
    }
}