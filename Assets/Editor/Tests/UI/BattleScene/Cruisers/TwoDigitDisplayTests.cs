using BattleCruisers.UI.BattleScene.Cruisers;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.BattleScene.Cruisers
{
    public class TwoDigitDisplayTests
    {
        private INumberDisplay _twoDigitDisplay, _firstDigitDisplay, _secondDigitDisplay;

        [SetUp]
        public void TestSetup()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _firstDigitDisplay = Substitute.For<INumberDisplay>();
            _secondDigitDisplay = Substitute.For<INumberDisplay>();

            _twoDigitDisplay = new TwoDigitDisplay(_firstDigitDisplay, _secondDigitDisplay);
        }

        [Test]
        public void SetNum_TooSmallValue_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _twoDigitDisplay.Num = -1);
        }

        [Test]
        public void SetNum_TooBigValue()
        {
            _twoDigitDisplay.Num = 100;
            _firstDigitDisplay.Received().Num = 9;
            _secondDigitDisplay.Received().Num = 9;
        }

        [Test]
        public void SetNum_ValidValue_SingleDigit_UpdatesDigitDisplays()
        {
            _twoDigitDisplay.Num = 6;

            _firstDigitDisplay.Received().Num = 0;
            _secondDigitDisplay.Received().Num = 6;
        }

        [Test]
        public void SetNum_ValidValue_TwoDigits_UpdatesDigitDisplays()
        {
            _twoDigitDisplay.Num = 16;

            _firstDigitDisplay.Received().Num = 1;
            _secondDigitDisplay.Received().Num = 6;
        }
    }
}