using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Cruisers
{
    public class NumOfDronesPanelInitialiser : MonoBehaviour
    {
        public INumberDisplay CreateTwoDigitDisplay()
        {
            SingleDigitDisplayController firstDigit = transform.FindNamedComponent<SingleDigitDisplayController>("FirstDigit");
            SingleDigitDisplayController secondDigit = transform.FindNamedComponent<SingleDigitDisplayController>("SecondDigit");
            return new TwoDigitDisplay(firstDigit, secondDigit);
        }
    }
}