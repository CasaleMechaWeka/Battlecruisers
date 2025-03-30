using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Navigation;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Helpers
{
    public class PvPPlayerBCruiserHelper : PvPCruiserHelper
    {
        public PvPPlayerBCruiserHelper(IPvPUIManager uIManager, ICameraFocuser cameraFocuser)
            : base(uIManager, cameraFocuser)
        {
        }

        public PvPPlayerBCruiserHelper(/*IPvPUIManager uIManager , ICameraFocuser cameraFocuser*/)
    : base(/*uIManager , cameraFocuser*/)
        {
        }

        public override void FocusCameraOnCruiser(bool isOwner, Team team)
        {
            _cameraFocuser.FocusOnRightCruiser();
        }

        public override void OnBuildingConstructionStarted(IPvPBuilding buildingStarted, PvPSlotAccessor slotAccessor, PvPSlotHighlighter slotHighlighter)
        {
            // Do nothing.  Buttons should only be shown for player cruiser :)
        }
    }
}
