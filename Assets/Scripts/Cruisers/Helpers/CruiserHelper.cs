using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras;
using BattleCruisers.Utils;

namespace BattleCruisers.Cruisers.Helpers
{
    public abstract class CruiserHelper : ICruiserHelper
    {
        protected readonly IUIManager _uiManager;
        protected readonly ICameraController _cameraController;

        protected CruiserHelper(IUIManager uIManager, ICameraController cameraController)
        {
            Helper.AssertIsNotNull(uIManager, cameraController);

            _uiManager = uIManager;
            _cameraController = cameraController;
        }

        public abstract void FocusCameraOnCruiser();
        public abstract void ShowBuildingGroupButtons();
    }
}
