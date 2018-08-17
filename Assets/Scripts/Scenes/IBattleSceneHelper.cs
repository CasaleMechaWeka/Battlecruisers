using BattleCruisers.AI;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;

namespace BattleCruisers.Scenes
{
    public interface IBattleSceneHelper
    {
        IUIManager CreateUIManager(IManagerArgs args);
        ILoadout GetPlayerLoadout();
        IArtificialIntelligence CreateAI(ICruiserController aiCruiser, ICruiserController playerCruiser, int currentLevelNum);
        ISlotFilter CreateHighlightableSlotFilter();

        IBroadcastingFilter<IBuildable> CreateBuildableButtonFilter(IDroneManager droneManager);
        IBroadcastingFilter<BuildingCategory> CreateCategoryButtonFilter();
        IFilter<IBuilding> CreateBuildingDeleteButtonFilter(ICruiser playerCruiser);
        IFilter<ITarget> CreateChooseTargetButtonVisiblityFilter();

        BasicFilter CreateBackButtonFilter();

        IBuildProgressCalculator PlayerCruiserBuildProgressCalculator { get; }
        IBuildProgressCalculator AICruiserBuildProgressCalculator { get; }
    }
}
