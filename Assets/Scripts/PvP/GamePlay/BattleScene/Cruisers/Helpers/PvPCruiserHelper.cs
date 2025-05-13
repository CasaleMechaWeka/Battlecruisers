using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.BattleScene.Navigation;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Helpers
{
    public abstract class PvPCruiserHelper : IPvPCruiserHelper
    {
        protected readonly PvPUIManager _uiManager;
        protected readonly ICameraFocuser _cameraFocuser;

        protected PvPCruiserHelper(PvPUIManager uIManager, ICameraFocuser cameraFocuser)
        {
            PvPHelper.AssertIsNotNull(uIManager, cameraFocuser);

            _uiManager = uIManager;
            _cameraFocuser = cameraFocuser;
        }

        protected PvPCruiserHelper(/*PvPUIManager uIManager, ICameraFocuser cameraFocuser*/)
        {
            //    PvPHelper.AssertIsNotNull(uIManager /*, cameraFocuser*/);

            //    _uiManager = uIManager;
            //    _cameraFocuser = cameraFocuser;
        }

        public abstract void FocusCameraOnCruiser(bool isOwner, Team team);
        public abstract void OnBuildingConstructionStarted(IPvPBuilding buildingStarted, PvPSlotAccessor slotAccessor, PvPSlotHighlighter slotHighlighter);
    }
}
