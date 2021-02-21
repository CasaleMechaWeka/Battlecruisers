using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Utils.BattleScene
{
    public class BattleCompletionHandlerTests
    {
        private IBattleCompletionHandler _battleCompletionHandler;
        private IApplicationModel _applicationModel;
        private ISceneNavigator _sceneNavigator;
        private IBroadcastingFilter _helpLabelVisibilityFilter;
        private int _battleCompletedCount;

        [SetUp]
        public void TestSetup()
        {
            _applicationModel = Substitute.For<IApplicationModel>();
            _sceneNavigator = Substitute.For<ISceneNavigator>();
            _helpLabelVisibilityFilter = Substitute.For<IBroadcastingFilter>();

            _battleCompletionHandler = new BattleCompletionHandler(_applicationModel, _sceneNavigator, _helpLabelVisibilityFilter);

            _battleCompletedCount = 0;
            _battleCompletionHandler.BattleCompleted += (sender, e) => _battleCompletedCount++;

            _helpLabelVisibilityFilter.IsMatch.Returns(true);
        }

        [Test]
        public void CompleteBattle_IsTutorial_Retry()
        {
            _applicationModel.Mode = GameMode.Tutorial;

            _battleCompletionHandler.CompleteBattle(wasVictory: default, retryLevel: true);

            CommonChecks();
            _sceneNavigator.Received().GoToScene(SceneNames.BATTLE_SCENE);
        }

        [Test]
        public void CompleteBattle_IsTutorial_DoNotRetry()
        {
            _applicationModel.Mode = GameMode.Tutorial;

            _battleCompletionHandler.CompleteBattle(wasVictory: default, retryLevel: false);

            CommonChecks();
            _sceneNavigator.Received().GoToScene(SceneNames.SCREENS_SCENE);
        }

        [Test]
        public void CompleteBattle_Campaign_IsVictory_DoNotRetry()
        {
            _applicationModel.Mode = GameMode.Campaign;
            _applicationModel.SelectedLevel = 77;

            _battleCompletionHandler.CompleteBattle(wasVictory: true, retryLevel: false);

            BattleResult expectedResult = new BattleResult(_applicationModel.SelectedLevel, wasVictory: true);
            _applicationModel.DataProvider.GameModel.Received().LastBattleResult = expectedResult;
            CommonChecks();
            _sceneNavigator.Received().GoToScene(SceneNames.SCREENS_SCENE);
        }

        [Test]
        public void CompleteBattle_Skirmish_IsDefeat_DoNotRetry()
        {
            _applicationModel.Mode = GameMode.Skirmish;

            _battleCompletionHandler.CompleteBattle(wasVictory: false, retryLevel: false);

            _applicationModel.Received().UserWonSkirmish = false;
            CommonChecks();
            _sceneNavigator.Received().GoToScene(SceneNames.SCREENS_SCENE);
        }

        private void CommonChecks()
        {
            Assert.AreEqual(1, _battleCompletedCount);
            _applicationModel.Received().ShowPostBattleScreen = true;
            _applicationModel.DataProvider.GameModel.Received().ShowHelpLabels = _helpLabelVisibilityFilter.IsMatch;
            _applicationModel.DataProvider.Received().SaveGame();
        }

        [Test]
        public void CompleteBattle_SecondTime_Ignored()
        {
            _battleCompletionHandler.CompleteBattle(wasVictory: false, retryLevel: false);
            Assert.AreEqual(1, _battleCompletedCount);

            _battleCompletionHandler.CompleteBattle(wasVictory: false, retryLevel: false);
            Assert.AreEqual(1, _battleCompletedCount);
        }
    }
}