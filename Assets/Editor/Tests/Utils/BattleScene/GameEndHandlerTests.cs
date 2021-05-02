using BattleCruisers.AI;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Buttons.Toggles;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.UI.Sound.Wind;
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
        private ICruiserDeathCameraFocuser _cameraFocuser;
        private BroadcastingFilter _navigationPermitter;
        private IUIManager _uiManager;
        private ITargetIndicator _targetIndicator;
        private IPrioritisedSoundPlayer _soundPlayer;
        private IWindManager _windManager;
        private IBuildingCategoryPermitter _buildingCategoryPermitter;
        private IToggleButtonGroup _speedButtonGroup;

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
            _cameraFocuser = Substitute.For<ICruiserDeathCameraFocuser>();
            _navigationPermitter = new BroadcastingFilter(isMatch: true);
            _uiManager = Substitute.For<IUIManager>();
            _targetIndicator = Substitute.For<ITargetIndicator>();
            _windManager = Substitute.For<IWindManager>();
            _buildingCategoryPermitter = Substitute.For<IBuildingCategoryPermitter>();
            _speedButtonGroup = Substitute.For<IToggleButtonGroup>();

            _soundPlayer = Substitute.For<IPrioritisedSoundPlayer>();
            _playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer.Returns(_soundPlayer);

            _gameEndHandler
                = new GameEndHandler(
                    _playerCruiser,
                    _aiCruiser,
                    _ai,
                    _battleCompletionHandler,
                    _deferrer,
                    _cameraFocuser,
                    _navigationPermitter,
                    _uiManager,
                    _targetIndicator,
                    _windManager,
                    _buildingCategoryPermitter,
                    _speedButtonGroup);

            _deferrer.Defer(Arg.Invoke(), Arg.Any<float>());

            SetupPlayerCruiserBuildables();
            SetupAiCruiserBuildables();
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

            _soundPlayer.Received().Enabled = false;
            _ai.Received().DisposeManagedState();
            victoryCruiser.Received().MakeInvincible();
            Assert.IsFalse(_navigationPermitter.IsMatch);
            _cameraFocuser.Received().FocusOnLosingCruiser(_aiCruiser);

            // Check losing cruiser buildables were destroyed
            _aiBuilding.Received().Destroy();
            _aiShip.Received().Destroy();
            _playerBuilding.DidNotReceive().Destroy();
            _playerShip.DidNotReceive().Destroy();

            // Check victory cruiser ships were stopped
            _playerShip.Received().DisableMovement();
            _playerShip.Received().StopMoving();

            _deferrer.Received().Defer(Arg.Any<Action>(), Arg.Any<float>());
            _battleCompletionHandler.Received().CompleteBattle(wasVictory: true, retryLevel: false);
            _uiManager.Received().HideItemDetails();
            _uiManager.Received().HideCurrentlyShownMenu();
            _targetIndicator.Received().Hide();
            _windManager.Received().Stop();
            _buildingCategoryPermitter.Received().AllowNoCategories();
            _speedButtonGroup.Received().SelectDefaultButton();
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
            _cameraFocuser.Received().FocusOnLosingCruiser(_playerCruiser);

            // Check losing cruiser buildables were destroyed
            _aiBuilding.DidNotReceive().Destroy();
            _aiShip.DidNotReceive().Destroy();
            _playerBuilding.Received().Destroy();
            _playerShip.Received().Destroy();

            // Check victory cruiser ships were stopped
            _aiShip.Received().DisableMovement();
            _aiShip.Received().StopMoving();

            _deferrer.Received().Defer(Arg.Any<Action>(), Arg.Any<float>());
            _battleCompletionHandler.Received().CompleteBattle(wasVictory: false, retryLevel: false);
            _uiManager.Received().HideItemDetails();
            _uiManager.Received().HideCurrentlyShownMenu();
            _targetIndicator.Received().Hide();
            _windManager.Received().Stop();
            _buildingCategoryPermitter.Received().AllowNoCategories();
            _speedButtonGroup.Received().SelectDefaultButton();
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
