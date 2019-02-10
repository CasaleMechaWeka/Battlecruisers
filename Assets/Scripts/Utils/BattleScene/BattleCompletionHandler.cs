using System;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Scenes;

namespace BattleCruisers.Utils.BattleScene
{
    public class BattleCompletionHandler : IBattleCompletionHandler
    {
        private readonly IApplicationModel _applicationModel;
        private readonly ISceneNavigator _sceneNavigator;

        public event EventHandler BattleCompleted;

        public BattleCompletionHandler(IApplicationModel applicationModel, ISceneNavigator sceneNavigator)
        {
            Helper.AssertIsNotNull(applicationModel, sceneNavigator);

            _applicationModel = applicationModel;
            _sceneNavigator = sceneNavigator;
        }

        public void CompleteBattle(bool wasVictory)
        {
            if (BattleCompleted != null)
            {
                BattleCompleted.Invoke(this, EventArgs.Empty);
            }

            BattleResult battleResult = new BattleResult(_applicationModel.SelectedLevel, wasVictory);

            if (!_applicationModel.IsTutorial)
            {
                // Completing the tutorial does not count as a real level, so 
                // only save save battle result if this was not the tutorial.
                _applicationModel.DataProvider.GameModel.LastBattleResult = battleResult;
                _applicationModel.DataProvider.SaveGame();
            }

            _applicationModel.ShowPostBattleScreen = true;

            _sceneNavigator.GoToScene(SceneNames.SCREENS_SCENE);
        }
    }
}