using BattleCruisers.Scenes;
using BattleCruisers.UI.Commands;
using BattleCruisers.UI.Common;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Utils.PlatformAbstractions;
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
        private bool _isDemo;
        private int _numOfLevelsUnlocked;

        private const int LAST_SET_IN_DEMO_INDEX = 1; // Demo has sets 0 and 1 available

        public ButtonController nextSetButton, previousSetButton;
        public ActionButton cancelButton;
        public GameObject lockedInDemoMessage;

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
            int lastPlayedLevelNum,
            IDifficultySpritesProvider difficultySpritesProvider,
            ITrashTalkDataList trashDataList,
            bool isDemo)
        {
            base.Initialise(soundPlayer, screensSceneGod);

            Helper.AssertIsNotNull(nextSetButton, previousSetButton, cancelButton, lockedInDemoMessage);
            Helper.AssertIsNotNull(levels, difficultySpritesProvider, trashDataList);

            _isDemo = isDemo;
            _numOfLevelsUnlocked = numOfLevelsUnlocked;

            await InitialiseLevelSetsAsync(screensSceneGod, levels, numOfLevelsUnlocked, difficultySpritesProvider, trashDataList);

            _nextSetCommand = new Command(NextSetCommandExecute, CanNextSetCommandExecute);
            nextSetButton.Initialise(_soundPlayer, _nextSetCommand);

            _previousSetCommand = new Command(PreviousSetCommandExecute, CanPreviousSetCommandExecute);
            previousSetButton.Initialise(_soundPlayer, _previousSetCommand);

            NavigationFeedbackButtonsPanel navigationFeedbackButtonsPanel = GetComponentInChildren<NavigationFeedbackButtonsPanel>();
            Assert.IsNotNull(navigationFeedbackButtonsPanel);
            navigationFeedbackButtonsPanel.Initialise(this);

            cancelButton.Initialise(_soundPlayer, Cancel);

            ShowLastPlayedLevelSet(_levelSets, lastPlayedLevelNum);
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
                await levelsSet.InitialiseAsync(screensSceneGod, levels, numOfLevelsUnlocked, _soundPlayer, difficultySpritesProvider, trashDataList, setIndex: j);
                levelsSet.IsVisible = false;
                _levelSets.Add(levelsSet);
            }
        }

        private void ShowLastPlayedLevelSet(IList<LevelsSetController> levelSets, int lastPlayedLevelNum)
        {
            foreach (LevelsSetController levelSet in levelSets)
            {
                if (levelSet.ContainsLevel(lastPlayedLevelNum))
                {
                    ShowSet(levelSet.SetIndex);
                    break;
                }
            }
        }

        public void ShowSet(int setIndex)
        {
            Assert.IsTrue(setIndex >= 0 && setIndex < _levelSets.Count);

            VisibleLevelsSet.IsVisible = false;
            VisibleSetIndex = setIndex;
            VisibleLevelsSet.IsVisible = true;

            bool showLockedInDemoMessage
                = _isDemo
                    && setIndex > LAST_SET_IN_DEMO_INDEX;
            lockedInDemoMessage.SetActive(showLockedInDemoMessage);
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
