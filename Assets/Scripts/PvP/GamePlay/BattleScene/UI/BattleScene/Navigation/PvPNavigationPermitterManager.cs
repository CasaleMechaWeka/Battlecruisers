using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation
{
    public class PvPNavigationPermitterManager : IPvPNavigationPermitterManager
    {
        private readonly PvPNavigationPermitters _navigationPermitters;

        public PvPNavigationPermitterManager(PvPNavigationPermitters navigationPermitters)
        {
            Assert.IsNotNull(navigationPermitters);
            _navigationPermitters = navigationPermitters;
        }

        public PvPNavigationPermittersState PauseNavigation()
        {
            PvPNavigationPermittersState prePauseState
                = new PvPNavigationPermittersState(
                    _navigationPermitters.NavigationFilter.IsMatch,
                    _navigationPermitters.NavigationButtonsFilter.IsMatch,
                    _navigationPermitters.ScrollWheelAndPinchZoomFilter.IsMatch,
                    _navigationPermitters.SwipeFilter.IsMatch);

            _navigationPermitters.NavigationFilter.IsMatch = false;

            return prePauseState;
        }

        public void RestoreNavigation(PvPNavigationPermittersState state)
        {
            Assert.IsNotNull(state);

            _navigationPermitters.NavigationFilter.IsMatch = state.CompositeNavigationFilter;
            _navigationPermitters.NavigationButtonsFilter.IsMatch = state.NavigationButtonsFilter;
            _navigationPermitters.ScrollWheelAndPinchZoomFilter.IsMatch = state.ScrollWheelAndPinchZoomFilter;
            _navigationPermitters.SwipeFilter.IsMatch = state.SwipeFilter;
        }
    }
}