using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Helpers
{
    public class PvPPlayerBCruiserHelper : PvPCruiserHelper
    {
        public PvPPlayerBCruiserHelper(/*IPvPUIManager uIManager, IPvPCameraFocuser cameraFocuser*/)
            : base(/*uIManager, cameraFocuser*/)
        {
        }

        public override void FocusCameraOnCruiser()
        {
            _cameraFocuser.FocusOnAICruiser();
        }

        public override void OnBuildingConstructionStarted(IPvPBuilding buildingStarted, IPvPSlotAccessor slotAccessor, IPvPSlotHighlighter slotHighlighter)
        {
            // Do nothing.  Buttons should only be shown for player cruiser :)
        }
    }
}
