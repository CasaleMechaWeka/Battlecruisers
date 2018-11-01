using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class NavigationWheelPanel : INavigationWheelPanel
    {
        private readonly IPyramid _panelArea;

        public INavigationWheel NavigationWheel { get; private set; }

        public NavigationWheelPanel(IPyramid panelArea, INavigationWheel navigationWheel)
        {
            Helper.AssertIsNotNull(panelArea, navigationWheel);

            _panelArea = panelArea;
            NavigationWheel = navigationWheel;
        }

        public float FindYProportion()
        {
            return FindLocalY() / _panelArea.Height;
        }

        public float FindXProportion()
        {
            float localYPosition = FindLocalY();

            IRange<float> globalXRangeAtHeight = _panelArea.FindGlobalXRange(localYPosition);

            // Sometimes _navigationWheel.CenterPosition.x can be slightly smaller (eg: by 3 x 10^7)
            // than the minimim expected position due to float rounding errors, so clamp to avoid this.
            float navigationWheelClampedXPosition = Mathf.Clamp(NavigationWheel.CenterPosition.x, globalXRangeAtHeight.Min, globalXRangeAtHeight.Max);

            float localXPositionAtHeight = navigationWheelClampedXPosition - globalXRangeAtHeight.Min;
            float widthAtHeight = globalXRangeAtHeight.Max - globalXRangeAtHeight.Min;
            return localXPositionAtHeight / widthAtHeight;
        }

        private float FindLocalY()
        {
            float localYPosition = NavigationWheel.CenterPosition.y - _panelArea.BottomLeftVertex.y;

            Assert.IsTrue(localYPosition >= 0);
            Assert.IsTrue(localYPosition <= _panelArea.Height);

            return localYPosition;
        }
    }
}