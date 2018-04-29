using System;

namespace BattleCruisers
{
    public interface IClickable
    {
        event EventHandler Clicked;
    }
}
