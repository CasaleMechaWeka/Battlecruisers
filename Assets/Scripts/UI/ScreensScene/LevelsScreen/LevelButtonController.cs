using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
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
        public Sprite defaultBackground, clickedBackground;
        public Color battlecruisersRed;

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            LevelInfo level,
            IScreensSceneGod screensSceneGod,
            Sprite[] difficultyIndicators,
            int numOfLevelsUnlocked,
            ITrashTalkData trashTalkData,
            IDismissableEmitter parent)
        {
            base.Initialise(soundPlayer, parent: parent);

            Helper.AssertIsNotNull(levelNumberText, levelNameText, levelStatsController, captainImage, targeter, defaultBackground, clickedBackground);
            Helper.AssertIsNotNull(level, screensSceneGod, difficultyIndicators, trashTalkData);

            _level = level;
            _screensSceneGod = screensSceneGod;

            levelNumberText.text = level.Num.ToString();
            levelNameText.text = trashTalkData.EnemyName;
            captainImage.sprite = trashTalkData.EnemySprite;
            levelStatsController.Initialise(level.DifficultyCompleted, difficultyIndicators);

            Enabled = numOfLevelsUnlocked >= level.Num;
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            ApplicationModel.Mode = GameMode.Campaign;
            _screensSceneGod.GoToTrashScreen(_level.Num);
        }

        protected override void ShowDisabledState()
        {
            captainImage.rectTransform.sizeDelta = new Vector2(disabledCaptainImageWidth, disabledCaptainImageWidth);
            SetEnabledState(isEnabled: false);
        }

        protected override void ShowEnabledState()
        {
            captainImage.rectTransform.sizeDelta = new Vector2(enabledCaptainImageWidth, enabledCaptainImageWidth);
            SetEnabledState(isEnabled: true);

            backgroundImage.sprite = defaultBackground;

            captainImage.color = Color.black;
            levelNumberText.color = Color.white;
            levelNameText.color = Color.white;
            levelStatsController.SetColour(Color.white);
        }

        protected override void ShowClickedState()
        {
            captainImage.rectTransform.sizeDelta = new Vector2(enabledCaptainImageWidth, enabledCaptainImageWidth);
            SetEnabledState(isEnabled: true);

            backgroundImage.sprite = clickedBackground;

            captainImage.color = battlecruisersRed;
            levelNumberText.color = battlecruisersRed;
            levelNameText.color = battlecruisersRed;
            levelStatsController.SetColour(battlecruisersRed);
        }

        protected override void ShowHoverState()
        {
            ShowEnabledState();
            captainImage.color = Color.white;
        }

        private void SetEnabledState(bool isEnabled)
        {
            levelNumberText.enabled = isEnabled;
            levelNameText.enabled = isEnabled;
            levelStatsController.enabled = isEnabled;
            backgroundImage.enabled = isEnabled;
            targeter.enabled = isEnabled;
        }
    }
}
