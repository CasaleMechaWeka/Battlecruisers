using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Explanation;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager
{
    public interface IPvPUIManager
    {
        void HideCurrentlyShownMenu();
        void SelectBuildingGroup(PvPBuildingCategory buildingCategory);
        void SelectBuildingFromMenu(IPvPBuildableWrapper<IPvPBuilding> buildingWrapper);
        void SelectBuilding(IPvPBuilding building);
        void ShowFactoryUnits(IPvPFactory factory);
        void ShowUnitDetails(IPvPUnit unit);
        void ShowCruiserDetails(IPvPCruiser cruiser);
        void HideItemDetails();
        void PeakBuildingDetails(IPvPBuilding building);
        void PeakUnitDetails(IPvPUnit unit);
        void UnpeakUnitDetails();
        void UnpeakBuildingDetails();

        // void SetExplanationPanel(IPvPExplanationPanel explanationPanelValue);
    }
}

