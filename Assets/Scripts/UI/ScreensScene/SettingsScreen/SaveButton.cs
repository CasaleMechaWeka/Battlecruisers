using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    // FELIX  Disable save button if there is nothing to save :)
    // FELIX  Avoid duplicate code with CancelButton?
    public class SaveButton : Togglable, IPointerClickHandler
    {
        private ISettingsScreen _settingsScreen;

        public void Initialise(ISettingsScreen settingsScreen)
        {
            Assert.IsNotNull(settingsScreen);
            _settingsScreen = settingsScreen;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _settingsScreen.Save();
        }
    }
}