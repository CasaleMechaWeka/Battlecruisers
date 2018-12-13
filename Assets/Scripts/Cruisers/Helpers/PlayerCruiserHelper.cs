using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Navigation;

namespace BattleCruisers.Cruisers.Helpers
{
    public class PlayerCruiserHelper : CruiserHelper
    {
        public PlayerCruiserHelper(IUIManager uIManager, ICameraFocuser cameraFocuser)
            : base(uIManager, cameraFocuser)
        {
        }

        public override void FocusCameraOnCruiser()
        {
            _cameraFocuser.FocusOnPlayerCruiser();
        }

        public override void ShowBuildingGroupButtons()
        {
            _uiManager.HideCurrentlyShownMenu();
        }
    }
}
