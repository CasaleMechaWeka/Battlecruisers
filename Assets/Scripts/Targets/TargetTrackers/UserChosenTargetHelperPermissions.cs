// FELIX Create UserChosen namespace :)
namespace BattleCruisers.Targets.TargetTrackers
{
    public class UserChosenTargetHelperPermissions : IUserChosenTargetHelperPermissions, IUserChosenTargetHelperSettablePermissions
    {
        public bool IsEnabled { get; set; }

        public UserChosenTargetHelperPermissions(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }
    }
}