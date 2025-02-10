using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers
{
    public interface IPvPTargetTracker : IManagedDisposable
    {
        event EventHandler TargetsChanged;

        bool ContainsTarget(ITarget target);
    }
}