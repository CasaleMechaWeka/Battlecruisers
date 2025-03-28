using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.Common.BuildableDetails.Stats;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils.Categorisation;
using BattleCruisers.Scenes;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Stats
{
    public abstract class PvPBuildableStatsController<TItem> : StatsController<TItem> where TItem : class, IPvPBuildable
    {
        public NumberStatValue drones, buildTime;
        public StarsStatValue health, cruiserDamage, shipDamage, airDamage;

        public override void Initialise()
        {
            PvPHelper.AssertIsNotNull(drones, buildTime, health, cruiserDamage, shipDamage, airDamage);

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
            health.ShowResult(ValueToStarsConverter.ConvertValueToStars(item.MaxHealth, ValueType.BuildableHealth), HigherIsBetterComparer.CompareStats(item.MaxHealth, itemToCompareTo.MaxHealth));

            ShowDamageStat(cruiserDamage, GetAntiCruiserDamage(item), GetAntiCruiserDamage(itemToCompareTo), ValueType.AntiCruiser);
            ShowDamageStat(shipDamage, GetAntiShipDamage(item), GetAntiShipDamage(itemToCompareTo), ValueType.AntiShip);
            ShowDamageStat(airDamage, GetAntiAirDamage(item), GetAntiAirDamage(itemToCompareTo), ValueType.AntiAir);
        }

        protected override void InternalShowStatsOfVariant(TItem item, VariantPrefab variant, TItem itemToCompareTo = null)
        {
            drones.ShowResult((item.NumOfDronesRequired + variant.statVariant.drone_num).ToString(), LowerIsBetterComparer.CompareStats(item.NumOfDronesRequired, itemToCompareTo.NumOfDronesRequired));
            buildTime.ShowResult(((item.BuildTimeInS + variant.statVariant.build_time) * (item.NumOfDronesRequired + variant.statVariant.drone_num) * 0.5f).ToString(),
                LowerIsBetterComparer.CompareStats((item.BuildTimeInS + variant.statVariant.build_time) * (item.NumOfDronesRequired + variant.statVariant.drone_num) * 0.5f,
                itemToCompareTo.BuildTimeInS * itemToCompareTo.NumOfDronesRequired));
            health.ShowResult(ValueToStarsConverter.ConvertValueToStars(item.MaxHealth + variant.statVariant.max_health, ValueType.BuildableHealth),
                HigherIsBetterComparer.CompareStats(item.MaxHealth + variant.statVariant.max_health, itemToCompareTo.MaxHealth));

            ShowDamageStat(cruiserDamage, GetAntiCruiserDamage(item), GetAntiCruiserDamage(itemToCompareTo), ValueType.AntiCruiser);
            ShowDamageStat(shipDamage, GetAntiShipDamage(item) * (variant.IsUnit() ? variant.GetUnit().AttackCapabilities.Contains(TargetType.Ships) ? variant.statVariant.damage : 0 : 0), GetAntiShipDamage(itemToCompareTo), ValueType.AntiShip);
            ShowDamageStat(airDamage, GetAntiAirDamage(item) * (variant.IsUnit() ? variant.GetUnit().AttackCapabilities.Contains(TargetType.Aircraft) ? variant.statVariant.damage : 0 : 0), GetAntiAirDamage(itemToCompareTo), ValueType.AntiAir);
        }


        private void ShowDamageStat(StarsStatValue damageStatsRow, float damagePerS, float comparingItemDamagePerS, ValueType type)
        {
            bool shouldShowRow = damagePerS > 0;
            damageStatsRow.gameObject.SetActive(shouldShowRow);

            if (shouldShowRow)
            {
                damageStatsRow.ShowResult(ValueToStarsConverter.ConvertValueToStars(damagePerS, type), HigherIsBetterComparer.CompareStats(damagePerS, comparingItemDamagePerS));
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