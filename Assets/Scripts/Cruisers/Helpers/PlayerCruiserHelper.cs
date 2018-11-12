using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras;

namespace BattleCruisers.Cruisers.Helpers
{
    public class PlayerCruiserHelper : CruiserHelper
    {
        public PlayerCruiserHelper(IUIManager uIManager, ICameraController cameraController)
            : base(uIManager, cameraController)
        {
        }

        public override void FocusCameraOnCruiser()
        {
            _cameraController.FocusOnPlayerCruiser();
        }

        public override void ShowBuildingGroupButtons()
        {
            _uiManager.HideCurrentlyShownMenu();
        }
    }
}
