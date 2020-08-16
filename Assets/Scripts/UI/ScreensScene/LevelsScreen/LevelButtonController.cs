using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers.Sprites;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class LevelButtonController : ElementWithClickSound
    {
        private LevelInfo _level;
        private IScreensSceneGod _screensSceneGod;

        public Text levelNumberText;
		public Text levelNameText;
        public LevelStatsController levelStatsController;

		private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;

        public async Task InitialiseAsync(
            ISingleSoundPlayer soundPlayer,
            LevelInfo level, 
            IScreensSceneGod screensSceneGod, 
            IDifficultySpritesProvider difficultySpritesProvider,
            int numOfLevelsUnlocked)
		{
            base.Initialise(soundPlayer);

            Helper.AssertIsNotNull(level, screensSceneGod, difficultySpritesProvider);
            Helper.AssertIsNotNull(levelNumberText, levelNameText, levelStatsController);

            _level = level;
            _screensSceneGod = screensSceneGod;

            levelNumberText.text = level.Num.ToString();
            levelNameText.text = level.Name;
            await levelStatsController.InitialiseAsync(level.DifficultyCompleted, difficultySpritesProvider);

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);

            Enabled = numOfLevelsUnlocked >= level.Num;
		}

        protected override void OnClicked()
        {
            base.OnClicked();
            _screensSceneGod.LoadLevel(_level.Num);
        }
    }
}
