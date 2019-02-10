using BattleCruisers.Cruisers;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class NavigationWheelPositionProvider : INavigationWheelPositionProvider
    {
        public Vector2 PlayerCruiserPosition { get; private set; }
        public Vector2 AICruiserPosition { get; private set; }
        public Vector2 MidLeftPosition { get; private set; }
        public Vector2 AINavalFactoryPosition { get; private set; }
        public Vector2 PlayerNavalFactoryPosition { get; private set; }

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

            float midLeftX = navigationPanelArea.BottomLeftVertex.x + navigationPanelArea.Width / 4;
            float midLeftY = navigationPanelArea.FindMaxY(midLeftX);
            MidLeftPosition = new Vector2(midLeftX, midLeftY);

            // FELIX  Remove :p
            float navalFactoryXDelta = navigationPanelArea.Width / 6;
            float navalFactoryY = navigationPanelArea.BottomLeftVertex.y;

            float aiNavalFactoryX = navigationPanelArea.BottomRightVertex.x - navalFactoryXDelta;
            AINavalFactoryPosition = new Vector2(aiNavalFactoryX, navalFactoryY);

            float playerNavalFactoryX = navigationPanelArea.BottomLeftVertex.x + navalFactoryXDelta;
            PlayerNavalFactoryPosition = new Vector2(playerNavalFactoryX, navalFactoryY);

            // Player cruiser naval factory
            float playerCruiserBowSlotXPosition = playerCruiser.Position.x + playerCruiser.Size.x / 2;
            Vector3 playerCruiserNavalFactoryTargetPosition = new Vector3(playerCruiserBowSlotXPosition, float.MinValue);
            ICameraTarget playerCruiserNavalFactoryTarget = new CameraTarget(playerCruiserNavalFactoryTargetPosition, validOrthographicSizeRange.Min);
            PlayerNavalFactoryPosition = cameraCalculator.FindNavigationWheelPosition(playerCruiserNavalFactoryTarget);

            // FELIX  AI naval factory :P
        }
    }
}