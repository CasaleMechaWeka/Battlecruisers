using BattleCruisers.UI.Common.Click;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Common.Click
{
    public class ClickHandlerTests
    {
        private ClickHandler _clickHandler;
        private float _doubleClickThresholdInS;
        private int _singleClickCount, _doubleClickCount;

        [SetUp]
        public void TestSetup()
        {
            _doubleClickThresholdInS = 0.5f;
            _clickHandler = new ClickHandler(_doubleClickThresholdInS);

            _singleClickCount = 0;
            _doubleClickCount = 0;

            _clickHandler.SingleClick += (sender, e) => _singleClickCount++;
            _clickHandler.DoubleClick += (sender, e) => _doubleClickCount++;
        }

        [Test]
        public void FirstClick_EmitsSingleClickEvent()
        {
            _clickHandler.OnClick(timeSinceGameStartInS: 0.1f);
            ExpectClickCounts(expectedSingleClickCount: 1, expectedDoubleClickCount: 0);
        }

        [Test]
        public void SecondClick_WithinThreshold_EmitsDoubleClickEvent()
        {
            _clickHandler.OnClick(timeSinceGameStartInS: 0.1f);
            ExpectClickCounts(expectedSingleClickCount: 1, expectedDoubleClickCount: 0);

            _clickHandler.OnClick(timeSinceGameStartInS: 0.2f);
            ExpectClickCounts(expectedSingleClickCount: 1, expectedDoubleClickCount: 1);
        }

        [Test]
        public void SecondClick_OutsideOfThreshold_EmitsSingleClickEvent()
        {
            _clickHandler.OnClick(timeSinceGameStartInS: 0.1f);
            ExpectClickCounts(expectedSingleClickCount: 1, expectedDoubleClickCount: 0);

            _clickHandler.OnClick(timeSinceGameStartInS: 0.7f);
            ExpectClickCounts(expectedSingleClickCount: 2, expectedDoubleClickCount: 0);
        }

        [Test]
        public void ThirdClick_WithinThreashold_EmitsSingleClickEvent()
        {
            _clickHandler.OnClick(timeSinceGameStartInS: 0.1f);
            ExpectClickCounts(expectedSingleClickCount: 1, expectedDoubleClickCount: 0);

            _clickHandler.OnClick(timeSinceGameStartInS: 0.2f);
            ExpectClickCounts(expectedSingleClickCount: 1, expectedDoubleClickCount: 1);

            _clickHandler.OnClick(timeSinceGameStartInS: 0.3f);
            ExpectClickCounts(expectedSingleClickCount: 2, expectedDoubleClickCount: 1);
        }

        private void ExpectClickCounts(int expectedSingleClickCount, int expectedDoubleClickCount)
        {
            Assert.AreEqual(expectedSingleClickCount, _singleClickCount);
            Assert.AreEqual(expectedDoubleClickCount, _doubleClickCount);
        }
    }
}