using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters.BoundsFinders;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    public class AccuracyAdjusterTests
    {
        private IAccuracyAdjuster _accuracyAdjuster;

        // Constructor parameters
        private ITargetBoundsFinder _boundsFinder;
        private IAngleCalculator _angleCalculator;
        private IAngleRangeFinder _angleRangeFinder;
        private IRandomGenerator _random;
        private float _projectileVelocityInMPerS;
        private ITurretStats _turretStats;

        // FindAngelInDegrees() parameters
        private float _idealFireAngleInDegrees;
        private Vector2 _sourcePosition, _targetPosition;
		private bool _isMirrored;

        // Other
        private IRange<Vector2> _onTargetBounds;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            CreateAccuracyAdjuster();
            CreateMethodParameters();
        }

        private void CreateAccuracyAdjuster()
        {
            _boundsFinder = Substitute.For<ITargetBoundsFinder>();
            _angleCalculator = Substitute.For<IAngleCalculator>();
            _angleRangeFinder = Substitute.For<IAngleRangeFinder>();
            _random = Substitute.For<IRandomGenerator>();
            _turretStats = Substitute.For<ITurretStats>();
            _turretStats.Accuracy.Returns(0.5f);

            _projectileVelocityInMPerS = 10;

            _accuracyAdjuster = new AccuracyAdjuster(_boundsFinder, _angleCalculator, _angleRangeFinder, _random, _turretStats);
        }

        private void CreateMethodParameters()
        {
            _sourcePosition = new Vector2(-35, 0);
            _targetPosition = new Vector2(35, 0);
            _idealFireAngleInDegrees = 45;
            _isMirrored = false;
        }

        [Test]
        public void FindAngleInDegrees()
        {
            // On target bounds
            _onTargetBounds = new Range<Vector2>(new Vector2(34, 0), new Vector2(36, 0));
            _boundsFinder
                .FindTargetBounds(_sourcePosition, _targetPosition)
                .Returns(_onTargetBounds);

            IRange<float> onTargetRange = new Range<float>(44, 46);

            // Min on target angle
            _angleCalculator
                .FindDesiredAngle(_sourcePosition, _onTargetBounds.Min, _isMirrored)
                .Returns(onTargetRange.Min);

            // Max on target angle
            _angleCalculator
                .FindDesiredAngle(_sourcePosition, _onTargetBounds.Max, _isMirrored)
                .Returns(onTargetRange.Max);

            IRange<float> fireRange = new Range<float>(40, 50);
            _angleRangeFinder
                .FindFireAngleRange(onTargetRange, _turretStats.Accuracy)
                .Returns(fireRange);

            float expectedFireAngle = 42;
            _random
                .Range(fireRange.Min, fireRange.Max)
                .Returns(expectedFireAngle);

            float actualFireAngle = _accuracyAdjuster.FindAngleInDegrees(_idealFireAngleInDegrees, _sourcePosition, _targetPosition, _isMirrored);
            Assert.AreEqual(expectedFireAngle, actualFireAngle);
        }
    }
}
