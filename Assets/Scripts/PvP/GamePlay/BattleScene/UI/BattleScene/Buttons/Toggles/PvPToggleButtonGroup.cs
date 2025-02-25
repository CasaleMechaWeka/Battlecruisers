using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.BattleScene.Buttons.Toggles;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Toggles
{
    public class PvPToggleButtonGroup : IPvPToggleButtonGroup
    {
        private readonly IToggleButton _defaultButton;

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

        public PvPToggleButtonGroup(IList<IToggleButton> buttons, IToggleButton defaultButton)
        {
            PvPHelper.AssertIsNotNull(buttons, defaultButton);

            _defaultButton = defaultButton;

            foreach (IToggleButton button in buttons)
            {
                button.Clicked += Button_Clicked;
            }

            SelectDefaultButton();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            SelectedButton = sender.Parse<IToggleButton>();
        }

        public void SelectDefaultButton()
        {
            SelectedButton = _defaultButton;
        }
    }
}