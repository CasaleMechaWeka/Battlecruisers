using BattleCruisers.Buildables;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using UnityEngine;

namespace BattleCruisers.UI.Common.BuildableDetails.Stats
{
    public abstract class StatsController<T> : MonoBehaviour where T : class, ITarget
    {
        public abstract void Initialise();
        public void ShowStats(T item, T itemToCompareTo = null)
        {
            if (itemToCompareTo == null)
            {
                itemToCompareTo = item;
            }

            InternalShowStats(item, itemToCompareTo);
        }

        public void ShowStatsOfVariant(T item, VariantPrefab variant, T itemToCompareTo = null)
        {
            if (itemToCompareTo == null)
            {
                itemToCompareTo = item;
            }
            InternalShowStatsOfVariant(item, variant, itemToCompareTo);
        }

        protected abstract void InternalShowStats(T item, T itemToCompareTo);

        protected abstract void InternalShowStatsOfVariant(T item, VariantPrefab variant, T itemToCompareTo);
    }
}