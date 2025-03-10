using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Utils.DataStrctures;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    public class LinearAngleRangeFinderTests
    {
        private IAngleRangeFinder _rangeFinder;

        [SetUp]
        public void SetuUp()
        {
            _rangeFinder = new LinearRangeFinder();
        }

        [Test]
        public void FindFireAngleRange_InvalidOnTargetRange_Throws()
        {
            IRange<float> invalidOnTargetRange = new Range<float>(0, -1);
            float accuracy = 0.5f;
            Assert.Throws<UnityAsserts.AssertionException>(() => _rangeFinder.FindFireAngleRange(invalidOnTargetRange, accuracy));
        }

        [Test]
        public void FindFireAngleRange_AccuracyOf100_ReturnsOnTargetRange()
        {
            IRange<float> onTargetRange = new Range<float>(0, 2);
            float accuracy = 1;
            Assert.AreEqual(onTargetRange, _rangeFinder.FindFireAngleRange(onTargetRange, accuracy));
        }

        [Test]
        public void FindFireAngleRange_AccuracyOf75()
        {
            IRange<float> onTargetRange = new Range<float>(0, 3);
            float accuracy = 0.75f;
            IRange<float> expectedRange = new Range<float>(-0.5f, 3.5f);
            Assert.AreEqual(expectedRange, _rangeFinder.FindFireAngleRange(onTargetRange, accuracy));
        }

        [Test]
        public void FindFireAngleRange_AccuracyOf50()
        {
            IRange<float> onTargetRange = new Range<float>(0, 2);
            float accuracy = 0.5f;
            IRange<float> expectedRange = new Range<float>(-1, 3);
            Assert.AreEqual(expectedRange, _rangeFinder.FindFireAngleRange(onTargetRange, accuracy));
        }

        [Test]
        public void FindFireAngleRange_AccuracyOf25()
        {
            IRange<float> onTargetRange = new Range<float>(0, 2);
            float accuracy = 0.25f;
            IRange<float> expectedRange = new Range<float>(-3, 5);
            Assert.AreEqual(expectedRange, _rangeFinder.FindFireAngleRange(onTargetRange, accuracy));
        }
    }
}
