using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders
{
    public interface IPvPBroadcastingTargetProvider : IPvPTargetProvider, IPvPManagedDisposable
    {
        event EventHandler TargetChanged;
    }
}
