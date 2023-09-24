using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using System;
using System.Diagnostics;
using Unity.Netcode;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.UserChosen
{
    public class PvPUserChosenTargetHelper : IPvPUserChosenTargetHelper
    {
        private readonly IPvPUserChosenTargetManager _userChosenTargetManager;
        private readonly IPvPPrioritisedSoundPlayer _soundPlayer;
        private readonly IPvPTargetIndicator _targetIndicator;

        public IPvPTarget UserChosenTarget => _userChosenTargetManager.HighestPriorityTarget?.Target;

        public event EventHandler UserChosenTargetChanged;

        public PvPUserChosenTargetHelper(
            IPvPUserChosenTargetManager userChosenTargetManager
            // IPvPPrioritisedSoundPlayer soundPlayer,
            // IPvPTargetIndicator targetIndicator
            )
        {
            PvPHelper.AssertIsNotNull(userChosenTargetManager /*, soundPlayer, targetIndicator*/);

            _userChosenTargetManager = userChosenTargetManager;
            // _soundPlayer = soundPlayer;
            // _targetIndicator = targetIndicator;

            _userChosenTargetManager.HighestPriorityTargetChanged += _userChosenTargetManager_HighestPriorityTargetChanged;
        }


        public PvPUserChosenTargetHelper(
            IPvPUserChosenTargetManager userChosenTargetManager,
            IPvPPrioritisedSoundPlayer soundPlayer,
            IPvPTargetIndicator targetIndicator
    )
        {
            PvPHelper.AssertIsNotNull(userChosenTargetManager, soundPlayer, targetIndicator);

            _userChosenTargetManager = userChosenTargetManager;
            _soundPlayer = soundPlayer;
            _targetIndicator = targetIndicator;

            _userChosenTargetManager.HighestPriorityTargetChanged += _userChosenTargetManager_HighestPriorityTargetChanged;
        }

        private void _userChosenTargetManager_HighestPriorityTargetChanged(object sender, EventArgs e)
        {
            // Logging.Log(Tags.USER_CHOSEN_TARGET, $"Highest priority target: {_userChosenTargetManager.HighestPriorityTarget}");

            UserChosenTargetChanged?.Invoke(this, EventArgs.Empty);

            if (_userChosenTargetManager.HighestPriorityTarget == null)
            {
                if (_targetIndicator != null)
                    _targetIndicator.Hide();
            }
        }

        public void ToggleChosenTarget(IPvPTarget target)
        {
            if (ReferenceEquals(UserChosenTarget, target))
            {
                // Clear user chosen target
                _userChosenTargetManager.Target = null;
                _soundPlayer.PlaySound(PvPPrioritisedSoundKeys.PvPEvents.PvPTargetting.TargetCleared);
                _targetIndicator.Hide();
                SynchedServerData.Instance.SetUserChosenTargetToServer(false, 0, SynchedServerData.Instance.GetTeam());
            }
            else
            {
                // Set user chosen target
                _userChosenTargetManager.Target = target;
                _soundPlayer.PlaySound(PvPPrioritisedSoundKeys.PvPEvents.PvPTargetting.NewTarget);
                _targetIndicator.Show(target.Position);

                ulong objectId;
                if (target.GameObject.GetComponent<PvPBuilding>() != null)
                {
                    if (target.GameObject.GetComponent<PvPBuilding>()._parent.GetComponent<NetworkObject>() != null)
                    {
                        objectId = (ulong)target.GameObject.GetComponent<PvPBuilding>()._parent.GetComponent<NetworkObject>().NetworkObjectId;
                        SynchedServerData.Instance.SetUserChosenTargetToServer(true, objectId, SynchedServerData.Instance.GetTeam());
                    }
                }
                else if (target.GameObject.GetComponent<PvPCruiser>() != null)
                {
                    if (target.GameObject.GetComponent<NetworkObject>() != null)
                    {
                        objectId = (ulong)target.GameObject.GetComponent<NetworkObject>().NetworkObjectId;
                        SynchedServerData.Instance.SetUserChosenTargetToServer(true, objectId, SynchedServerData.Instance.GetTeam());
                    }
                }
            }
        }
    }
}