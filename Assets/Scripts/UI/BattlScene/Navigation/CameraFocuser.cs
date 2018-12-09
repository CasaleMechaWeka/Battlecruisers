using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    // FELIX  Update tests :)
    public class CameraFocuser : ICameraFocuser
    {
        private readonly IPyramid _navigationPanelArea;
        private readonly INavigationWheel _navigationWheel;
        private readonly Vector2 _midLeftTarget;

        public CameraFocuser(IPyramid navigationPanelArea, INavigationWheel navigationWheel)
        {
            Helper.AssertIsNotNull(navigationPanelArea, navigationWheel);

            _navigationPanelArea = navigationPanelArea;
            _navigationWheel = navigationWheel;

            // FELIX  Move calculation to other class?  NavigationWheelPositionProvider?
            float midLeftX = _navigationPanelArea.BottomLeftVertex.x + _navigationPanelArea.Width / 4;
            float midLeftY = _navigationPanelArea.FindMaxY(midLeftX);
            _midLeftTarget = new Vector2(midLeftX, midLeftY);
        }

        public void FocusOnPlayerCruiser()
        {
            _navigationWheel.CenterPosition = _navigationPanelArea.BottomLeftVertex;
        }

        public void FocusOnAiCruiser()
        {
            _navigationWheel.CenterPosition = _navigationPanelArea.BottomRightVertex;
        }

        public void FocusMidLeft()
        {
            _navigationWheel.CenterPosition = _midLeftTarget;
        }
    }
}