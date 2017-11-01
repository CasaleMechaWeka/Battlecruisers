using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Utils.DataStrctures;
using NUnit.Framework;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Turrets.AccuracyAdjusters
{
    public class GravityAffectedTargetBoundsFinderTests
    {
        private ITargetBoundsFinder _boundsFinder;
        private Vector2 _leftCruiserPosition, _rightCruiserPosition;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _boundsFinder = new GravityAffectedTargetBoundsFinder(targetXMarginInM: 1);

            _leftCruiserPosition = new Vector2(-35, 0);
            _rightCruiserPosition = new Vector2(35, 0);
        }

        [Test]
        public void FindTargetBounds_FiringLeftToRight()
        {
            IRange<Vector2> targetBounds = _boundsFinder.FindTargetBounds(_leftCruiserPosition, _rightCruiserPosition);
            IRange<Vector2> expectedBounds = new Range<Vector2>(new Vector2(34, 0), new Vector2(36, 0));

            Assert.AreEqual(expectedBounds, targetBounds);
        }

        [Test]
        public void FindTargetBounds_FiringRightToLeft()
        {
            IRange<Vector2> targetBounds = _boundsFinder.FindTargetBounds(_rightCruiserPosition, _leftCruiserPosition);
            IRange<Vector2> expectedBounds = new Range<Vector2>(new Vector2(-34, 0), new Vector2(-36, 0));

            Assert.AreEqual(expectedBounds, targetBounds);
        }
    }
}
