using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene
{
    public abstract class PvPModalManager
    {
        private readonly IPvPNavigationPermitterManager _navigationPermitterManager;
        protected readonly IPvPPauseGameManager _pauseGameManager;

        private PvPNavigationPermittersState _stateOnShowMenu;

        protected PvPModalManager(
            IPvPNavigationPermitterManager navigationPermitterManager,
            IPvPPauseGameManager pauseGameManager)
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