using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;

namespace BattleCruisers.UI.BattleScene
{
    public abstract class ModalManager
    {
        private readonly NavigationPermitterManager _navigationPermitterManager;
        protected readonly PauseGameManager _pauseGameManager;

        private NavigationPermittersState _stateOnShowMenu;

        protected ModalManager(
            NavigationPermitterManager navigationPermitterManager,
            PauseGameManager pauseGameManager)
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