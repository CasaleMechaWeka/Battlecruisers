using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Repairables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.UI.Sound.Players;
using System;
using Unity.Netcode;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Buttons
{
    public class PvPRepairButtonController : PvPCanvasGroupButton
    {
        private IPvPDroneFocuser _droneFocuser;
        private IPvPRepairManager _repairManager;

        private IPvPTarget _repairable;
        public IPvPTarget Repairable
        {
            private get { return _repairable; }
            set
            {
                if (_repairable != null)
                {
                    _repairable.RepairCommand.CanExecuteChanged -= RepairCommand_CanExecuteChanged;
                }

                _repairable = value;

                if (_repairable != null)
                {
                    _repairable.RepairCommand.CanExecuteChanged += RepairCommand_CanExecuteChanged;
                    UpdateVisibility();
                }
            }
        }

        // Only show repair button for user repairables, not opponent repairables
        private bool ShowRepairButton
        {
            get
            {
                return
                    Repairable != null && SynchedServerData.Instance != null &&
                      (SynchedServerData.Instance.GetTeam() == Cruisers.Team.LEFT ? Repairable.Faction == Faction.Blues : Repairable.Faction == Faction.Reds) &&
                      Repairable.RepairCommand.CanExecute;
            }
        }

        public void Initialise(ISingleSoundPlayer soundPlayer, IPvPDroneFocuser droneFocuser, IPvPRepairManager repairManager)
        {
            base.Initialise(soundPlayer);

            PvPHelper.AssertIsNotNull(droneFocuser, repairManager);
            _droneFocuser = droneFocuser;
            _repairManager = repairManager;
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            if (NetworkManager.Singleton.IsServer)
            {
                IPvPDroneConsumer repairDroneConsumer = _repairManager.GetDroneConsumer(Repairable);
                _droneFocuser.ToggleDroneConsumerFocus(repairDroneConsumer, isTriggeredByPlayer: true);
            }
            else if (!NetworkManager.Singleton.IsHost)
                _repairable.clickedRepairButton?.Invoke();
        }

        private void RepairCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            gameObject.SetActive(ShowRepairButton);
        }
    }
}
