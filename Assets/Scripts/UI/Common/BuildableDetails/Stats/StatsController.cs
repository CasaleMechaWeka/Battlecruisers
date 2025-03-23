using BattleCruisers.Buildables;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils.Categorisation;
using UnityEngine;

namespace BattleCruisers.UI.Common.BuildableDetails.Stats
{
    public abstract class StatsController<T> : MonoBehaviour where T : class, ITarget
    {
        protected IValueToStarsConverter _unitMovementSpeedConverter;
        protected IValueToStarsConverter _buildableHealthConverter, _cruiserHealthConverter;
        protected IValueToStarsConverter _antiAirDamageConverter, _antiShipDamageConverter, _antiCruiserConverter;
        protected IStatsComparer _higherIsBetterComparer, _lowerIsBetterComparer;

        public virtual void Initialise()
        {
            _unitMovementSpeedConverter = new UnitMovementSpeedToStarsConverter();
            _buildableHealthConverter = new BuildableHealthToStarsConverter();
            _cruiserHealthConverter = new CruiserHealthToStarsConverter();
            _antiAirDamageConverter = new AntiAirDamageToStarsConverter();
            _antiShipDamageConverter = new AntiShipDamageToStarsConverter();
            _antiCruiserConverter = new AntiCruiserDamageToStarsConverter();
            _higherIsBetterComparer = new HigherIsBetterComparer();
            _lowerIsBetterComparer = new LowerIsBetterComparer();
        }

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