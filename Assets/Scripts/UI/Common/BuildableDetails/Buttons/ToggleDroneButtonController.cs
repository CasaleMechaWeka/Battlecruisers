using BattleCruisers.Buildables;
using BattleCruisers.UI.Sound;
using System;

namespace BattleCruisers.UI.Common.BuildableDetails.Buttons
{
    public class ToggleDroneButtonController : CanvasGroupButton, IButton
    {
        protected override ISoundKey ClickSound => null;

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

        public event EventHandler Clicked;

        protected override void OnClicked()
        {
            base.OnClicked();

			_buildable.ToggleDroneConsumerFocusCommand.Execute();
   
            Clicked?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateVisibility()
        {
            gameObject.SetActive(ShowToggleDroneButton);
        }

        private void ToggleDroneConsumerFocusCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            UpdateVisibility();
        }
    }
}
