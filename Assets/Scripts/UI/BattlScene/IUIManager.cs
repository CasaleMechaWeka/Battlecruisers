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
        void SelectBuilding(Building building, ICruiser buildingParent);
        void SelectBuildingFromFriendlyCruiser(Building building);
        void SelectBuildingFromEnemyCruiser(Building building);
        void ShowFactoryUnits(Factory factory);
        void ShowUnitDetails(IUnit unit);
	}
}