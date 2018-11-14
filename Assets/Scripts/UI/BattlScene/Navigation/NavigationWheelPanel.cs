using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class NavigationWheelPanel : INavigationWheelPanel
    {
        public INavigationWheel NavigationWheel { get; private set; }
        public IPyramid PanelArea { get; private set; }

        public NavigationWheelPanel(IPyramid panelArea, INavigationWheel navigationWheel)
        {
            Helper.AssertIsNotNull(panelArea, navigationWheel);

            PanelArea = panelArea;
            NavigationWheel = navigationWheel;
        }

        public float FindYProportion()
        {
            return FindLocalY() / PanelArea.Height;
        }

        public float FindXProportion()
        {
            float localYPosition = FindLocalY();

            IRange<float> globalXRangeAtHeight = PanelArea.FindGlobalXRange(localYPosition);

            // Sometimes _navigationWheel.CenterPosition.x can be slightly smaller (eg: by 3 x 10^7)
            // than the minimim expected position due to float rounding errors, so clamp to avoid this.
            float navigationWheelClampedXPosition = Mathf.Clamp(NavigationWheel.CenterPosition.x, globalXRangeAtHeight.Min, globalXRangeAtHeight.Max);

            float localXPositionAtHeight = navigationWheelClampedXPosition - globalXRangeAtHeight.Min;
            float widthAtHeight = globalXRangeAtHeight.Max - globalXRangeAtHeight.Min;
            return localXPositionAtHeight / widthAtHeight;
        }

        private float FindLocalY()
        {
            float localYPosition = NavigationWheel.CenterPosition.y - PanelArea.BottomLeftVertex.y;

            Assert.IsTrue(localYPosition >= 0);
            Assert.IsTrue(localYPosition <= PanelArea.Height);

            return localYPosition;
        }
    }
}