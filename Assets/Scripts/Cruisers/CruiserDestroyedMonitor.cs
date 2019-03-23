using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.Threading;
using System;
using System.Linq;

namespace BattleCruisers.Cruisers
{
    // FELIX  Update tests :)
    public class CruiserDestroyedMonitor : ICruiserDestroyedMonitor
    {
        private readonly ICruiser _playerCruiser, _aiCruiser;
        private readonly IBattleCompletionHandler _battleCompletionHandler;
        private readonly IDeferrer _deferrer;
        private readonly ICameraFocuser _cameraFocuser;
        // FELIX Actually, I don't think I want to disable navigation :P
        // => Test what happens when victory while navigating
        // => Potentially revert commit :)
        private readonly BroadcastingFilter _navigationPermitter;

        private const float POST_GAME_WAIT_TIME_IN_S = 5;

        public event EventHandler<CruiserDestroyedEventArgs> CruiserDestroyed;

        public CruiserDestroyedMonitor(
            ICruiser playerCruiser,
            ICruiser aiCruiser,
            IBattleCompletionHandler battleCompletionHandler,
            IDeferrer deferrer,
            ICameraFocuser cameraFocuser,
            BroadcastingFilter navigationPermitter)
        {
            Helper.AssertIsNotNull(playerCruiser, aiCruiser, battleCompletionHandler, deferrer, cameraFocuser, navigationPermitter);

            _playerCruiser = playerCruiser;
            _aiCruiser = aiCruiser;
            _battleCompletionHandler = battleCompletionHandler;
            _deferrer = deferrer;
            _cameraFocuser = cameraFocuser;
            _navigationPermitter = navigationPermitter;

            _playerCruiser.Destroyed += _playerCruiser_Destroyed;
            _aiCruiser.Destroyed += _aiCruiser_Destroyed;
        }

        private void _playerCruiser_Destroyed(object sender, DestroyedEventArgs e)
        {
            OnCruiserDestroyed(false, _aiCruiser, _playerCruiser);
        }

        private void _aiCruiser_Destroyed(object sender, DestroyedEventArgs e)
        {
            OnCruiserDestroyed(true, _playerCruiser, _aiCruiser);
        }

        private void OnCruiserDestroyed(bool wasVictory, ICruiser victoryCruiser, ICruiser losingCruiser)
        {
            CruiserDestroyed?.Invoke(this, new CruiserDestroyedEventArgs(wasVictory));

            victoryCruiser.MakeInvincible();
            _navigationPermitter.IsMatch = false;
            FocusOnLosingCruiser(losingCruiser);
            DestroyCruiserBuildables(losingCruiser);
            StopAllShips(victoryCruiser);

            _deferrer.Defer(() => _battleCompletionHandler.CompleteBattle(wasVictory), POST_GAME_WAIT_TIME_IN_S);
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
    }
}