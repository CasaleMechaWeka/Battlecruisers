using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons
{
    public class ButtonColour
    {
        public static Color Default = Color.black;
        public static Color Selected = Color.white;
        public static Color SelectedNew = new Color(0.67843f, 0.204f, 0.118f, 1);
    }

    /// <summary>
    /// Highlights (changes the colour of) a buildable button while this buttons'
    /// buildable is being displayed in the informator.
    /// </summary>
    public class PvPBuildableButtonColourController
    {
        private readonly IPvPBroadcastingProperty<IPvPTarget> _selectedItem;
        private readonly IDictionary<IPvPTarget, IPvPBuildableButton> _buildableToButton;

        private IPvPBuildableButton _selectedButton;
        private IPvPBuildableButton SelectedButton
        {
            set
            {
                if (_selectedButton != null)
                {
                    _selectedButton.Color = ButtonColour.Default;
                }

                _selectedButton = value;

                if (_selectedButton != null)
                {
                    _selectedButton.Color = ButtonColour.Selected;
                }
            }
        }

        public PvPBuildableButtonColourController(IPvPBroadcastingProperty<IPvPTarget> selectedItem, IReadOnlyCollection<IPvPBuildableButton> buttons)
        {
            PvPHelper.AssertIsNotNull(selectedItem, buttons);

            _selectedItem = selectedItem;
            _selectedItem.ValueChanged += _selectedItem_ValueChanged;

            _buildableToButton = new Dictionary<IPvPTarget, IPvPBuildableButton>();
            foreach (IPvPBuildableButton button in buttons)
            {
                _buildableToButton.Add(button.Buildable, button);
            }
        }

        private void _selectedItem_ValueChanged(object sender, EventArgs e)
        {
            if (_selectedItem.Value != null
                && _buildableToButton.ContainsKey(_selectedItem.Value))
            {
                SelectedButton = _buildableToButton[_selectedItem.Value];
            }
            else
            {
                SelectedButton = null;
            }
        }
    }
}