using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Models;
using BattleCruisers.UI;
using BattleCruisers.UI.BattleScene.Manager;

namespace BattleCruisers.Scenes
{
    public interface IBattleSceneHelper
    {
        IUIManager CreateUIManager(IManagerArgs args);
        ILoadout GetPlayerLoadout();
        void CreateAI(ICruiserController aiCruiser, ICruiserController playerCruiser, int currentLevelNum);
        ISlotFilter CreateHighlightableSlotFilter();

        IFilter<IBuildable> CreateBuildableButtonFilter(IDroneManager droneManager);
        IFilter<BuildingCategory> CreateCategoryButtonFilter();
        IFilter<IBuilding> CreateBuildingDeleteButtonFilter(ICruiser playerCruiser);

        BasicFilter CreateNavigationFilter();
        BasicFilter CreateBackButtonFilter();

        IBuildProgressCalculator PlayerCruiserBuildProgressCalculator { get; }
        IBuildProgressCalculator AICruiserBuildProgressCalculator { get; }
    }
}
