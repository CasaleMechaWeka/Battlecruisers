namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation
{
    public class PvPNavigationPermittersState
    {
        public bool CompositeNavigationFilter { get; }
        public bool NavigationButtonsFilter { get; }
        public bool ScrollWheelAndPinchZoomFilter { get; }
        public bool SwipeFilter { get; }

        public PvPNavigationPermittersState(
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