using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Scenes.BattleScene
{
    public abstract class BattleSceneHelper : IBattleSceneHelper
    {
        protected readonly BuildProgressCalculatorFactory _calculatorFactory;

        public abstract bool ShowInGameHints { get; }
        public abstract BuildingCategoryFilter BuildingCategoryPermitter { get; }
        public virtual IPrefabKey PlayerCruiser => DataProvider.GameModel.PlayerLoadout.Hull;

        protected BattleSceneHelper()
        {
            _calculatorFactory
                = new BuildProgressCalculatorFactory(
                    new BuildSpeedCalculator());
        }

        public abstract IManagedDisposable CreateAI(Cruiser aiCruiser, Cruiser playerCruiser, int currentLevelNum);
        public abstract ButtonVisibilityFilters CreateButtonVisibilityFilters(DroneManager droneManager);
        public abstract IManagedDisposable CreateDroneEventSoundPlayer(ICruiser playerCruiser, IDeferrer deferrer);
        public abstract IFilter<Slot> CreateHighlightableSlotFilter();
        public abstract UIManager CreateUIManager();
        public abstract IUserChosenTargetHelper CreateUserChosenTargetHelper(IUserChosenTargetManager playerCruiserUserChosenTargetManager, IPrioritisedSoundPlayer soundPlayer, TargetIndicatorController targetIndicator);
        public abstract IPrioritisedSoundPlayer GetBuildableButtonSoundPlayer(ICruiser playerCruiser);
        public abstract Loadout GetPlayerLoadout();
        public abstract void InitialiseUIManager(ICruiser PlayerCruiser,
                                                 ICruiser AICruiser,
                                                 BuildMenu BuildMenu,
                                                 ItemDetailsManager DetailsManager,
                                                 IPrioritisedSoundPlayer SoundPlayer,
                                                 SingleSoundPlayer UISoundPlayer);
        public abstract IBuildProgressCalculator CreatePlayerCruiserBuildProgressCalculator();
        public abstract IBuildProgressCalculator CreateAICruiserBuildProgressCalculator();


        public virtual Level GetLevel()
        {
            return StaticData.Levels[ApplicationModel.SelectedLevel - 1];
        }

        public virtual SideQuestData GetSideQuest()
        {
            return StaticData.SideQuests[ApplicationModel.SelectedSideQuestID];
        }

        public virtual async Task<string> GetEnemyNameAsync(int levelNum)
        {
            TrashTalkData levelTrashTalkData;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[BattleSceneHelper] Getting enemy name for level {levelNum}, Mode: {ApplicationModel.Mode}");
#endif

            if (ApplicationModel.Mode == GameMode.SideQuest)
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.Log($"[BattleSceneHelper] Getting side quest trash talk for level {levelNum}");
#endif
                levelTrashTalkData = StaticData.SideQuestTrashTalk[levelNum];
            }
            else if (ApplicationModel.Mode == GameMode.ChainBattle)
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.Log($"[BattleSceneHelper] Getting ChainBattle trash talk for level {levelNum}");
#endif
                levelTrashTalkData = StaticData.GetChainBattleTrashTalk(ApplicationModel.SelectedChainBattle);
            }
            else
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.Log($"[BattleSceneHelper] Getting campaign trash talk for level {levelNum}");
#endif
                levelTrashTalkData = StaticData.LevelTrashTalk[levelNum];
            }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[BattleSceneHelper] Full trash talk data for {(ApplicationModel.Mode == GameMode.SideQuest ? "side quest" : "level")} {levelNum}:\n" +
                     $"  Enemy Name:         {LocTableCache.StoryTable.GetString(levelTrashTalkData.EnemyNameKey)}\n" +
                     $"  Player Text:        {LocTableCache.StoryTable.GetString(levelTrashTalkData.PlayerTextKey)}\n" +
                     $"  Enemy Text:         {LocTableCache.StoryTable.GetString(levelTrashTalkData.EnemyTextKey)}\n" +
                     $"  Player Talks First: {levelTrashTalkData.PlayerTalksFirst}\n");
#endif

            return LocTableCache.StoryTable.GetString(levelTrashTalkData.EnemyNameKey);
        }

        public virtual IPrefabKey GetAiCruiserKey()
        {
            if (ApplicationModel.Mode == GameMode.SideQuest)
                return StaticData.SideQuests[ApplicationModel.SelectedSideQuestID].Hull;
            else
                return StaticData.Levels[ApplicationModel.SelectedLevel - 1].Hull;
        }
    }
}