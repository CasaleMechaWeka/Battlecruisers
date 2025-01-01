using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.HeckleMessage;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager
{
    public interface IPvPUIManager
    {
        void HideCurrentlyShownMenu();
        void SelectBuildingGroup(BuildingCategory buildingCategory);
        void SelectBuildingFromMenu(IPvPBuildableWrapper<IPvPBuilding> buildingWrapper);
        void SelectBuilding(IPvPBuilding building);
        void ShowFactoryUnits(IPvPFactory factory);
        void HideSlotsIfCannotAffordable();
        void ShowUnitDetails(IPvPUnit unit);
        void ShowCruiserDetails(IPvPCruiser cruiser);
        void HideItemDetails();
        void PeakBuildingDetails(IPvPBuilding building);
        void PeakUnitDetails(IPvPUnit unit);
        void UnpeakUnitDetails();
        void UnpeakBuildingDetails();
        void SetHecklePanel(PvPHecklePanelController hecklePanel);
        PvPHecklePanelController hecklePanelController { get; set; }
        // void SetExplanationPanel(IPvPExplanationPanel explanationPanelValue);
    }
}

