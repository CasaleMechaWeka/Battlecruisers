using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Stats
{
    public class PvPUnitStatsController : PvPBuildableStatsController<IPvPUnit>
    {
        public PvPStarsStatValue speed;

        public override void Initialise()
        {
            base.Initialise();

            Assert.IsNotNull(speed);
            speed.Initialise();
        }

        protected override void InternalShowStats(IPvPUnit item, IPvPUnit itemToCompareTo)
        {
            base.InternalShowStats(item, itemToCompareTo);

            int starRating = _unitMovementSpeedConverter.ConvertValueToStars(item.MaxVelocityInMPerS);
            if (starRating == 0)
            {
                starRating = 1;
            }
            PvPComparisonResult comparisonResult = _higherIsBetterComparer.CompareStats(item.MaxVelocityInMPerS, itemToCompareTo.MaxVelocityInMPerS);
            speed.ShowResult(starRating, comparisonResult);
        }

        // For units that can attack both ships and the cruiser (ships),
        // just show their ship damage.
        protected override float GetAntiCruiserDamage(IPvPUnit item)
        {
            if (item.AttackCapabilities.Contains(PvPTargetType.Ships))
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