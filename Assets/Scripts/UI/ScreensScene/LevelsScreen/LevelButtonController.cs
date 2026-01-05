using System.Threading.Tasks;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    [RequireComponent(typeof(Image))]
    public class LevelButtonController : ElementWithClickSound
    {
        private LevelInfo _level;
        private ScreensSceneGod _screensSceneGod;

        public Text levelNumberText, levelNameText;
        public LevelStatsController levelStatsController;
        public Image captainImage, backgroundImage, targeter;
        public Image hullImage; // Image component for displaying the enemy cruiser hull
        public Image skyImage; // Image component for displaying the sky background
        public int enabledCaptainImageWidth = 300;
        public int disabledCaptainImageWidth = 150;
        public Sprite defaultBackground, clickedBackground;
        public Color battlecruisersRed;

        private const string SKY_SPRITE_ROOT_PATH = "Assets/Resources_moved/Sprites/Skies/";
        private const string SPRITES_FILE_EXTENSION = ".png";

        private void Awake()
        {
            // Ensure the Image on this GameObject catches clicks
            Image clickCatcher = GetComponent<Image>();
            if (clickCatcher != null)
            {
                clickCatcher.raycastTarget = true;
            }
        }

        public async Task Initialise(
            SingleSoundPlayer soundPlayer,
            LevelInfo level,
            ScreensSceneGod screensSceneGod,
            Sprite[] difficultyIndicators,
            int numOfLevelsUnlocked,
            TrashTalkData trashTalkData)
        {
            base.Initialise(soundPlayer);

            Helper.AssertIsNotNull(levelNumberText, levelNameText, levelStatsController, captainImage);
            Helper.AssertIsNotNull(level, screensSceneGod, difficultyIndicators, trashTalkData);

            _level = level;
            _screensSceneGod = screensSceneGod;

            levelNumberText.text = level.Num.ToString();
            levelNameText.text = LocTableCache.StoryTable.GetString(trashTalkData.EnemyNameKey);
            captainImage.sprite = await SpriteFetcher.GetSpriteAsync(trashTalkData.EnemySpritePath);

            // Disable raycastTarget on decorative images so they don't block button clicks
            if (hullImage != null)
                hullImage.raycastTarget = false;
            if (skyImage != null)
                skyImage.raycastTarget = false;

            // Set the hull image and sky image for this level
            int levelIndex = level.Num - 1;
            if (levelIndex >= 0 && levelIndex < StaticData.Levels.Count)
            {
                Level staticLevel = StaticData.Levels[levelIndex];

                // Set the hull image
                if (hullImage != null)
                {
                    ICruiser enemyCruiserPrefab = PrefabFactory.GetCruiserPrefab(staticLevel.Hull);
                    hullImage.sprite = enemyCruiserPrefab.Sprite;
                }

                // Set the sky image
                if (skyImage != null)
                {
                    string skyPath = SKY_SPRITE_ROOT_PATH + staticLevel.SkyMaterialName + SPRITES_FILE_EXTENSION;
                    skyImage.sprite = await SpriteFetcher.GetSpriteAsync(skyPath);
                }
            }

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

            if (backgroundImage != null && defaultBackground != null)
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

            if (backgroundImage != null && clickedBackground != null)
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
            if (backgroundImage != null)
                backgroundImage.enabled = isEnabled;
            if (targeter != null)
                targeter.enabled = isEnabled;
            if (hullImage != null)
                hullImage.enabled = isEnabled;
            if (skyImage != null)
                skyImage.enabled = isEnabled;
        }
    }
}
