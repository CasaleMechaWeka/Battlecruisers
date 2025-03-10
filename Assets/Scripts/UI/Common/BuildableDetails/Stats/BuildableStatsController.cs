using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Categorisation;
//using Unity.Tutorials.Core.Editor;
using UnityEngine;

namespace BattleCruisers.UI.Common.BuildableDetails.Stats
{
    public abstract class BuildableStatsController<TItem> : StatsController<TItem> where TItem : class, IBuildable
    {
        public NumberStatValue drones, buildTime;
        public StarsStatValue health, cruiserDamage, shipDamage, airDamage;

        public override void Initialise()
        {
            base.Initialise();

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
            drones.ShowResult(item.NumOfDronesRequired.ToString(), _lowerIsBetterComparer.CompareStats(item.NumOfDronesRequired, itemToCompareTo.NumOfDronesRequired));
            buildTime.ShowResult((item.BuildTimeInS * item.NumOfDronesRequired * 0.5f).ToString(), _lowerIsBetterComparer.CompareStats((item.BuildTimeInS * item.NumOfDronesRequired), (itemToCompareTo.BuildTimeInS * itemToCompareTo.NumOfDronesRequired)));

            int healthStars = _buildableHealthConverter.ConvertValueToStars(item.MaxHealth);
            health.ShowResult(healthStars, _higherIsBetterComparer.CompareStats(item.MaxHealth, itemToCompareTo.MaxHealth));

            ShowDamageStat(cruiserDamage, GetAntiCruiserDamage(item), GetAntiCruiserDamage(itemToCompareTo), _antiCruiserConverter);
            ShowDamageStat(shipDamage, GetAntiShipDamage(item), GetAntiShipDamage(itemToCompareTo), _antiShipDamageConverter);
            ShowDamageStat(airDamage, GetAntiAirDamage(item), GetAntiAirDamage(itemToCompareTo), _antiAirDamageConverter);
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
            int healthStars = _buildableHealthConverter.ConvertValueToStars(newMaxHealth);
            Debug.Log($"Health Stars: {healthStars} for newMaxHealth: {newMaxHealth}");
            health.ShowResult(healthStars, _higherIsBetterComparer.CompareStats(newMaxHealth, itemToCompareTo.MaxHealth));

            // Calculate and log drones required
            int newDronesRequired = item.NumOfDronesRequired + variant.statVariant.drone_num;
            Debug.Log($"Base Drones: {item.NumOfDronesRequired}, Drone Adjustment: {variant.statVariant.drone_num}, New Drones Required: {newDronesRequired}");
            drones.ShowResult(newDronesRequired.ToString(), _lowerIsBetterComparer.CompareStats(newDronesRequired, itemToCompareTo.NumOfDronesRequired));

            // Calculate and log build time
            float newBuildTime = (item.BuildTimeInS + variant.statVariant.build_time) * newDronesRequired * 0.5f;
            Debug.Log($"Base Build Time: {item.BuildTimeInS}, Build Time Adjustment: {variant.statVariant.build_time}, New Build Time: {newBuildTime}");
            buildTime.ShowResult(newBuildTime.ToString(), _lowerIsBetterComparer.CompareStats(newBuildTime, itemToCompareTo.BuildTimeInS * itemToCompareTo.NumOfDronesRequired));

            // Calculate and show damage stats
            ShowDamageStat(cruiserDamage, GetAntiCruiserDamage(item), GetAntiCruiserDamage(itemToCompareTo), _antiCruiserConverter);
            ShowDamageStat(shipDamage, GetAntiShipDamage(item) * (variant.IsUnit() ? variant.GetUnit(ScreensSceneGod.Instance._prefabFactory).AttackCapabilities.Contains(TargetType.Ships) ? variant.statVariant.damage : 0 : 0), GetAntiShipDamage(itemToCompareTo), _antiShipDamageConverter);
            ShowDamageStat(airDamage, GetAntiAirDamage(item) * (variant.IsUnit() ? variant.GetUnit(ScreensSceneGod.Instance._prefabFactory).AttackCapabilities.Contains(TargetType.Aircraft) ? variant.statVariant.damage : 0 : 0), GetAntiAirDamage(itemToCompareTo), _antiAirDamageConverter);
        }

        private void ShowDamageStat(StarsStatValue damageStatsRow, float damagePerS, float comparingItemDamagePerS, IValueToStarsConverter converter)
        {
            bool shouldShowRow = damagePerS > 0;
            damageStatsRow.gameObject.SetActive(shouldShowRow);

            if (shouldShowRow)
            {
                int damageStars = converter.ConvertValueToStars(damagePerS);
                damageStatsRow.ShowResult(damageStars, _higherIsBetterComparer.CompareStats(damagePerS, comparingItemDamagePerS));
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
