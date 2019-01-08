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
        private IBattleCompletionHandler _battleCompletionHandler;
        private IApplicationModel _applicationModel;
        private ISceneNavigator _sceneNavigator;
        private int _battleCompletedCount;

        [SetUp]
        public void TestSetup()
        {
            _applicationModel = Substitute.For<IApplicationModel>();
            _sceneNavigator = Substitute.For<ISceneNavigator>();

            _battleCompletionHandler = new BattleCompletionHandler(_applicationModel, _sceneNavigator);

            _battleCompletedCount = 0;
            _battleCompletionHandler.BattleCompleted += (sender, e) => _battleCompletedCount++;
        }

        [Test]
        public void CompleteBattle_IsTutorial_DoesNotSaveResult()
        {
            _applicationModel.IsTutorial = true;

            _battleCompletionHandler.CompleteBattle(wasVictory: true);

            ReceivedCommonCompletion();
        }

        [Test]
        public void CompleteBattle_IsNotTutorial_SavesResult()
        {
            _applicationModel.IsTutorial = false;
            _applicationModel.SelectedLevel = 77;

            _battleCompletionHandler.CompleteBattle(wasVictory: true);

            BattleResult expectedResult = new BattleResult(_applicationModel.SelectedLevel, wasVictory: true);
            Assert.AreEqual(expectedResult, _applicationModel.DataProvider.GameModel.LastBattleResult);
            _applicationModel.DataProvider.Received().SaveGame();

            ReceivedCommonCompletion();
        }

        private void ReceivedCommonCompletion()
        {
            Assert.AreEqual(1, _battleCompletedCount);
            _applicationModel.Received().IsTutorial = false;
            _applicationModel.Received().ShowPostBattleScreen = true;
            _sceneNavigator.Received().GoToScene(SceneNames.SCREENS_SCENE);
        }
    }
}