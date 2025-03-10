using BattleCruisers.Data.Models.PrefabKeys;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Cache
{
    public interface IPvPUntypedMultiCache<TBase> where TBase : class
    {
        TChild GetPrefab<TChild>(IPrefabKey prefabKey) where TChild : class, TBase;
        ICollection<IPrefabKey> GetKeys();
        ICollection<TBase> GetValues();
    }
}