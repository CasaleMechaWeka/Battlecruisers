using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    public enum ComparisonState
    {
        NotComparing,   // Single item details visible, compare button visible
        ReadyToCompare, // Single item details visible, no compare button
        Comparing       // Two item details visible, no compare button
    }

    // FELIX  IBroadcastingProperty, but no setter :/  Hmmmm
    public interface IComparisonStateTracker
    {
        ComparisonState State { get; }

        event EventHandler StateChanged;
    }
}