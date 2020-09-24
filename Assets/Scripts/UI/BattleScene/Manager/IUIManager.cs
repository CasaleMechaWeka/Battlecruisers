using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;

namespace BattleCruisers.UI.BattleScene.Manager
{
    public interface IUIManager
	{
        void HideCurrentlyShownMenu();
        void SelectBuildingGroup(BuildingCategory buildingCategory);
        void SelectBuildingFromMenu(IBuildableWrapper<IBuilding> buildingWrapper);
        void SelectBuilding(IBuilding building);
        void ShowFactoryUnits(IFactory factory);
        void ShowUnitDetails(IUnit unit);
        void ShowCruiserDetails(ICruiser cruiser);
        void HideItemDetails();
	}
}
