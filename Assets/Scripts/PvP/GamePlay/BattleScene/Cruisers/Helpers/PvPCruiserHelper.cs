using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Helpers
{
    public abstract class PvPCruiserHelper : IPvPCruiserHelper
    {
        protected readonly IPvPUIManager _uiManager;
        protected readonly IPvPCameraFocuser _cameraFocuser;

        protected PvPCruiserHelper( IPvPUIManager uIManager, IPvPCameraFocuser cameraFocuser)
        {
             PvPHelper.AssertIsNotNull(uIManager, cameraFocuser);

             _uiManager = uIManager;
             _cameraFocuser = cameraFocuser;
        }

        protected PvPCruiserHelper(/*IPvPUIManager uIManager, IPvPCameraFocuser cameraFocuser*/)
        {
        //    PvPHelper.AssertIsNotNull(uIManager /*, cameraFocuser*/);

        //    _uiManager = uIManager;
        //    _cameraFocuser = cameraFocuser;
        }

        public abstract void FocusCameraOnCruiser();
        public abstract void OnBuildingConstructionStarted(IPvPBuilding buildingStarted, IPvPSlotAccessor slotAccessor, IPvPSlotHighlighter slotHighlighter);
    }
}
