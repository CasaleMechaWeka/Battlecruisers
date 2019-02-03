using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public abstract class HomeScreenButton : MonoBehaviour, IPointerClickHandler
    {
        protected IHomeScreen _homeScreen;

        public void Initialise(IHomeScreen homeScreen)
        {
            Assert.IsNotNull(homeScreen);
            _homeScreen = homeScreen;
        }

        public abstract void OnPointerClick(PointerEventData eventData);
    }
}