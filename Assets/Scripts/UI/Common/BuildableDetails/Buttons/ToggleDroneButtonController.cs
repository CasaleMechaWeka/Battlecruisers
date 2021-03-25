using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.UI.Common.BuildableDetails.Buttons
{
    public class ToggleDroneButtonController : CanvasGroupButton, IButton
    {
        private IBuildable _buildable;
        public IBuildable Buildable
        {
            private get { return _buildable; }
            set
            {
                if (_buildable != null)
                {
                    _buildable.ToggleDroneConsumerFocusCommand.CanExecuteChanged -= ToggleDroneConsumerFocusCommand_CanExecuteChanged;
                }

                _buildable = value;

                if (_buildable != null)
                {
                    _buildable.ToggleDroneConsumerFocusCommand.CanExecuteChanged += ToggleDroneConsumerFocusCommand_CanExecuteChanged;
					UpdateVisibility();
                }
            }
        }

        // Should only be visible for player buildables, not AI buildables
        private bool ShowToggleDroneButton 
        { 
            get 
            {
                return
                    _buildable.Faction == Faction.Blues
                    && _buildable.ToggleDroneConsumerFocusCommand.CanExecute;
            } 
        }

        protected override void OnClicked()
        {
            base.OnClicked();
			_buildable.ToggleDroneConsumerFocusCommand.Execute();
        }

        private void ToggleDroneConsumerFocusCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            gameObject.SetActive(ShowToggleDroneButton);
        }
    }
}
