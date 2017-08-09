using System.Collections.Generic;
using BattleCruisers.Data;
using BattleCruisers.Scenes;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public interface IUIFactory
	{
		void CreateLevelButton(HorizontalOrVerticalLayoutGroup buttonParent, ILevel level, bool isLevelUnlocked, IScreensSceneGod screensSceneGod);
        LevelsSetController CreateLevelsSet(IScreensSceneGod screensSceneGod, LevelsScreenController levelsScreen, UIFactory uiFactory, IList<ILevel> levels, int numOfLevelsUnlocked);
	}

	public class UIFactory : MonoBehaviour, IUIFactory
	{
		public Button levelButtonPrefab;
        public LevelsSetController levelsSetPrefab;

		public void CreateLevelButton(HorizontalOrVerticalLayoutGroup buttonParent, ILevel level, bool isLevelUnlocked, IScreensSceneGod screensSceneGod)
		{
			Button levelButton = Instantiate<Button>(levelButtonPrefab);
			levelButton.transform.SetParent(buttonParent.transform, worldPositionStays: false);
			levelButton.GetComponent<LevelButtonController>().Initialise(level, isLevelUnlocked, screensSceneGod);
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
