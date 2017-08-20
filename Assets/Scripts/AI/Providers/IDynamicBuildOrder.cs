using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.Providers
{
    /// <summary>
    /// Subset if IEnumerator<IPrefabKey>
    /// </summary>
    public interface IDynamicBuildOrder
    {
        IPrefabKey Current { get; }

        bool MoveNext();
    }
}