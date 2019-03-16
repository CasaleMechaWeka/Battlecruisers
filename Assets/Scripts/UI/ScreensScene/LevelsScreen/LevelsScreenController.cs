using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Commands;
using BattleCruisers.UI.Common;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
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

		public void Initialise(IScreensSceneGod screensSceneGod, IList<LevelInfo> levels, int numOfLevelsUnlocked, int lastPlayedLevelNum)
        {
            base.Initialise(screensSceneGod);

            int numOfSets = levels.Count / SET_SIZE;
            InitialiseLevelSets(screensSceneGod, levels, numOfLevelsUnlocked, numOfSets);
			
			_nextSetCommand = new Command(NextSetCommandExecute, CanNextSetCommandExecute);
            nextSetButton.Initialise(_nextSetCommand);

            _previousSetCommand = new Command(PreviousSetCommandExecute, CanPreviousSetCommandExecute);
            previousSetButton.Initialise(_previousSetCommand);

            NavigationFeedbackButtonsPanel navigationFeedbackButtonsPanel = GetComponentInChildren<NavigationFeedbackButtonsPanel>();
            Assert.IsNotNull(navigationFeedbackButtonsPanel);
            navigationFeedbackButtonsPanel.Initialise(this, numOfSets);

            VisibleSetIndex = (lastPlayedLevelNum - 1) / SET_SIZE;
            ShowSet(VisibleSetIndex);

            HomeButtonController homeButton = GetComponentInChildren<HomeButtonController>();
            Assert.IsNotNull(homeButton);
            homeButton.Initialise(screensSceneGod);
        }

        private void InitialiseLevelSets(IScreensSceneGod screensSceneGod, IList<LevelInfo> levels, int numOfLevelsUnlocked, int numOfSets)
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
                levelsSet.Initialise(screensSceneGod, setLevels, numOfLevelsUnlocked);
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
