using System;
using System.Collections.Generic;
using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Commands;
using BattleCruisers.UI.Common;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class LevelsScreenController : ScreenController
	{
        private IList<LevelsSetController> _levelSets;
        private ICommand _nextSetCommand, _previousSetCommand;

        public ButtonController nextSetButton, previousSetButton;
        public HorizontalOrVerticalLayoutGroup navigationFeedbackButtonsWrapper;

        private const int SET_SIZE = 7;

        private LevelsSetController VisibleLevelsSet { get { return _levelSets[VisibleSetIndex]; } }

        private int _visibleSetIndex;
        public int VisibleSetIndex 
        { 
            get { return _visibleSetIndex; }
            private set
            {
                _visibleSetIndex = value;

                _nextSetCommand.EmitCanExecuteChanged();
                _previousSetCommand.EmitCanExecuteChanged();

                if (VisibleSetChanged != null)
                {
                    VisibleSetChanged.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler VisibleSetChanged;

		public void Initialise(IScreensSceneGod screensSceneGod, IList<ILevel> levels, int numOfLevelsUnlocked, int lastPlayedLevelNum)
        {
            base.Initialise(screensSceneGod);

            UIFactory uiFactory = GetComponent<UIFactory>();
            Assert.IsNotNull(uiFactory);

            CreateLevelSets(screensSceneGod, levels, numOfLevelsUnlocked, uiFactory);
			
			_nextSetCommand = new Command(NextSetCommandExecute, CanNextSetCommandExecute);
            nextSetButton.Initialise(_nextSetCommand);

            _previousSetCommand = new Command(PreviousSetCommandExecute, CanPreviousSetCommandExecute);
            previousSetButton.Initialise(_previousSetCommand);

            VisibleSetIndex = (lastPlayedLevelNum - 1) / SET_SIZE;
            ShowSet(VisibleSetIndex);
        }

        private void CreateLevelSets(IScreensSceneGod screensSceneGod, IList<ILevel> levels, int numOfLevelsUnlocked, UIFactory uiFactory)
        {
            Assert.IsTrue(levels.Count % SET_SIZE == 0);

            int numOfSets = levels.Count / SET_SIZE;
            _levelSets = new List<LevelsSetController>(numOfSets);

            for (int j = 0; j < numOfSets; j++)
            {
                IList<ILevel> setLevels = new List<ILevel>(SET_SIZE);

                for (int i = 0; i < SET_SIZE; ++i)
                {
                    setLevels.Add(levels[j * SET_SIZE + i]);
                }

                LevelsSetController levelsSet = uiFactory.CreateLevelsSet(screensSceneGod, this, uiFactory, setLevels, numOfLevelsUnlocked);
                levelsSet.gameObject.SetActive(false);
                _levelSets.Add(levelsSet);

                uiFactory.CreateNavigationFeedbackButton(navigationFeedbackButtonsWrapper, this, j);
            }
        }

        public void ShowSet(int setIndex)
        {
            Assert.IsTrue(setIndex >= 0 && setIndex < _levelSets.Count);

            VisibleLevelsSet.gameObject.SetActive(false);
            VisibleSetIndex = setIndex;
            VisibleLevelsSet.gameObject.SetActive(true);
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
