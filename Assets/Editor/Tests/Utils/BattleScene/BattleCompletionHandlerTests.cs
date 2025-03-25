using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Scenes;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Utils.BattleScene
{
    public class BattleCompletionHandlerTests
    {
        private BattleCompletionHandler _battleCompletionHandler;
        private ISceneNavigator _sceneNavigator;
        private int _battleCompletedCount;

        [SetUp]
        public void TestSetup()
        {
            _sceneNavigator = Substitute.For<ISceneNavigator>();

            _battleCompletionHandler = new BattleCompletionHandler(_sceneNavigator);

            _battleCompletedCount = 0;
            _battleCompletionHandler.BattleCompleted += (sender, e) => _battleCompletedCount++;
        }

        [Test]
        public void CompleteBattle_IsTutorial_Retry()
        {
            ApplicationModel.Mode = GameMode.Tutorial;

            _battleCompletionHandler.CompleteBattle(wasVictory: default, retryLevel: true);

            CommonChecks();
            _sceneNavigator.Received().GoToScene(SceneNames.BATTLE_SCENE, true);
        }

        [Test]
        public void CompleteBattle_IsTutorial_DoNotRetry()
        {
            ApplicationModel.Mode = GameMode.Tutorial;

            _battleCompletionHandler.CompleteBattle(wasVictory: default, retryLevel: false);

            CommonChecks();
            _sceneNavigator.Received().GoToScene(SceneNames.SCREENS_SCENE, true);
        }

        [Test]
        public void CompleteBattle_Campaign_IsVictory_DoNotRetry()
        {
            ApplicationModel.Mode = GameMode.Campaign;
            ApplicationModel.SelectedLevel = 77;

            _battleCompletionHandler.CompleteBattle(wasVictory: true, retryLevel: false);

            BattleResult expectedResult = new BattleResult(ApplicationModel.SelectedLevel, wasVictory: true);
            DataProvider.GameModel.Received().LastBattleResult = expectedResult;
            CommonChecks();
            _sceneNavigator.Received().GoToScene(SceneNames.SCREENS_SCENE, true);
        }

        [Test]
        public void CompleteBattle_Skirmish_IsDefeat_DoNotRetry()
        {
            ApplicationModel.Mode = GameMode.Skirmish;

            _battleCompletionHandler.CompleteBattle(wasVictory: false, retryLevel: false);

            ApplicationModel.UserWonSkirmish = false;
            CommonChecks();
            _sceneNavigator.Received().GoToScene(SceneNames.SCREENS_SCENE, true);
        }

        private void CommonChecks()
        {
            Assert.AreEqual(1, _battleCompletedCount);
            ApplicationModel.ShowPostBattleScreen = true;
            DataProvider.SaveGame();
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