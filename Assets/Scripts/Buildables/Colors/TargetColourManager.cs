using BattleCruisers.Utils.Properties;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Colors
{
    // FELIX  Test :)
    // FELIX  Handle user chosen targets :D
    public class TargetColourManager
    {
        private readonly IBroadcastingProperty<ITarget> _itemShownInInformator;

        private static Color DefaultColor = Color.black;
        private static Color SelectedColor = new Color(186f/255f, 56f/255f, 32f/255f);    // Orange
        private static Color TargettedColor = new Color(176f/255f, 51f/255f, 17f/255f);   // Dark orange

        private ITarget _selectedTarget;
        private ITarget SelectedTarget
        {
            set
            {
                if (_selectedTarget != null)
                {
                    _selectedTarget.Color = DefaultColor;
                }

                _selectedTarget = value;

                if (_selectedTarget != null)
                {
                    _selectedTarget.Color = SelectedColor;
                }
            }
        }

        public TargetColourManager(IBroadcastingProperty<ITarget> itemShownInInformator)
        {
            Assert.IsNotNull(itemShownInInformator);

            _itemShownInInformator = itemShownInInformator;
            _itemShownInInformator.ValueChanged += _itemShownInInformator_ValueChanged;
        }

        private void _itemShownInInformator_ValueChanged(object sender, EventArgs e)
        {
            SelectedTarget = null;

            if (_itemShownInInformator.Value != null
                && _itemShownInInformator.Value.IsInScene)
            {
                SelectedTarget = _itemShownInInformator.Value;
            }
        }
    }
}