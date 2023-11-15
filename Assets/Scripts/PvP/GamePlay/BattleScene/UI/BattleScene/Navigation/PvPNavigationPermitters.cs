using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation
{
    public class PvPNavigationPermitters
    {
        public PvPCompositeBroadcastingFilter NavigationFilter { get; }
        public PvPBroadcastingFilter NavigationButtonsFilter { get; }
        public PvPBroadcastingFilter ScrollWheelAndPinchZoomFilter { get; }
        public PvPBroadcastingFilter SwipeFilter { get; }
        public PvPBroadcastingFilter HotkeyFilter { get; }

        public PvPNavigationPermitters()
        {
            NavigationButtonsFilter = new PvPBroadcastingFilter(isMatch: true);
            ScrollWheelAndPinchZoomFilter = new PvPBroadcastingFilter(isMatch: true);
            SwipeFilter = new PvPBroadcastingFilter(isMatch: true);
            HotkeyFilter = new PvPBroadcastingFilter(isMatch: true);
            NavigationFilter = new PvPCompositeBroadcastingFilter(true, NavigationButtonsFilter, ScrollWheelAndPinchZoomFilter, SwipeFilter, HotkeyFilter);
        }
    }
}