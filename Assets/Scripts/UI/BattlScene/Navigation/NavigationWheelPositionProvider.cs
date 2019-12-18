using BattleCruisers.Cruisers;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class NavigationWheelPositionProvider : INavigationWheelPositionProvider
    {
        public Vector2 PlayerCruiserPosition { get; }
        public Vector2 PlayerCruiserZoomedOutPosition { get; }
        public Vector2 AICruiserPosition { get; }
        public Vector2 AICruiserZoomedOutPosition { get; }
        public Vector2 MidLeftPosition { get; }
        public Vector2 AINavalFactoryPosition { get; }
        public Vector2 PlayerNavalFactoryPosition { get; }
        public Vector2 OverviewPosition { get; }

        private const float CRUISER_ZOOMED_OUT_ORTHOGRAPHIC_SIZE = 10;

        public NavigationWheelPositionProvider(
            IPyramid navigationPanelArea, 
            ICameraNavigationWheelCalculator cameraCalculator,
            IRange<float> validOrthographicSizeRange,
            ICruiser playerCruiser,
            ICruiser aiCruiser)
        {
            Helper.AssertIsNotNull(navigationPanelArea, cameraCalculator, validOrthographicSizeRange, playerCruiser, aiCruiser);

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
            PlayerNavalFactoryPosition = cameraCalculator.FindNavigationWheelPosition(playerCruiserNavalFactoryTarget);

            // AI cruiser naval factory
            float aiCruiserBowSlotXPosition = aiCruiser.Position.x - aiCruiser.Size.x / 2;
            Vector3 aiCruiserNavalFactoryTargetPosition = new Vector3(aiCruiserBowSlotXPosition, float.MinValue);
            ICameraTarget aiCruiserNavalFactoryTarget = new CameraTarget(aiCruiserNavalFactoryTargetPosition, validOrthographicSizeRange.Min);
            AINavalFactoryPosition = cameraCalculator.FindNavigationWheelPosition(aiCruiserNavalFactoryTarget);

            // Player cruiser zoomed out position
            ICameraTarget playerCruiserZoomedOutTarget = new CameraTarget(playerCruiser.Position, CRUISER_ZOOMED_OUT_ORTHOGRAPHIC_SIZE);
            PlayerCruiserZoomedOutPosition = cameraCalculator.FindNavigationWheelPosition(playerCruiserZoomedOutTarget);

            // AI cruiser zoomed out position
            ICameraTarget aiCruiserZoomedOutTarget = new CameraTarget(aiCruiser.Position, CRUISER_ZOOMED_OUT_ORTHOGRAPHIC_SIZE);
            AICruiserZoomedOutPosition = cameraCalculator.FindNavigationWheelPosition(aiCruiserZoomedOutTarget);
        }
    }
}