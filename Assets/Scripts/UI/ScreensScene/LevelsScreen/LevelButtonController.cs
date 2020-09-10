using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers.Sprites;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class LevelButtonController : ElementWithClickSound
    {
        private LevelInfo _level;
        private IScreensSceneGod _screensSceneGod;

        public Text levelNumberText, levelNameText;
        public LevelStatsController levelStatsController;
        public Image captainImage, backgroundImage, targeter;
        public int enabledCaptainImageWidth = 300;
        public int disabledCaptainImageWidth = 150;

        public async Task InitialiseAsync(
            ISingleSoundPlayer soundPlayer,
            LevelInfo level, 
            IScreensSceneGod screensSceneGod, 
            IDifficultySpritesProvider difficultySpritesProvider,
            int numOfLevelsUnlocked)
		{
            base.Initialise(soundPlayer);

            Helper.AssertIsNotNull(level, screensSceneGod, difficultySpritesProvider);
            Helper.AssertIsNotNull(levelNumberText, levelNameText, levelStatsController, captainImage, targeter);

            _level = level;
            _screensSceneGod = screensSceneGod;

            levelNumberText.text = level.Num.ToString();
            levelNameText.text = level.Name;
            await levelStatsController.InitialiseAsync(level.DifficultyCompleted, difficultySpritesProvider);

            Enabled = numOfLevelsUnlocked >= level.Num;
		}

        protected override void OnClicked()
        {
            base.OnClicked();
            _screensSceneGod.LoadLevel(_level.Num);
        }

        protected override void ShowEnabledState()
        {
            captainImage.rectTransform.sizeDelta = new Vector2(enabledCaptainImageWidth, enabledCaptainImageWidth);
            SetEnabledState(isEnabled: true);
            captainImage.color = Color.black;
        }

        protected override void ShowDisabledState()
        {
            captainImage.rectTransform.sizeDelta = new Vector2(disabledCaptainImageWidth, disabledCaptainImageWidth);
            SetEnabledState(isEnabled: false);
        }

        private void SetEnabledState(bool isEnabled)
        {
            levelNumberText.enabled = isEnabled;
            levelNameText.enabled = isEnabled;
            levelStatsController.enabled = isEnabled;
            backgroundImage.enabled = isEnabled;
            targeter.enabled = isEnabled;
        }

        protected override void ShowClickedState()
        {
            captainImage.color = Color.white;
        }

        protected override void ShowHoverState()
        {
            ShowClickedState();
        }
    }
}
