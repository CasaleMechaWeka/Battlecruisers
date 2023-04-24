using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Tutorial.Explanation;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Manager
{
    public interface IPvPUIManager
    {
        void HideCurrentlyShownMenu();
        void SelectBuildingGroup(BuildingCategory buildingCategory);
        void SelectBuildingFromMenu(IBuildableWrapper<IBuilding> buildingWrapper);
        void SelectBuilding(IBuilding building);
        void ShowFactoryUnits(IFactory factory);
        void ShowUnitDetails(IUnit unit);
        void ShowCruiserDetails(ICruiser cruiser);
        void HideItemDetails();
        void PeakBuildingDetails(IBuilding building);
        void PeakUnitDetails(IUnit unit);
        void UnpeakUnitDetails();
        void UnpeakBuildingDetails();

        void SetExplanationPanel(IExplanationPanel explanationPanelValue);
    }
}

