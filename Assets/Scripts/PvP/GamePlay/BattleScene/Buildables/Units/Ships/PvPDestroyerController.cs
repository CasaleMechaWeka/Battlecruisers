using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Ships
{
    public class PvPDestroyerController : PvPAnimatedShipController
    {
        private IPvPBarrelWrapper _directFireAntiSea, _mortar, _directFireAntiAir, _missileLauncher;//, _samSite;

        private float _optimalArmamentRangeInM;
        public override float OptimalArmamentRangeInM => _optimalArmamentRangeInM;

        private const float OPTIMAL_RANGE_BUFFER_IN_M = 1;

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);
            _optimalArmamentRangeInM = FindOptimalArmamentRangeInM();
        }

        /// <summary>
        /// Want to:
        /// + Stay out of range of mortars
        /// + But go close enough for more than one destroyer to attack
        /// </summary>
        private float FindOptimalArmamentRangeInM()
        {
            return _mortar.RangeInM + Size.x / 2 + OPTIMAL_RANGE_BUFFER_IN_M;
        }

        protected override IList<IPvPBarrelWrapper> GetTurrets()
        {
            IList<IPvPBarrelWrapper> turrets = new List<IPvPBarrelWrapper>();

            // Anti ship turret
            _directFireAntiSea = transform.FindNamedComponent<IPvPBarrelWrapper>("DirectFireAntiSea");
            turrets.Add(_directFireAntiSea);

            // Mortar
            _mortar = transform.FindNamedComponent<IPvPBarrelWrapper>("Mortar");
            turrets.Add(_mortar);

            // Anti air turret
            _directFireAntiAir = transform.FindNamedComponent<IPvPBarrelWrapper>("DirectBurstFireAntiAir");
            turrets.Add(_directFireAntiAir);

            // SAM site
            //_samSite = transform.FindNamedComponent<IBarrelWrapper>("SamSite");
            //turrets.Add(_samSite);

            // Missile launcher
            _missileLauncher = transform.FindNamedComponent<IPvPBarrelWrapper>("MissileLauncher");
            turrets.Add(_missileLauncher);

            return turrets;
        }

        protected override void InitialiseTurrets()
        {
            _directFireAntiSea.Initialise(this, _factoryProvider, _cruiserSpecificFactories, PvPSoundKeys.PvPFiring.BigCannon);
            _mortar.Initialise(this, _factoryProvider, _cruiserSpecificFactories, PvPSoundKeys.PvPFiring.BigCannon);
            _missileLauncher.Initialise(this, _factoryProvider, _cruiserSpecificFactories, PvPSoundKeys.PvPFiring.RocketLauncher);
            _directFireAntiAir.Initialise(this, _factoryProvider, _cruiserSpecificFactories, PvPSoundKeys.PvPFiring.AntiAir);
            //_samSite.Initialise(this, _factoryProvider, _cruiserSpecificFactories, SoundKeys.Firing.Missile);
        }

        protected override List<SpriteRenderer> GetNonTurretRenderers()
        {
            List<SpriteRenderer> renderers = base.GetNonTurretRenderers();

            SpriteRenderer wheelRenderer = transform.FindNamedComponent<SpriteRenderer>("WheelAnimation/Wheel");
            renderers.Add(wheelRenderer);

            return renderers;
        }

        //------------------------------------ methods for sync, written by Sava ------------------------------//

        public NetworkVariable<float> PvP_BuildProgress = new NetworkVariable<float>();


        protected override void OnShipCompleted()
        {
            if (IsServer)
                base.OnShipCompleted();
        }
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
