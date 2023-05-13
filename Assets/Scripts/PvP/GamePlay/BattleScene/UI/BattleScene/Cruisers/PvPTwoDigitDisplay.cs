using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Cruisers
{
    public class PvPTwoDigitDisplay : IPvPNumberDisplay
    {
        private readonly IPvPNumberDisplay _firstDigit, _secondDigit;

        private const int MIN_VALUE = 0;
        private const int MAX_VALUE = 99;

        public int Num
        {
            set
            {
                Assert.IsTrue(value >= MIN_VALUE);

                if (value > MAX_VALUE)
                {
                    _firstDigit.Num = 9;
                    _secondDigit.Num = 9;
                }
                else
                {
                    _firstDigit.Num = value / 10;
                    _secondDigit.Num = value % 10;
                }
            }
        }

        public PvPTwoDigitDisplay(IPvPNumberDisplay firstDigit, IPvPNumberDisplay secondDigit)
        {
            PvPHelper.AssertIsNotNull(firstDigit, secondDigit);

            _firstDigit = firstDigit;
            _secondDigit = secondDigit;
        }
    }
}