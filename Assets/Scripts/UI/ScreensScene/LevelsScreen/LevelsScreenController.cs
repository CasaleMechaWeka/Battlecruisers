using BattleCruisers.Scenes;
using BattleCruisers.UI.Commands;
using BattleCruisers.UI.Common;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class LevelsScreenController : ScreenController
	{
        private IList<IGameObject> _levelSets;
        private ICommand _nextSetCommand, _previousSetCommand;

        public ButtonController nextSetButton, previousSetButton;

        private const int SET_SIZE = 7;

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

            int numOfSets = levels.Count / SET_SIZE;
            await InitialiseLevelSetsAsync(screensSceneGod, levels, numOfLevelsUnlocked, numOfSets, difficultySpritesProvider);
			
			_nextSetCommand = new Command(NextSetCommandExecute, CanNextSetCommandExecute);
            nextSetButton.Initialise(_soundPlayer, _nextSetCommand);

            _previousSetCommand = new Command(PreviousSetCommandExecute, CanPreviousSetCommandExecute);
            previousSetButton.Initialise(_soundPlayer, _previousSetCommand);

            NavigationFeedbackButtonsPanel navigationFeedbackButtonsPanel = GetComponentInChildren<NavigationFeedbackButtonsPanel>();
            Assert.IsNotNull(navigationFeedbackButtonsPanel);
            navigationFeedbackButtonsPanel.Initialise(this, numOfSets);

            VisibleSetIndex = (lastPlayedLevelNum - 1) / SET_SIZE;
            ShowSet(VisibleSetIndex);

            HomeButtonController homeButton = GetComponentInChildren<HomeButtonController>();
            Assert.IsNotNull(homeButton);
            homeButton.Initialise(_soundPlayer, screensSceneGod, this);
        }

        private async Task InitialiseLevelSetsAsync(
            IScreensSceneGod screensSceneGod, 
            IList<LevelInfo> levels, 
            int numOfLevelsUnlocked, 
            int numOfSets,
            IDifficultySpritesProvider difficultySpritesProvider)
        {
            Assert.IsTrue(levels.Count % SET_SIZE == 0);

            LevelsSetController[] levelSets = GetComponentsInChildren<LevelsSetController>();

            Assert.AreEqual(numOfSets, levelSets.Length);
            _levelSets = new List<IGameObject>(numOfSets);

            for (int j = 0; j < numOfSets; j++)
            {
                int startIndex = j * SET_SIZE;
                IList<LevelInfo> setLevels
                    = levels
                        .Skip(startIndex)
                        .Take(SET_SIZE)
                        .ToList();

                LevelsSetController levelsSet = levelSets[j];
                await levelsSet.InitialiseAsync(screensSceneGod, setLevels, numOfLevelsUnlocked, _soundPlayer, difficultySpritesProvider);
                levelsSet.IsVisible = false;
                _levelSets.Add(levelsSet);
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

        public void GoHome()
        {
            _screensSceneGod.GoToHomeScreen();
        }
	}
}
