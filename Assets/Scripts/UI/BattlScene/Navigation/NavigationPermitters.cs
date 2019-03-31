using BattleCruisers.UI.Filters;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class NavigationPermitters
    {
        public CompositeBroadcastingFilter NavigationFilter { get; }
        public BroadcastingFilter NavigationWheelFilter { get; }
        public BroadcastingFilter ScrollWheelFilter { get; }

        public NavigationPermitters()
        {
            NavigationWheelFilter = new BroadcastingFilter(isMatch: true);
            ScrollWheelFilter = new BroadcastingFilter(isMatch: true);
            NavigationFilter = new CompositeBroadcastingFilter(true, NavigationWheelFilter, ScrollWheelFilter);
        }
    }
}