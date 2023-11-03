using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Common.BuildableDetails.Stats
{
    public class UnitStatsController : BuildableStatsController<IUnit>
    {

        public StarsStatValue speed;
        public override void Initialise()
        {
            base.Initialise();
            Helper.AssertIsNotNull(speed);
            speed.Initialise();
        }

        protected override void InternalShowStats(IUnit item, IUnit itemToCompareTo)
        {
            base.InternalShowStats(item, itemToCompareTo);

            int starRating = _unitMovementSpeedConverter.ConvertValueToStars(item.MaxVelocityInMPerS);
            if (starRating == 0)
            {
                starRating = 1;
            }
            ComparisonResult comparisonResult = _higherIsBetterComparer.CompareStats(item.MaxVelocityInMPerS, itemToCompareTo.MaxVelocityInMPerS);
            speed.ShowResult(starRating, comparisonResult);
        }

        protected override void InternalShowStatsOfVariant(IUnit item, VariantPrefab variant, IUnit itemToCompareTo)
        {
            base.InternalShowStatsOfVariant(item, variant, itemToCompareTo);
            int starRating = _unitMovementSpeedConverter.ConvertValueToStars(item.MaxVelocityInMPerS + variant.statVariant.max_velocity);
            if (starRating == 0)
            {
                starRating = 1;
            }
            ComparisonResult comparisonResult = _higherIsBetterComparer.CompareStats(item.MaxVelocityInMPerS + variant.statVariant.max_velocity, itemToCompareTo.MaxVelocityInMPerS);
            speed.ShowResult(starRating, comparisonResult);
        }

        // For units that can attack both ships and the cruiser (ships),
        // just show their ship damage.
        protected override float GetAntiCruiserDamage(IUnit item)
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