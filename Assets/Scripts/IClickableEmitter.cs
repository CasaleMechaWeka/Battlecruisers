using System;

namespace BattleCruisers
{
    // FELIX  Move to UI namespace :)
    public interface IClickableEmitter
    {
        event EventHandler Clicked;
    }
}
