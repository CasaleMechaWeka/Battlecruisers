using BattleCruisers.AI;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils.Threading;
using System.Linq;
using UnityCommon.PlatformAbstractions;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.BattleScene
{
    public class GameEndHandler : IGameEndHandler
    {
        private readonly ICruiser _playerCruiser, _aiCruiser;
        private readonly IArtificialIntelligence _ai;
        private readonly IBattleCompletionHandler _battleCompletionHandler;
        private readonly IDeferrer _deferrer;
        private readonly ICameraFocuser _cameraFocuser;
        private readonly IPermitter _navigationPermitter;
        private readonly ITime _time;

        private bool _handledCruiserDeath, _handledGameEnd;

        private const float POST_GAME_WAIT_TIME_IN_S = 5;

        public GameEndHandler(
            ICruiser playerCruiser, 
            ICruiser aiCruiser, 
            IArtificialIntelligence ai, 
            IBattleCompletionHandler battleCompletionHandler, 
            IDeferrer deferrer, 
            ICameraFocuser cameraFocuser, 
            IPermitter navigationPermitter,
            ITime time)
        {
            Helper.AssertIsNotNull(playerCruiser, aiCruiser, ai, battleCompletionHandler, deferrer, cameraFocuser, navigationPermitter, time);

            _playerCruiser = playerCruiser;
            _aiCruiser = aiCruiser;
            _ai = ai;
            _battleCompletionHandler = battleCompletionHandler;
            _deferrer = deferrer;
            _cameraFocuser = cameraFocuser;
            _navigationPermitter = navigationPermitter;
            _time = time;

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

            _ai.DisposeManagedState();
            victoryCruiser.MakeInvincible();
            _navigationPermitter.IsMatch = false;
            FocusOnLosingCruiser(losingCruiser);
            DestroyCruiserBuildables(losingCruiser);
            StopAllShips(victoryCruiser);

            // Want to play cruiser sinking animation in real time, regardless of time player has set
            _time.TimeScale = 1;

            _deferrer.Defer(() => _battleCompletionHandler.CompleteBattle(wasPlayerVictory), POST_GAME_WAIT_TIME_IN_S);
        }

        private void FocusOnLosingCruiser(ICruiser losingCruiser)
        {
            if (losingCruiser.IsPlayerCruiser)
            {
                _cameraFocuser.FocusOnPlayerCruiser();
            }
            else
            {
                _cameraFocuser.FocusOnAICruiser();
            }
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