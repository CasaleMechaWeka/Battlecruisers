using System;
using BattleCruisers.Buildables;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildableDetails.Buttons
{
    public class ToggleDroneButtonController : Togglable, IButton
    {
        private Button _button;

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

        public override void Initialise()
        {
            base.Initialise();

            _button = GetComponent<Button>();
            Assert.IsNotNull(_button);
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
			_buildable.ToggleDroneConsumerFocusCommand.Execute();
   
            if (Clicked != null)
            {
                Clicked.Invoke(this, EventArgs.Empty);
            }
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
