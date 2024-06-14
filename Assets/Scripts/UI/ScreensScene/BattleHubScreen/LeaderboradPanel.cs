using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
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
            int placenumber,
            string captain)
        {
            /*            CaptainNameToKey nameToKey = new CaptainNameToKey(StaticPrefabKeys.CaptainExos.AllKeys, prefabFactory);
                        IPrefabKey key = nameToKey.GetKey(captain);*/
            string str_index = captain.ToUpper().Replace("CAPTAINEXO", "");
            int index = 10;
            int.TryParse(str_index, out index);
            CaptainExo captainexo = prefabFactory.GetCaptainExo(StaticPrefabKeys.CaptainExos.GetCaptainExoKey(index));
            Text name = PlayerName.gameObject.GetComponent<Text>();
            name.text = playerName;
            Text eolString = EOL.GetComponent<Text>();
            eolString.text = Mathf.Floor((float)eol).ToString();
            Text number = PlaceNumber.GetComponent<Text>();
            number.text = (placenumber + 1).ToString();
            Image image = Captain.GetComponent<Image>();
            image.sprite = captainexo.captainExoImage;
        }
    }
}
