using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Fetchers;
using Mono.CompilerServices.SymbolWriter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BattleCruisers.Utils;
using System.Linq;

namespace BattleCruisers.UI.ScreensScene
{
    public class CaptainItemController : MonoBehaviour
    {
        public Image _captainImage;
        public CanvasGroupButton clickingArea;
        public GameObject _ownedItemMark;
        public GameObject _clickedFeedback;
        private ICaptainData _captainData;
        private ISingleSoundPlayer _soundPlayer;
        private CaptainsContainer _captainsContainer;
        private Sprite _captainSprite;
        public int _index;

        public void StaticInitialise(
            ISingleSoundPlayer soundPlayer,
            /*IPrefabFactory prefabFactory,*/
            Sprite spriteCaptain,
            ICaptainData captainData,
            CaptainsContainer captainsContainer,
            int index
            )
        {
            Helper.AssertIsNotNull(soundPlayer, /*prefabFactory, */captainData, _captainImage, clickingArea, _ownedItemMark, _clickedFeedback, captainsContainer);
            _captainData = captainData;
            _soundPlayer = soundPlayer;
            _captainsContainer = captainsContainer;
            _captainSprite = spriteCaptain;
            _index = index;

            _captainImage.sprite = _captainSprite;
            _clickedFeedback.SetActive(false);

            _ownedItemMark.SetActive(_captainData.IsOwned);
            clickingArea.Initialise(_soundPlayer, OnClicked);
        }

        public void OnClicked()
        {
            if (!_clickedFeedback.activeSelf)
            {
                _clickedFeedback.SetActive(true);
                _captainsContainer.visualOfCaptains[_index].SetActive(true);
                _captainsContainer.captainDataChanged.Invoke(this, new CaptainDataEventArgs
                {
                    captainData = _captainData,
                    captainImage = _captainSprite
                });
            }
        }
    }
}
