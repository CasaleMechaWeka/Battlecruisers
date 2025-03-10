using BattleCruisers.Data;
using BattleCruisers.UI.Music;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen.States
{
    public class DefeatState : PostBattleState
    {
		public const string LOSS_TITLE_KEY = "UI/PostBattleScreen/Title/Defeat";

        public DefeatState(
            PostBattleScreenController postBattleScreen,
            IApplicationModel appModel,
            IMusicPlayer musicPlayer,
            ILocTable screensSceneStrings)
            : base (postBattleScreen, appModel, musicPlayer, screensSceneStrings)
        {
            Helper.AssertIsNotNull(postBattleScreen, musicPlayer);

            postBattleScreen.title.text = _screensSceneStrings.GetString(LOSS_TITLE_KEY);
            postBattleScreen.defeatMessage.SetActive(true);
            musicPlayer.PlayDefeatMusic();
            if(appModel.Mode == GameMode.CoinBattle)
            {
                postBattleScreen.postSkirmishButtonsPanel.gameObject.SetActive(true);
            }
            else
            {
                postBattleScreen.postBattleButtonsPanel.gameObject.SetActive(true);
            }
            
            //Reset gamemode to Campaign
            appModel.Mode = GameMode.Campaign;
        }

        public override bool ShowVictoryBackground => false;
    }
}