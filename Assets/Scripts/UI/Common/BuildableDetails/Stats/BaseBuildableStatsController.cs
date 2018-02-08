using BattleCruisers.Buildables;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Common.BuildingDetails.Stats
{
    public abstract class BaseBuildableStatsController<TItem> : StatsController<TItem> where TItem : class, IBuildable
	{
        private StatsRowNumberController _dronesRow;
        private StatsRowNumberController _buildTimeRow;
        private StatsRowStarsController _healthRow;
        private StatsRowStarsController _damageRow;

		private const string DAMAGE_LABEL = "Damage";
		private const string BUILD_TIME_LABEL = "BuildTime";
		private const string BUILD_TIME_SUFFIX = "s";

        public override void Initialise()
        {
            base.Initialise();

            _dronesRow = transform.FindNamedComponent<StatsRowNumberController>("DronesRow");
            _buildTimeRow = transform.FindNamedComponent<StatsRowNumberController>("BuildTimeRow");
            _healthRow = transform.FindNamedComponent<StatsRowStarsController>("HealthRow");
            _damageRow = transform.FindNamedComponent<StatsRowStarsController>("DamageRow");
        }

        protected override void InternalShowStats(TItem item, TItem itemToCompareTo)
		{
			_dronesRow.Initialise(DRONES_LABEL, item.NumOfDronesRequired.ToString(), _lowerIsBetterComparer.CompareStats(item.NumOfDronesRequired, itemToCompareTo.NumOfDronesRequired));
			_buildTimeRow.Initialise(BUILD_TIME_LABEL, item.BuildTimeInS.ToString() + BUILD_TIME_SUFFIX, _lowerIsBetterComparer.CompareStats(item.BuildTimeInS, itemToCompareTo.BuildTimeInS));
			_healthRow.Initialise(HEALTH_LABEL, _valueToStarsConverter.HealthValueToStars(item.MaxHealth), _higherIsBetterComparer.CompareStats(item.MaxHealth, itemToCompareTo.MaxHealth));
			_damageRow.Initialise(DAMAGE_LABEL, _valueToStarsConverter.DamageValueToStars(item.Damage), _higherIsBetterComparer.CompareStats(item.Damage, itemToCompareTo.Damage));
		}
	}
}