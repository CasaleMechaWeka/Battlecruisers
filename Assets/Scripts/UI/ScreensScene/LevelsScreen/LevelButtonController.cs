using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class LevelButtonController : MonoBehaviour 
	{
		public Button button;
		public Text levelName;
		public CanvasGroup canvasGroup;

		public void Initialise(ILevel level, bool isLevelUnlocked, IScreensSceneGod screensSceneGod)
		{
            levelName.text = level.Num + ". " + level.Name;

			if (isLevelUnlocked)
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
