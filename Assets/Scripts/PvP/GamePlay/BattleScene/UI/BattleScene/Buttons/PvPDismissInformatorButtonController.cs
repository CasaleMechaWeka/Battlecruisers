namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons
{
    public class PvPDismissInformatorButtonController : PvPDismissPanelButtonController
    {
        protected override void OnClicked()
        {
            base.OnClicked();
            _uiManager.HideItemDetails();
        }
    }
}
