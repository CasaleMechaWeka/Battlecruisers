namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class DismissInformatorButtonController : DismissPanelButtonController
    {
        protected override void OnClicked()
        {
            base.OnClicked();
            _uiManager.HideItemDetails();
        }
    }
}
