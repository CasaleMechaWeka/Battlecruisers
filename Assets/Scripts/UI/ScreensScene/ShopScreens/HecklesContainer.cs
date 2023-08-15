using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Localisation;
using DG.Tweening;
using System;
using Unity.Services.Economy;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene
{
    public class HecklesContainer : MonoBehaviour
    {
        private ILocTable commonStrings;
        public EventHandler<HeckleDataEventArgs> heckleDataChanged;

        public Text t_heckleMessage;
        public GameObject obj_heckleMessage;
        public GameObject btnBuy, ownFeedback;
        public HeckleItemController currentItem;
        public Text hecklePrice;
        private ISingleSoundPlayer _soundPlayer;
        private IDataProvider _dataProvider;
        public void Initialize(ISingleSoundPlayer soundPlayer, IDataProvider dataProvider)
        {
            commonStrings = LandingSceneGod.Instance.commonStrings;
            heckleDataChanged += HeckleDataChanged;
            _soundPlayer = soundPlayer;
            _dataProvider = dataProvider;
            btnBuy.GetComponent<CanvasGroupButton>().Initialise(_soundPlayer, Purchase);
        }

        private void Purchase()
        {
            
        }

        private async void HeckleDataChanged(object sender, HeckleDataEventArgs e)
        {
            currentItem._clickedFeedback.SetActive(false);
            currentItem = (HeckleItemController)sender;
            ScreensSceneGod.Instance.characterOfShop.GetComponent<Animator>().SetTrigger("select");
            if (e.heckleData.IsOwned)
            {
                btnBuy.SetActive(false);
                ownFeedback.SetActive(true);
            }
            else
            {
                btnBuy.SetActive(true);
                ownFeedback.SetActive(false);
            }

            t_heckleMessage.text = commonStrings.GetString(e.heckleData.StringKeyBase);
            hecklePrice.text = (await _dataProvider.GetHeckleCost(e.heckleData.Index)).ToString();
            obj_heckleMessage.GetComponent<RectTransform>().localScale = Vector3.zero;
            obj_heckleMessage.GetComponent<RectTransform>().DOScale(Vector3.one, 0.2f);
        }

        private void OnDestroy()
        {
            heckleDataChanged -= HeckleDataChanged;
        }

    }

    public class HeckleDataEventArgs : EventArgs
    {
        public IHeckleData heckleData { get; set; }
    }
}
