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

            float navalFactoryXDelta = navigationPanelArea.Width / 6;
            float navalFactoryY = navigationPanelArea.BottomLeftVertex.y;

            float aiNavalFactoryX = navigationPanelArea.BottomRightVertex.x - navalFactoryXDelta;
            AINavalFactoryPosition = new Vector2(aiNavalFactoryX, navalFactoryY);

            float playerNavalFactoryX = navigationPanelArea.BottomLeftVertex.x + navalFactoryXDelta;
            PlayerNavalFactoryPosition = new Vector2(playerNavalFactoryX, navalFactoryY);
        }
    }
}