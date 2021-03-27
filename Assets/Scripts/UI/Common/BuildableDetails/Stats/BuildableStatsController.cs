using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Categorisation;

namespace BattleCruisers.UI.Common.BuildableDetails.Stats
{
    // FELIX  Avoid duplacite code with compact versions. Handle once we have the 
    // loadout and loot screen informator designs.
    public abstract class BuildableStatsController<TItem> : StatsController<TItem> where TItem : class, IBuildable
	{
        private StatsRowNumberController _dronesRow, _buildTimeRow;
        private StatsRowStarsController _healthRow, _antiShipDamageRow, _antiAirDamageRow, _antiCruiserDamageRow;

		private const string BUILD_TIME_SUFFIX = "s";

        public override void Initialise()
        {
            base.Initialise();

            _dronesRow = transform.FindNamedComponent<StatsRowNumberController>("DronesRow");
            _dronesRow.Initialise();

            _buildTimeRow = transform.FindNamedComponent<StatsRowNumberController>("BuildTimeRow");
            _buildTimeRow.Initialise();

            _healthRow = transform.FindNamedComponent<StatsRowStarsController>("HealthRow");
            _healthRow.Initialise();

            _antiShipDamageRow = transform.FindNamedComponent<StatsRowStarsController>("AntiShipDamageRow");
            _antiShipDamageRow.Initialise();

            _antiAirDamageRow = transform.FindNamedComponent<StatsRowStarsController>("AntiAirDamageRow");
            _antiAirDamageRow.Initialise();

            _antiCruiserDamageRow = transform.FindNamedComponent<StatsRowStarsController>("AntiCruiserDamageRow");
            _antiCruiserDamageRow.Initialise();
        }

        protected override void InternalShowStats(TItem item, TItem itemToCompareTo)
		{
			_dronesRow.ShowResult(item.NumOfDronesRequired.ToString(), _lowerIsBetterComparer.CompareStats(item.NumOfDronesRequired, itemToCompareTo.NumOfDronesRequired));
			_buildTimeRow.ShowResult(item.BuildTimeInS.ToString() + BUILD_TIME_SUFFIX, _lowerIsBetterComparer.CompareStats(item.BuildTimeInS, itemToCompareTo.BuildTimeInS));
            _healthRow.ShowResult(_buildableHealthConverter.ConvertValueToStars(item.MaxHealth), _higherIsBetterComparer.CompareStats(item.MaxHealth, itemToCompareTo.MaxHealth));

            ShowDamageStat(_antiAirDamageRow, GetAntiAirDamage(item), GetAntiAirDamage(itemToCompareTo), _antiAirDamageConverter);
            ShowDamageStat(_antiShipDamageRow, GetAntiShipDamage(item), GetAntiShipDamage(itemToCompareTo), _antiShipDamageConverter);
            ShowDamageStat(_antiCruiserDamageRow, GetAntiCruiserDamage(item), GetAntiCruiserDamage(itemToCompareTo), _antiCruiserConverter);
        }

        private void ShowDamageStat(StatsRowStarsController damageStatsRow, float damagePerS, float comparingItemDamagePerS, IValueToStarsConverter converter)
        {
            bool shouldShowRow = damagePerS > 0;
            damageStatsRow.gameObject.SetActive(shouldShowRow);

            if (shouldShowRow)
            {
                damageStatsRow.ShowResult(converter.ConvertValueToStars(damagePerS), _higherIsBetterComparer.CompareStats(damagePerS, comparingItemDamagePerS));
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

            foreach (IDamageCapability damageCapability in item.DamageCapabilities)
            {
                if (damageCapability.AttackCapabilities.Contains(targetType))
                {
                    damagePerS = damageCapability.DamagePerS;
                    break;
                }
            }

            return damagePerS;
        }
	}
}