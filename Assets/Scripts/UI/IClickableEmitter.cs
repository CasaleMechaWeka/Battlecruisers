using System;

namespace BattleCruisers.UI
{
    public interface IClickableEmitter
    {
        event EventHandler Clicked;
    }
}
