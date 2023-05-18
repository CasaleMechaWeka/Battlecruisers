using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AccuracyAdjusters.BoundsFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    /// <summary>
    /// NOTE:  All angles are in degrees.
    /// </summary>
    public class PvPAccuracyAdjuster : IPvPAccuracyAdjuster
    {
        private readonly IPvPTargetBoundsFinder _boundsFinder;
        private readonly IPvPAngleCalculator _angleCalculator;
        private readonly IPvPAngleRangeFinder _angleRangeFinder;
        private readonly IPvPRandomGenerator _random;
        private readonly IPvPTurretStats _turretStats;

        public PvPAccuracyAdjuster(
            IPvPTargetBoundsFinder boundsFinder,
            IPvPAngleCalculator angleCalculator,
            IPvPAngleRangeFinder angleRangeFinder,
            IPvPRandomGenerator random,
            IPvPTurretStats turretStats)
        {
            PvPHelper.AssertIsNotNull(boundsFinder, angleCalculator, angleRangeFinder, random, turretStats);

            _boundsFinder = boundsFinder;
            _angleCalculator = angleCalculator;
            _angleRangeFinder = angleRangeFinder;
            _random = random;
            _turretStats = turretStats;
        }

        public float FindAngleInDegrees(float idealFireAngle, Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored)
        {
            IPvPRange<Vector2> onTargetBounds = _boundsFinder.FindTargetBounds(sourcePosition, targetPosition);

            float angleForCloserTarget = _angleCalculator.FindDesiredAngle(sourcePosition, onTargetBounds.Min, isSourceMirrored);
            float angleForFurtherTarget = _angleCalculator.FindDesiredAngle(sourcePosition, onTargetBounds.Max, isSourceMirrored);

            // Logging.Log(Tags.ACCURACY_ADJUSTERS, $"angleForCloserTarget: {angleForCloserTarget}  angleForFurtherTarget: {angleForFurtherTarget}");

            IPvPRange<float> onTargetAngleRange = new PvPOrderedRange(angleForCloserTarget, angleForFurtherTarget);
            IPvPRange<float> fireAngleRange = _angleRangeFinder.FindFireAngleRange(onTargetAngleRange, _turretStats.Accuracy);

            // Logging.Log(Tags.ACCURACY_ADJUSTERS, $"fireAngleRange: {fireAngleRange.Min} - {fireAngleRange.Max}");

            return _random.Range(fireAngleRange.Min, fireAngleRange.Max);
        }
    }
}
