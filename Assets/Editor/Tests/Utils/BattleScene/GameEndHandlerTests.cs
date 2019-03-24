using BattleCruisers.AI;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Utils.BattleScene
{
    public class GameEndHandlerTests
    {
        private IGameEndHandler _gameEndHandler;

        private ICruiser _playerCruiser, _aiCruiser;
        private IArtificialIntelligence _ai;
        private IBattleCompletionHandler _battleCompletionHandler;
        private IDeferrer _deferrer;
        private ICameraFocuser _cameraFocuser;
        private BroadcastingFilter _navigationPermitter;

        private IBuilding _playerBuilding, _aiBuilding;
        private IShip _playerShip, _aiShip;

        [SetUp]
        public void TestSetup()
        {
            _playerCruiser = Substitute.For<ICruiser>();
            _playerCruiser.IsPlayerCruiser.Returns(true);

            _aiCruiser = Substitute.For<ICruiser>();
            _aiCruiser.IsPlayerCruiser.Returns(false);

            _ai = Substitute.For<IArtificialIntelligence>();
            _battleCompletionHandler = Substitute.For<IBattleCompletionHandler>();
            _deferrer = Substitute.For<IDeferrer>();
            _cameraFocuser = Substitute.For<ICameraFocuser>();
            _navigationPermitter = new BroadcastingFilter(isMatch: true);

            _gameEndHandler
                = new GameEndHandler(
                    _playerCruiser,
                    _aiCruiser,
                    _ai,
                    _battleCompletionHandler,
                    _deferrer,
                    _cameraFocuser,
                    _navigationPermitter);

            _deferrer.Defer(Arg.Invoke(), Arg.Any<float>());

            SetupPlayerCruiserBuildables();
            SetupAiCruiserBuildables();

            UnityAsserts.Assert.raiseExceptions = true;
        }

        private void SetupPlayerCruiserBuildables()
        {
            _playerBuilding = Substitute.For<IBuilding>();
            List<IBuilding> playerBuildings = new List<IBuilding>()
            {
                _playerBuilding
            };
            _playerCruiser.BuildingMonitor.AliveBuildings.Returns(playerBuildings.AsReadOnly());

            _playerShip = Substitute.For<IShip>();
            List<IUnit> playerUnits = new List<IUnit>()
            {
                _playerShip
            };
            _playerCruiser.UnitMonitor.AliveUnits.Returns(playerUnits.AsReadOnly());
        }

        private void SetupAiCruiserBuildables()
        {
            _aiBuilding = Substitute.For<IBuilding>();
            List<IBuilding> aiBuildings = new List<IBuilding>()
            {
                _aiBuilding
            };
            _aiCruiser.BuildingMonitor.AliveBuildings.Returns(aiBuildings.AsReadOnly());

            _aiShip = Substitute.For<IShip>();
            List<IUnit> aiUnits = new List<IUnit>()
            {
                _aiShip
            };
            _aiCruiser.UnitMonitor.AliveUnits.Returns(aiUnits.AsReadOnly());
        }

        [Test]
        public void HandleCruiserDestroyed_AiCruiserDestroyed()
        {
            ICruiser victoryCruiser = _playerCruiser;
            ICruiser losingCruiser = _aiCruiser;

            _gameEndHandler.HandleCruiserDestroyed(wasPlayerVictory: true);

            _ai.Received().DisposeManagedState();
            victoryCruiser.Received().MakeInvincible();
            Assert.IsFalse(_navigationPermitter.IsMatch);
            _cameraFocuser.Received().FocusOnAICruiser();

            // Check losing cruiser buildables were destroyed
            _aiBuilding.Received().Destroy();
            _aiShip.Received().Destroy();
            _playerBuilding.DidNotReceive().Destroy();
            _playerShip.DidNotReceive().Destroy();

            // Check victory cruiser ships were stopped
            _playerShip.Received().DisableMovement();
            _playerShip.Received().StopMoving();

            _deferrer.Received().Defer(Arg.Any<Action>(), Arg.Any<float>());
            _battleCompletionHandler.Received().CompleteBattle(wasVictory: true);
        }

        [Test]
        public void HandleCruiserDestroyed_PlayerCruiserDestroyed()
        {
            ICruiser victoryCruiser = _aiCruiser;
            ICruiser losingCruiser = _playerCruiser;

            _gameEndHandler.HandleCruiserDestroyed(wasPlayerVictory: false);

            _ai.Received().DisposeManagedState();
            victoryCruiser.Received().MakeInvincible();
            Assert.IsFalse(_navigationPermitter.IsMatch);
            _cameraFocuser.Received().FocusOnPlayerCruiser();

            // Check losing cruiser buildables were destroyed
            _aiBuilding.DidNotReceive().Destroy();
            _aiShip.DidNotReceive().Destroy();
            _playerBuilding.Received().Destroy();
            _playerShip.Received().Destroy();

            // Check victory cruiser ships were stopped
            _aiShip.Received().DisableMovement();
            _aiShip.Received().StopMoving();

            _deferrer.Received().Defer(Arg.Any<Action>(), Arg.Any<float>());
            _battleCompletionHandler.Received().CompleteBattle(wasVictory: false);
        }

        [Test]
        public void HandleCruiserDestroyed_SecondCall_Throws()
        {
            _gameEndHandler.HandleCruiserDestroyed(wasPlayerVictory: true);
            Assert.Throws<UnityAsserts.AssertionException>(() => _gameEndHandler.HandleCruiserDestroyed(wasPlayerVictory: true));
        }

        [Test]
        public void HandleCruiserDestroyed_AfterHandledGameEnd_Throws()
        {
            _gameEndHandler.HandleGameEnd();
            Assert.Throws<UnityAsserts.AssertionException>(() => _gameEndHandler.HandleCruiserDestroyed(wasPlayerVictory: true));
        }

        [Test]
        public void HandleGameEnd()
        {
            _gameEndHandler.HandleGameEnd();
            _ai.Received().DisposeManagedState();
        }

        [Test]
        public void HandleGameEnd_PreviouslyHandledCruiserDestroyed()
        {
            _gameEndHandler.HandleCruiserDestroyed(wasPlayerVictory: true);

            _ai.ClearReceivedCalls();
            _gameEndHandler.HandleGameEnd();
            _ai.DidNotReceive().DisposeManagedState();
        }

        [Test]
        public void HandleGameEnd_SecondCall_Throws()
        {
            _gameEndHandler.HandleGameEnd();
            Assert.Throws<UnityAsserts.AssertionException>(() => _gameEndHandler.HandleGameEnd());
        }
    }
}
