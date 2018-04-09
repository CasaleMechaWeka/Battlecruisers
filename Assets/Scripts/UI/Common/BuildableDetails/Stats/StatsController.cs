using BattleCruisers.Buildables;
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
            IValueToStarsConverterFactory converterFactory = new ValueToStarsConverterFactory();

            _unitMovementSpeedConverter = converterFactory.CreateUnitMovementSpeedConverter();
            _buildableHealthConverter = converterFactory.CreateBuildableHealthConverter();
            _cruiserHealthConverter = converterFactory.CreateCruiserHealthConverter();
            _antiAirDamageConverter = converterFactory.CreateAntiAirDamageConverter();
            _antiShipDamageConverter = converterFactory.CreateAntiShipDamageConverter();
            _antiCruiserConverter = converterFactory.CreateAntiCruiserDamageConverter();

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

		protected abstract void InternalShowStats(T item, T itemToCompareTo);
	}
}