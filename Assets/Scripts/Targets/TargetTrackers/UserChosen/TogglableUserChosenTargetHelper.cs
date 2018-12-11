using BattleCruisers.Buildables;
using BattleCruisers.Utils;

namespace BattleCruisers.Targets.TargetTrackers.UserChosen
{
    // FELIX  Test :D
    public class TogglableUserChosenTargetHelper : IUserChosenTargetHelper
    {
        private readonly IUserChosenTargetHelper _baseHelper;
        private readonly IUserChosenTargetHelperPermissions _permissions;

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