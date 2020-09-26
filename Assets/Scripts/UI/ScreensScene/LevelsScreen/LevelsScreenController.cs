using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Commands;
using BattleCruisers.UI.Common;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers.Sprites;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class LevelsScreenController : ScreenController
	{
        private IList<LevelsSetController> _levelSets;
        private ICommand _nextSetCommand, _previousSetCommand;
        private int _numOfLevelsUnlocked;
        private IApplicationModel _appModel;

        public ButtonController nextSetButton, previousSetButton;
        public ActionButton cancelButton;

        private LevelsSetController VisibleLevelsSet => _levelSets[VisibleSetIndex];

        private int _visibleSetIndex;
        public int VisibleSetIndex 
        { 
            get { return _visibleSetIndex; }
            private set
            {
                _visibleSetIndex = value;

                _nextSetCommand.EmitCanExecuteChanged();
                _previousSetCommand.EmitCanExecuteChanged();

                VisibleSetChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler VisibleSetChanged;

		public async Task InitialiseAsync(
            ISingleSoundPlayer soundPlayer,
            IScreensSceneGod screensSceneGod,
            IList<LevelInfo> levels, 
            int numOfLevelsUnlocked, 
            IDifficultySpritesProvider difficultySpritesProvider,
            ITrashTalkDataList trashDataList,
            IApplicationModel appModel)
        {
            base.Initialise(soundPlayer, screensSceneGod);

            Helper.AssertIsNotNull(nextSetButton, previousSetButton, cancelButton);
            Helper.AssertIsNotNull(levels, difficultySpritesProvider, trashDataList, appModel);

            _numOfLevelsUnlocked = numOfLevelsUnlocked;
            _appModel = appModel;

            await InitialiseLevelSetsAsync(screensSceneGod, levels, numOfLevelsUnlocked, difficultySpritesProvider, trashDataList);

            _nextSetCommand = new Command(NextSetCommandExecute, CanNextSetCommandExecute);
            nextSetButton.Initialise(_soundPlayer, _nextSetCommand);

            _previousSetCommand = new Command(PreviousSetCommandExecute, CanPreviousSetCommandExecute);
            previousSetButton.Initialise(_soundPlayer, _previousSetCommand);

            cancelButton.Initialise(_soundPlayer, Cancel);
        }

        private async Task InitialiseLevelSetsAsync(
            IScreensSceneGod screensSceneGod, 
            IList<LevelInfo> levels, 
            int numOfLevelsUnlocked, 
            IDifficultySpritesProvider difficultySpritesProvider,
            ITrashTalkDataList trashDataList)
        {
            LevelsSetController[] levelSets = GetComponentsInChildren<LevelsSetController>();

            _levelSets = new List<LevelsSetController>(levelSets.Length);

            for (int j = 0; j < levelSets.Length; j++)
            {
                LevelsSetController levelsSet = levelSets[j];
                await levelsSet.InitialiseAsync(screensSceneGod, this, levels, numOfLevelsUnlocked, _soundPlayer, difficultySpritesProvider, trashDataList, setIndex: j);
                levelsSet.IsVisible = false;
                _levelSets.Add(levelsSet);
            }
        }

        public override void OnPresenting(object activationParameter)
        {
            base.OnPresenting(activationParameter);

            int levelNumToShow = FindLevelNumToShow();
            ShowLastPlayedLevelSet(_levelSets, levelNumToShow);
        }

        private int FindLevelNumToShow()
        {
            if (_appModel.SelectedLevel != ApplicationModel.DEFAULT_SELECTED_LEVEL)
            {
                return _appModel.SelectedLevel;
            }

            BattleResult lastBattleResult = _appModel.DataProvider.GameModel.LastBattleResult;
            if (lastBattleResult != null)
            {
                // FELIX  Avoid duplicate code with HomeScreenController
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

        private void ShowLastPlayedLevelSet(IList<LevelsSetController> levelSets, int levelToShow)
        {
            int levelSetToShow = 0;

            foreach (LevelsSetController levelSet in levelSets)
            {
                if (levelSet.ContainsLevel(levelToShow))
                {
                    levelSetToShow = levelSet.SetIndex;
                    break;
                }
            }

            ShowSet(levelSetToShow);
        }

        public void ShowSet(int setIndex)
        {
            Assert.IsTrue(setIndex >= 0 && setIndex < _levelSets.Count);

            VisibleLevelsSet.IsVisible = false;
            VisibleSetIndex = setIndex;
            VisibleLevelsSet.IsVisible = true;
        }

		private void NextSetCommandExecute()
		{
			ShowSet(VisibleSetIndex + 1);
		}

		private bool CanNextSetCommandExecute()
		{
            return
                VisibleSetIndex < _levelSets.Count - 1
                && VisibleLevelsSet.LastLevelNum < _numOfLevelsUnlocked;
		}

        private void PreviousSetCommandExecute()
        {
            ShowSet(VisibleSetIndex - 1);
        }

        private bool CanPreviousSetCommandExecute()
        {
            return VisibleSetIndex > 0;
        }

        public override void Cancel()
        {
            _screensSceneGod.GoToHomeScreen();
        }
    }
}
