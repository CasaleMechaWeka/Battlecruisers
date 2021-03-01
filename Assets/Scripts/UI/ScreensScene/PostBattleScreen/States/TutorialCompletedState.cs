using BattleCruisers.Data;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen.States
{
    public class TutorialCompletedState : PostBattleState
    {
        private const string TUTORIAL_TITLE_KEY = "UI/PostBattleScreen/Title/Tutorial";
        private const string TUTORIAL_APPRAISAL_DRONE_TEXT = "UI/PostBattleScreen/TutorialDroneText";

        public TutorialCompletedState(
            PostBattleScreenController postBattleScreen,
            IApplicationModel appModel,
            IMusicPlayer musicPlayer,
            ILocTable screensSceneStrings,
            ISingleSoundPlayer soundPlayer)
            : base(postBattleScreen, appModel, musicPlayer, screensSceneStrings)
        {
            Assert.IsNotNull(soundPlayer);

            appModel.Mode = GameMode.Campaign;
            postBattleScreen.title.text = _screensSceneStrings.GetString(TUTORIAL_TITLE_KEY);

            postBattleScreen.title.color = Color.black;
            postBattleScreen.levelName.levelName.color = Color.black;

            string droneText = _screensSceneStrings.GetString(TUTORIAL_APPRAISAL_DRONE_TEXT);
            postBattleScreen.appraisalSection.Initialise(droneText, screensSceneStrings, soundPlayer);
            musicPlayer.PlayVictoryMusic();
            postBattleScreen.levelName.gameObject.SetActive(false);

            postBattleScreen.postTutorialButtonsPanel.Initialise(postBattleScreen, soundPlayer, appModel.DataProvider.GameModel);
        }
    }
}