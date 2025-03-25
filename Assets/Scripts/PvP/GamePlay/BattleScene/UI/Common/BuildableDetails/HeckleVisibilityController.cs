using BattleCruisers.Data;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails
{
    public class HeckleVisibilityController : MonoBehaviour
    {
        private void Start()
        {
            UpdateHeckleVisibility();
        }

        private void UpdateHeckleVisibility()
        {
            // Access the application model to get settings manager
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
            var settingsManager = DataProvider.SettingsManager;

            // Enable or disable the GameObject based on the HecklesAllowed setting
            gameObject.SetActive(settingsManager.HecklesAllowed);
        }
    }

}