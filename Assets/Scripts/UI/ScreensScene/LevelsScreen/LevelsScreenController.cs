using BattleCruisers.Scenes;
using BattleCruisers.UI.Commands;
using BattleCruisers.UI.Common;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Utils.PlatformAbstractions;
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

        public ButtonController nextSetButton, previousSetButton;
        public ActionButton cancelButton;

        private IGameObject VisibleLevelsSet => _levelSets[VisibleSetIndex];

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
            IDifficultySpritesProvider difficultySpritesProvider)
        {
            base.Initialise(soundPlayer, screensSceneGod);

            Helper.AssertIsNotNull(levels, difficultySpritesProvider);
            Helper.AssertIsNotNull(nextSetButton, previousSetButton, cancelButton);

            await InitialiseLevelSetsAsync(screensSceneGod, levels, numOfLevelsUnlocked, difficultySpritesProvider);

            _nextSetCommand = new Command(NextSetCommandExecute, CanNextSetCommandExecute);
            nextSetButton.Initialise(_soundPlayer, _nextSetCommand);

            _previousSetCommand = new Command(PreviousSetCommandExecute, CanPreviousSetCommandExecute);
            previousSetButton.Initialise(_soundPlayer, _previousSetCommand);

            NavigationFeedbackButtonsPanel navigationFeedbackButtonsPanel = GetComponentInChildren<NavigationFeedbackButtonsPanel>();
            Assert.IsNotNull(navigationFeedbackButtonsPanel);
            navigationFeedbackButtonsPanel.Initialise(this);

            cancelButton.Initialise(_soundPlayer, GoHome);

            ShowLastPlayedLevelSet(_levelSets, lastPlayedLevelNum);
        }

        private async Task InitialiseLevelSetsAsync(
            IScreensSceneGod screensSceneGod, 
            IList<LevelInfo> levels, 
            int numOfLevelsUnlocked, 
            IDifficultySpritesProvider difficultySpritesProvider)
        {
            LevelsSetController[] levelSets = GetComponentsInChildren<LevelsSetController>();

            _levelSets = new List<LevelsSetController>(levelSets.Length);

            for (int j = 0; j < levelSets.Length; j++)
            {
                LevelsSetController levelsSet = levelSets[j];
                await levelsSet.InitialiseAsync(screensSceneGod, levels, numOfLevelsUnlocked, _soundPlayer, difficultySpritesProvider, setIndex: j);
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
        }

		private void NextSetCommandExecute()
		{
			ShowSet(VisibleSetIndex + 1);
		}

		private bool CanNextSetCommandExecute()
		{
			return VisibleSetIndex < _levelSets.Count - 1;
		}

        private void PreviousSetCommandExecute()
        {
            ShowSet(VisibleSetIndex - 1);
        }

        private bool CanPreviousSetCommandExecute()
        {
            return VisibleSetIndex > 0;
        }

        private void GoHome()
        {
            _screensSceneGod.GoToHomeScreen();
        }
	}
}
