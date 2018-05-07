using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
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

        IFilter<IBuildable> CreateBuildableButtonActivenessDecider(IDroneManager droneManager);
        IFilter<BuildingCategory> CreateCategoryButtonActivenessDecider();
        IFilter<IBuilding> CreateBuildingDeleteButtonActivenessDecider(ICruiser playerCruiser);

        BasicDecider CreateNavigationDecider();
        BasicDecider CreateBackButtonDecider();
    }
}
