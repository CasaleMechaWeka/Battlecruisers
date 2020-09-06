using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Navigation;

namespace BattleCruisers.Cruisers.Helpers
{
    public class TutorialAICruiserHelper : CruiserHelper
    {
        public TutorialAICruiserHelper(IUIManager uIManager, ICameraFocuser cameraFocuser) 
            : base(uIManager, cameraFocuser)
        {
        }

        public override void FocusCameraOnCruiser()
        {
             // Disabled for tutorial
        }

        public override void OnBuildingConstructionStarted(IBuilding buildingStarted, ISlotAccessor slotAccessor, ISlotHighlighter slotHighlighter)
        {
            // Do nothing.  Buttons should only be shown for player cruiser :)
        }
    }
}
