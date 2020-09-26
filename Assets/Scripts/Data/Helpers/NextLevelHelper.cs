using BattleCruisers.Data.Models;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Helpers
{
    // FELIX  test
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
            if (_appModel.SelectedLevel != ApplicationModel.DEFAULT_SELECTED_LEVEL)
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

                return nextLevelToShow;
            }

            return 1;
        }
    }
}