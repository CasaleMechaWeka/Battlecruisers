namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class DismissSelectorPanelButtonController : DismissPanelButtonController
    {
        protected override void OnClicked()
        {
            _uiManager.HideCurrentlyShownMenu();
        }
    }
}
