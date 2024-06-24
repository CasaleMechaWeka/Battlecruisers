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
        public override bool KeepDistanceFromEnemyCruiser => false;

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

        protected override void OnShipCompleted()
        {
            if (IsServer)
                base.OnShipCompleted();
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
            else
            {
                BuildProgress = PvP_BuildProgress.Value;
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsServer)
                pvp_Health.Value = maxHealth;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
        }

        // Visibility 
        protected override void OnValueChangedIsEnableRenderes(bool isEnabled)
        {
            if (IsServer)
            {
                OnValueChangedIsEnabledRendersClientRpc(isEnabled);
            }
            else
                base.OnValueChangedIsEnableRenderes(isEnabled);
        }

        // ProgressController Visible
        protected override void CallRpc_ProgressControllerVisible(bool isEnabled)
        {
            if (IsServer)
            {
                OnProgressControllerVisibleClientRpc(isEnabled);
                base.CallRpc_ProgressControllerVisible(isEnabled);
            }
            else
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
            if (IsServer)
                OnBuildableProgressEventClientRpc();
            else
                base.OnBuildableProgressEvent();
        }

        protected override void OnCompletedBuildableEvent()
        {
            if (IsServer)
                OnCompletedBuildableEventClientRpc();
            else
                base.OnCompletedBuildableEvent();
        }

        protected override void OnDestroyedEvent()
        {
            if (IsServer)
                OnDestroyedEventClientRpc();
            else
                base.OnDestroyedEvent();
        }

        protected override void OnBuildableCompleted()
        {
            if (IsServer)
            {
                base.OnBuildableCompleted();
                OnBuildableCompletedClientRpc();
                _antiSeaTurret.ApplyVariantStats(this);
            }
            else
            {
                OnBuildableCompleted_PvPClient();
                _antiSeaTurret.ApplyVariantStats(this);
            }
        }

        protected override void StartMovementEffectsOfClient()
        {
            if (!IsHost)
                base.StartMovementEffectsOfClient();
            else
                StartMovementEffectsClientRpc();

        }

        protected override void StopMovementEffectsOfClient()
        {
            if (!IsHost)
                base.StopMovementEffectsOfClient();
            else
                StopMovementEffectsClientRpc();
        }

        protected override void ResetAndHideOfClient()
        {
            if (!IsHost)
                base.ResetAndHideOfClient();
            else
                ResetHideClientRpc();
        }

        //-------------------------------------- RPCs -------------------------------------------------//

        [ClientRpc]
        private void OnValueChangedIsEnabledRendersClientRpc(bool isEnabled)
        {
            if (!IsHost)
                OnValueChangedIsEnableRenderes(isEnabled);
        }

        [ClientRpc]
        private void OnProgressControllerVisibleClientRpc(bool isEnabled)
        {

            _buildableProgress.gameObject.SetActive(isEnabled);
            if (!IsHost)
                CallRpc_ProgressControllerVisible(isEnabled);
        }

        [ClientRpc]
        private void OnSetPositionClientRpc(Vector3 pos)
        {
            if (!IsHost)
                Position = pos;
        }

        [ClientRpc]
        private void OnSetRotationClientRpc(Quaternion rotation)
        {
            if (!IsHost)
                Rotation = rotation;
        }

        [ClientRpc]
        private void OnBuildableProgressEventClientRpc()
        {
            if (!IsHost)
                OnBuildableProgressEvent();
        }

        [ClientRpc]
        private void OnCompletedBuildableEventClientRpc()
        {
            if (!IsHost)
                OnCompletedBuildableEvent();
        }

        [ClientRpc]
        private void OnDestroyedEventClientRpc()
        {
            if (!IsHost)
                OnDestroyedEvent();
        }

        [ClientRpc]
        private void OnBuildableCompletedClientRpc()
        {
            if (!IsHost)
                OnBuildableCompleted();
        }

        [ClientRpc]
        private void OnBuildableStateValueChangedClientRpc(PvPBuildableState state)
        {
            if (!IsHost)
                BuildableState = state;
        }
        [ClientRpc]
        private void StartMovementEffectsClientRpc()
        {
            if (!IsHost)
                StartMovementEffectsOfClient();
        }
        [ClientRpc]
        private void StopMovementEffectsClientRpc()
        {
            if (!IsHost)
                StopMovementEffectsOfClient();
        }

        [ClientRpc]
        private void ResetHideClientRpc()
        {
            if (!IsHost)
                ResetAndHideOfClient();
        }
    }
}
