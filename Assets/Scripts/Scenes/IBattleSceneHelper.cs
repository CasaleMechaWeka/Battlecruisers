using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data.Models;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Manager;

namespace BattleCruisers.Scenes
{
    public interface IBattleSceneHelper
    {
        IUIManager CreateUIManager(IManagerArgs args);
        ILoadout GetPlayerLoadout();
        void CreateAI(ICruiserController aiCruiser, ICruiserController playerCruiser, int currentLevelNum);

        // FELIX  Rename
        IBuildableButtonActivenessDecider<IBuildable> CreateButtonActivenessDecider(IDroneManager droneManager);
        IBuildableButtonActivenessDecider<BuildingCategory> CreateCategoryButtonActivenessDecider();
    }
}
