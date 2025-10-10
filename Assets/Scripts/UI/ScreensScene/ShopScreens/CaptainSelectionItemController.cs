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
        public GameObject ClickedFeedback;
        public TextMeshProUGUI CaptainName;
        private SingleSoundPlayer _soundPlayer;
        private Sprite _captainSprite;
        private ProfileDetailsController _profileDetails;
        public int _index;

        public void StaticInitialise(
            SingleSoundPlayer soundPlayer,
            Sprite spriteCaptain,
            ProfileDetailsController profileDetails,
            int index
            )
        {
            Helper.AssertIsNotNull(soundPlayer, _captainImage, clickingArea, ClickedFeedback, profileDetails);
            _soundPlayer = soundPlayer;
            _captainSprite = spriteCaptain;
            _index = index;
            _profileDetails = profileDetails;
            _captainImage.sprite = _captainSprite;
            ClickedFeedback.SetActive(false);
            clickingArea.Initialise(_soundPlayer, OnClicked);
        }

        private void OnClicked()
        {
            ClickedFeedback.SetActive(true);
            _profileDetails.ShowCaptain(_index);
        }
    }
}
