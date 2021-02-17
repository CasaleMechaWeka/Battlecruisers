using BattleCruisers.AI;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.BattleScene
{
    public abstract class BattleSceneHelper : IBattleSceneHelper
    {
        protected readonly IApplicationModel _appModel;

        public abstract bool ShowInGameHints { get; }
        public abstract IBuildProgressCalculator PlayerCruiserBuildProgressCalculator { get; }
        public abstract IBuildProgressCalculator AICruiserBuildProgressCalculator { get; }
        public abstract IBuildingCategoryPermitter BuildingCategoryPermitter { get; }

        public abstract IArtificialIntelligence CreateAI(ICruiserController aiCruiser, ICruiserController playerCruiser, int currentLevelNum);
        public abstract IButtonVisibilityFilters CreateButtonVisibilityFilters(IDroneManager droneManager);
        public abstract IManagedDisposable CreateDroneEventSoundPlayer(ICruiser playerCruiser, IDeferrer deferrer);
        public abstract ISlotFilter CreateHighlightableSlotFilter();
        public abstract IUIManager CreateUIManager();
        public abstract IUserChosenTargetHelper CreateUserChosenTargetHelper(IUserChosenTargetManager playerCruiserUserChosenTargetManager, IPrioritisedSoundPlayer soundPlayer, ITargetIndicator targetIndicator);
        public abstract IPrioritisedSoundPlayer GetBuildableButtonSoundPlayer(ICruiser playerCruiser);
        public abstract ILoadout GetPlayerLoadout();
        public abstract void InitialiseUIManager(ManagerArgs args);

        protected BattleSceneHelper(IApplicationModel appModel)
        {
            Assert.IsNotNull(appModel);
            _appModel = appModel;
        }

        public virtual ILevel GetLevel()
        {
            // FELIX  Different for skirmish :)
            return _appModel.DataProvider.GetLevel(_appModel.SelectedLevel);
        }
    }
}