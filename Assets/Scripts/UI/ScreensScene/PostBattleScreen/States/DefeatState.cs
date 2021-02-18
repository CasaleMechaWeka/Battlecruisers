using BattleCruisers.UI.Music;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen.States
{
    public class DefeatState : IPostBattleState
    {
		public const string LOSS_TITLE = "Bad luck!";

        public DefeatState(
            PostBattleScreenController postBattleScreen,
            IMusicPlayer musicPlayer)
        {
            Helper.AssertIsNotNull(postBattleScreen, musicPlayer);

            postBattleScreen.title.text = LOSS_TITLE;
            postBattleScreen.defeatMessage.SetActive(true);
            musicPlayer.PlayDefeatMusic();
            postBattleScreen.postBattleButtonsPanel.gameObject.SetActive(true);
        }

        public bool ShowVictoryBackground()
        {
            return false;
        }
    }
}