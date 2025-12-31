using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.UI.Common.BuildableDetails.Stats;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils.Categorisation;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Stats
{
    public class PvPUnitStatsController : PvPBuildableStatsController<IPvPUnit>
    {
        public StarsStatValue speed;

        public override void Initialise()
        {
            base.Initialise();

            Assert.IsNotNull(speed);
            speed.Initialise();
        }

        protected override void InternalShowStats(IPvPUnit item, IPvPUnit itemToCompareTo)
        {
            base.InternalShowStats(item, itemToCompareTo);

            int starRating = ValueToStarsConverter.ConvertValueToStars(item.MaxVelocityInMPerS, ValueType.MovementSpeed);
            if (starRating == 0)
            {
                starRating = 1;
            }
            ComparisonResult comparisonResult = HigherIsBetterComparer.CompareStats(item.MaxVelocityInMPerS, itemToCompareTo.MaxVelocityInMPerS);
            speed.ShowResult(starRating, comparisonResult);
        }


        protected override void InternalShowStatsOfVariant(IPvPUnit item, VariantPrefab variant, IPvPUnit itemToCompareTo)
        {
            base.InternalShowStatsOfVariant(item, variant, itemToCompareTo);
            int starRating = ValueToStarsConverter.ConvertValueToStars(item.MaxVelocityInMPerS + variant.statVariant.max_velocity, ValueType.MovementSpeed);
            if (starRating == 0)
            {
                starRating = 1;
            }
            ComparisonResult comparisonResult = HigherIsBetterComparer.CompareStats(item.MaxVelocityInMPerS + variant.statVariant.max_velocity, itemToCompareTo.MaxVelocityInMPerS);
            speed.ShowResult(starRating, comparisonResult);
        }

        // For units that can attack both ships and the cruiser (ships),
        // just show their ship damage.
        protected override float GetAntiCruiserDamage(IPvPUnit item)
        {
            if (item.AttackCapabilities.Contains(TargetType.Ships))
            {
                // Eg:  Ships (Attack boat, frigate, etc)
                return 0;
            }
            else
            {
                // Eg:  Bomber
                return base.GetAntiCruiserDamage(item);
            }
        }
    }
}