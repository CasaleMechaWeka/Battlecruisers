using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders
{
    public interface IPvPBroadcastingTargetProvider : ITargetProvider, IManagedDisposable
    {
        event EventHandler TargetChanged;
    }
}
