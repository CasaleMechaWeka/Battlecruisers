using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers
{
    public interface IPvPTargetTracker : IPvPManagedDisposable
    {
        event EventHandler TargetsChanged;

        bool ContainsTarget(IPvPTarget target);
    }
}