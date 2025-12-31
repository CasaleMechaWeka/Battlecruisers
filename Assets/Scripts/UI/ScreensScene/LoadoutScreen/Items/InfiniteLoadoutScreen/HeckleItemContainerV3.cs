using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene
{
    public class HeckleItemContainerV3 : MonoBehaviour
    {
        public Text HeckleText;
        public CanvasGroupButton ClickingArea;
        public GameObject ClickedFeedback;
        private SingleSoundPlayer SoundPlayer;
        private ProfileDetailsController _profileDetails;
        public int Index;

        public void StaticInitialise(
            SingleSoundPlayer soundPlayer,
            ProfileDetailsController profileDetails,
            int index)
        {
            Helper.AssertIsNotNull(soundPlayer, ClickingArea, ClickedFeedback, profileDetails);
            SoundPlayer = soundPlayer;
            Index = index;
            HeckleText.text = index.ToString("000");
            _profileDetails = profileDetails;
            ClickedFeedback.SetActive(false);
            ClickingArea.Initialise(SoundPlayer, OnClicked);
        }

        private void OnClicked()
        {
            ClickedFeedback.SetActive(true);
            _profileDetails.ShowHeckle(Index);
        }
    }
}
