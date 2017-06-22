using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.Common.BuildingDetails.Stats;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI.Common.BuildingDetails.Stats
{
	public abstract class StatsController<T> : MonoBehaviour where T : Target
	{
		protected ValueToStarsConverter _valueToStarsConverter;
		protected IStatsComparer _higherIsBetterComparer, _lowerIsBetterComparer;

		protected const string HEALTH_LABEL = "Health";
		protected const string DRONES_LABEL = "Drones";

		void Awake()
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

			ShowStats(item, itemToCompareTo);
		}

		protected abstract void InternalShowStats(T item, T itemToCompareTo);
	}
}