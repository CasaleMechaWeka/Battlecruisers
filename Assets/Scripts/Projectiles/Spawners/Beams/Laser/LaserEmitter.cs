using BattleCruisers.Buildables;
using BattleCruisers.Data.Static;
using BattleCruisers.Effects.Laser;
using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System.Threading.Tasks;
using UnityCommon.PlatformAbstractions.Time;
using UnityCommon.Properties;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners.Beams.Laser
{
    public class LaserEmitter : BeamEmitter, ILaserEmitter
    {
        private ILaserRenderer _laserRenderer;
        private IAudioSource _audioSource;
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

            AudioSource audioSource = GetComponentInChildren<AudioSource>();
            Assert.IsNotNull(audioSource);
            _audioSource = new AudioSourceBC(audioSource);

            _laserImpact = GetComponentInChildren<LaserImpact>();
            Assert.IsNotNull(_laserImpact);
            _laserImpact.Initialise();

            IParticleSystemGroupInitialiser laserMuzzleEffectInitialiser = transform.FindNamedComponent<IParticleSystemGroupInitialiser>("LaserMuzzleEffect");
            _laserMuzzleEffect = laserMuzzleEffectInitialiser.CreateParticleSystemGroup();

            _isLaserFiring = new SettableBroadcastingProperty<bool>(false);
            IsLaserFiring = new BroadcastingProperty<bool>(_isLaserFiring);
        }

        public async Task InitialiseAsync(
            ITargetFilter targetFilter,
            float damagePerS,
            ITarget parent,
            ISoundFetcher soundFetcher,
            IDeltaTimeProvider deltaTimeProvider)
        {
            base.Initialise(targetFilter, parent);
            Helper.AssertIsNotNull(soundFetcher, deltaTimeProvider);
            Assert.IsTrue(damagePerS > 0);

            _damagePerS = damagePerS;
            _deltaTimeProvider = deltaTimeProvider;
            _audioSource.AudioClip = await soundFetcher.GetSoundAsync(SoundKeys.Firing.Laser);
            _laserSoundPlayer = new LaserSoundPlayer(_laserRenderer, _audioSource);
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
            _laserImpact.Hide();
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
