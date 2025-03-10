using BattleCruisers.UI.BattleScene.Cruisers;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Cruisers
{
    public class PvPNumOfDronesPanelInitialiser : MonoBehaviour
    {
        public INumberDisplay CreateTwoDigitDisplay()
        {
            PvPSingleDigitDisplayController firstDigit = transform.FindNamedComponent<PvPSingleDigitDisplayController>("FirstDigit");
            firstDigit.Initialise();

            PvPSingleDigitDisplayController secondDigit = transform.FindNamedComponent<PvPSingleDigitDisplayController>("SecondDigit");
            secondDigit.Initialise();

            return new PvPTwoDigitDisplay(firstDigit, secondDigit);
        }
    }
}