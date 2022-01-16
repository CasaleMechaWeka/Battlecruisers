using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;

namespace BattleCruisers.UI.BattleScene
{
    public abstract class ModalManager
    {
        private readonly INavigationPermitterManager _navigationPermitterManager;
        protected readonly IPauseGameManager _pauseGameManager;

        private NavigationPermittersState _stateOnShowMenu;

        protected ModalManager(
            INavigationPermitterManager navigationPermitterManager,
            IPauseGameManager pauseGameManager)
        {
            Helper.AssertIsNotNull(pauseGameManager, navigationPermitterManager);

            _pauseGameManager = pauseGameManager;
            _navigationPermitterManager = navigationPermitterManager;
        }

        protected void ShowModal()
        {
            if (_stateOnShowMenu == null)
            {
                _stateOnShowMenu = _navigationPermitterManager.PauseNavigation();
            }
            //_pauseGameManager.PauseGame();
        }

        protected void HideModal()
        {
            if (_stateOnShowMenu != null)
            {
                _navigationPermitterManager.RestoreNavigation(_stateOnShowMenu);
                _stateOnShowMenu = null;
            }
            //_pauseGameManager.ResumeGame();
        }
    }
}