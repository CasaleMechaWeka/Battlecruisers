using System;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Scenes;
using UnityCommon.PlatformAbstractions.Time;

namespace BattleCruisers.Utils.BattleScene
{
    public class BattleCompletionHandler : IBattleCompletionHandler
    {
        private readonly IApplicationModel _applicationModel;
        private readonly ISceneNavigator _sceneNavigator;
        private bool _isCompleted;

        public event EventHandler BattleCompleted;

        public BattleCompletionHandler(IApplicationModel applicationModel, ISceneNavigator sceneNavigator)
        {
            Helper.AssertIsNotNull(applicationModel, sceneNavigator);

            _applicationModel = applicationModel;
            _sceneNavigator = sceneNavigator;
            _isCompleted = false;
        }

        public void CompleteBattle(bool wasVictory)
        {
            if (_isCompleted)
            {
                // Battle should only be completed once
                return;
            }
            _isCompleted = true;

            BattleCompleted?.Invoke(this, EventArgs.Empty);

            BattleResult battleResult = new BattleResult(_applicationModel.SelectedLevel, wasVictory);

            // FELIX  Don't save battle result if was skirmish :P
            if (!_applicationModel.IsTutorial)
            {
                // Completing the tutorial does not count as a real level, so 
                // only save battle result if this was not the tutorial.
                _applicationModel.DataProvider.GameModel.LastBattleResult = battleResult;
                _applicationModel.DataProvider.SaveGame();
            }

            _applicationModel.ShowPostBattleScreen = true;
            TimeBC.Instance.TimeScale = 1;

            _sceneNavigator.GoToScene(SceneNames.SCREENS_SCENE);
        }
    }
}