using System;
using BattleCruisers.Buildables;
using BattleCruisers.Utils;

namespace BattleCruisers.Targets.TargetTrackers.UserChosen
{
    public class TogglableUserChosenTargetHelper : IUserChosenTargetHelper
    {
        private readonly IUserChosenTargetHelper _baseHelper;
        private readonly bool _isEnabled;

        public ITarget UserChosenTarget => _baseHelper.UserChosenTarget;

        public event EventHandler UserChosenTargetChanged
        {
            add { _baseHelper.UserChosenTargetChanged += value; }
            remove { _baseHelper.UserChosenTargetChanged -= value; }
        }

        public TogglableUserChosenTargetHelper(IUserChosenTargetHelper baseHelper, bool isEnabled)
        {
            Helper.AssertIsNotNull(baseHelper);

            _baseHelper = baseHelper;
            _isEnabled = isEnabled;
        }

        public void ToggleChosenTarget(ITarget target)
        {
            if (_isEnabled)
            {
                _baseHelper.ToggleChosenTarget(target);
            }
        }
    }
}