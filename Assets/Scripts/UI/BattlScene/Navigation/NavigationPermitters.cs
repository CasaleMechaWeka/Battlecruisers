using BattleCruisers.UI.Filters;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class NavigationPermitters
    {
        public CompositeBroadcastingFilter NavigationFilter { get; }
        public BroadcastingFilter NavigationButtonsFilter { get; }
        public BroadcastingFilter ScrollWheelAndPinchZoomFilter { get; }
        public BroadcastingFilter SwipeFilter { get; }

        public NavigationPermitters()
        {
            NavigationButtonsFilter = new BroadcastingFilter(isMatch: true);
            ScrollWheelAndPinchZoomFilter = new BroadcastingFilter(isMatch: true);
            SwipeFilter = new BroadcastingFilter(isMatch: true);
            NavigationFilter = new CompositeBroadcastingFilter(true, NavigationButtonsFilter, ScrollWheelAndPinchZoomFilter, SwipeFilter);
        }
    }
}