using BattleCruisers.Data;
using BattleCruisers.UI.Music;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen.States
{
    public class DefeatState : PostBattleState
    {
		public const string LOSS_TITLE = "Bad luck!";

        public DefeatState(
            PostBattleScreenController postBattleScreen,
            IApplicationModel appModel,
            IMusicPlayer musicPlayer)
            : base (postBattleScreen, appModel, musicPlayer)
        {
            Helper.AssertIsNotNull(postBattleScreen, musicPlayer);

            postBattleScreen.title.text = LOSS_TITLE;
            postBattleScreen.defeatMessage.SetActive(true);
            musicPlayer.PlayDefeatMusic();
            postBattleScreen.postBattleButtonsPanel.gameObject.SetActive(true);
        }

        public override bool ShowVictoryBackground => false;
    }
}