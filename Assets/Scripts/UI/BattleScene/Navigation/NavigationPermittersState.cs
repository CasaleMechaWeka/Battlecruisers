namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class NavigationPermittersState
    {
        public bool CompositeNavigationFilter { get; }
        public bool NavigationButtonsFilter { get; }
        public bool ScrollWheelAndPinchZoomFilter { get; }
        public bool SwipeFilter { get; }

        public NavigationPermittersState(
            bool compositeNavigationFilter, 
            bool navigationButtonsFilter, 
            bool scrollWheelAndPinchZoomFilter, 
            bool swipeFilter)
        {
            CompositeNavigationFilter = compositeNavigationFilter;
            NavigationButtonsFilter = navigationButtonsFilter;
            ScrollWheelAndPinchZoomFilter = scrollWheelAndPinchZoomFilter;
            SwipeFilter = swipeFilter;
        }
    }
}