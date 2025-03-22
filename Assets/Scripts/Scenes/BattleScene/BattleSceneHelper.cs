using BattleCruisers.AI;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Clouds.Stats;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Scenes.BattleScene
{
    public abstract class BattleSceneHelper : IBattleSceneHelper
    {
        private readonly PrefabFetcher _prefabFetcher;
        private readonly ILocTable _storyStrings;
        protected readonly IBackgroundStatsProvider _backgroundStatsProvider;
        protected readonly IBuildProgressCalculatorFactory _calculatorFactory;
        private ITrashTalkProvider _trashTalkProvider;

        protected readonly IApplicationModel _appModel;
        protected IDataProvider DataProvider => _appModel.DataProvider;

        public abstract bool ShowInGameHints { get; }
        public abstract IBuildingCategoryPermitter BuildingCategoryPermitter { get; }
        public virtual IPrefabKey PlayerCruiser => _appModel.DataProvider.GameModel.PlayerLoadout.Hull;

        protected BattleSceneHelper(IApplicationModel appModel, PrefabFetcher prefabFetcher, ILocTable storyStrings)
        {
            Helper.AssertIsNotNull(appModel, prefabFetcher, storyStrings);

            _appModel = appModel;
            _prefabFetcher = prefabFetcher;
            _storyStrings = storyStrings;
            _backgroundStatsProvider = new BackgroundStatsProvider(_prefabFetcher);
            _calculatorFactory
                = new BuildProgressCalculatorFactory(
                    new BuildSpeedCalculator());
            _trashTalkProvider = new TrashTalkProvider(_prefabFetcher, _storyStrings);
        }

        public abstract IArtificialIntelligence CreateAI(ICruiserController aiCruiser, ICruiserController playerCruiser, int currentLevelNum);
        public abstract IButtonVisibilityFilters CreateButtonVisibilityFilters(IDroneManager droneManager);
        public abstract IManagedDisposable CreateDroneEventSoundPlayer(ICruiser playerCruiser, IDeferrer deferrer);
        public abstract ISlotFilter CreateHighlightableSlotFilter();
        public abstract IUIManager CreateUIManager();
        public abstract IUserChosenTargetHelper CreateUserChosenTargetHelper(IUserChosenTargetManager playerCruiserUserChosenTargetManager, IPrioritisedSoundPlayer soundPlayer, ITargetIndicator targetIndicator);
        public abstract IPrioritisedSoundPlayer GetBuildableButtonSoundPlayer(ICruiser playerCruiser);
        public abstract ILoadout GetPlayerLoadout();
        public abstract void InitialiseUIManager(ManagerArgs args);
        public abstract IBuildProgressCalculator CreatePlayerCruiserBuildProgressCalculator();
        public abstract IBuildProgressCalculator CreateAICruiserBuildProgressCalculator();


        public virtual ILevel GetLevel()
        {
            return _appModel.DataProvider.GetLevel(_appModel.SelectedLevel);
        }

        public virtual ISideQuestData GetSideQuest()
        {
            return _appModel.DataProvider.GetSideQuest(_appModel.SelectedSideQuestID);
        }

        public virtual async Task<string> GetEnemyNameAsync(int levelNum)
        {
            ITrashTalkData levelTrashTalkData;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[BattleSceneHelper] Getting enemy name for level {levelNum}, Mode: {_appModel.Mode}");
#endif

            if (_appModel.Mode == GameMode.SideQuest)
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.Log($"[BattleSceneHelper] Getting side quest trash talk for level {levelNum}");
#endif
                levelTrashTalkData = await _trashTalkProvider.GetTrashTalkAsync(levelNum, true);
            }
            else
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.Log($"[BattleSceneHelper] Getting campaign trash talk for level {levelNum}");
#endif
                levelTrashTalkData = await _trashTalkProvider.GetTrashTalkAsync(levelNum);
            }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[BattleSceneHelper] Full trash talk data for {(_appModel.Mode == GameMode.SideQuest ? "side quest" : "level")} {levelNum}:\n" +
                     $"  Enemy Name: {levelTrashTalkData.EnemyName}\n" +
                     $"  Player Text: {levelTrashTalkData.PlayerText}\n" +
                     $"  Enemy Text: {levelTrashTalkData.EnemyText}\n" +
                     $"  Player Talks First: {levelTrashTalkData.PlayerTalksFirst}\n" +
                     $"  String Key Base: {levelTrashTalkData.StringKeyBase}");
#endif

            return levelTrashTalkData.EnemyName;
        }

        public virtual async Task<PrefabContainer<BackgroundImageStats>> GetBackgroundStatsAsync(int levelNum)
        {
            return await _backgroundStatsProvider.GetStatsAsyncLevel(levelNum);
        }

        public virtual IPrefabKey GetAiCruiserKey()
        {
            if (_appModel.Mode == GameMode.SideQuest)
                return _appModel.DataProvider.GetSideQuest(_appModel.SelectedSideQuestID).Hull;
            else
                return _appModel.DataProvider.GetLevel(_appModel.SelectedLevel).Hull;
        }
    }
}