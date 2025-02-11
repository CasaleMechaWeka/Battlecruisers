using BattleCruisers.UI.Filters;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation
{
    public class PvPNavigationPermitters
    {
        public CompositeBroadcastingFilter NavigationFilter { get; }
        public BroadcastingFilter NavigationButtonsFilter { get; }
        public BroadcastingFilter ScrollWheelAndPinchZoomFilter { get; }
        public BroadcastingFilter SwipeFilter { get; }
        public BroadcastingFilter HotkeyFilter { get; }

        public PvPNavigationPermitters()
        {
            NavigationButtonsFilter = new BroadcastingFilter(isMatch: true);
            ScrollWheelAndPinchZoomFilter = new BroadcastingFilter(isMatch: true);
            SwipeFilter = new BroadcastingFilter(isMatch: true);
            HotkeyFilter = new BroadcastingFilter(isMatch: true);
            NavigationFilter = new CompositeBroadcastingFilter(true, NavigationButtonsFilter, ScrollWheelAndPinchZoomFilter, SwipeFilter, HotkeyFilter);
        }
    }
}