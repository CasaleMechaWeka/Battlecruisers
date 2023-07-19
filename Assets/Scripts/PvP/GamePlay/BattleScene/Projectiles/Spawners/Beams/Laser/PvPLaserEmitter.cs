using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Laser;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using System.Threading.Tasks;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners.Beams.Laser
{
    public class PvPLaserEmitter : PvPBeamEmitter, IPvPLaserEmitter
    {
        private IPvPLaserRenderer _laserRenderer;
        private IPvPLaserSoundPlayer _laserSoundPlayer;
        private float _damagePerS;
        private PvPLaserImpact _laserImpact;
        private IPvPParticleSystemGroup _laserMuzzleEffect;
        private IPvPDeltaTimeProvider _deltaTimeProvider;

        private IPvPSettableBroadcastingProperty<bool> _isLaserFiring;
        public IPvPBroadcastingProperty<bool> IsLaserFiring { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            _laserRenderer = new PvPLaserRenderer(lineRenderer);

            _laserImpact = GetComponentInChildren<PvPLaserImpact>();
            Assert.IsNotNull(_laserImpact);

            IPvPParticleSystemGroupInitialiser laserMuzzleEffectInitialiser = transform.FindNamedComponent<IPvPParticleSystemGroupInitialiser>("LaserMuzzleEffect");
            _laserMuzzleEffect = laserMuzzleEffectInitialiser.CreateParticleSystemGroup();

            _isLaserFiring = new PvPSettableBroadcastingProperty<bool>(false);
            IsLaserFiring = new PvPBroadcastingProperty<bool>(_isLaserFiring);
        }

        public async Task InitialiseAsync(
            IPvPTargetFilter targetFilter,
            float damagePerS,
            IPvPTarget parent,
            ISettingsManager settingsManager,
            IPvPDeltaTimeProvider deltaTimeProvider,
            IPvPDeferrer timeScaleDeferrer)
        {
            base.Initialise(targetFilter, parent, settingsManager);
            Assert.IsNotNull(deltaTimeProvider);
            Assert.IsTrue(damagePerS > 0);

            _damagePerS = damagePerS;
            _deltaTimeProvider = deltaTimeProvider;
            //    _laserSoundPlayer = new PvPLaserSoundPlayer(_laserRenderer, _audioSource);
            _laserImpact.Initialise(timeScaleDeferrer);
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsClient)
            {
                _laserSoundPlayer = new PvPLaserSoundPlayer(_laserRenderer, _audioSource);
                _laserImpact.Initialise(PvPBattleSceneGodClient.Instance.factoryProvider.DeferrerProvider.Deferrer);
            }
        }
        protected override void PlaySparks_PvP()
        {
            PlaySparksClientRpc();
        }
        protected override void StopSparks_PvP()
        {
            StopSparksClientRpc();
        }

        protected override void HandleCollision(IPvPBeamCollision collision)
        {
            _laserRenderer.ShowLaser(transform.position, collision.CollisionPoint);
            _laserImpact.Show(collision.CollisionPoint);
            _laserMuzzleEffect.Play();

            float damage = _deltaTimeProvider.DeltaTime * _damagePerS;
            collision.Target.TakeDamage(damage, _parent);

            _isLaserFiring.Value = true;

            HandleCollision_PvP(transform.position, collision.CollisionPoint);
        }


        private void HandleCollision_PvP(Vector3 transPos, Vector3 collisionPos)
        {
            HandleCollisionClientRpc(transPos, collisionPos);
        }
        public void StopLaser()
        {
            // Logging.LogMethod(Tags.BEAM);

            _laserRenderer.HideLaser();
            _laserMuzzleEffect.Stop();
            _isLaserFiring.Value = false;
            StopLaser_PvP();
        }

        private void StopLaser_PvP()
        {
            StopLaserClientRpc();
        }
        public override void DisposeManagedState()
        {
            base.DisposeManagedState();
            _laserSoundPlayer?.DisposeManagedState();
        }

        [ClientRpc]
        private void PlaySparksClientRpc()
        {
            constantSparks.Play();
        }

        [ClientRpc]
        private void StopSparksClientRpc()
        {
            constantSparks.Stop();
        }

        [ClientRpc]
        private void HandleCollisionClientRpc(Vector3 transPos, Vector3 collisionPos)
        {
            _laserRenderer.ShowLaser(transPos, collisionPos);
            _laserImpact.Show(collisionPos);
            _laserMuzzleEffect.Play();
        }
        [ClientRpc]
        private void StopLaserClientRpc()
        {
            _laserRenderer.HideLaser();
            _laserMuzzleEffect.Stop();
            _isLaserFiring.Value = false;
        }
    }
}
