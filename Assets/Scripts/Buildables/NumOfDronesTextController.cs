using System;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.Commands;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables
{
    // FELIX  Reuse this for drone animations?
    public class NumOfDronesTextController : MonoBehaviour, IManagedDisposable
    {
        private IBuildable _buildable;
        private ICommand _toggleDroneConsumerFocusCommand;

        private const string DEFAULT_NUM_OF_DRONES = "0";

        public ITextMesh NumOfDronesText { get; private set; }

        private bool ShouldBeVisible
        {
            get
            {
                // Only show player cruiser drone numbers, unless this is a debug build
                return
                    _toggleDroneConsumerFocusCommand.CanExecute
                    && (_buildable.ParentCruiser.IsPlayerCruiser
                        || Debug.isDebugBuild);
            }
        }

        public void Initialise(IBuildable buildable)
        {
            Assert.IsNotNull(buildable);

            _buildable = buildable;

            _toggleDroneConsumerFocusCommand = _buildable.ToggleDroneConsumerFocusCommand;
            _toggleDroneConsumerFocusCommand.CanExecuteChanged += _toggleDroneConsumerFocusCommand_CanExecuteChanged;

            _buildable.DroneNumChanged += _buildable_DroneNumChanged;

            TextMesh numOfDronesText = gameObject.GetComponentInChildren<TextMesh>(includeInactive: true);
            Assert.IsNotNull(numOfDronesText);
            NumOfDronesText = new TextMeshWrapper(numOfDronesText);

            NumOfDronesText.Text = DEFAULT_NUM_OF_DRONES;

            UpdateVisibility();
        }

        private void _toggleDroneConsumerFocusCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            UpdateVisibility();
        }

        private void _buildable_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
        {
            NumOfDronesText.Text = e.NewNumOfDrones.ToString();
        }

        private void UpdateVisibility()
        {
            NumOfDronesText.SetActive(ShouldBeVisible);
        }

        public void DisposeManagedState()
        {
            _toggleDroneConsumerFocusCommand.CanExecuteChanged -= _toggleDroneConsumerFocusCommand_CanExecuteChanged;
            _buildable.DroneNumChanged -= _buildable_DroneNumChanged;
        }
    }
}
