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
        public ICameraTarget PlayerCruiserDeathTarget { get; }
        public ICameraTarget PlayerCruiserNukedTarget { get; }
        public Vector2 PlayerNavalFactoryPosition { get; }

        public Vector2 AICruiserPosition { get; }
        public ICameraTarget AICruiserDeathTarget { get; }
        public ICameraTarget AICruiserNukedTarget { get; }
        public Vector2 AINavalFactoryPosition { get; }

        public Vector2 MidLeftPosition { get; }
        public Vector2 OverviewPosition { get; }

        private const float CRUISER_DEATH_ORTHOGRAPHIC_SIZE = 10;
        private const float NUKE_ORTHOGRAPHIC_SIZE = 30;

        public NavigationWheelPositionProvider(
            IPyramid navigationPanelArea,
            ICameraCalculator cameraCalculator,
            ICameraNavigationWheelCalculator navWheelCalculator,
            IRange<float> validOrthographicSizeRange,
            ICruiser playerCruiser,
            ICruiser aiCruiser,
            ICamera camera)
        {
            Helper.AssertIsNotNull(navigationPanelArea, cameraCalculator, navWheelCalculator, validOrthographicSizeRange, playerCruiser, aiCruiser, camera);

            PlayerCruiserPosition = navigationPanelArea.BottomLeftVertex;
            AICruiserPosition = navigationPanelArea.BottomRightVertex;
            OverviewPosition = navigationPanelArea.TopCenterVertex;

            float midLeftX = navigationPanelArea.BottomLeftVertex.x + navigationPanelArea.Width / 4;
            float midLeftY = navigationPanelArea.FindMaxY(midLeftX);
            MidLeftPosition = new Vector2(midLeftX, midLeftY);

            // Player cruiser naval factory
            float playerCruiserBowSlotXPosition = playerCruiser.Position.x + playerCruiser.Size.x / 2;
            Vector3 playerCruiserNavalFactoryTargetPosition = new Vector3(playerCruiserBowSlotXPosition, float.MinValue);
            ICameraTarget playerCruiserNavalFactoryTarget = new CameraTarget(playerCruiserNavalFactoryTargetPosition, validOrthographicSizeRange.Min);
            PlayerNavalFactoryPosition = navWheelCalculator.FindNavigationWheelPosition(playerCruiserNavalFactoryTarget);

            // AI cruiser naval factory
            float aiCruiserBowSlotXPosition = aiCruiser.Position.x - aiCruiser.Size.x / 2;
            Vector3 aiCruiserNavalFactoryTargetPosition = new Vector3(aiCruiserBowSlotXPosition, float.MinValue);
            ICameraTarget aiCruiserNavalFactoryTarget = new CameraTarget(aiCruiserNavalFactoryTargetPosition, validOrthographicSizeRange.Min);
            AINavalFactoryPosition = navWheelCalculator.FindNavigationWheelPosition(aiCruiserNavalFactoryTarget);

            PlayerCruiserDeathTarget = CreateTarget(cameraCalculator, CRUISER_DEATH_ORTHOGRAPHIC_SIZE, playerCruiser.Position.x);
            PlayerCruiserNukedTarget = CreateTarget(cameraCalculator, NUKE_ORTHOGRAPHIC_SIZE, playerCruiser.Position.x);

            AICruiserDeathTarget = CreateTarget(cameraCalculator, CRUISER_DEATH_ORTHOGRAPHIC_SIZE, aiCruiser.Position.x);
            AICruiserNukedTarget = CreateTarget(cameraCalculator, NUKE_ORTHOGRAPHIC_SIZE, aiCruiser.Position.x);
        }

        private ICameraTarget CreateTarget(ICameraCalculator cameraCalculator, float orthographicSize, float xPosition)
        {
            float yPosition = cameraCalculator.FindCameraYPosition(orthographicSize);
            Vector3 position = new Vector3(xPosition, yPosition);
            return new CameraTarget(position, orthographicSize);
        }
    }
}