using BattleCruisers.Data;
using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Sound.Players;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class DecreaseDifficultySuggestionController : MonoBehaviour
    {
        public CanvasGroupButton yesButton, noButton;

        private PostBattleScreenController postBattleScreen;

        public void Initialise(PostBattleScreenController screenController, SingleSoundPlayer soundPlayer)
        {
            postBattleScreen = screenController;
            Assert.IsNotNull(postBattleScreen, "PostBattleScreen is not assigned.");

            yesButton.Initialise(soundPlayer, OnYesClicked);
            noButton.Initialise(soundPlayer, OnNoClicked);
        }

        private void OnYesClicked()
        {
            DataProvider.SettingsManager.AIDifficulty = Difficulty.Normal;
            postBattleScreen.GoToHomeScreen();
        }

        private void OnNoClicked()
        {
            postBattleScreen.GoToHomeScreen();
        }
    }
}
