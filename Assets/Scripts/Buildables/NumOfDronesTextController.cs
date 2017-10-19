using System;
using BattleCruisers.Drones;
using BattleCruisers.UI.Commands;
using BattleCruisers.Utils;
using BattleCruisers.Utils.UIWrappers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables
{
    public class NumOfDronesTextController : MonoBehaviour, IManagedDisposable
    {
        private IBuildable _buildable;
        private ICommand _toggleDroneConsumerFocusCommand;
        public ITextMesh NumOfDronesText { get; private set; }

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
            NumOfDronesText.SetActive(_toggleDroneConsumerFocusCommand.CanExecute);
        }

        public void DisposeManagedState()
        {
            _toggleDroneConsumerFocusCommand.CanExecuteChanged -= _toggleDroneConsumerFocusCommand_CanExecuteChanged;
            _buildable.DroneNumChanged -= _buildable_DroneNumChanged;
        }
    }
}
