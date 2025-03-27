using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Categorisation;
using UnityEngine;

namespace BattleCruisers.UI.Common.BuildableDetails.Stats
{
    public abstract class BuildableStatsController<TItem> : StatsController<TItem> where TItem : class, IBuildable
    {
        public NumberStatValue drones, buildTime;
        public StarsStatValue health, cruiserDamage, shipDamage, airDamage;

        public override void Initialise()
        {
            Helper.AssertIsNotNull(drones, buildTime, health, cruiserDamage, shipDamage, airDamage);

            drones.Initialise();
            buildTime.Initialise();
            health.Initialise();
            cruiserDamage.Initialise();
            shipDamage.Initialise();
            airDamage.Initialise();
        }

        protected override void InternalShowStats(TItem item, TItem itemToCompareTo)
        {
            drones.ShowResult(item.NumOfDronesRequired.ToString(), LowerIsBetterComparer.CompareStats(item.NumOfDronesRequired, itemToCompareTo.NumOfDronesRequired));
            buildTime.ShowResult((item.BuildTimeInS * item.NumOfDronesRequired * 0.5f).ToString(), LowerIsBetterComparer.CompareStats((item.BuildTimeInS * item.NumOfDronesRequired), (itemToCompareTo.BuildTimeInS * itemToCompareTo.NumOfDronesRequired)));

            int healthStars = ValueToStarsConverter.ConvertValueToStars(item.MaxHealth, ValueType.BuildableHealth);
            health.ShowResult(healthStars, HigherIsBetterComparer.CompareStats(item.MaxHealth, itemToCompareTo.MaxHealth));

            ShowDamageStat(cruiserDamage, GetAntiCruiserDamage(item), GetAntiCruiserDamage(itemToCompareTo), ValueType.AntiCruiser);
            ShowDamageStat(shipDamage, GetAntiShipDamage(item), GetAntiShipDamage(itemToCompareTo), ValueType.AntiShip);
            ShowDamageStat(airDamage, GetAntiAirDamage(item), GetAntiAirDamage(itemToCompareTo), ValueType.AntiAir);
        }

        protected override void InternalShowStatsOfVariant(TItem item, VariantPrefab variant, TItem itemToCompareTo = null)
        {
            // Ensure itemToCompareTo is not null
            if (itemToCompareTo == null)
            {
                itemToCompareTo = item;
            }

            // Calculate and log the new health value with variant
            float baseHealth = item.MaxHealth;
            float healthAdjustment = variant.statVariant.max_health;
            float newMaxHealth = baseHealth + healthAdjustment;
            Debug.Log($"Base Health: {baseHealth}, Health Adjustment: {healthAdjustment}, New Max Health: {newMaxHealth}");

            // Ensure the new health value is clamped to a minimum of 1
            if (newMaxHealth < 1)
            {
                Debug.LogWarning($"Calculated newMaxHealth ({newMaxHealth}) is less than 1. Clamping to 1.");
                newMaxHealth = 1;
            }

            // Convert valid health value to stars
            int healthStars = ValueToStarsConverter.ConvertValueToStars(newMaxHealth, ValueType.BuildableHealth);
            Debug.Log($"Health Stars: {healthStars} for newMaxHealth: {newMaxHealth}");
            health.ShowResult(healthStars, HigherIsBetterComparer.CompareStats(newMaxHealth, itemToCompareTo.MaxHealth));

            // Calculate and log drones required
            int newDronesRequired = item.NumOfDronesRequired + variant.statVariant.drone_num;
            Debug.Log($"Base Drones: {item.NumOfDronesRequired}, Drone Adjustment: {variant.statVariant.drone_num}, New Drones Required: {newDronesRequired}");
            drones.ShowResult(newDronesRequired.ToString(), LowerIsBetterComparer.CompareStats(newDronesRequired, itemToCompareTo.NumOfDronesRequired));

            // Calculate and log build time
            float newBuildTime = (item.BuildTimeInS + variant.statVariant.build_time) * newDronesRequired * 0.5f;
            Debug.Log($"Base Build Time: {item.BuildTimeInS}, Build Time Adjustment: {variant.statVariant.build_time}, New Build Time: {newBuildTime}");
            buildTime.ShowResult(newBuildTime.ToString(), LowerIsBetterComparer.CompareStats(newBuildTime, itemToCompareTo.BuildTimeInS * itemToCompareTo.NumOfDronesRequired));

            // Calculate and show damage stats
            ShowDamageStat(cruiserDamage, GetAntiCruiserDamage(item), GetAntiCruiserDamage(itemToCompareTo), ValueType.AntiCruiser);
            ShowDamageStat(shipDamage, GetAntiShipDamage(item) * (variant.IsUnit() ? variant.GetUnit(ScreensSceneGod.Instance._prefabFactory).AttackCapabilities.Contains(TargetType.Ships) ? variant.statVariant.damage : 0 : 0), GetAntiShipDamage(itemToCompareTo), ValueType.AntiShip);
            ShowDamageStat(airDamage, GetAntiAirDamage(item) * (variant.IsUnit() ? variant.GetUnit(ScreensSceneGod.Instance._prefabFactory).AttackCapabilities.Contains(TargetType.Aircraft) ? variant.statVariant.damage : 0 : 0), GetAntiAirDamage(itemToCompareTo), ValueType.AntiAir);
        }

        private void ShowDamageStat(StarsStatValue damageStatsRow, float damagePerS, float comparingItemDamagePerS, ValueType type)
        {
            bool shouldShowRow = damagePerS > 0;
            damageStatsRow.gameObject.SetActive(shouldShowRow);

            if (shouldShowRow)
            {
                int damageStars = ValueToStarsConverter.ConvertValueToStars(damagePerS, type);
                // Ensure the star rating is at least 1
                if (damageStars == 0)
                {
                    damageStars = 1;
                }
                damageStatsRow.ShowResult(damageStars, HigherIsBetterComparer.CompareStats(damagePerS, comparingItemDamagePerS));
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
