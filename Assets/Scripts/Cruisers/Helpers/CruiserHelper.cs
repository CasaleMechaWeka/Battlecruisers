using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils;

namespace BattleCruisers.Cruisers.Helpers
{
    public abstract class CruiserHelper : ICruiserHelper
    {
        protected readonly UIManager _uiManager;
        protected readonly ICameraFocuser _cameraFocuser;

        protected CruiserHelper(UIManager uIManager, ICameraFocuser cameraFocuser)
        {
            Helper.AssertIsNotNull(uIManager, cameraFocuser);

            _uiManager = uIManager;
            _cameraFocuser = cameraFocuser;
        }

        public abstract void FocusCameraOnCruiser();
        public abstract void OnBuildingConstructionStarted(IBuilding buildingStarted, SlotAccessor slotAccessor, SlotHighlighter slotHighlighter);
    }
}
