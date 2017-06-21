using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.Common.BuildingDetails.Stats;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI.Common.BuildingDetails
{
	public class BuildableStatsController : MonoBehaviour 
	{
		// FELIX  Inject
		private ValueToStarsConverter _valueToStarsConverter;
		private IStatsComparer _higherIsBetterComparer, _lowerIsBetterComparer;

		public StatsRowNumberController droneRow;
		public StatsRowNumberController buildTimeRow;
		public StatsRowStarsController healthRow;
		public StatsRowStarsController damageRow;

		private const string HEALTH_LABEL = "Health";
		private const string DAMAGE_LABEL = "Damage";
		private const string DRONES_LABEL = "Drones";
		private const string BUILD_TIME_LABEL = "BuildTime";

		private const string BUILD_TIME_SUFFIX = "s";

		void Awake()
		{
			_valueToStarsConverter = new ValueToStarsConverter();
			_higherIsBetterComparer = new HigherIsBetterComparer();
			_lowerIsBetterComparer = new LowerIsBetterComparer();
		}

		public void ShowBuildableStats(Buildable buildable, Buildable buildableToCompareTo = null)
		{
			if (buildableToCompareTo == null)
			{
				buildableToCompareTo = buildable;
			}

			droneRow.Initialise(DRONES_LABEL, buildable.numOfDronesRequired.ToString(), _lowerIsBetterComparer.CompareStats(buildable.numOfDronesRequired, buildableToCompareTo.numOfDronesRequired));
			buildTimeRow.Initialise(BUILD_TIME_LABEL, buildable.buildTimeInS.ToString() + BUILD_TIME_SUFFIX, _lowerIsBetterComparer.CompareStats(buildable.buildTimeInS, buildableToCompareTo.buildTimeInS));
			healthRow.Initialise(HEALTH_LABEL, _valueToStarsConverter.HealthValueToStars(buildable.maxHealth), _higherIsBetterComparer.CompareStats(buildable.Health, buildableToCompareTo.Health));
			damageRow.Initialise(DAMAGE_LABEL, _valueToStarsConverter.DamageValueToStars(buildable.Damage), _higherIsBetterComparer.CompareStats(buildable.Damage, buildableToCompareTo.Damage));
		}
	}
}