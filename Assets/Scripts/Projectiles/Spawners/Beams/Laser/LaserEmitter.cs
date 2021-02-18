using BattleCruisers.Buildables;
using BattleCruisers.Data.Settings;
using BattleCruisers.Effects.Laser;
using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
using System.Threading.Tasks;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners.Beams.Laser
{
    public class LaserEmitter : BeamEmitter, ILaserEmitter
    {
        private ILaserRenderer _laserRenderer;
        private ILaserSoundPlayer _laserSoundPlayer;
        private float _damagePerS;
        private LaserImpact _laserImpact;
        private IParticleSystemGroup _laserMuzzleEffect;
        private IDeltaTimeProvider _deltaTimeProvider;

        private ISettableBroadcastingProperty<bool> _isLaserFiring;
        public IBroadcastingProperty<bool> IsLaserFiring { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            _laserRenderer = new LaserRenderer(lineRenderer);

            _laserImpact = GetComponentInChildren<LaserImpact>();
            Assert.IsNotNull(_laserImpact);

            IParticleSystemGroupInitialiser laserMuzzleEffectInitialiser = transform.FindNamedComponent<IParticleSystemGroupInitialiser>("LaserMuzzleEffect");
            _laserMuzzleEffect = laserMuzzleEffectInitialiser.CreateParticleSystemGroup();

            _isLaserFiring = new SettableBroadcastingProperty<bool>(false);
            IsLaserFiring = new BroadcastingProperty<bool>(_isLaserFiring);
        }

        public async Task InitialiseAsync(
            ITargetFilter targetFilter,
            float damagePerS,
            ITarget parent,
            ISettingsManager settingsManager,
            IDeltaTimeProvider deltaTimeProvider,
            IDeferrer timeScaleDeferrer)
        {
            base.Initialise(targetFilter, parent, settingsManager);
            Assert.IsNotNull(deltaTimeProvider);
            Assert.IsTrue(damagePerS > 0);

            _damagePerS = damagePerS;
            _deltaTimeProvider = deltaTimeProvider;
            _laserSoundPlayer = new LaserSoundPlayer(_laserRenderer, _audioSource);
            _laserImpact.Initialise(timeScaleDeferrer);
        }

        protected override void HandleCollision(IBeamCollision collision)
        {
            _laserRenderer.ShowLaser(transform.position, collision.CollisionPoint);
            _laserImpact.Show(collision.CollisionPoint);
            _laserMuzzleEffect.Play();

            float damage = _deltaTimeProvider.DeltaTime * _damagePerS;
            collision.Target.TakeDamage(damage, _parent);

            _isLaserFiring.Value = true;
        }

        public void StopLaser()
        {
            Logging.LogMethod(Tags.BEAM);

            _laserRenderer.HideLaser();
            _laserMuzzleEffect.Stop();
            _isLaserFiring.Value = false;
        }

        public override void DisposeManagedState()
        {
            base.DisposeManagedState();
            _laserSoundPlayer?.DisposeManagedState();
        }
    }
}
