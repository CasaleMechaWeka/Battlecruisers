using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Helpers
{
    public class PvPPlayerCruiserHelper : PvPCruiserHelper
    {
        public PvPPlayerCruiserHelper( IPvPUIManager uIManager, IPvPCameraFocuser cameraFocuser)
            : base(uIManager, cameraFocuser)
        {
        }

        public override void FocusCameraOnCruiser()
        {
            if (SynchedServerData.Instance.GetTeam() == Team.LEFT)
                _cameraFocuser.FocusOnLeftPlayerCruiser();
            else
                _cameraFocuser.FocusOnRightPlayerCruiser();
        }

        public override void OnBuildingConstructionStarted(IPvPBuilding buildingStarted, IPvPSlotAccessor slotAccessor, IPvPSlotHighlighter slotHighlighter)
        {
            if (!slotAccessor.IsSlotAvailableForPlayer(buildingStarted.SlotSpecification))
            {
                _uiManager.HideCurrentlyShownMenu();
            }
            else
            {
                // Unhighlight the one slot that has just been taken
                slotHighlighter.HighlightAvailableSlots(buildingStarted.SlotSpecification);
            }
        }
    }
}
