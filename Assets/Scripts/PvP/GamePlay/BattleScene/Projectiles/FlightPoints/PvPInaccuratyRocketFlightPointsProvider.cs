using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.FlightPoints
{
    /// <summary>
    /// Similar to RocketFlightPointsProvider, but introduces inaccuracy.
    /// </summary>
    public class PvPInaccuratyRocketFlightPointsProvider : PvPRocketFlightPointsProvider
    {
        private readonly PvPFlightPointStats _stats;
        private readonly IPvPRandomGenerator _random;

        // Because may miss target on the x axis, need to aim lower to hit cruiser or water
        private const float TARGET_Y_ADJUSTMENT = -5;

        public PvPInaccuratyRocketFlightPointsProvider(PvPFlightPointStats stats)
        {
            Assert.IsNotNull(stats);

            _stats = stats;
            _random = PvPRandomGenerator.Instance;
        }

        protected override Vector2 CreateAscendPoint(Vector2 sourcePosition, Vector2 targetPosition, float cruisingPointsXOffset, float cruisingAltitudeInM)
        {
            Vector2 idealPoint = base.CreateAscendPoint(sourcePosition, targetPosition, cruisingPointsXOffset, cruisingAltitudeInM);
            return FuzzCruisingPoint(idealPoint, _stats.AscendPointRadiusVariationM);
        }

        protected override Vector2 CreateDescendPoint(Vector2 sourcePosition, Vector2 targetPosition, float cruisingPointsXOffset, float cruisingAltitudeInM)
        {
            Vector2 idealPoint = base.CreateDescendPoint(sourcePosition, targetPosition, cruisingPointsXOffset, cruisingAltitudeInM);
            return FuzzCruisingPoint(idealPoint, _stats.DescendPointRadiusVariationM);
        }

        private Vector2 FuzzCruisingPoint(Vector2 idealPoint, float fuzzRadiusM)
        {
            return
                new Vector2(
                    _random.RangeFromCenter(idealPoint.x, fuzzRadiusM),
                    _random.RangeFromCenter(idealPoint.y, fuzzRadiusM));
        }

        protected override Vector2 CreateTargetPoint(Vector2 targetPosition)
        {
            Vector2 idealPoint = base.CreateTargetPoint(targetPosition);
            return FuzzTargetPoint(idealPoint, _stats.TargetPointXRadiusVariationM);
        }

        private Vector2 FuzzTargetPoint(Vector2 targetPoint, float xFuzzRadiusM)
        {
            return
                new Vector2(
                    _random.RangeFromCenter(targetPoint.x, xFuzzRadiusM),
                    targetPoint.y + TARGET_Y_ADJUSTMENT);
        }
    }
}
