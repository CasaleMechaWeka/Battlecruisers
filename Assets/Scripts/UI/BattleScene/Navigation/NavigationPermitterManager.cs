using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class NavigationPermitterManager : INavigationPermitterManager
    {
        private readonly NavigationPermitters _navigationPermitters;

        public NavigationPermitterManager(NavigationPermitters navigationPermitters)
        {
            Assert.IsNotNull(navigationPermitters);
            _navigationPermitters = navigationPermitters;
        }

        public NavigationPermittersState PauseNavigation()
        {
            NavigationPermittersState prePauseState
                = new NavigationPermittersState(
                    _navigationPermitters.NavigationFilter.IsMatch,
                    _navigationPermitters.NavigationButtonsFilter.IsMatch,
                    _navigationPermitters.ScrollWheelAndPinchZoomFilter.IsMatch,
                    _navigationPermitters.SwipeFilter.IsMatch);

            _navigationPermitters.NavigationFilter.IsMatch = false;

            return prePauseState;
        }

        public void RestoreNavigation(NavigationPermittersState state)
        {
            Assert.IsNotNull(state);

            _navigationPermitters.NavigationFilter.IsMatch = state.CompositeNavigationFilter;
            _navigationPermitters.NavigationButtonsFilter.IsMatch = state.NavigationButtonsFilter;
            _navigationPermitters.ScrollWheelAndPinchZoomFilter.IsMatch = state.ScrollWheelAndPinchZoomFilter;
            _navigationPermitters.SwipeFilter.IsMatch = state.SwipeFilter;
        }
    }
}