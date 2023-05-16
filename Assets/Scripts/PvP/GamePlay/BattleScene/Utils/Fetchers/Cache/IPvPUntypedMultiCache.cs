using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Cache
{
    public interface IPvPUntypedMultiCache<TBase> where TBase : class
    {
        TChild GetPrefab<TChild>(IPvPPrefabKey prefabKey) where TChild : class, TBase;
    }
}