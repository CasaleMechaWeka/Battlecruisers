using BattleCruisers.Data.Helpers;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Commands;
using BattleCruisers.UI.Common;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers.Sprites;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class LevelsScreenController : ScreenController
	{
        private IList<LevelsSetController> _levelSets;
        private ICommand _nextSetCommand, _previousSetCommand;
        private int _numOfLevelsUnlocked;
        private INextLevelHelper _nextLevelHelper;

        public ButtonController nextSetButton, previousSetButton;
        public CanvasGroupButton cancelButton;

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
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            IList<LevelInfo> levels, 
            int numOfLevelsUnlocked, 
            IDifficultySpritesProvider difficultySpritesProvider,
            ITrashTalkProvider trashDataList,
            INextLevelHelper nextLevelHelper)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(nextSetButton, previousSetButton, cancelButton);
            Helper.AssertIsNotNull(levels, difficultySpritesProvider, trashDataList, nextLevelHelper);

            _numOfLevelsUnlocked = numOfLevelsUnlocked;
            _nextLevelHelper = nextLevelHelper;

            await InitialiseLevelSetsAsync(soundPlayer, screensSceneGod, levels, numOfLevelsUnlocked, difficultySpritesProvider, trashDataList);

            _nextSetCommand = new Command(NextSetCommandExecute, CanNextSetCommandExecute);
            nextSetButton.Initialise(soundPlayer, _nextSetCommand);

            _previousSetCommand = new Command(PreviousSetCommandExecute, CanPreviousSetCommandExecute);
            previousSetButton.Initialise(soundPlayer, _previousSetCommand);
            cancelButton.Initialise(soundPlayer, Cancel);
        }

        private async Task InitialiseLevelSetsAsync(
            ISingleSoundPlayer soundPlayer,
            IScreensSceneGod screensSceneGod, 
            IList<LevelInfo> levels, 
            int numOfLevelsUnlocked, 
            IDifficultySpritesProvider difficultySpritesProvider,
            ITrashTalkProvider trashDataList)
        {
            LevelsSetController[] levelSets = GetComponentsInChildren<LevelsSetController>();

            _levelSets = new List<LevelsSetController>(levelSets.Length);

            for (int j = 0; j < levelSets.Length; j++)
            {
                LevelsSetController levelsSet = levelSets[j];
                await levelsSet.InitialiseAsync(screensSceneGod, this, levels, numOfLevelsUnlocked, soundPlayer, difficultySpritesProvider, trashDataList, setIndex: j);
                levelsSet.IsVisible = false;
                _levelSets.Add(levelsSet);
            }
        }

        public override void OnPresenting(object activationParameter)
        {
            base.OnPresenting(activationParameter);

            int levelNumToShow = _nextLevelHelper.FindNextLevel();
            ShowLastPlayedLevelSet(_levelSets, levelNumToShow);
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
