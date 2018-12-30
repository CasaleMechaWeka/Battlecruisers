using System;
using BattleCruisers.Buildables;
using BattleCruisers.Utils;

namespace BattleCruisers.Targets.TargetTrackers.UserChosen
{
    // FELIX  Update tests :)
    public class TogglableUserChosenTargetHelper : IUserChosenTargetHelper
    {
        private readonly IUserChosenTargetHelper _baseHelper;
        private readonly IUserChosenTargetHelperPermissions _permissions;

        public ITarget UserChosenTarget { get { return _baseHelper.UserChosenTarget; } }

        public event EventHandler UserChosenTargetChanged
        {
            add { _baseHelper.UserChosenTargetChanged += value; }
            remove { _baseHelper.UserChosenTargetChanged -= value; }
        }

        public TogglableUserChosenTargetHelper(IUserChosenTargetHelper baseHelper, IUserChosenTargetHelperPermissions permissions)
        {
            Helper.AssertIsNotNull(baseHelper, permissions);

            _baseHelper = baseHelper;
            _permissions = permissions;
        }

        public void ToggleChosenTarget(ITarget target)
        {
            if (_permissions.IsEnabled)
            {
                _baseHelper.ToggleChosenTarget(target);
            }
        }
    }
}