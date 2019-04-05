using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    // FELIX  Test
    public class BuildableButtonColourController
    {
        private readonly IBroadcastingProperty<ITarget> _selectedItem;
        private readonly IDictionary<ITarget, IBuildableButton> _buildableToButton;

        private IBuildableButton _selectedButton;
        private IBuildableButton SelectedButton
        {
            set
            {
                if (_selectedButton != null)
                {
                    _selectedButton.Color = Color.black;
                }

                _selectedButton = value;

                if (_selectedButton != null)
                {
                    _selectedButton.Color = Color.white;
                }
            }
        }

        public BuildableButtonColourController(IBroadcastingProperty<ITarget> selectedItem, IReadOnlyCollection<IBuildableButton> buttons)
        {
            Helper.AssertIsNotNull(selectedItem, buttons);

            _selectedItem = selectedItem;
            _selectedItem.ValueChanged += _selectedItem_ValueChanged;

            _buildableToButton = new Dictionary<ITarget, IBuildableButton>();
            foreach (IBuildableButton button in buttons)
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