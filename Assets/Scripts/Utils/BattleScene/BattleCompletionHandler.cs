using System;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils.PlatformAbstractions.Time;

namespace BattleCruisers.Utils.BattleScene
{
    public class BattleCompletionHandler : IBattleCompletionHandler
    {
        private readonly IApplicationModel _applicationModel;
        private readonly ISceneNavigator _sceneNavigator;
        private readonly IBroadcastingFilter _helpLabelsVisibilityFilter;
        private bool _isCompleted;

        public event EventHandler BattleCompleted;

        public BattleCompletionHandler(
            IApplicationModel applicationModel, 
            ISceneNavigator sceneNavigator,
            IBroadcastingFilter helpLabelsVisibilityFilter)
        {
            Helper.AssertIsNotNull(applicationModel, sceneNavigator, helpLabelsVisibilityFilter);

            _applicationModel = applicationModel;
            _sceneNavigator = sceneNavigator;
            _helpLabelsVisibilityFilter = helpLabelsVisibilityFilter;

            _isCompleted = false;
        }

        public void CompleteBattle(bool wasVictory, bool retryLevel)
        {
            if (_isCompleted)
            {
                // Battle should only be completed once
                return;
            }
            _isCompleted = true;

            BattleCompleted?.Invoke(this, EventArgs.Empty);

            switch (_applicationModel.Mode)
            {
                case GameMode.Campaign:
                    // Completing the tutorial does not count as a real level, so 
                    // only save battle result if this was not the tutorial.
                    BattleResult battleResult = new BattleResult(_applicationModel.SelectedLevel, wasVictory);
                    _applicationModel.DataProvider.GameModel.LastBattleResult = battleResult;
                    break;

                case GameMode.Skirmish:
                    _applicationModel.UserWonSkirmish = wasVictory;
                    break;
            }

            // FELIX  Remove
            //_applicationModel.DataProvider.GameModel.ShowHelpLabels = _helpLabelsVisibilityFilter.IsMatch;
            _applicationModel.DataProvider.SaveGame();

            _applicationModel.ShowPostBattleScreen = true;
            TimeBC.Instance.TimeScale = 1;

            if (retryLevel)
            {
                _sceneNavigator.GoToScene(SceneNames.BATTLE_SCENE);
            }
            else
            {
                _sceneNavigator.GoToScene(SceneNames.SCREENS_SCENE);
            }
        }
    }
}