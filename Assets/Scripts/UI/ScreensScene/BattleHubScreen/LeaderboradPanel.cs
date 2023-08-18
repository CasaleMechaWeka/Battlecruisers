using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Fetchers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    public class LeaderboradPanel : MonoBehaviour
    {
        public GameObject PlayerName;
        public GameObject EOL;
        public GameObject Captain;
        public GameObject PlaceNumber;

        public void Initialise(ISingleSoundPlayer soundPlayer,
            IPrefabFactory prefabFactory,
            string playerName,
            double eol,
            int placenumber)
        {
            Text name = PlayerName.gameObject.GetComponent<Text>();
            name.text = playerName;
            Text eolString = EOL.GetComponent<Text>();
            eolString.text = eol.ToString();
            Text number = PlaceNumber.GetComponent<Text>();
            number.text = (placenumber + 1).ToString();
        }
    }

}
