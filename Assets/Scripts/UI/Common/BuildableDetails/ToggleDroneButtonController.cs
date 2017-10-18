using System;
using BattleCruisers.Buildables;
using BattleCruisers.Drones;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public class ToggleDroneButtonController : MonoBehaviour
    {
        private Button _button;
        private IDroneManager _droneManager;

        private IBuildable _buildable;
        public IBuildable Buildable
        {
            private get { return _buildable; }
            set
            {
                if (_buildable != null)
                {
                    _buildable.CompletedBuildable -= Buildable_CompletedBuildable;
                }

                _buildable = value;
				UpdateVisibility();

                if (_buildable != null && _buildable.BuildableState != BuildableState.Completed)
                {
                    _buildable.CompletedBuildable += Buildable_CompletedBuildable;
                }
            }
        }

        // FELIX  Units should never show number of drones text mesh :/
        // Should only be visible for player buildables, not AI buildables
        private bool ShowToggleDroneButton 
        { 
            get 
            {
                return
                    _buildable != null
                    && _buildable.Faction == Faction.Blues
                    && _buildable.IsDroneConsumerFocusable;
            } 
        }

        public void Initialise(IDroneManager droneManager)
        {
            Assert.IsNotNull(droneManager);
            _droneManager = droneManager;

            _button = GetComponent<Button>();
            Assert.IsNotNull(_button);
            _button.onClick.AddListener(ToggleDroneButton);
        }

        private void ToggleDroneButton()
        {
            _droneManager.ToggleDroneConsumerFocus(Buildable.DroneConsumer);
        }

        private void Buildable_CompletedBuildable(object sender, EventArgs e)
        {
            _buildable.CompletedBuildable -= Buildable_CompletedBuildable;
            UpdateVisibility();
        }
        
        private void UpdateVisibility()
        {
            gameObject.SetActive(ShowToggleDroneButton);
        }
    }
}
