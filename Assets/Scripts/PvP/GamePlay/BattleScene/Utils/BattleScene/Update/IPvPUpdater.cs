using System;
using BattleCruisers.Utils.PlatformAbstractions.Time;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update
{
    public interface IPvPUpdater : IDeltaTimeProvider
    {
        event EventHandler Updated;
    }
}