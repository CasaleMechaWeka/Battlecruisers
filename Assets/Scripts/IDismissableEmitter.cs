using System;

namespace BattleCruisers
{
    // FELIX  Move to UI namespace :)
    public interface IDismissableEmitter
    {
        event EventHandler Dismissed;
    }
}
