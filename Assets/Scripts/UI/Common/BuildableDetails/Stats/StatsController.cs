using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI.Common.BuildingDetails.Stats
{
    public abstract class StatsController<T> : MonoBehaviour where T : class, ITarget
	{
		protected ValueToStarsConverter _valueToStarsConverter;
		protected IStatsComparer _higherIsBetterComparer, _lowerIsBetterComparer;

		protected const string HEALTH_LABEL = "Health";
		protected const string DRONES_LABEL = "Drones";

		public virtual void Initialise()
		{
			_valueToStarsConverter = new ValueToStarsConverter();
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