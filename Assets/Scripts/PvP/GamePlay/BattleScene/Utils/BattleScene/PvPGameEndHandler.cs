using BattleCruisers.AI;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Toggles;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Wind;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Buttons.Toggles;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound.Wind;
using BattleCruisers.Utils.Threading;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene
{
    public class PvPGameEndHandler : IPvPGameEndHandler
    {
        private readonly IPvPCruiser _playerCruiser, _aiCruiser;
        private readonly IPvPArtificialIntelligence _ai;
        private readonly IPvPBattleCompletionHandler _battleCompletionHandler;
        private readonly IPvPDeferrer _deferrer;
        private readonly IPvPCruiserDeathCameraFocuser _cameraFocuser;
        private readonly IPvPPermitter _navigationPermitter;
        private readonly IPvPUIManager _uiManager;
        private readonly IPvPTargetIndicator _targetIndicator;
        private readonly IPvPWindManager _windManager;
        private readonly IPvPBuildingCategoryPermitter _buildingCategoryPermitter;
        private readonly IPvPToggleButtonGroup _speedButtonGroup;

        private bool _handledCruiserDeath, _handledGameEnd;

        private const float POST_GAME_WAIT_TIME_IN_S = 10;

        public PvPGameEndHandler(
            IPvPCruiser playerCruiser,
            IPvPCruiser aiCruiser,
            IPvPArtificialIntelligence ai,
            IPvPBattleCompletionHandler battleCompletionHandler,
            IPvPDeferrer deferrer,
            IPvPCruiserDeathCameraFocuser cameraFocuser,
            IPvPPermitter navigationPermitter,
            IPvPUIManager uiManager,
            IPvPTargetIndicator targetIndicator,
            IPvPWindManager windManager,
            IPvPBuildingCategoryPermitter buildingCategoryPermitter,
            IPvPToggleButtonGroup speedButtonGroup)
        {
            PvPHelper.AssertIsNotNull(
                playerCruiser,
                aiCruiser,
                ai,
                battleCompletionHandler,
                deferrer,
                cameraFocuser,
                navigationPermitter,
                uiManager,
                targetIndicator,
                windManager,
                speedButtonGroup);

            _playerCruiser = playerCruiser;
            _aiCruiser = aiCruiser;
            _ai = ai;
            _battleCompletionHandler = battleCompletionHandler;
            _deferrer = deferrer;
            _cameraFocuser = cameraFocuser;
            _navigationPermitter = navigationPermitter;
            _uiManager = uiManager;
            _targetIndicator = targetIndicator;
            _windManager = windManager;
            _buildingCategoryPermitter = buildingCategoryPermitter;
            _speedButtonGroup = speedButtonGroup;

            _handledCruiserDeath = false;
            _handledGameEnd = false;
        }

        public void HandleCruiserDestroyed(bool wasPlayerVictory)
        {
            Assert.IsFalse(_handledCruiserDeath, "Should only be called once.");
            Assert.IsFalse(_handledGameEnd, "Should never be called after the game has ended.");
            _handledCruiserDeath = true;

            IPvPCruiser victoryCruiser = wasPlayerVictory ? _playerCruiser : _aiCruiser;
            IPvPCruiser losingCruiser = wasPlayerVictory ? _aiCruiser : _playerCruiser;

            _playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer.Enabled = false;
            _ai.DisposeManagedState();
            victoryCruiser.MakeInvincible();
            _navigationPermitter.IsMatch = false;
            _cameraFocuser.FocusOnLosingCruiser(losingCruiser);
            DestroyCruiserBuildables(losingCruiser);
            StopAllShips(victoryCruiser);
            _uiManager.HideCurrentlyShownMenu();
            _uiManager.HideItemDetails();
            _targetIndicator.Hide();
            _windManager.Stop();
            _buildingCategoryPermitter.AllowNoCategories();
            // Want to play cruiser sinking animation in real time, regardless of time player has set
            _speedButtonGroup.SelectDefaultButton();

            _deferrer.Defer(() => _battleCompletionHandler.CompleteBattle(wasPlayerVictory, retryLevel: false), POST_GAME_WAIT_TIME_IN_S);
        }

        public void HandleCruiserDestroyed(bool wasPlayerVictory, long destructionScore)
        {
            Assert.IsFalse(_handledCruiserDeath, "Should only be called once.");
            Assert.IsFalse(_handledGameEnd, "Should never be called after the game has ended.");
            _handledCruiserDeath = true;

            IPvPCruiser victoryCruiser = wasPlayerVictory ? _playerCruiser : _aiCruiser;
            IPvPCruiser losingCruiser = wasPlayerVictory ? _aiCruiser : _playerCruiser;

            _playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer.Enabled = false;
            _ai.DisposeManagedState();
            victoryCruiser.MakeInvincible();
            _navigationPermitter.IsMatch = false;
            _cameraFocuser.FocusOnLosingCruiser(losingCruiser);
            DestroyCruiserBuildables(losingCruiser);
            StopAllShips(victoryCruiser);
            _uiManager.HideCurrentlyShownMenu();
            _uiManager.HideItemDetails();
            _targetIndicator.Hide();
            _windManager.Stop();
            _buildingCategoryPermitter.AllowNoCategories();
            // Want to play cruiser sinking animation in real time, regardless of time player has set
            _speedButtonGroup.SelectDefaultButton();

            _deferrer.Defer(() => _battleCompletionHandler.CompleteBattle(wasPlayerVictory, retryLevel: false, destructionScore), POST_GAME_WAIT_TIME_IN_S);
        }

        private void DestroyCruiserBuildables(IPvPCruiser cruiser)
        {
            foreach (IBuilding building in cruiser.BuildingMonitor.AliveBuildings.ToList())
            {
                if (!building.IsDestroyed)
                {
                    building.Destroy();
                }
            }

            foreach (IUnit unit in cruiser.UnitMonitor.AliveUnits.ToList())
            {
                if (!unit.IsDestroyed)
                {
                    unit.Destroy();
                }
            }
        }

        /// <summary>
        /// Prevent ships from moving, otherwise they will happily move into the
        /// sinking cruiser.  Can disregard the losing cruiser's ships, as they
        /// have been automatically destroyed.
        /// </summary>
        private void StopAllShips(IPvPCruiser victoryCruiser)
        {
            foreach (IUnit unit in victoryCruiser.UnitMonitor.AliveUnits)
            {
                if (unit is IShip ship)
                {
                    ship.DisableMovement();
                    ship.StopMoving();
                }
            }
        }

        public void HandleGameEnd()
        {
            Assert.IsFalse(_handledGameEnd, "Should only be called once.");
            _handledGameEnd = true;

            if (!_handledCruiserDeath)
            {
                _ai.DisposeManagedState();
            }

            _windManager.DisposeManagedState();
        }
    }
}