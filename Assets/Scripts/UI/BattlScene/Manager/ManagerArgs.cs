using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Manager
{
    public class ManagerArgs : IManagerArgs
    {
        public ICruiser PlayerCruiser { get; private set; }
        public ICruiser AICruiser { get; private set; }
        public ICameraController CameraController { get; private set; }
        public IBuildMenu BuildMenu { get; private set; }
        public IBuildableDetailsManager DetailsManager { get; private set; }

        public ManagerArgs(
            ICruiser playerCruiser,
            ICruiser aiCruiser,
            ICameraController cameraController,
            IBuildMenu buildMenu,
            IBuildableDetailsManager detailsManager)
        {
            Helper.AssertIsNotNull(playerCruiser, aiCruiser, cameraController, buildMenu, detailsManager);

            PlayerCruiser = playerCruiser;
            AICruiser = aiCruiser;
            CameraController = cameraController;
            BuildMenu = buildMenu;
            DetailsManager = detailsManager;
        }    
    }
}
