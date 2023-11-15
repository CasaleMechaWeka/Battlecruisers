using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Toggles
{
    public class PvPToggleButtonGroup : IPvPToggleButtonGroup
    {
        private readonly IPvPToggleButton _defaultButton;

        private IPvPToggleButton _selectedButton;
        private IPvPToggleButton SelectedButton
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

        public PvPToggleButtonGroup(IList<IPvPToggleButton> buttons, IPvPToggleButton defaultButton)
        {
            PvPHelper.AssertIsNotNull(buttons, defaultButton);

            _defaultButton = defaultButton;

            foreach (IPvPToggleButton button in buttons)
            {
                button.Clicked += Button_Clicked;
            }

            SelectDefaultButton();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            SelectedButton = sender.Parse<IPvPToggleButton>();
        }

        public void SelectDefaultButton()
        {
            SelectedButton = _defaultButton;
        }
    }
}