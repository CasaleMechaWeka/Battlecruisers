using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Common.BuildableDetails.Stats
{
    public class UnitStatsController : BuildableStatsController<IUnit>
	{
        private StatsRowStarsController _movementSpeedRow;

        public override void Initialise()
        {
            base.Initialise();

            _movementSpeedRow = transform.FindNamedComponent<StatsRowStarsController>("MovementSpeedRow");
        }

        protected override void InternalShowStats(IUnit item, IUnit itemToCompareTo)
        {
            base.InternalShowStats(item, itemToCompareTo);

            int starRating = _unitMovementSpeedConverter.ConvertValueToStars(item.MaxVelocityInMPerS);
            ComparisonResult comparisonResult = _higherIsBetterComparer.CompareStats(item.MaxVelocityInMPerS, itemToCompareTo.MaxVelocityInMPerS);
            _movementSpeedRow.ShowResult(starRating, comparisonResult);
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