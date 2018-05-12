using System;

namespace BattleCruisers
{
    // FELIX  Rename to IClickableEmitter?
    public interface IClickable
    {
        event EventHandler Clicked;
    }
}
