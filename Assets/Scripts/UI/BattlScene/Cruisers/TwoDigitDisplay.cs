using System;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

                // FELIX  NEXT :D
                //int firstDigit = 
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