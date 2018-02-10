using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Common.BuildingDetails.Stats
{
    public abstract class BaseBuildableStatsController<TItem> : StatsController<TItem> where TItem : class, IBuildable
	{
        private StatsRowNumberController _dronesRow, _buildTimeRow;
        private StatsRowStarsController _healthRow, _antiShipDamageRow, _antiAirDamageRow, _antiCruiserDamageRow;

		private const string DAMAGE_LABEL = "Damage";
		private const string BUILD_TIME_LABEL = "BuildTime";
		private const string BUILD_TIME_SUFFIX = "s";

        public override void Initialise()
        {
            base.Initialise();

            _dronesRow = transform.FindNamedComponent<StatsRowNumberController>("DronesRow");
            _buildTimeRow = transform.FindNamedComponent<StatsRowNumberController>("BuildTimeRow");
            _healthRow = transform.FindNamedComponent<StatsRowStarsController>("HealthRow");
            _antiShipDamageRow = transform.FindNamedComponent<StatsRowStarsController>("AntiShipDamageRow");
            _antiAirDamageRow = transform.FindNamedComponent<StatsRowStarsController>("AntiAirDamageRow");
            _antiCruiserDamageRow = transform.FindNamedComponent<StatsRowStarsController>("AntiCruiserDamageRow");
        }

        protected override void InternalShowStats(TItem item, TItem itemToCompareTo)
		{
			_dronesRow.Initialise(DRONES_LABEL, item.NumOfDronesRequired.ToString(), _lowerIsBetterComparer.CompareStats(item.NumOfDronesRequired, itemToCompareTo.NumOfDronesRequired));
			_buildTimeRow.Initialise(BUILD_TIME_LABEL, item.BuildTimeInS.ToString() + BUILD_TIME_SUFFIX, _lowerIsBetterComparer.CompareStats(item.BuildTimeInS, itemToCompareTo.BuildTimeInS));
			_healthRow.Initialise(HEALTH_LABEL, _valueToStarsConverter.HealthValueToStars(item.MaxHealth), _higherIsBetterComparer.CompareStats(item.MaxHealth, itemToCompareTo.MaxHealth));

            ShowDamageStat(_antiAirDamageRow, GetAntiAirDamage(item));
            ShowDamageStat(_antiShipDamageRow, GetAntiShipDamage(item));
            ShowDamageStat(_antiCruiserDamageRow, GetAntiCruiserDamage(item));
        }

        private void ShowDamageStat(StatsRowStarsController damageStatsRow, float damagePerS)
        {
            bool shouldShowRow = damagePerS > 0;
            damageStatsRow.gameObject.SetActive(shouldShowRow);

            if (shouldShowRow)
            {
                // FELIX
				//_damageRow.Initialise(DAMAGE_LABEL, _valueToStarsConverter.DamageValueToStars(item.Damage), _higherIsBetterComparer.CompareStats(item.Damage, itemToCompareTo.Damage));
                //damageStatsRow.Initialise();
            }
        }

        protected virtual float GetAntiAirDamage(TItem item)
        {
            return GetDamageForTargetType(item, TargetType.Aircraft);
        }

        protected virtual float GetAntiShipDamage(TItem item)
        {
            return GetDamageForTargetType(item, TargetType.Ships);
        }

        protected virtual float GetAntiCruiserDamage(TItem item)
        {
            return GetDamageForTargetType(item, TargetType.Cruiser);
        }

        protected float GetDamageForTargetType(TItem item, TargetType targetType)
        {
            float damagePerS = 0;

            foreach (IDamage damageStat in item.DamageStats)
            {
                if (damageStat.AttackCapabilities.Contains(targetType))
                {
                    damagePerS = damageStat.DamagePerS;
                    break;
                }
            }

            return damagePerS;
        }
	}
}