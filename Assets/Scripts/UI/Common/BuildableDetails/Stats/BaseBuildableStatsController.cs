using BattleCruisers.Buildables;

namespace BattleCruisers.UI.Common.BuildingDetails.Stats
{
    public abstract class BaseBuildableStatsController<TItem> : StatsController<TItem> where TItem : class, IBuildable
	{
		public StatsRowNumberController droneRow;
		public StatsRowNumberController buildTimeRow;
		public StatsRowStarsController healthRow;
		public StatsRowStarsController damageRow;

		private const string DAMAGE_LABEL = "Damage";
		private const string BUILD_TIME_LABEL = "BuildTime";
		private const string BUILD_TIME_SUFFIX = "s";

        protected override void InternalShowStats(TItem item, TItem itemToCompareTo)
		{
			droneRow.Initialise(DRONES_LABEL, item.NumOfDronesRequired.ToString(), _lowerIsBetterComparer.CompareStats(item.NumOfDronesRequired, itemToCompareTo.NumOfDronesRequired));
			buildTimeRow.Initialise(BUILD_TIME_LABEL, item.BuildTimeInS.ToString() + BUILD_TIME_SUFFIX, _lowerIsBetterComparer.CompareStats(item.BuildTimeInS, itemToCompareTo.BuildTimeInS));
			healthRow.Initialise(HEALTH_LABEL, _valueToStarsConverter.HealthValueToStars(item.Health), _higherIsBetterComparer.CompareStats(item.Health, itemToCompareTo.Health));
			damageRow.Initialise(DAMAGE_LABEL, _valueToStarsConverter.DamageValueToStars(item.Damage), _higherIsBetterComparer.CompareStats(item.Damage, itemToCompareTo.Damage));
		}
	}
}