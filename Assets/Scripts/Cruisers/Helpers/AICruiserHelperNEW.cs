using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Navigation;

namespace BattleCruisers.Cruisers.Helpers
{
    public class AICruiserHelperNEW : CruiserHelperNEW
    {
        public AICruiserHelperNEW(IUIManager uIManager, ICameraFocuser cameraFocuser) 
            : base(uIManager, cameraFocuser)
        {
        }

        public override void FocusCameraOnCruiser()
        {
            _cameraFocuser.FocusOnAICruiser();
        }

        public override void ShowBuildingGroupButtons()
        {
            // Do nothing.  Buttons should only be shown for player cruiser :)
        }
    }
}
