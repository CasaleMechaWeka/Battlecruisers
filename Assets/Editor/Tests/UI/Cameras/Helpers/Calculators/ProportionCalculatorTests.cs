using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Utils.DataStrctures;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Cameras.Helpers.Calculators
{
    public class ProportionCalculatorTests
    {
        private IProportionCalculator _calculator;

        [SetUp]
        public void TestSetup()
        {
            _calculator = new ProportionCalculator();
        }

        [Test]
        public void FindProportion_TooSmall_Clamped()
        {
            float value = 7;
            IRange<float> valueRange = new Range<float>(8, 10);

            Assert.AreEqual(0, _calculator.FindProportion(value, valueRange));
        }

        [Test]
        public void FindProportion_TooBig_Clamped()
        {
            float value = 11;
            IRange<float> valueRange = new Range<float>(8, 10);

            Assert.AreEqual(1, _calculator.FindProportion(value, valueRange));
        }

        [Test]
        public void FindProportion_Valid_Clamped()
        {
            float value = 9;
            IRange<float> valueRange = new Range<float>(8, 10);

            Assert.AreEqual(0.5f, _calculator.FindProportion(value, valueRange));
        }

        [Test]
        public void FindProportion_0Range()
        {
            float value = 9;
            IRange<float> valueRange = new Range<float>(0, 0);

            Assert.AreEqual(0, _calculator.FindProportion(value, valueRange));
        }

        [Test]
        public void FindProportionalValue()
        {
            float proportion = 0.5f;
            IRange<float> valueRange = new Range<float>(4, 10);

            Assert.AreEqual(7, _calculator.FindProportionalValue(proportion, valueRange));
        }
    }
}