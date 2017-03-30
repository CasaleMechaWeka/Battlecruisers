using BattleCruisers.Buildings;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI.BuildingDetails
{
	public class BuildableStatsController : MonoBehaviour 
	{
		// FELIX  Inject
		private ValueToStarsConverter _valueToStarsConverter;

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
		}

		public void ShowBuildableStats(Buildable buildable)
		{
			droneRow.Initialise(DRONES_LABEL, buildable.numOfDronesRequired.ToString());
			buildTimeRow.Initialise(BUILD_TIME_LABEL, buildable.buildTimeInS.ToString() + BUILD_TIME_SUFFIX);
			healthRow.Initialise(HEALTH_LABEL, _valueToStarsConverter.HealthValueToStars(buildable.health));
			damageRow.Initialise(DAMAGE_LABEL, _valueToStarsConverter.DamageValueToStars(buildable.Damage));
		}
	}
}