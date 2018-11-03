using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Buttons.Toggles
{
    public class ToggleButtonGroup
    {
        private IToggleButton _selectedButton;
        private IToggleButton SelectedButton
        {
            get { return _selectedButton; }
            set
            {
                if (_selectedButton != null)
                {
                    _selectedButton.IsSelected = false;
                }

                _selectedButton = value;

                if (_selectedButton != null)
                {
                    _selectedButton.IsSelected = true;
                }
            }
        }

        public ToggleButtonGroup(IList<IToggleButton> buttons)
        {
            Assert.IsNotNull(buttons);

            foreach (IToggleButton button in buttons)
            {
                button.Clicked += Button_Clicked;
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            IToggleButton clickedButton = sender.Parse<IToggleButton>();
            SelectedButton = ReferenceEquals(SelectedButton, clickedButton) ? null : clickedButton;
        }
    }
}