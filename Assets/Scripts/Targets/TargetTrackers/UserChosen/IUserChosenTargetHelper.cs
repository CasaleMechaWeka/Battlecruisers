using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.Targets.TargetTrackers.UserChosen
{
    public interface IUserChosenTargetHelper
    {
        ITarget UserChosenTarget { get; }

        event EventHandler UserChosenTargetChanged;

        /// <summary>
        /// If the user chosen target is the given target, clears the
        /// user chosen target.
        /// 
        /// Otherwise, sets the user chosen target to the given target.
        /// </summary>
        void ToggleChosenTarget(ITarget target);
    }
}