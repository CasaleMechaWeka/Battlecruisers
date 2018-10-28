using BattleCruisers.Utils;
using BattleCruisers.Utils.Clamper;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class NavigationWheelInitialiser : MonoBehaviour
    {
        public GameObject bottomLeftVertex, bottomRightVertex, topCenterVertex;

        public INavigationWheelPanel InitialiseNavigationWheel()
        {
            Helper.AssertIsNotNull(bottomLeftVertex, bottomRightVertex, topCenterVertex);

            IPyramid navigationWheelArea 
                = new Pyramid(
                    bottomLeftVertex.transform.position, 
                    bottomRightVertex.transform.position, 
                    topCenterVertex.transform.position);
            IPositionClamper navigationWheelPositionClamper = new PyramidPositionClamper(navigationWheelArea);

            NavigationWheel navigationWheel = GetComponentInChildren<NavigationWheel>();
            Assert.IsNotNull(navigationWheel);
            navigationWheel.Initialise(navigationWheelPositionClamper);

            return new NavigationWheelPanel(navigationWheelArea, new TransformBC(navigationWheel.transform));
        }
    }
}