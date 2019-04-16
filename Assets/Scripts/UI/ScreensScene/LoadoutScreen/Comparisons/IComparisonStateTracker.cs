using UnityCommon.Properties;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons
{
    public enum ComparisonState
    {
        NotComparing,   // Single item details visible, compare button visible
        ReadyToCompare, // Single item details visible, no compare button
        Comparing       // Two item details visible, no compare button
    }

    public interface IComparisonStateTracker
    {
        IBroadcastingProperty<ComparisonState> State { get; }
    }
}