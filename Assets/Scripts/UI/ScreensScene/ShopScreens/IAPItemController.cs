using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;
using UnityEngine.UI;


namespace BattleCruisers.UI.ScreensScene
{
    public class IAPItemController : MonoBehaviour
    {
        public Image _iapImage;
        public CanvasGroupButton clickingArea;
        public GameObject _clickedFeedback;
        public IIAPData _iapData;
        private ISingleSoundPlayer _soundPlayer;
        private BlackMarketScreenController _blackMarketScreenController;

        public async void StaticInitialise(
            ISingleSoundPlayer soundPlayer,
            IIAPData iapData,
            BlackMarketScreenController blackMarketScreenController
            )
        {
            Helper.AssertIsNotNull(soundPlayer, iapData, _iapImage, clickingArea, _clickedFeedback, blackMarketScreenController);
            _soundPlayer = soundPlayer;
            _iapData = iapData;
            _blackMarketScreenController = blackMarketScreenController;

            //    _iapImage.sprite = iapIcon;

            SpriteFetcher spriteFetcher = new SpriteFetcher();
            ISpriteWrapper spWrapper = await spriteFetcher.GetSpriteAsync("Assets/Resources_moved/Sprites/UI/IAP/" + _iapData.IAPIconName + ".png");
            _iapImage.sprite = spWrapper.Sprite;
           // _clickedFeedback.SetActive(false);

            clickingArea.Initialise(_soundPlayer, OnClicked);
        }

        public void OnClicked()
        {
            _clickedFeedback.SetActive(true);

            _blackMarketScreenController.iapDataChanged.Invoke(this, new IAPDataEventArgs
            {
                iapData = _iapData
            });
        }
    }
}
