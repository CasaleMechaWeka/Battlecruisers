using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace BattleCruisers.UI.ScreensScene
{
    public class CaptainSelectionItemController : MonoBehaviour
    {
        public Image _captainImage;
        public CanvasGroupButton clickingArea;
        public GameObject _clickedFeedback;
        public TextMeshProUGUI CaptainName;
        private CaptainData _captainData;
        private ISingleSoundPlayer _soundPlayer;
        private Sprite _captainSprite;
        private CaptainSelectorPanel _captainSelectorPanel;
        public int _index;
        public void StaticInitialise(
            ISingleSoundPlayer soundPlayer,
            /*PrefabFactory prefabFactory,*/
            Sprite spriteCaptain,
            CaptainData captainData,
            CaptainSelectorPanel captainSelectorPanel,
            int index
            )
        {
            Helper.AssertIsNotNull(soundPlayer, /*prefabFactory, */captainData, _captainImage, clickingArea, _clickedFeedback, captainSelectorPanel);
            _captainData = captainData;
            _soundPlayer = soundPlayer;
            _captainSprite = spriteCaptain;
            _index = index;
            _captainSelectorPanel = captainSelectorPanel;
            _captainImage.sprite = _captainSprite;
            _clickedFeedback.SetActive(false);
            clickingArea.Initialise(_soundPlayer, OnClicked);
        }

        private void OnClicked()
        {
            _clickedFeedback.SetActive(true);
            _captainSelectorPanel.visualOfCaptains[_index].SetActive(true);
            _captainSelectorPanel.captainDataChanged.Invoke(this, new CaptainDataEventArgs
            {
                captainData = _captainData,
                captainImage = _captainSprite
            });
        }

    }
}

