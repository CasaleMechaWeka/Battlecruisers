using BattleCruisers.Data;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen.States
{
    public class TutorialCompletedState
    {
		private const string TUTORIAL_TITLE = "Tutorial Completed :D";

        public void Initialise(
            PostBattleScreenController postBattleScreen,
            IApplicationModel appModel,
            ISingleSoundPlayer soundPlayer,
            IMusicPlayer musicPlayer)
        {
            Helper.AssertIsNotNull(postBattleScreen, appModel, soundPlayer, musicPlayer);

            appModel.IsTutorial = false;
            postBattleScreen.title.text = TUTORIAL_TITLE;
            postBattleScreen.postTutorialMessage.SetActive(true);
            musicPlayer.PlayVictoryMusic();

            postBattleScreen.postTutorialButtonsPanel.Initialise(postBattleScreen, soundPlayer);
            postBattleScreen.postTutorialButtonsPanel.gameObject.SetActive(true);
        }
    }
}