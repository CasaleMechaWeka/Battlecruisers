using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Helpers
{
    public class PvPPlayerACruiserHelper : PvPCruiserHelper
    {
        public PvPPlayerACruiserHelper(/* IPvPUIManager uIManager, IPvPCameraFocuser cameraFocuser*/)
            : base(/* uIManager, cameraFocuser*/)
        {
        }

        public override void FocusCameraOnCruiser()
        {
            _cameraFocuser.FocusOnLeftPlayerCruiser();
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
