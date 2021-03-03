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
using BattleCruisers.Utils.Threading;
using System.Threading.Tasks;

namespace BattleCruisers.Scenes.BattleScene
{
    public abstract class BattleSceneHelper : IBattleSceneHelper
    {
        private readonly IPrefabFetcher _prefabFetcher;
        protected readonly IBackgroundStatsProvider _backgroundStatsProvider;

        protected readonly IApplicationModel _appModel;
        protected IDataProvider DataProvider => _appModel.DataProvider;

        public abstract bool ShowInGameHints { get; }
        public abstract IBuildingCategoryPermitter BuildingCategoryPermitter { get; }
        public virtual IPrefabKey PlayerCruiser => _appModel.DataProvider.GameModel.PlayerLoadout.Hull;

        protected BattleSceneHelper(IApplicationModel appModel, IPrefabFetcher prefabFetcher)
        {
            Helper.AssertIsNotNull(appModel, prefabFetcher);

            _appModel = appModel;
            _prefabFetcher = prefabFetcher;
            _backgroundStatsProvider = new BackgroundStatsProvider(_prefabFetcher);
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

        public virtual async Task<string> GetEnemyNameAsync(int levelNum)
        {
            ITrashTalkProvider trashTalkProvider = new TrashTalkProvider(_prefabFetcher);
            ITrashTalkData levelTrashTalkData = await trashTalkProvider.GetTrashTalkAsync(levelNum);
            return levelTrashTalkData.EnemyName.ToUpper();
        }

        public virtual async Task<IPrefabContainer<BackgroundImageStats>> GetBackgroundStatsAsync(int levelNum)
        {
            return await _backgroundStatsProvider.GetStatsAsync(levelNum);
        }

        public virtual IPrefabKey GetAiCruiserKey()
        {
            return _appModel.DataProvider.GetLevel(_appModel.SelectedLevel).Hull;
        }
    }
}