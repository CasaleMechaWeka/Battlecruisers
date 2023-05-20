using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Helpers
{
    public class PvPTutorialPlayerCruiserHelper : PvPCruiserHelper
    {
        public PvPTutorialPlayerCruiserHelper( /* IPvPUIManager uIManager, IPvPCameraFocuser cameraFocuser */)
            : base( /* uIManager, cameraFocuser */)
        {
        }

        public override void FocusCameraOnCruiser()
        {
            // Disabled for tutorial
        }

        public override void OnBuildingConstructionStarted(IPvPBuilding buildingStarted, IPvPSlotAccessor slotAccessor, IPvPSlotHighlighter slotHighlighter)
        {
            _uiManager.HideCurrentlyShownMenu();
        }
    }
}
