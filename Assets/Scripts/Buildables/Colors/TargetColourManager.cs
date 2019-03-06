using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using System;
using UnityEngine;

namespace BattleCruisers.Buildables.Colors
{
    // FELIX  Test :)

    /// <summary>
    /// Handles changing a targets colour when the user has:
    /// 1. Selected it (to view the target details)
    /// 2. Chosen as the number one target (to make everything attack that target above all others)
    /// </summary>
    public class TargetColourManager
    {
        private readonly IBroadcastingProperty<ITarget> _itemShownInInformator;
        private readonly IRankedTargetTracker _userChosenTargetTracker;

        private static Color DefaultColor = Color.black;
        private static Color SelectedColor = new Color(186f/255f, 56f/255f, 32f/255f);    // Orange
        private static Color TargettedColor = Color.red;

        // The user chosen target colour trumps the selected target colour.
        // Hence, do NOT set the SelectedColor if the TargettedColour is
        // already set.
        private ITarget _selectedTarget;
        private ITarget SelectedTarget
        {
            set
            {
                if (_selectedTarget != null
                    && !ReferenceEquals(_userChosenTarget, _selectedTarget))
                {
                    _selectedTarget.Color = DefaultColor;
                }

                _selectedTarget = value;

                if (_selectedTarget != null
                    && !ReferenceEquals(_userChosenTarget, _selectedTarget))
                {
                    _selectedTarget.Color = SelectedColor;
                }
            }
        }

        private ITarget _userChosenTarget;
        private ITarget UserChosenTarget
        {
            set
            {
                if (_userChosenTarget != null)
                {
                    // When the user clears their chosen target, the target may still be selected.
                    // In this case apply the SelectedColor instead of the DefaultColor.
                    _userChosenTarget.Color = ReferenceEquals(_userChosenTarget, _selectedTarget) ? SelectedColor : DefaultColor;
                }

                _userChosenTarget = value;

                if (_userChosenTarget != null)
                {
                    _userChosenTarget.Color = TargettedColor;
                }
            }
        }

        public TargetColourManager(IBroadcastingProperty<ITarget> itemShownInInformator, IRankedTargetTracker userChosenTargetTracker)
        {
            Helper.AssertIsNotNull(itemShownInInformator, userChosenTargetTracker);

            _itemShownInInformator = itemShownInInformator;
            _itemShownInInformator.ValueChanged += _itemShownInInformator_ValueChanged;

            _userChosenTargetTracker = userChosenTargetTracker;
            _userChosenTargetTracker.HighestPriorityTargetChanged += _userChosenTargetTracker_HighestPriorityTargetChanged;
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

        private void _userChosenTargetTracker_HighestPriorityTargetChanged(object sender, EventArgs e)
        {
            UserChosenTarget = null;

            if (_userChosenTargetTracker.HighestPriorityTarget != null)
            {
                UserChosenTarget = _userChosenTargetTracker.HighestPriorityTarget.Target;
            }
        }
    }
}