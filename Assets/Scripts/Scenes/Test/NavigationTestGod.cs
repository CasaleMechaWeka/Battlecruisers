using BattleCruisers.UI.BattleScene.Navigation;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class NavigationTestGod : MonoBehaviour
    {
        void Start()
        {
            NavigationWheelInitialiser navigationWheelInitialiser = FindObjectOfType<NavigationWheelInitialiser>();
            navigationWheelInitialiser.InitialiseNavigationWheel();
        }
    }
}