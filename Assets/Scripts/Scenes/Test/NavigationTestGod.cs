using BattleCruisers.UI.BattleScene.Navigation;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class NavigationTestGod : MonoBehaviour
    {
        private INavigationWheelPanel _navigationWheelPanel;

        private void Start()
        {
            NavigationWheelInitialiser navigationWheelInitialiser = FindObjectOfType<NavigationWheelInitialiser>();
            _navigationWheelPanel = navigationWheelInitialiser.InitialiseNavigationWheel();
        }

        private void Update()
        {
            // FLEIX  TEMP
            //float yProportion = _navigationWheelPanel.FindNavigationWheelYPositionAsProportionOfMaxHeight();
            //Debug.Log("yProportion: " + yProportion);

            float xProportion = _navigationWheelPanel.FindNavigationWheelXPositionAsProportionOfValidWidth();
            Debug.Log("xProportion: " + xProportion);
        }
    }
}