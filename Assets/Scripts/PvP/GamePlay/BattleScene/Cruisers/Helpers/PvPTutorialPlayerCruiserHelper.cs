using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Navigation;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Helpers
{
    public class PvPTutorialPlayerCruiserHelper : PvPCruiserHelper
    {
        public PvPTutorialPlayerCruiserHelper(PvPUIManager uIManager, ICameraFocuser cameraFocuser)
            : base(uIManager, cameraFocuser)
        {
        }

        public override void FocusCameraOnCruiser(bool isClient, Team team)
        {
            // Disabled for tutorial
        }

        public override void OnBuildingConstructionStarted(IPvPBuilding buildingStarted, PvPSlotAccessor slotAccessor, PvPSlotHighlighter slotHighlighter)
        {
            _uiManager.HideCurrentlyShownMenu();
        }
    }
}
