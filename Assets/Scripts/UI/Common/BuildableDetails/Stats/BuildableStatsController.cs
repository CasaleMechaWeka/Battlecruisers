using BattleCruisers.Buildables;

namespace BattleCruisers.UI.Common.BuildingDetails.Stats
{
    public class BuildableStatsController : StatsController<Buildable>
	{
		public StatsRowNumberController droneRow;
		public StatsRowNumberController buildTimeRow;
		public StatsRowStarsController healthRow;
		public StatsRowStarsController damageRow;

		private const string DAMAGE_LABEL = "Damage";
		private const string BUILD_TIME_LABEL = "BuildTime";
		private const string BUILD_TIME_SUFFIX = "s";

		protected override void InternalShowStats(Buildable item, Buildable itemToCompareTo)
		{
			droneRow.Initialise(DRONES_LABEL, item.numOfDronesRequired.ToString(), _lowerIsBetterComparer.CompareStats(item.numOfDronesRequired, itemToCompareTo.numOfDronesRequired));
			buildTimeRow.Initialise(BUILD_TIME_LABEL, item.buildTimeInS.ToString() + BUILD_TIME_SUFFIX, _lowerIsBetterComparer.CompareStats(item.buildTimeInS, itemToCompareTo.buildTimeInS));
			healthRow.Initialise(HEALTH_LABEL, _valueToStarsConverter.HealthValueToStars(item.maxHealth), _higherIsBetterComparer.CompareStats(item.Health, itemToCompareTo.Health));
			damageRow.Initialise(DAMAGE_LABEL, _valueToStarsConverter.DamageValueToStars(item.Damage), _higherIsBetterComparer.CompareStats(item.Damage, itemToCompareTo.Damage));
		}
	}
}