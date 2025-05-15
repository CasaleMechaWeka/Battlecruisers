using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene
{
    public class BodykitItemController : MonoBehaviour
    {
        public Image _bodyKitImage;
        public CanvasGroupButton clickingArea;
        public GameObject _ownedItemMark;
        public GameObject _lockedItemMark;
        public GameObject _clickedFeedback;
        private BodykitData _bodykitData;
        private SingleSoundPlayer _soundPlayer;
        private BodykitsContainer _bodykitContainer;
        private Sprite _bodykitSprite;
        public int _index;
        private bool isCruiserOwned = false;
        private bool _isOwned;

        public void StaticInitialise(
            SingleSoundPlayer soundPlayer,
            Sprite spriteBodykit,
            BodykitData bodykitData,
            BodykitsContainer bodykitContainer,
            int index,
            bool isOwned)
        {
            Helper.AssertIsNotNull(soundPlayer, bodykitData, _bodyKitImage, clickingArea, _ownedItemMark, _clickedFeedback, bodykitContainer);
            _bodykitData = bodykitData;
            _soundPlayer = soundPlayer;
            _bodykitContainer = bodykitContainer;
            _bodykitSprite = spriteBodykit;
            _index = index;
            _isOwned = isOwned;

            _bodyKitImage.sprite = _bodykitSprite;
            _clickedFeedback.SetActive(false);

            _ownedItemMark.SetActive(_isOwned);

            IHullNameToKey hullNameToKey = new HullNameToKey(StaticData.HullKeys);

            isCruiserOwned = DataProvider.GameModel.UnlockedHulls.Contains(hullNameToKey.GetKeyFromHullType(
                PrefabFactory.GetBodykit(StaticPrefabKeys.BodyKits.GetBodykitKey(index)).cruiserType.ToString()));

            _lockedItemMark.SetActive(!isCruiserOwned);
            clickingArea.Initialise(_soundPlayer, OnClicked);
        }

        public void OnClicked()
        {
            _bodykitContainer.onBodykitItemClick.Invoke(this, new BodykitDataEventArgs { });
            if (!_clickedFeedback.activeSelf)
            {
                _clickedFeedback.SetActive(true);
                _bodykitContainer.bodykitDataChanged.Invoke(this, new BodykitDataEventArgs
                {
                    bodykitData = _bodykitData,
                    bodykitImage = _bodykitSprite,
                    purchasable = isCruiserOwned && !_isOwned
                });
            }
        }
    }
}
