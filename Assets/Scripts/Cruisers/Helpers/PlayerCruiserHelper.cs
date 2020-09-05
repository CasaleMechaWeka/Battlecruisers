using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
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

        public override void OnBuildingConstructionStarted(IBuilding buildingStarted, ISlotAccessor slotAccessor, ISlotHighlighter slotHighlighter)
        {
            if (!slotAccessor.IsSlotAvailableForPlayer(buildingStarted.SlotSpecification))
            {
                _uiManager.HideCurrentlyShownMenu();
            }
            else
            {
                // Unhighlight the one slot that has just been taken
                slotHighlighter.HighlightAvailableSlots(buildingStarted.SlotSpecification);
            }
        }
    }
}
