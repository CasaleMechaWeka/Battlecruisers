using BattleCruisers.AI;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Ships;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Toggles;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Wind;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using BattleCruisers.Projectiles;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Buttons.Toggles;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound.Wind;
using BattleCruisers.Utils.Threading;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene
{
    public class PvPGameEndHandler : IPvPGameEndHandler
    {
        private readonly IPvPCruiser _playerACruiser, _playerBCruiser;
        private IPvPArtificialIntelligence _ai_LeftPlayer;
        private IPvPArtificialIntelligence _ai_RightPlayer;
        private readonly PvPBattleSceneGodTunnel _battleSceneGodTunnel;
        private readonly IPvPDeferrer _deferrer;
        private readonly IPvPCruiserDeathCameraFocuser _cameraFocuser;
        private readonly IPvPPermitter _navigationPermitter;
        private readonly IPvPUIManager _uiManager;
        private readonly IPvPTargetIndicator _targetIndicator;
        // private readonly IPvPWindManager _windManager;
        private readonly IPvPBuildingCategoryPermitter _buildingCategoryPermitter;
        private readonly IPvPToggleButtonGroup _speedButtonGroup;

        private bool _handledCruiserDeath, _handledGameEnd;

        private const float POST_GAME_WAIT_TIME_IN_S = 10;

        public PvPGameEndHandler(
            IPvPCruiser playerACruiser,
            IPvPCruiser playerBCruiser,
            PvPBattleSceneGodTunnel battleSceneGodTunnel,
            IPvPDeferrer deferrer
            )
        {
            PvPHelper.AssertIsNotNull(
                playerACruiser,
                playerBCruiser,
                battleSceneGodTunnel,
                deferrer
                );

            _playerACruiser = playerACruiser;
            _playerBCruiser = playerBCruiser;
            _battleSceneGodTunnel = battleSceneGodTunnel;
            _deferrer = deferrer;
            _handledCruiserDeath = false;
            _handledGameEnd = false;
        }

        public void HandleCruiserDestroyed(bool wasPlayerVictory)
        {
            Assert.IsFalse(_handledCruiserDeath, "Should only be called once.");
            Assert.IsFalse(_handledGameEnd, "Should never be called after the game has ended.");
            _handledCruiserDeath = true;

            IPvPCruiser victoryCruiser = wasPlayerVictory ? _playerACruiser : _playerBCruiser;
            IPvPCruiser losingCruiser = wasPlayerVictory ? _playerBCruiser : _playerACruiser;

            PvPBattleSceneGodServer.enemyCruiserSprite = losingCruiser.Sprite;
            PvPBattleSceneGodServer.enemyCruiserName = losingCruiser.Name;

            //---> Code by ANUJ
            ClearProjectiles();
            //<---
            _playerACruiser.FactoryProvider.Sound.PrioritisedSoundPlayer.Enabled = false;
            _ai_LeftPlayer.DisposeManagedState();
            _ai_RightPlayer.DisposeManagedState();
            victoryCruiser.MakeInvincible();

            DestroyCruiserBuildables(losingCruiser);
            StopAllShips(victoryCruiser);
            _battleSceneGodTunnel.HandleCruiserDestroyed();
            _deferrer.Defer(() => _battleSceneGodTunnel.CompleteBattle(wasPlayerVictory, retryLevel: false), POST_GAME_WAIT_TIME_IN_S);
        }

        public void HandleCruiserDestroyed(bool wasPlayerVictory, long destructionScore)
        {
            Assert.IsFalse(_handledCruiserDeath, "Should only be called once.");
            Assert.IsFalse(_handledGameEnd, "Should never be called after the game has ended.");
            _handledCruiserDeath = true;

            IPvPCruiser victoryCruiser = wasPlayerVictory ? _playerACruiser : _playerBCruiser;
            IPvPCruiser losingCruiser = wasPlayerVictory ? _playerBCruiser : _playerACruiser;

            PvPBattleSceneGodServer.enemyCruiserSprite = losingCruiser.Sprite;
            PvPBattleSceneGodServer.enemyCruiserName = losingCruiser.Name;
            //---> Code by ANUJ
            ClearProjectiles();
            //<---
            if (_ai_LeftPlayer != null)
                _ai_LeftPlayer.DisposeManagedState();
            if (_ai_RightPlayer != null)
                _ai_RightPlayer?.DisposeManagedState();
            victoryCruiser.MakeInvincible();
            DestroyCruiserBuildables(losingCruiser);
            StopAllShips(victoryCruiser);
            _battleSceneGodTunnel.HandleCruiserDestroyed();
            _deferrer.Defer(() => _battleSceneGodTunnel.CompleteBattle(wasPlayerVictory, retryLevel: false, destructionScore), POST_GAME_WAIT_TIME_IN_S);
            _deferrer.Defer(() => DestroyCruiserBuildables(victoryCruiser), POST_GAME_WAIT_TIME_IN_S);
        }



        public void RegisterAIOfLeftPlayer(IPvPArtificialIntelligence ai_LeftPlayer)
        {
            _ai_LeftPlayer = ai_LeftPlayer;
        }

        public void RegisterAIOfRightPlayer(IPvPArtificialIntelligence ai_RightPlayer)
        {
            _ai_RightPlayer = ai_RightPlayer;
        }

        private void DestroyCruiserBuildables(IPvPCruiser cruiser)
        {
            foreach (IPvPBuilding building in cruiser.BuildingMonitor.AliveBuildings.ToList())
            {
                if (!building.IsDestroyed)
                {
                    building.Destroy();
                }
            }

            foreach (IPvPUnit unit in cruiser.UnitMonitor.AliveUnits.ToList())
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
            foreach (IPvPUnit unit in victoryCruiser.UnitMonitor.AliveUnits)
            {
                if (unit is IPvPShip ship)
                {
                    ship.DisableMovement();
                    ship.StopMoving();
                }
            }
        }

        //---> Code by ANUJ
        private void ClearProjectiles()
        {
            PvPSmartMissileController[] smartMissiles = MonoBehaviour.FindObjectsOfType<PvPSmartMissileController>();
            foreach (PvPSmartMissileController missile in smartMissiles)
            {
                missile.enabled = false;
            }
        }
        //<---
        public void HandleGameEnd()
        {
            Assert.IsFalse(_handledGameEnd, "Should only be called once.");
            _handledGameEnd = true;

            if (!_handledCruiserDeath)
            {
                if (_ai_LeftPlayer != null)
                    _ai_LeftPlayer.DisposeManagedState();
                if (_ai_RightPlayer != null)
                    _ai_RightPlayer.DisposeManagedState();
            }
        }
    }
}