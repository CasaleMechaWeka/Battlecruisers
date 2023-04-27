using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Update
{
    public interface IPvPUpdater : IPvPDeltaTimeProvider
    {
        event EventHandler Updated;
    }
}