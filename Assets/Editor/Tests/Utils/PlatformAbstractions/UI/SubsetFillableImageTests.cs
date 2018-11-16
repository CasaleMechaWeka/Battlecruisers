using BattleCruisers.Utils.PlatformAbstractions.UI;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Utils.PlatformAbstractions.UI
{
    public class SubsetFillableImageTests
    {
        private IFillableImage _subsetImage, _coreImage;
        private IFillCalculator _fillCalculator;

        [SetUp]
        public void TestSetup()
        {
            _coreImage = Substitute.For<IFillableImage>();
            _fillCalculator = Substitute.For<IFillCalculator>();
            _subsetImage = new SubsetFillableImage(_coreImage, _fillCalculator);

            _coreImage.FillAmount = 0.17f;
        }

        [Test]
        public void FillAmount_Get()
        {
            float calculatedRawFillAmount = 0.71f;
            _fillCalculator.AdjustedToRaw(_coreImage.FillAmount).Returns(calculatedRawFillAmount);

            float rawFillAmount = _subsetImage.FillAmount;

            Assert.AreEqual(rawFillAmount, calculatedRawFillAmount);
        }

        [Test]
        public void FillAmount_Set()
        {
            float newFillAmount = 0.88f;
            float calculatedAdjustedFillAmount = 0.95f;
            _fillCalculator.RawToAdjusted(newFillAmount).Returns(calculatedAdjustedFillAmount);

            _subsetImage.FillAmount = newFillAmount;

            _coreImage.Received().FillAmount = calculatedAdjustedFillAmount;
        }

        [Test]
        public void IsVisible_Get()
        {
            _coreImage.IsVisible = true;
            Assert.AreEqual(_subsetImage.IsVisible, _coreImage.IsVisible);
        }

        [Test]
        public void IsVisible_Set()
        {
            _coreImage.IsVisible = true;
            _subsetImage.IsVisible = false;
            Assert.IsFalse(_coreImage.IsVisible);
        }
    }
}