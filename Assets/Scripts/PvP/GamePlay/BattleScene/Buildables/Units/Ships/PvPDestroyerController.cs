using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils;
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
        public override bool KeepDistanceFromEnemyCruiser => false;

        private const float OPTIMAL_RANGE_BUFFER_IN_M = 1;

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);
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
            _directFireAntiSea.Initialise(this, _cruiserSpecificFactories, SoundKeys.Firing.BigCannon);
            _mortar.Initialise(this, _cruiserSpecificFactories, SoundKeys.Firing.BigCannon);
            _missileLauncher.Initialise(this, _cruiserSpecificFactories, SoundKeys.Firing.RocketLauncher);
            _directFireAntiAir.Initialise(this, _cruiserSpecificFactories, SoundKeys.Firing.AntiAir);
            //_samSite.Initialise(this, _factoryProvider, _cruiserSpecificFactories, SoundKeys.Firing.Missile);
        }

        protected override List<SpriteRenderer> GetNonTurretRenderers()
        {
            List<SpriteRenderer> renderers = base.GetNonTurretRenderers();

            SpriteRenderer wheelRenderer = transform.FindNamedComponent<SpriteRenderer>("WheelAnimation/Wheel");
            renderers.Add(wheelRenderer);

            return renderers;
        }

        protected override void OnShipCompleted()
        {
            if (IsServer)
                base.OnShipCompleted();
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
