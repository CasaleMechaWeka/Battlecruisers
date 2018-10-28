using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class NavigationWheelPanel : INavigationWheelPanel
    {
        private readonly IPyramid _panelArea;
        private readonly INavigationWheel _navigationWheel;

        public NavigationWheelPanel(IPyramid panelArea, INavigationWheel navigationWheel)
        {
            Helper.AssertIsNotNull(panelArea, navigationWheel);

            _panelArea = panelArea;
            _navigationWheel = navigationWheel;
        }

        public float FindNavigationWheelYPositionAsProportionOfMaxHeight()
        {
            float localYPosition = _navigationWheel.CenterPosition.y - _panelArea.BottomLeftVertex.y;

            Assert.IsTrue(localYPosition >= 0);
            Assert.IsTrue(localYPosition <= _panelArea.Height);

            return localYPosition / _panelArea.Height;
        }

        public float FindNavigationWheelXPositionAsProportionOfValidWidth()
        {
            // FELIX
            return 0;
        }
    }
}