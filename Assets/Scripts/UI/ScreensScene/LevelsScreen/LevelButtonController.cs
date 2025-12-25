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
            base.Initialise(soundPlayer, parent: parent);

            Helper.AssertIsNotNull(levelNumberText, levelNameText, levelStatsController, captainImage);
            Helper.AssertIsNotNull(level, screensSceneGod, difficultyIndicators, trashTalkData);

            _level = level;
            _screensSceneGod = screensSceneGod;

            levelNumberText.text = level.Num.ToString();

            // For ChainBattle levels, use the ChainBattle name instead of enemy name
            var chainBattle = StaticData.GetChainBattle(level.Num);
            if (chainBattle != null)
            {
                // For Fei ChainBattle level 32, use fallback text since localization key doesn't exist
                if (level.Num == 32 && chainBattle.levelNameKey.Contains("FEI"))
                {
                    levelNameText.text = "Stealth Raptor - Fei";
                }
                else
                {
                    levelNameText.text = LocTableCache.StoryTable.GetString(chainBattle.levelNameKey);
                }
            }
            else
            {
            levelNameText.text = LocTableCache.StoryTable.GetString(trashTalkData.EnemyNameKey);
            }
            captainImage.sprite = await SpriteFetcher.GetSpriteAsync(trashTalkData.EnemySpritePath);
            
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
        }

        protected override void OnClicked()
        {
            base.OnClicked();

            // Check if this is a ChainBattle level
            var chainBattle = StaticData.GetChainBattle(_level.Num);
            if (chainBattle != null)
            {
                // This is a ChainBattle level - use integrated BattleScene approach
                ApplicationModel.Mode = GameMode.ChainBattle;
                ApplicationModel.SelectedLevel = chainBattle.levelNumber;
                ApplicationModel.SelectedChainBattle = chainBattle;

                // Continue with normal battle loading flow (BattleSceneGod will handle ChainBattle setup)
            }
            else
            {
                // Regular campaign level
            ApplicationModel.Mode = GameMode.Campaign;
            _screensSceneGod.GoToTrashScreen(_level.Num);
            }
        }

        protected override void ShowDisabledState()
        {
            // When disabled, deactivate the entire button (captain image visibility handled separately)
            gameObject.SetActive(false);
        }

        protected override void ShowEnabledState()
        {
            // Activate the button when enabled
            gameObject.SetActive(true);

            captainImage.color = Color.black;
            levelNumberText.color = Color.white;
            levelNameText.color = Color.white;
            levelStatsController.SetColour(Color.white);
        }

        protected override void ShowClickedState()
        {
            // Ensure button is active when clicked
            gameObject.SetActive(true);

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
    }
}
