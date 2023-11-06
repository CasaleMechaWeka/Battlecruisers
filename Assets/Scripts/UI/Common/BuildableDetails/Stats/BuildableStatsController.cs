using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Categorisation;
//using Unity.Tutorials.Core.Editor;
using UnityEngine;
using UnityEngine.UI;

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
            health.ShowResult(_buildableHealthConverter.ConvertValueToStars(item.MaxHealth), _higherIsBetterComparer.CompareStats(item.MaxHealth, itemToCompareTo.MaxHealth));

            ShowDamageStat(cruiserDamage, GetAntiCruiserDamage(item), GetAntiCruiserDamage(itemToCompareTo), _antiCruiserConverter);
            ShowDamageStat(shipDamage, GetAntiShipDamage(item), GetAntiShipDamage(itemToCompareTo), _antiShipDamageConverter);
            ShowDamageStat(airDamage, GetAntiAirDamage(item), GetAntiAirDamage(itemToCompareTo), _antiAirDamageConverter);
        }

        protected override void InternalShowStatsOfVariant(TItem item, VariantPrefab variant, TItem itemToCompareTo = null)
        {
            drones.ShowResult((item.NumOfDronesRequired + variant.statVariant.drone_num).ToString() , _lowerIsBetterComparer.CompareStats(item.NumOfDronesRequired, itemToCompareTo.NumOfDronesRequired));
            buildTime.ShowResult(((item.BuildTimeInS + variant.statVariant.build_time) * item.NumOfDronesRequired * 0.5f).ToString(), _lowerIsBetterComparer.CompareStats((item.BuildTimeInS * item.NumOfDronesRequired), (itemToCompareTo.BuildTimeInS * itemToCompareTo.NumOfDronesRequired)));
            health.ShowResult(_buildableHealthConverter.ConvertValueToStars(item.MaxHealth + variant.statVariant.max_health), _higherIsBetterComparer.CompareStats(item.MaxHealth, itemToCompareTo.MaxHealth));

            ShowDamageStat(cruiserDamage, GetAntiCruiserDamage(item) , GetAntiCruiserDamage(itemToCompareTo), _antiCruiserConverter);
            ShowDamageStat(shipDamage, GetAntiShipDamage(item) + (variant.IsUnit() ? variant.GetUnit(ScreensSceneGod.Instance._prefabFactory).AttackCapabilities.Contains(TargetType.Ships) ? variant.statVariant.damage : 0 : 0), GetAntiShipDamage(itemToCompareTo), _antiShipDamageConverter);
            ShowDamageStat(airDamage, GetAntiAirDamage(item) + (variant.IsUnit() ? variant.GetUnit(ScreensSceneGod.Instance._prefabFactory).AttackCapabilities.Contains(TargetType.Aircraft) ? variant.statVariant.damage : 0 : 0), GetAntiAirDamage(itemToCompareTo), _antiAirDamageConverter);
        }

        private void ShowDamageStat(StarsStatValue damageStatsRow, float damagePerS, float comparingItemDamagePerS, IValueToStarsConverter converter)
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