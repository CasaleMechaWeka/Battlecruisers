using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils.Clamper;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class NavigationTestGod : MonoBehaviour
    {
        void Start()
        {
            // FELIX  Create NavigationWheelInitialiser (that has access to 3 triangle points, 
            // instead of hardcoding here :P)
            IPositionClamper navigationWheelPositionClamper
                = new TrianglePositionClamper(
                    bottomLeftVertex: new Vector2(500, 500),
                    bottomRightVertex: new Vector2(1000, 500),
                    topCenterVertex: new Vector2(750, 1000));

            NavigationWheel navigationWheel = FindObjectOfType<NavigationWheel>();
            navigationWheel.Initialise(navigationWheelPositionClamper);
        }
    }
}