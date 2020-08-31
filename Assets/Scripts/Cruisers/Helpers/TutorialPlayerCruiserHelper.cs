using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Navigation;

namespace BattleCruisers.Cruisers.Helpers
{
    public class TutorialPlayerCruiserHelper : CruiserHelper
    {
        public TutorialPlayerCruiserHelper(IUIManager uIManager, ICameraFocuser cameraFocuser)
            : base(uIManager, cameraFocuser)
        {
        }

        public override void FocusCameraOnCruiser()
        {
             // Disabled for tutorial
        }

        public override void OnBuildingConstructionStarted(IBuilding buildingStarted, ISlotAccessor slotAccessor)
        {
            _uiManager.HideCurrentlyShownMenu();
        }
    }
}
