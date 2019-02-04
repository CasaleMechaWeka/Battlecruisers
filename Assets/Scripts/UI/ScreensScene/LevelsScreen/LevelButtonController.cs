using BattleCruisers.Scenes;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    // FELIX  Extend Togglable :P
    // FELIX  Implement IPointerClickHandler instead of public Button field :)
    public class LevelButtonController : MonoBehaviour 
	{
		public Button button;
        public Text levelNumberText;
		public Text levelNameText;
        public LevelStatsController levelStatsController;
		public CanvasGroup canvasGroup;

		public void Initialise(LevelInfo level, IScreensSceneGod screensSceneGod, int numOfLevelsUnlocked)
		{
            Helper.AssertIsNotNull(button, levelNumberText, levelNameText, levelStatsController, canvasGroup, level, screensSceneGod);

            levelNumberText.text = level.Num.ToString();
            levelNameText.text = level.Name;
            levelStatsController.Initialise(level.DifficultyCompleted);

			if (numOfLevelsUnlocked >= level.Num)
			{
                button.onClick.AddListener(() => screensSceneGod.LoadLevel(level.Num));
			}
			else
			{
				canvasGroup.alpha = Constants.DISABLED_UI_ALPHA;
			}
		}
	}
}
