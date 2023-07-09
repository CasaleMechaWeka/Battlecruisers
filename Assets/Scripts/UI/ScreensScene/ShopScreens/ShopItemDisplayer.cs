using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Data.Static;
using UnityEngine;
using UnityEngine.UI;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils.Factories;
using BattleCruisers.UI.Sound.Players;

namespace BattleCruisers.UI.ScreensScene
{
    public class ShopItemDisplayer : MonoBehaviour
    {
        public GameObject _name;
        public GameObject _image;
        public GameObject _description;
        public GameObject _buyButton;
        public GameObject _price;

        public void initialise(ISingleSoundPlayer soundPlayer)
        {
            
        }

        public void DisplayItem(CaptainExoData captainExo)
        {
            if(this.gameObject.activeSelf == false)
            {
                this.gameObject.SetActive(true);
            }
            Image image = _image.GetComponent<Image>();
            Text name = _name.GetComponent<Text>();
            Text description = _description.GetComponent<Text>();
            Text costText = _price.GetComponent<Text>();
            image.sprite = captainExo.CaptainExoImage;
            name.text = captainExo.CaptainExoName;
            costText.text = (captainExo.CaptainExoCost).ToString();

            OwnedFeedback feedback = _buyButton.GetComponentInChildren<OwnedFeedback>();
            //Debug.Log(feedback);
            if(feedback != null)
            {
                feedback.gameObject.SetActive(false);
                if (captainExo.IsOwned == true)
                {
                    feedback.gameObject.SetActive(true);
                }
            }
            
        }
    }
}

