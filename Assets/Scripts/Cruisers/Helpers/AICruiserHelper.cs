using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras;

namespace BattleCruisers.Cruisers.Helpers
{
    public class AICruiserHelper : CruiserHelper
    {
        public AICruiserHelper(IUIManager uIManager, ICameraController cameraController) 
            : base(uIManager, cameraController)
        {
        }

        public override void FocusCameraOnCruiser()
        {
            _cameraController.FocusOnAiCruiser();
        }

        public override void ShowBuildingGroupButtons()
        {
            // Do nothing.  Buttons should only be shown for player cruiser :)
        }
    }
}
