using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    // FELIX  Test :P
    public class CameraFocuser : ICameraFocuser
    {
        private readonly IPyramid _navigationPanelArea;
        private readonly INavigationWheel _navigationWheel;

        public CameraFocuser(IPyramid navigationPanelArea, INavigationWheel navigationWheel)
        {
            Helper.AssertIsNotNull(navigationPanelArea, navigationWheel);

            _navigationPanelArea = navigationPanelArea;
            _navigationWheel = navigationWheel;
        }

        public void FocusOnPlayerCruiser()
        {
            _navigationWheel.CenterPosition = _navigationPanelArea.BottomLeftVertex;
        }

        public void FocusOnAiCruiser()
        {
            _navigationWheel.CenterPosition = _navigationPanelArea.BottomRightVertex;
        }
    }
}