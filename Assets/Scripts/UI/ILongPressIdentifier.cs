using System;

namespace BattleCruisers.UI
{
    public interface ILongPressIdentifier
    {
        int IntervalNumber { get; }

        event EventHandler LongPressStart;
        event EventHandler LongPressEnd;
        event EventHandler LongPressInterval;
    }
}