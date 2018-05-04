using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public interface IBuildMenu
    {
        IBuildingCategoryButtonsPanel CategoryButtonsPanel { get; }

        void HideBuildMenu();
        void ShowBuildMenu();
        void ShowBuildingGroupsMenu();
        void ShowBuildingGroupMenu(BuildingCategory buildingCategory);
        void ShowUnitsMenu(IFactory factory);
    }
}
