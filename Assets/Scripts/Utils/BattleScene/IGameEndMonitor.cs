using System;

namespace BattleCruisers.Utils.BattleScene
{
    public interface IGameEndMonitor
    {
        event EventHandler GameEnded;
    }
}