using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Categorisation;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Stats
{
    public abstract class PvPBuildableStatsController<TItem> : PvPStatsController<TItem> where TItem : class, IPvPBuildable
    {
        public PvPNumberStatValue drones, buildTime;
        public PvPStarsStatValue health, cruiserDamage, shipDamage, airDamage;

        public override void Initialise()
        {
            base.Initialise();

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
            drones.ShowResult(item.NumOfDronesRequired.ToString(), _lowerIsBetterComparer.CompareStats(item.NumOfDronesRequired, itemToCompareTo.NumOfDronesRequired));
            buildTime.ShowResult((item.BuildTimeInS * item.NumOfDronesRequired * 0.5f).ToString(), _lowerIsBetterComparer.CompareStats((item.BuildTimeInS * item.NumOfDronesRequired), (itemToCompareTo.BuildTimeInS * itemToCompareTo.NumOfDronesRequired)));
            health.ShowResult(_buildableHealthConverter.ConvertValueToStars(item.MaxHealth), _higherIsBetterComparer.CompareStats(item.MaxHealth, itemToCompareTo.MaxHealth));

            ShowDamageStat(cruiserDamage, GetAntiCruiserDamage(item), GetAntiCruiserDamage(itemToCompareTo), _antiCruiserConverter);
            ShowDamageStat(shipDamage, GetAntiShipDamage(item), GetAntiShipDamage(itemToCompareTo), _antiShipDamageConverter);
            ShowDamageStat(airDamage, GetAntiAirDamage(item), GetAntiAirDamage(itemToCompareTo), _antiAirDamageConverter);
        }

        private void ShowDamageStat(PvPStarsStatValue damageStatsRow, float damagePerS, float comparingItemDamagePerS, IPvPValueToStarsConverter converter)
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
            return GetDamageForTargetType(item, PvPTargetType.Aircraft);
        }

        protected virtual float GetAntiShipDamage(TItem item)
        {
            return GetDamageForTargetType(item, PvPTargetType.Ships);
        }

        protected virtual float GetAntiCruiserDamage(TItem item)
        {
            return GetDamageForTargetType(item, PvPTargetType.Cruiser);
        }

        protected float GetDamageForTargetType(TItem item, PvPTargetType targetType)
        {
            float damagePerS = 0;

            foreach (IPvPDamageCapability damageCapability in item.DamageCapabilities)
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