using BattleCruisers.Data;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound.Players;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen.States
{
    public class TutorialCompletedState : PostBattleState
    {
        private const string TUTORIAL_TITLE = "Tutorial Completed :D";
        private const string TUTORIAL_APPRAISAL_DRONE_TEXT = "You are now a qualified Battlecruiser Captain, certified by the UAC. On behalf of us crew of builder drones, I’d like to thank you for choosing to steal a Trident-class Battlecruiser! We hope your joyride is comfortable.";

        public TutorialCompletedState(
            PostBattleScreenController postBattleScreen,
            IApplicationModel appModel,
            IMusicPlayer musicPlayer,
            ISingleSoundPlayer soundPlayer)
            : base(postBattleScreen, appModel, musicPlayer)
        {
            Assert.IsNotNull(soundPlayer);

            appModel.Mode = GameMode.Campaign;
            postBattleScreen.title.text = TUTORIAL_TITLE;

            postBattleScreen.title.color = Color.black;
            postBattleScreen.levelName.levelName.color = Color.black;

            postBattleScreen.appraisalSection.Initialise(TUTORIAL_APPRAISAL_DRONE_TEXT, soundPlayer);
            musicPlayer.PlayVictoryMusic();
            postBattleScreen.levelName.gameObject.SetActive(false);

            postBattleScreen.postTutorialButtonsPanel.Initialise(postBattleScreen, soundPlayer, appModel.DataProvider.GameModel);
        }
    }
}