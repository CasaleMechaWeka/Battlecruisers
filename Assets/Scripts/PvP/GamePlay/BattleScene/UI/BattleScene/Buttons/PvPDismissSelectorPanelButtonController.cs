namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons
{
    public class PvPDismissSelectorPanelButtonController : PvPDismissPanelButtonController
    {
        protected override void OnClicked()
        {
            base.OnClicked();
            _uiManager.HideCurrentlyShownMenu();
        }
    }
}
