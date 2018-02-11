using BattleCruisers.Utils.Categorisation;
using NUnit.Framework;

namespace BattleCruisers.Tests.Utils.Categorisation
{
    public class DummyValuetoStarsConverter : ValueToStarsConverter
    {
        private static readonly float[] CATEGORY_THRESHOLDS = 
        { 
            2, // 2 - 3.99 = 1 star
            4, // 4 - 5.99 = 2 stars
            6, // 6 - 7.99 = 3 stars
            8, // 8 - 9.99 = 4 stars
            10 // 10+      = 5 stars
        };

        public DummyValuetoStarsConverter() 
            : base(CATEGORY_THRESHOLDS)
        {
        }
    }

    public class ValueToStarsConverterTests
    {
        private IValueToStarsConverter _converter;

        [SetUp]
        public void SetuUp()
        {
            _converter = new DummyValuetoStarsConverter();
        }

        [Test]
        public void BelowFirstThreshold_0Stars()
        {
            Assert.AreEqual(0, _converter.ConvertValueToStars(1.99f));
        }

        [Test]
        public void EqualToFirstThreshold_1Stars()
        {
            Assert.AreEqual(1, _converter.ConvertValueToStars(2));
        }

        [Test]
        public void GreatThanFirst_LessThanSecond_1Stars()
        {
            Assert.AreEqual(1, _converter.ConvertValueToStars(3));
        }

        [Test]
        public void EqualToSecondThreshold_2Stars()
        {
            Assert.AreEqual(2, _converter.ConvertValueToStars(4));
        }

        [Test]
        public void EqualToLastThreshold_5Stars()
        {
            Assert.AreEqual(5, _converter.ConvertValueToStars(10));
        }

        [Test]
        public void GreaterThanLastThreshold_5Stars()
        {
            Assert.AreEqual(5, _converter.ConvertValueToStars(11));
        }
    }
}
