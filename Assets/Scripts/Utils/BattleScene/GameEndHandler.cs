using BattleCruisers.AI;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound.Wind;
using BattleCruisers.Utils.Threading;
using System.Linq;
using UnityCommon.PlatformAbstractions.Time;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.BattleScene
{
    public class GameEndHandler : IGameEndHandler
    {
        private readonly ICruiser _playerCruiser, _aiCruiser;
        private readonly IArtificialIntelligence _ai;
        private readonly IBattleCompletionHandler _battleCompletionHandler;
        private readonly IDeferrer _deferrer;
        private readonly ICruiserDeathCameraFocuser _cameraFocuser;
        private readonly IPermitter _navigationPermitter;
        private readonly ITime _time;
        private readonly IUIManager _uiManager;
        private readonly ITargetIndicator _targetIndicator;
        private readonly IWindManager _windManager;
        private readonly IBuildingCategoryPermitter _buildingCategoryPermitter;
        private readonly IPermitter _helpLabelsPermitter;

        private bool _handledCruiserDeath, _handledGameEnd;

        private const float POST_GAME_WAIT_TIME_IN_S = 10;

        public GameEndHandler(
            ICruiser playerCruiser, 
            ICruiser aiCruiser, 
            IArtificialIntelligence ai, 
            IBattleCompletionHandler battleCompletionHandler, 
            IDeferrer deferrer,
            ICruiserDeathCameraFocuser cameraFocuser, 
            IPermitter navigationPermitter,
            ITime time,
            IUIManager uiManager,
            ITargetIndicator targetIndicator,
            IWindManager windManager,
            IBuildingCategoryPermitter buildingCategoryPermitter,
            IPermitter helpLabelsPermitter)
        {
            Helper.AssertIsNotNull(
                playerCruiser, 
                aiCruiser, 
                ai, 
                battleCompletionHandler, 
                deferrer, 
                cameraFocuser, 
                navigationPermitter, 
                time, 
                uiManager, 
                targetIndicator,
                windManager,
                helpLabelsPermitter);

            _playerCruiser = playerCruiser;
            _aiCruiser = aiCruiser;
            _ai = ai;
            _battleCompletionHandler = battleCompletionHandler;
            _deferrer = deferrer;
            _cameraFocuser = cameraFocuser;
            _navigationPermitter = navigationPermitter;
            _time = time;
            _uiManager = uiManager;
            _targetIndicator = targetIndicator;
            _windManager = windManager;
            _buildingCategoryPermitter = buildingCategoryPermitter;
            _helpLabelsPermitter = helpLabelsPermitter;

            _handledCruiserDeath = false;
            _handledGameEnd = false;
        }

        public void HandleCruiserDestroyed(bool wasPlayerVictory)
        {
            Assert.IsFalse(_handledCruiserDeath, "Should only be called once.");
            Assert.IsFalse(_handledGameEnd, "Should never be called after the game has ended.");
            _handledCruiserDeath = true;

            ICruiser victoryCruiser = wasPlayerVictory ? _playerCruiser : _aiCruiser;
            ICruiser losingCruiser = wasPlayerVictory ? _aiCruiser : _playerCruiser;

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
            _helpLabelsPermitter.IsMatch = false;

            // Want to play cruiser sinking animation in real time, regardless of time player has set
            _time.TimeScale = 1;

            _deferrer.Defer(() => _battleCompletionHandler.CompleteBattle(wasPlayerVictory), POST_GAME_WAIT_TIME_IN_S);
        }

        private void DestroyCruiserBuildables(ICruiser cruiser)
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
        private void StopAllShips(ICruiser victoryCruiser)
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
        }
    }
}