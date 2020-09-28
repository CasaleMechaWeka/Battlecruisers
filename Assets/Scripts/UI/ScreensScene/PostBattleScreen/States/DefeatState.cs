using BattleCruisers.UI.Music;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen.States
{
    public class DefeatState
    {
		private const string LOSS_TITLE = "Bad luck!";

        public void Initialise(
            PostBattleScreenController postBattleScreen,
            IMusicPlayer musicPlayer)
        {
            Helper.AssertIsNotNull(postBattleScreen, musicPlayer);

            postBattleScreen.title.text = LOSS_TITLE;
            postBattleScreen.defeatMessage.SetActive(true);
            musicPlayer.PlayDefeatMusic();
        }
    }
}