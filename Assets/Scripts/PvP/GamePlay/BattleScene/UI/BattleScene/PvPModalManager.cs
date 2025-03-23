using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils.BattleScene;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene
{
    public abstract class PvPModalManager
    {
        private readonly INavigationPermitterManager _navigationPermitterManager;
        protected readonly PauseGameManager _pauseGameManager;

        private NavigationPermittersState _stateOnShowMenu;

        protected PvPModalManager(
            INavigationPermitterManager navigationPermitterManager,
            PauseGameManager pauseGameManager)
        {
            PvPHelper.AssertIsNotNull(pauseGameManager, navigationPermitterManager);

            _pauseGameManager = pauseGameManager;
            _navigationPermitterManager = navigationPermitterManager;
        }

        protected PvPModalManager(
             // IPvPNavigationPermitterManager navigationPermitterManager,
             //  IPvPPauseGameManager pauseGameManager
             )
        {
            // PvPHelper.AssertIsNotNull(pauseGameManager);

            // _pauseGameManager = pauseGameManager;
            // _navigationPermitterManager = navigationPermitterManager;
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