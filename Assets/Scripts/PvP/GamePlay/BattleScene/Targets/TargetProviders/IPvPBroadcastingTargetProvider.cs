using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders
{
    public interface IPvPBroadcastingTargetProvider : IPvPTargetProvider, IManagedDisposable
    {
        event EventHandler TargetChanged;
    }
}
