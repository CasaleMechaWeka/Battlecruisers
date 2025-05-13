using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Navigation;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Helpers
{
    public class PvPPlayerCruiserHelper : PvPCruiserHelper
    {
        public PvPPlayerCruiserHelper(PvPUIManager uIManager, ICameraFocuser cameraFocuser)
            : base(uIManager, cameraFocuser)
        {
        }

        public override void FocusCameraOnCruiser(bool isOwner, Team team)
        {
            if (isOwner)
            {
                if (team == Team.LEFT)
                    _cameraFocuser.FocusOnLeftCruiser();
                else
                    _cameraFocuser.FocusOnRightCruiser();
            }
            else
            {
                if (team == Team.LEFT)
                    _cameraFocuser.FocusOnRightCruiser();
                else
                    _cameraFocuser.FocusOnLeftCruiser();
            }
        }

        public PvPPlayerCruiserHelper(/*PvPUIManager uIManager, ICameraFocuser cameraFocuser*/)
    : base(/*uIManager, cameraFocuser*/)
        {

        }


        public override void OnBuildingConstructionStarted(IPvPBuilding buildingStarted, PvPSlotAccessor slotAccessor, PvPSlotHighlighter slotHighlighter)
        {
            if (!slotAccessor.IsSlotAvailableForPlayer(buildingStarted.SlotSpecification))
            {
                _uiManager?.HideCurrentlyShownMenu();
            }
            else
            {
                // Unhighlight the one slot that has just been taken
                slotHighlighter.HighlightAvailableSlots(buildingStarted.SlotSpecification);
            }
        }
    }
}
