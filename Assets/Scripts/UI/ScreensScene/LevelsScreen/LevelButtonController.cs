using BattleCruisers.Scenes;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class LevelButtonController : ClickableTogglable
    {
        private LevelInfo _level;
        private IScreensSceneGod _screensSceneGod;

        public Text levelNumberText;
		public Text levelNameText;
        public LevelStatsController levelStatsController;

		private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;

        public void Initialise(LevelInfo level, IScreensSceneGod screensSceneGod, int numOfLevelsUnlocked)
		{
            Helper.AssertIsNotNull(levelNumberText, levelNameText, levelStatsController, level, screensSceneGod);

            _level = level;
            _screensSceneGod = screensSceneGod;

            levelNumberText.text = level.Num.ToString();
            levelNameText.text = level.Name;
            levelStatsController.Initialise(level.DifficultyCompleted);

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);

            Enabled = numOfLevelsUnlocked >= level.Num;
		}

        protected override void OnClicked()
        {
            _screensSceneGod.LoadLevel(_level.Num);
        }
    }
}
