using System;

namespace BattleCruisers
{
    public interface IDismissableEmitter
    {
        event EventHandler Dismissed;
    }
}
