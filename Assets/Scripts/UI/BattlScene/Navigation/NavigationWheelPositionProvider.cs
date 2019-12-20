using BattleCruisers.Cruisers;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class NavigationWheelPositionProvider : INavigationWheelPositionProvider
    {
        public Vector2 PlayerCruiserPosition { get; }
        public Vector2 PlayerCruiserDeathPosition { get; }
        public Vector2 AICruiserPosition { get; }
        public Vector2 AICruiserDeathPosition { get; }
        public ICameraTarget PlayerCruiserNukedTarget { get; }
        public ICameraTarget AICruiserNukedTarget { get; }
        public Vector2 MidLeftPosition { get; }
        public Vector2 AINavalFactoryPosition { get; }
        public Vector2 PlayerNavalFactoryPosition { get; }
        public Vector2 OverviewPosition { get; }

        private const float CRUISER_DEATH_ORTHOGRAPHIC_SIZE = 10;
        private const float NUKE_ORTHOGRAPHIC_SIZE = 30;
        private const float NUKE_CAMERA_POSITION_Y = 15;

        public NavigationWheelPositionProvider(
            IPyramid navigationPanelArea, 
            ICameraNavigationWheelCalculator cameraCalculator,
            IRange<float> validOrthographicSizeRange,
            ICruiser playerCruiser,
            ICruiser aiCruiser,
            ICamera camera)
        {
            Helper.AssertIsNotNull(navigationPanelArea, cameraCalculator, validOrthographicSizeRange, playerCruiser, aiCruiser, camera);

            PlayerCruiserPosition = navigationPanelArea.BottomLeftVertex;
            AICruiserPosition = navigationPanelArea.BottomRightVertex;
            OverviewPosition = navigationPanelArea.TopCenterVertex;

            PlayerCruiserNukedTarget 
                = new CameraTarget(
                    new Vector3(playerCruiser.Position.x, NUKE_CAMERA_POSITION_Y, camera.Transform.Position.z), 
                    NUKE_ORTHOGRAPHIC_SIZE);
            AICruiserNukedTarget 
                = new CameraTarget(
                    new Vector3(aiCruiser.Position.x, NUKE_CAMERA_POSITION_Y, camera.Transform.Position.z), 
                    NUKE_ORTHOGRAPHIC_SIZE);

            float midLeftX = navigationPanelArea.BottomLeftVertex.x + navigationPanelArea.Width / 4;
            float midLeftY = navigationPanelArea.FindMaxY(midLeftX);
            MidLeftPosition = new Vector2(midLeftX, midLeftY);

            // Player cruiser naval factory
            float playerCruiserBowSlotXPosition = playerCruiser.Position.x + playerCruiser.Size.x / 2;
            Vector3 playerCruiserNavalFactoryTargetPosition = new Vector3(playerCruiserBowSlotXPosition, float.MinValue);
            ICameraTarget playerCruiserNavalFactoryTarget = new CameraTarget(playerCruiserNavalFactoryTargetPosition, validOrthographicSizeRange.Min);
            PlayerNavalFactoryPosition = cameraCalculator.FindNavigationWheelPosition(playerCruiserNavalFactoryTarget);

            // AI cruiser naval factory
            float aiCruiserBowSlotXPosition = aiCruiser.Position.x - aiCruiser.Size.x / 2;
            Vector3 aiCruiserNavalFactoryTargetPosition = new Vector3(aiCruiserBowSlotXPosition, float.MinValue);
            ICameraTarget aiCruiserNavalFactoryTarget = new CameraTarget(aiCruiserNavalFactoryTargetPosition, validOrthographicSizeRange.Min);
            AINavalFactoryPosition = cameraCalculator.FindNavigationWheelPosition(aiCruiserNavalFactoryTarget);

            // Player cruiser death position
            ICameraTarget playerCruiserDeathTarget = new CameraTarget(playerCruiser.Position, CRUISER_DEATH_ORTHOGRAPHIC_SIZE);
            PlayerCruiserDeathPosition = cameraCalculator.FindNavigationWheelPosition(playerCruiserDeathTarget);

            // AI cruiser death position
            ICameraTarget aiCruiserDeathTarget = new CameraTarget(aiCruiser.Position, CRUISER_DEATH_ORTHOGRAPHIC_SIZE);
            AICruiserDeathPosition = cameraCalculator.FindNavigationWheelPosition(aiCruiserDeathTarget);
        }
    }
}