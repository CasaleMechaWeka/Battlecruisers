using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
using BattleCruisers.Data;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.UI.Music;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen.States
{
    public class TutorialCompletedState : MonoBehaviour
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

            PostTutorialButtonsPanel postTutorialButtonsPanel = GetComponentInChildren<PostTutorialButtonsPanel>(includeInactive: true);
            Assert.IsNotNull(postTutorialButtonsPanel);
            postTutorialButtonsPanel.Initialise(postBattleScreen, soundPlayer);
            postTutorialButtonsPanel.gameObject.SetActive(true);
        }
    }
}