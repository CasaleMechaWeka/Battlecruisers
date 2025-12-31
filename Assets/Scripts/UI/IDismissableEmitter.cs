using System;

namespace BattleCruisers.UI
{
    public interface IDismissableEmitter
    {
        event EventHandler Dismissed;
    }
}
