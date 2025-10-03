using BattleCruisers.Data;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Heckles
{
    /// <summary>
    /// Controls visibility of the heckle message UI based on player settings.
    /// Attach this to the HeckleMessage GameObject to enable/disable it based on settings.
    /// </summary>
    public class HeckleVisibilityController : MonoBehaviour
    {
        private void Start()
        {
            UpdateHeckleVisibility();
        }

        private void UpdateHeckleVisibility()
        {
            // Access the settings manager to get the HecklesAllowed setting
            var settingsManager = DataProvider.SettingsManager;

            // Enable or disable the GameObject based on the HecklesAllowed setting
            gameObject.SetActive(settingsManager.HecklesAllowed);
        }
    }
}

