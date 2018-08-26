using System.Collections.Generic;
using BattleCruisers.Data;
using BattleCruisers.Scenes;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    // FELIX  Remove.  Crete UI via inspector :)
    public interface IUIFactory
	{
		void CreateLevelButton(HorizontalOrVerticalLayoutGroup buttonParent, ILevel level, bool isLevelUnlocked, IScreensSceneGod screensSceneGod);
        void CreateNavigationFeedbackButton(HorizontalOrVerticalLayoutGroup buttonParent, LevelsScreenController levelsScreenController, int setIndex);
		LevelsSetController CreateLevelsSet(IScreensSceneGod screensSceneGod, LevelsScreenController levelsScreen, UIFactory uiFactory, IList<ILevel> levels, int numOfLevelsUnlocked);
	}

	public class UIFactory : MonoBehaviour, IUIFactory
	{
        public LevelButtonController levelButtonPrefab;
        public LevelsSetController levelsSetPrefab;
        public NavigationFeedbackButtonController navigationFeedabckButtonPrefab;

		public void CreateLevelButton(HorizontalOrVerticalLayoutGroup buttonParent, ILevel level, bool isLevelUnlocked, IScreensSceneGod screensSceneGod)
		{
            LevelButtonController levelButton = Instantiate(levelButtonPrefab);
			levelButton.transform.SetParent(buttonParent.transform, worldPositionStays: false);
            levelButton.Initialise(level, isLevelUnlocked, screensSceneGod);
		}

        public void CreateNavigationFeedbackButton(HorizontalOrVerticalLayoutGroup buttonParent, LevelsScreenController levelsScreenController, int setIndex)
        {
            NavigationFeedbackButtonController navigationFeedbackButton = Instantiate(navigationFeedabckButtonPrefab);
            navigationFeedbackButton.transform.SetParent(buttonParent.transform, worldPositionStays: false);
            navigationFeedbackButton.Initialise(levelsScreenController, setIndex);
        }
		
		public LevelsSetController CreateLevelsSet(IScreensSceneGod screensSceneGod, LevelsScreenController levelsScreen, UIFactory uiFactory, IList<ILevel> levels, int numOfLevelsUnlocked)
		{
			LevelsSetController levelsSet = Instantiate(levelsSetPrefab);
			levelsSet.transform.SetParent(levelsScreen.transform, worldPositionStays: false);
			levelsSet.Initialise(screensSceneGod, uiFactory, levels, numOfLevelsUnlocked);
			return levelsSet;
		}
    }
}
