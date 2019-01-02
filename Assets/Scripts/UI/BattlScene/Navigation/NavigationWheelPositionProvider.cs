using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class NavigationWheelPositionProvider : INavigationWheelPositionProvider
    {
        public Vector2 PlayerCruiserPosition { get; private set; }
        public Vector2 AICruiserPosition { get; private set; }
        public Vector2 MidLeftPosition { get; private set; }
        public Vector2 AINavalFactoryPosition { get; private set; }
        public Vector2 PlayerNavalFactoryPosition { get; private set; }

        public NavigationWheelPositionProvider(IPyramid navigationPanelArea)
        {
            Assert.IsNotNull(navigationPanelArea);

            PlayerCruiserPosition = navigationPanelArea.BottomLeftVertex;
            AICruiserPosition = navigationPanelArea.BottomRightVertex;

            float midLeftX = navigationPanelArea.BottomLeftVertex.x + navigationPanelArea.Width / 4;
            float midLeftY = navigationPanelArea.FindMaxY(midLeftX);
            MidLeftPosition = new Vector2(midLeftX, midLeftY);

            float navigationFactoryXDelta = navigationPanelArea.Width / 6;
            float navigationFactoryY = navigationPanelArea.BottomLeftVertex.y;

            float aiNavalFactoryX = navigationPanelArea.BottomRightVertex.x - navigationFactoryXDelta;
            AINavalFactoryPosition = new Vector2(aiNavalFactoryX, navigationFactoryY);

            float playerNavalFactoryX = navigationPanelArea.BottomLeftVertex.x + navigationFactoryXDelta;
            PlayerNavalFactoryPosition = new Vector2(playerNavalFactoryX, navigationFactoryY);
        }
    }
}