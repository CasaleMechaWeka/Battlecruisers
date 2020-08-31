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

        public override void OnBuildingConstructionStarted(IBuilding buildingStarted, ISlotAccessor slotAccessor)
        {
            if (!slotAccessor.IsSlotAvailable(buildingStarted.SlotSpecification))
            {
                _uiManager.HideCurrentlyShownMenu();
            }
        }
    }
}
