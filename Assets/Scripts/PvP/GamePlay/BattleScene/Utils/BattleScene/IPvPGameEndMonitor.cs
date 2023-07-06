using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene
{
    public interface IPvPGameEndMonitor
    {
        event EventHandler GameEnded;
    }
}