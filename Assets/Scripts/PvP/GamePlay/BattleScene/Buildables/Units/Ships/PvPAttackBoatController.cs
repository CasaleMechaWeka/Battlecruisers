using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Ships
{
    public class PvPAttackBoatController : PvPAnimatedShipController
    {
        private IPvPBarrelWrapper _antiSeaTurret;

        public override float OptimalArmamentRangeInM => _antiSeaTurret.RangeInM;
        //    protected override bool ShowSmokeWhenDestroyed => true;

        protected override Vector2 MaskHighlightableSize
        {
            get
            {
                return
                    new Vector2(
                        base.MaskHighlightableSize.x * 1.5f,
                        base.MaskHighlightableSize.y * 2);
            }
        }

        protected override IList<IPvPBarrelWrapper> GetTurrets()
        {
            IList<IPvPBarrelWrapper> turrets = new List<IPvPBarrelWrapper>();

            _antiSeaTurret = gameObject.GetComponentInChildren<IPvPBarrelWrapper>();
            Assert.IsNotNull(_antiSeaTurret);
            turrets.Add(_antiSeaTurret);

            return turrets;
        }

        protected override void InitialiseTurrets()
        {
            _antiSeaTurret.Initialise(this, _factoryProvider, _cruiserSpecificFactories, PvPSoundKeys.PvPFiring.AttackBoat);
        }

        protected override List<SpriteRenderer> GetNonTurretRenderers()
        {
            List<SpriteRenderer> renderers = base.GetNonTurretRenderers();

            Transform pistonsParent = transform.FindNamedComponent<Transform>("Pistons");
            SpriteRenderer[] pistonRenderers = pistonsParent.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
            renderers.AddRange(pistonRenderers);

            return renderers;
        }

        //------------------------------------ methods for sync, written by Sava ------------------------------//

        public NetworkVariable<float> PvP_BuildProgress = new NetworkVariable<float>();

        private void LateUpdate()
        {
            if (IsServer)
            {
                if (PvP_BuildProgress.Value != BuildProgress)
                    PvP_BuildProgress.Value = BuildProgress;
            }
            if (IsClient)
            {
                BuildProgress = PvP_BuildProgress.Value;
            }
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
                pvp_Health.Value = maxHealth;
        }

        // Visibility 
        protected override void OnValueChangedIsEnableRenderes(bool isEnabled)
        {
            if (IsClient)
                base.OnValueChangedIsEnableRenderes(isEnabled);
            if (IsServer)
                OnValueChangedIsEnabledRendersClientRpc(isEnabled);
        }

        // ProgressController Visible
        protected override void CallRpc_ProgressControllerVisible(bool isEnabled)
        {
            if (IsServer)
                OnProgressControllerVisibleClientRpc(isEnabled);
            if (IsClient)
                base.CallRpc_ProgressControllerVisible(isEnabled);
        }


        // set Position of PvPBuildable
        protected override void CallRpc_SetPosition(Vector3 pos)
        {
            //  OnSetPositionClientRpc(pos);
        }

        // Set Rotation of PvPBuildable
        protected override void CallRpc_SetRotation(Quaternion rotation)
        {
            OnSetRotationClientRpc(rotation);
        }

        // BuildableStatus
        protected override void OnBuildableStateValueChanged(PvPBuildableState state)
        {
            OnBuildableStateValueChangedClientRpc(state);
        }

        protected override void OnBuildableProgressEvent()
        {
            if (IsClient)
                base.OnBuildableProgressEvent();
            if (IsServer)
                OnBuildableProgressEventClientRpc();
        }

        protected override void OnCompletedBuildableEvent()
        {
            if (IsClient)
                base.OnCompletedBuildableEvent();
            if (IsServer)
                OnCompletedBuildableEventClientRpc();
        }

        protected override void OnDestroyedEvent()
        {
            if (IsClient)
                base.OnDestroyedEvent();
            if (IsServer)
                OnDestroyedEventClientRpc();
        }

        protected override void OnBuildableCompleted()
        {
            if (IsServer)
            {
                base.OnBuildableCompleted();
                OnBuildableCompletedClientRpc();
            }
            if (IsClient)
                OnBuildableCompleted_PvPClient();
        }

        protected override void StartMovementEffectsOfClient()
        {
            if (IsClient)
                base.StartMovementEffectsOfClient();
            else
                StartMovementEffectsClientRpc();

        }

        protected override void StopMovementEffectsOfClient()
        {
            if (IsClient)
                base.StopMovementEffectsOfClient();
            else
                StopMovementEffectsClientRpc();
        }

        protected override void ResetAndHideOfClient()
        {
            if (IsClient)
                base.ResetAndHideOfClient();
            else
                ResetHideClientRpc();
        }

        //-------------------------------------- RPCs -------------------------------------------------//

        [ClientRpc]
        private void OnValueChangedIsEnabledRendersClientRpc(bool isEnabled)
        {
            OnValueChangedIsEnableRenderes(isEnabled);
        }

        [ClientRpc]
        private void OnProgressControllerVisibleClientRpc(bool isEnabled)
        {
            _buildableProgress.gameObject.SetActive(isEnabled);
            CallRpc_ProgressControllerVisible(isEnabled);
        }

        [ClientRpc]
        private void OnSetPositionClientRpc(Vector3 pos)
        {
            Position = pos;
        }

        [ClientRpc]
        private void OnSetRotationClientRpc(Quaternion rotation)
        {
            Rotation = rotation;
        }

        [ClientRpc]
        private void OnActivatePvPClientRpc()
        {
            Activate_PvPClient();
        }

        [ClientRpc]
        private void OnBuildableProgressEventClientRpc()
        {
            OnBuildableProgressEvent();
        }

        [ClientRpc]
        private void OnCompletedBuildableEventClientRpc()
        {
            OnCompletedBuildableEvent();
        }

        [ClientRpc]
        private void OnDestroyedEventClientRpc()
        {
            OnDestroyedEvent();
        }

        [ClientRpc]
        private void OnBuildableCompletedClientRpc()
        {
            OnBuildableCompleted();
        }

        [ClientRpc]
        private void OnBuildableStateValueChangedClientRpc(PvPBuildableState state)
        {
            BuildableState = state;
        }
        [ClientRpc]
        private void StartMovementEffectsClientRpc()
        {
            StartMovementEffectsOfClient();
        }
        [ClientRpc]
        private void StopMovementEffectsClientRpc()
        {
            StopMovementEffectsOfClient();
        }

        [ClientRpc]
        private void ResetHideClientRpc()
        {
            ResetAndHideOfClient();
        }
    }
}
