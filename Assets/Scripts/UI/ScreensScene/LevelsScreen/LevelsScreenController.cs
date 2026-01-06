using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Commands;
using BattleCruisers.UI.Common;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers.Sprites;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class LevelsScreenController : ScreenController
    {
        private IList<LevelsSetController> _levelSets;
        private Command _nextSetCommand, _previousSetCommand;
        private int _numOfLevelsUnlocked;

        public ButtonController nextSetButton, previousSetButton;
        public CanvasGroupButton cancelButton;

        private LevelsSetController VisibleLevelsSet => _levelSets[VisibleSetIndex];

        private int _visibleSetIndex;
        public int VisibleSetIndex
        {
            get { return _visibleSetIndex; }
            private set
            {
                _visibleSetIndex = value;

                _nextSetCommand.EmitCanExecuteChanged();
                _previousSetCommand.EmitCanExecuteChanged();

                VisibleSetChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler VisibleSetChanged;


        public async Task InitialiseAsync(
            ScreensSceneGod screensSceneGod,
            SingleSoundPlayer soundPlayer,
            IList<LevelInfo> levels,
            int numOfLevelsUnlocked,
            Sprite[] difficultyIndicators)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(nextSetButton, previousSetButton, cancelButton);
            Helper.AssertIsNotNull(levels, difficultyIndicators);

            _numOfLevelsUnlocked = numOfLevelsUnlocked;

            await InitialiseLevelSetsAsync(soundPlayer, screensSceneGod, levels, numOfLevelsUnlocked, difficultyIndicators);

            // Programmatically assign captain images to all level and sidequest buttons
            await AssignCaptainImagesAsync(levels);

            _nextSetCommand = new Command(NextSetCommandExecute, CanNextSetCommandExecute);
            nextSetButton.Initialise(soundPlayer, _nextSetCommand);

            _previousSetCommand = new Command(PreviousSetCommandExecute, CanPreviousSetCommandExecute);
            previousSetButton.Initialise(soundPlayer, _previousSetCommand);
            cancelButton.Initialise(soundPlayer, Cancel);
        }

        private async Task InitialiseLevelSetsAsync(
            SingleSoundPlayer soundPlayer,
            ScreensSceneGod screensSceneGod,
            IList<LevelInfo> levels,
            int numOfLevelsUnlocked,
            Sprite[] difficultyIndicators)
        {
            LevelsSetController[] levelSets = GetComponentsInChildren<LevelsSetController>();

            _levelSets = new List<LevelsSetController>(levelSets.Length);

            List<Task> levelSetInitialisations = new List<Task>();

            for (int j = 0; j < levelSets.Length; j++)
                levelSetInitialisations.Add(levelSets[j].InitialiseAsync(screensSceneGod, this, levels, numOfLevelsUnlocked, soundPlayer, difficultyIndicators, setIndex: j));

            await Task.WhenAll(levelSetInitialisations);

            for (int j = 0; j < levelSets.Length; j++)
            {
                levelSets[j].IsVisible = false;
                _levelSets.Add(levelSets[j]);
            }

        }

        public override void OnPresenting(object activationParameter)
        {
            base.OnPresenting(activationParameter);

            int levelNumToShow = DataProvider.GameModel.SelectedLevel <= 31 ? DataProvider.GameModel.SelectedLevel : 1;
            Debug.Log($"Presenting LevelsScreen with levelNumToShow: {levelNumToShow}");
            ShowLastPlayedLevelSet(_levelSets, levelNumToShow);
        }

        private void ShowLastPlayedLevelSet(IList<LevelsSetController> levelSets, int levelToShow)
        {
            int levelSetToShow = 0;

            foreach (LevelsSetController levelSet in levelSets)
                if (levelSet.ContainsLevel(levelToShow))
                {
                    levelSetToShow = levelSet.SetIndex;
                    break;
                }

            ShowSet(levelSetToShow);
        }

        public void ShowSet(int setIndex)
        {
            Assert.IsTrue(setIndex >= 0 && setIndex < _levelSets.Count);

            VisibleLevelsSet.IsVisible = false;
            VisibleSetIndex = setIndex;
            VisibleLevelsSet.IsVisible = true;
        }

        private void NextSetCommandExecute()
        {
            ShowSet(VisibleSetIndex + 1);
        }

        private bool CanNextSetCommandExecute()
        {
            return
                VisibleSetIndex < _levelSets.Count - 1
                && VisibleLevelsSet.LastLevelNum < _numOfLevelsUnlocked;
        }

        private void PreviousSetCommandExecute()
        {
            ShowSet(VisibleSetIndex - 1);
        }

        private bool CanPreviousSetCommandExecute()
        {
            return VisibleSetIndex > 0;
        }

        public override void Cancel()
        {
            _screensSceneGod.GotoHubScreen();
        }

        /// <summary>
        /// Programmatically assigns captain images and hull images to all level and sidequest buttons.
        /// This fixes image references after the NPC naming convention change. 
        /// </summary>
        private async Task AssignCaptainImagesAsync(IList<LevelInfo> levels)
        {
            List<Image> spriteTargetComponents = new List<Image>();
            List<Task<Sprite>> spriteLoads = new List<Task<Sprite>>();

            // Assign images to level buttons
            foreach (LevelsSetController levelSet in _levelSets)
            {
                LevelButtonController[] levelButtons = levelSet.GetComponentsInChildren<LevelButtonController>();

                for (int i = 0; i < levelButtons.Length; i++)
                {
                    LevelButtonController button = levelButtons[i];
                    int levelIndex = levelSet.firstLevelIndex + i;

                    if(levelIndex >= levels.Count)
                    {
                        Debug.LogError($"Invalid Level: {levelIndex}");
                        continue;
                    }

                    LevelInfo level = levels[levelIndex];

                    Transform captainImageTransform = button.transform.Find("CaptainImage");
                    if (captainImageTransform == null)
                    {
                        Debug.LogWarning($"Level {level.Num}: CaptainImage child GameObject not found.");
                        continue;
                    }

                    Image captainImage = captainImageTransform.GetComponent<Image>();
                    if (captainImage == null)
                    {
                        Debug.LogWarning($"Level {level.Num}: CaptainImage GameObject found but Image component is missing.");
                        continue;
                    }

                    Task<Sprite> spriteLoad = SpriteFetcher.GetSpriteAsync(StaticData.LevelTrashTalk[level.Num].EnemySpritePath);
                    spriteTargetComponents.Add(captainImage);
                    spriteLoads.Add(spriteLoad);

                    if (button.captainImage != null && button.captainImage != captainImage)
                    {
                        spriteTargetComponents.Add(button.captainImage);
                        spriteLoads.Add(spriteLoad);
                    }
                }

                // Assign images to sidequest buttons
                SideQuestButtonController[] sideQuestButtons = levelSet.GetComponentsInChildren<SideQuestButtonController>();
            
                for (int i = 0; i < sideQuestButtons.Length; i++)
                {
                    int sideQuestID = sideQuestButtons[i].sideQuestID;
                    if (sideQuestID < 0 || sideQuestID >= StaticData.SideQuestTrashTalk.Count)
                    {
                        Debug.LogError($"Invalid SideQuestID: {sideQuestID}");   
                        continue;
                    }

                    // Find the CompleteSideQuest -> Captain child GameObject
                    Transform completeSideQuestTransform = sideQuestButtons[i].transform.Find("ButtonImages/CompleteSideQuest");
                    if(completeSideQuestTransform == null)
                    {
                        Debug.LogWarning($"SideQuest {sideQuestID}: CompleteSideQuest child GameObject not found.");
                        continue;
                    }

                    Transform captainTransform = completeSideQuestTransform.Find("Captain");
                    if (captainTransform == null)
                    {
                        Debug.LogWarning($"SideQuest {sideQuestID}: Captain child GameObject not found under CompleteSideQuest.");
                        continue;
                    }

                    Image captainImage = captainTransform.GetComponent<Image>();
                    if(captainImage == null)
                    {
                        Debug.LogWarning($"SideQuest {sideQuestID}: Captain GameObject found but Image component is missing.");
                        continue;
                    }

                    spriteTargetComponents.Add(captainImage);
                    spriteLoads.Add(SpriteFetcher.GetSpriteAsync(StaticData.SideQuestTrashTalk[sideQuestID].EnemySpritePath));
                }
            }

            await Task.WhenAll(spriteLoads);

            for(int i = 0; i < spriteTargetComponents.Count; i++)
                spriteTargetComponents[i].sprite = spriteLoads[i].Result;
        }
    }
}
