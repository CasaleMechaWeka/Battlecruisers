using BattleCruisers.Utils;
using BattleCruisers.Utils.Clamper;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class NavigationWheelInitialiser : MonoBehaviour
    {
        public GameObject bottomLeftVertex, bottomRightVertex, topCenterVertex;

        public NavigationWheel InitialiseNavigationWheel()
        {
            Helper.AssertIsNotNull(bottomLeftVertex, bottomRightVertex, topCenterVertex);

            IPositionClamper navigationWheelPositionClamper
                = new TrianglePositionClamper(
                    bottomLeftVertex.transform.position,
                    bottomRightVertex.transform.position,
                    topCenterVertex.transform.position);

            NavigationWheel navigationWheel = GetComponentInChildren<NavigationWheel>();
            Assert.IsNotNull(navigationWheel);
            navigationWheel.Initialise(navigationWheelPositionClamper);

            return navigationWheel;
        }
    }
}