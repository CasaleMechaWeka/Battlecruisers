using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Utils.Fetchers.Cache
{
    public interface IUntypedMultiCache<TBase> where TBase : class
    {
        TChild GetPrefab<TChild>(IPrefabKey prefabKey) where TChild : class, TBase;
    }
}