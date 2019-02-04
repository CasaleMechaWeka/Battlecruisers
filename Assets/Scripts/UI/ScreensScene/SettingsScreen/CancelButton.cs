using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class CancelButton : Togglable, IPointerClickHandler
    {
        private ISettingsScreen _settingsScreen;

        public void Initialise(ISettingsScreen settingsScreen)
        {
            Assert.IsNotNull(settingsScreen);
            _settingsScreen = settingsScreen;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _settingsScreen.Cancel();
        }
    }
}