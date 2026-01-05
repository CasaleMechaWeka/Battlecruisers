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
    public class LevelButtonController : ElementWithClickSound
    {
        private LevelInfo _level;
        private ScreensSceneGod _screensSceneGod;

        public Text levelNumberText, levelNameText;
        public LevelStatsController levelStatsController;
        public Image captainImage, backgroundImage;
        public Image hullImage; // Image component for displaying the enemy cruiser hull
        public Image skyImage; // Image component for displaying the sky background
        
        private const string SKY_SPRITE_ROOT_PATH = "Assets/Resources_moved/Sprites/Skies/";
        private const string SPRITES_FILE_EXTENSION = ".png";

        public async Task Initialise(
            SingleSoundPlayer soundPlayer,
            LevelInfo level,
            ScreensSceneGod screensSceneGod,
            Sprite[] difficultyIndicators,
            int numOfLevelsUnlocked,
            TrashTalkData trashTalkData,
            IDismissableEmitter parent)
        {
            Debug.Log($"LevelButton {level.Num}: Initialise START - script enabled={enabled}, gameObject active={gameObject.activeInHierarchy}");
            base.Initialise(soundPlayer, parent: parent);

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
            // Ensure backgroundImage can receive clicks (acts as the click target)
            if (backgroundImage != null)
                backgroundImage.raycastTarget = true;

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
            else
            {
                Debug.LogWarning($"Level {level.Num}: Level index {levelIndex} is out of range for StaticData.Levels");
            }
            
            levelStatsController.Initialise(level.DifficultyCompleted, difficultyIndicators);

            Enabled = numOfLevelsUnlocked >= level.Num;
            Debug.Log($"LevelButton {level.Num}: Initialise END - Enabled={Enabled}, script enabled={enabled}, backgroundImage={backgroundImage}, raycastTarget={backgroundImage?.raycastTarget}");
        }

        protected override void OnClicked()
        {
            Debug.Log($"LevelButton {_level?.Num}: OnClicked!");
            base.OnClicked();
            ApplicationModel.Mode = GameMode.Campaign;
            _screensSceneGod.GoToTrashScreen(_level.Num);
        }

        public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
        {
            Debug.Log($"LevelButton {_level?.Num}: OnPointerDown! enabled={enabled}");
            base.OnPointerDown(eventData);
        }

        protected override void ShowDisabledState()
        {
            // Hide visual components but keep GameObject active so it can still receive events if re-enabled
            SetComponentsEnabled(false);
        }

        protected override void ShowEnabledState()
        {
            SetComponentsEnabled(true);

            captainImage.color = Color.black;
            levelNumberText.color = Color.white;
            levelNameText.color = Color.white;
            levelStatsController.SetColour(Color.white);
        }

        protected override void ShowClickedState()
        {
            SetComponentsEnabled(true);

            captainImage.color = Color.red;
            levelNumberText.color = Color.red;
            levelNameText.color = Color.red;
            levelStatsController.SetColour(Color.red);
        }

        protected override void ShowHoverState()
        {
            ShowEnabledState();
            captainImage.color = Color.white;
        }

        private void SetComponentsEnabled(bool isEnabled)
        {
            levelNumberText.enabled = isEnabled;
            levelNameText.enabled = isEnabled;
            levelStatsController.enabled = isEnabled;
            if (backgroundImage != null)
                backgroundImage.enabled = isEnabled;
            if (hullImage != null)
                hullImage.enabled = isEnabled;
            if (skyImage != null)
                skyImage.enabled = isEnabled;
        }
    }
}
