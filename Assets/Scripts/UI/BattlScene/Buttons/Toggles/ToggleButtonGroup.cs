using BattleCruisers.Utils;
using System;
using System.Collections.Generic;

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

        public ToggleButtonGroup(IList<IToggleButton> buttons, IToggleButton defaultButton)
        {
            Helper.AssertIsNotNull(buttons, defaultButton);

            foreach (IToggleButton button in buttons)
            {
                button.Clicked += Button_Clicked;
            }

            SelectedButton = defaultButton;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            SelectedButton = sender.Parse<IToggleButton>();
        }
    }
}