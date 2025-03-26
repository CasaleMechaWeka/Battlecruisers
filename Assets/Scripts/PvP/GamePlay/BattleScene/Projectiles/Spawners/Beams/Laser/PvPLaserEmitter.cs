using BattleCruisers.Buildables;
using BattleCruisers.Data.Settings;
using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Laser;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Ships;
using BattleCruisers.Utils.Threading;
using BattleCruisers.Projectiles.Spawners.Beams.Laser;
using BattleCruisers.Projectiles.Spawners.Beams;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners.Beams.Laser
{
    public class PvPLaserEmitter : PvPBeamEmitter, ILaserEmitter
    {
        private ILaserRenderer _laserRenderer;
        private IManagedDisposable _laserSoundPlayer;
        private float _damagePerS;
        private PvPLaserImpact _laserImpact;
        private IParticleSystemGroup _laserMuzzleEffect;
        private IDeltaTimeProvider _deltaTimeProvider;

        private ISettableBroadcastingProperty<bool> _isLaserFiring;
        public IBroadcastingProperty<bool> IsLaserFiring { get; private set; }

        public PvPArchonBattleshipController archonBattleShip;

        protected override void Awake()
        {
            base.Awake();

            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            _laserRenderer = new LaserRenderer(lineRenderer);

            _laserImpact = GetComponentInChildren<PvPLaserImpact>();
            Assert.IsNotNull(_laserImpact);

            IParticleSystemGroupInitialiser laserMuzzleEffectInitialiser = transform.FindNamedComponent<IParticleSystemGroupInitialiser>("LaserMuzzleEffect");
            _laserMuzzleEffect = laserMuzzleEffectInitialiser.CreateParticleSystemGroup();

            _isLaserFiring = new SettableBroadcastingProperty<bool>(false);
            IsLaserFiring = new BroadcastingProperty<bool>(_isLaserFiring);
        }

        public void Initialise(
            ITargetFilter targetFilter,
            float damagePerS,
            ITarget parent,
            SettingsManager settingsManager,
            IDeltaTimeProvider deltaTimeProvider,
            IDeferrer timeScaleDeferrer)
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
                _laserSoundPlayer = new LaserSoundPlayer(_laserRenderer, _audioSource);
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

        protected override void HandleCollision(IBeamCollision collision)
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
