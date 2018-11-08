using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Cruisers
{
    // FELIX  Test :)
    public class TwoDigitDisplay : INumberDisplay
    {
        private readonly INumberDisplay _firstDigit, _secondDigit;

        private const int MIN_VALUE = 0;
        private const int MAX_VALUE = 99;

        public int Num
        {
            set
            {
                Assert.IsTrue(value >= MIN_VALUE);
                Assert.IsTrue(value <= MAX_VALUE);

                _firstDigit.Num = value / 10;
                _secondDigit.Num = value % 10;
            }
        }

        public TwoDigitDisplay(INumberDisplay firstDigit, INumberDisplay secondDigit)
        {
            Helper.AssertIsNotNull(firstDigit, secondDigit);

            _firstDigit = firstDigit;
            _secondDigit = secondDigit;
        }
    }
}