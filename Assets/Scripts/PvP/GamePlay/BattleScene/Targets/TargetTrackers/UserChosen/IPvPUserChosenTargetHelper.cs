using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.UserChosen
{
    public interface IPvPUserChosenTargetHelper
    {
        IPvPTarget UserChosenTarget { get; }

        event EventHandler UserChosenTargetChanged;

        /// <summary>
        /// If the user chosen target is the given target, clears the
        /// user chosen target.
        /// 
        /// Otherwise, sets the user chosen target to the given target.
        /// </summary>
        void ToggleChosenTarget(IPvPTarget target);
    }
}