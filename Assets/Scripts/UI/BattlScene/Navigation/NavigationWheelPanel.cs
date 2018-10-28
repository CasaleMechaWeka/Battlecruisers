using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    // FELIX  Test :)
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
            return FindLocalY() / _panelArea.Height;
        }

        public float FindNavigationWheelXPositionAsProportionOfValidWidth()
        {
            float localYPosition = FindLocalY();

            IRange<float> globalXRangeAtHeight = _panelArea.FindGlobalXRange(localYPosition);

            // Sometimes these can have minor differences (eg: -3 x 10^7), so clamp
            // to avoid these.
            float navigationWheelClampedXPosition = Mathf.Clamp(_navigationWheel.CenterPosition.x, globalXRangeAtHeight.Min, globalXRangeAtHeight.Max);

            float localXPositionAtHeight = navigationWheelClampedXPosition - globalXRangeAtHeight.Min;
            float widthAtHeight = globalXRangeAtHeight.Max - globalXRangeAtHeight.Min;
            return localXPositionAtHeight / widthAtHeight;
        }

        private float FindLocalY()
        {
            float localYPosition = _navigationWheel.CenterPosition.y - _panelArea.BottomLeftVertex.y;

            Assert.IsTrue(localYPosition >= 0);
            Assert.IsTrue(localYPosition <= _panelArea.Height);

            return localYPosition;
        }
    }
}