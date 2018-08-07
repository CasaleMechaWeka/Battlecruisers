using BattleCruisers.Buildables;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetTrackers
{
    public class UserChosenTargetHelper : IUserChosenTargetHelper
    {
        private readonly IUserChosenTargetManager _userChosenTargetManager;

        public UserChosenTargetHelper(IUserChosenTargetManager userChosenTargetManager)
        {
            Assert.IsNotNull(userChosenTargetManager);
            _userChosenTargetManager = userChosenTargetManager;
        }

        public void ToggleChosenTarget(ITarget target)
        {
            ITarget currentUserChosenTarget = _userChosenTargetManager.HighestPriorityTarget != null ? _userChosenTargetManager.HighestPriorityTarget.Target : null;

            if (ReferenceEquals(currentUserChosenTarget, target))
            {
                // Clear user chosen target
                _userChosenTargetManager.Target = null;
            }
            else
            {
                // Set user chosen target
                _userChosenTargetManager.Target = target;
            }
        }
    }
}