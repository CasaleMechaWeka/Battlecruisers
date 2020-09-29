using BattleCruisers.Data;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen.States
{
    public class TutorialCompletedState
    {
		private const string TUTORIAL_TITLE = "Tutorial Completed :D";
		private const string TUTORIAL_APPRAISAL_DRONE_TEXT = "Your 10 minute tutorial simulation is complete! You are now a qualified Battlecruiser Captain, certified by the UAC.";

        public void Initialise(
            PostBattleScreenController postBattleScreen,
            IApplicationModel appModel,
            ISingleSoundPlayer soundPlayer,
            IMusicPlayer musicPlayer)
        {
            Helper.AssertIsNotNull(postBattleScreen, appModel, soundPlayer, musicPlayer);

            appModel.IsTutorial = false;
            postBattleScreen.title.text = TUTORIAL_TITLE;

            postBattleScreen.title.color = Color.black;
            postBattleScreen.levelName.levelName.color = Color.black;

            postBattleScreen.appraisalSection.Initialise(TUTORIAL_APPRAISAL_DRONE_TEXT, soundPlayer);
            musicPlayer.PlayVictoryMusic();
            postBattleScreen.levelName.gameObject.SetActive(false);

            postBattleScreen.postBattleButtonsPanel.gameObject.SetActive(false);
            postBattleScreen.postTutorialButtonsPanel.Initialise(postBattleScreen, soundPlayer);
            postBattleScreen.postTutorialButtonsPanel.gameObject.SetActive(true);
        }
    }
}