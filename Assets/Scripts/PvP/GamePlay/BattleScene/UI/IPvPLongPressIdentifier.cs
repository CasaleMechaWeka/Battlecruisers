using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI
{
    public interface IPvPLongPressIdentifier
    {
        int IntervalNumber { get; }

        event EventHandler LongPressStart;
        event EventHandler LongPressEnd;
        event EventHandler LongPressInterval;
    }
}