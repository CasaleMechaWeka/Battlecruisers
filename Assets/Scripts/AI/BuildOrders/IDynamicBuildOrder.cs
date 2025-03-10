using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.BuildOrders
{
    /// <summary>
    /// Subset if IEnumerator<IPrefabKey>.  Slight modification on the
    /// Current property.
    /// </summary>
    public interface IDynamicBuildOrder
    {
        /// <summary>
        /// Gets the prefab key.  If the last MoveNext() call returned false,
        /// will return null (unlike IEnumerator, where the behaviour is undefined).
        /// </summary>
        BuildingKey Current { get; }

        /// <summary>
        /// Returns true while the build order has a valid Current.  Returns
        /// false once we have run out of valid Currents.  Subsequent calls
        /// will also return false.
        /// </summary>
        bool MoveNext();
    }
}
