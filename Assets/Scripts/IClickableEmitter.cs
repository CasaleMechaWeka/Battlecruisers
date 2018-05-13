using System;

namespace BattleCruisers
{
    public interface IClickableEmitter
    {
        event EventHandler Clicked;
    }
}
