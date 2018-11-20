using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Clamping;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class NavigationWheelInitialiser : MonoBehaviour
    {
        public GameObject bottomLeftVertex, bottomRightVertex, topCenterVertex;
        public GameObject navigationWheelPanelHighlight;

        public INavigationWheelPanel InitialiseNavigationWheel(IBroadcastingFilter navigationWheelEnabledFilter)
        {
            Helper.AssertIsNotNull(bottomLeftVertex, bottomRightVertex, topCenterVertex, navigationWheelPanelHighlight, navigationWheelEnabledFilter);

            IPyramid navigationWheelArea 
                = new Pyramid(
                    bottomLeftVertex.transform.position, 
                    bottomRightVertex.transform.position, 
                    topCenterVertex.transform.position);
            IPositionClamper navigationWheelPositionClamper = new PyramidPositionClamper(navigationWheelArea);

            NavigationWheel navigationWheel = GetComponentInChildren<NavigationWheel>();
            Assert.IsNotNull(navigationWheel);
            navigationWheel.Initialise(navigationWheelPositionClamper, navigationWheelPanelHighlight, navigationWheelEnabledFilter);

            return new NavigationWheelPanel(navigationWheelArea, navigationWheel);
        }
    }
}