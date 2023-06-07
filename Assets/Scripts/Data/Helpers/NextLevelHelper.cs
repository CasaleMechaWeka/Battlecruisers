using BattleCruisers.Data.Models;
using UnityEngine.Assertions;
using UnityEngine;


namespace BattleCruisers.Data.Helpers
{
    public class NextLevelHelper : INextLevelHelper
    {
        private readonly IApplicationModel _appModel;

        public NextLevelHelper(IApplicationModel appModel)
        {
            Assert.IsNotNull(appModel);
            _appModel = appModel;
        }

        public int FindNextLevel()
        {
            if (_appModel.SelectedLevel >= 32)
            {
                Debug.Log("_appModel.SelectedLevel: " + _appModel.SelectedLevel);
                int nextLevelToShow = 1;
                Debug.Log("nextLevelToShow: " + nextLevelToShow);
                return nextLevelToShow;
            }

            if (_appModel.SelectedLevel != GameModel.UNSET_SELECTED_LEVEL)
            {
                return _appModel.SelectedLevel;
            }

            BattleResult lastBattleResult = _appModel.DataProvider.GameModel.LastBattleResult;
            if (lastBattleResult != null)
            {
                int nextLevelToShow = lastBattleResult.LevelNum;

                if (lastBattleResult.WasVictory
                    && nextLevelToShow < _appModel.DataProvider.LockedInfo.NumOfLevelsUnlocked)
                {
                    nextLevelToShow++;
                }

                Debug.Log("nextLevelToShow: " + nextLevelToShow);
                return nextLevelToShow;
            }

            return 1;
        }

    }
}