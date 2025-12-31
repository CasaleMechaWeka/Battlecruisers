using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Navigation;

namespace BattleCruisers.Cruisers.Helpers
{
    public class AICruiserHelper : CruiserHelper
    {
        public AICruiserHelper(UIManager uIManager, ICameraFocuser cameraFocuser)
            : base(uIManager, cameraFocuser)
        {
        }

        public override void FocusCameraOnCruiser()
        {
            _cameraFocuser.FocusOnRightCruiser();
        }

        public override void OnBuildingConstructionStarted(IBuilding buildingStarted, SlotAccessor slotAccessor, SlotHighlighter slotHighlighter)
        {
            // Do nothing.  Buttons should only be shown for player cruiser :)
        }
    }
}
