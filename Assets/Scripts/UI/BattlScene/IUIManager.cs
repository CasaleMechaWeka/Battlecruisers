using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;

namespace BattleCruisers.UI.BattleScene
{
    public interface IUIManager
	{
        void ShowBuildingGroups();
        void SelectBuildingGroup(BuildingCategory buildingCategory);
        void SelectBuildingFromMenu(IBuildableWrapper<IBuilding> buildingWrapper);
        void SelectBuilding(IBuilding building, ICruiser buildingParent);
        void SelectBuildingFromFriendlyCruiser(IBuilding building);
        void SelectBuildingFromEnemyCruiser(IBuilding building);
        void ShowFactoryUnits(IFactory factory);
        void ShowUnitDetails(IUnit unit);
        void ShowCruiserDetails(ICruiser cruiser);
	}
}
