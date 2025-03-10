namespace BattleCruisers.Targets.TargetTrackers.UserChosen
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