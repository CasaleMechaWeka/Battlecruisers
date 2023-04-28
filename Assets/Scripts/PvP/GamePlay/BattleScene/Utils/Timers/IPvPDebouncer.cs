using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Timers
{
    public interface IPvPDebouncer
    {
        void Debounce(Action action);
    }
}