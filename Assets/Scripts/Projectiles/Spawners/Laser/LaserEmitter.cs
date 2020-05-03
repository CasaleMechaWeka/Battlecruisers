using BattleCruisers.Buildables;
using BattleCruisers.Data.Static;
using BattleCruisers.Effects.Laser;
using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System.Threading.Tasks;
using UnityCommon.PlatformAbstractions;
using UnityCommon.Properties;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners.Laser
{
    public class LaserEmitter : MonoBehaviour, ILaserEmitter
    {
        private ILaserRenderer _laserRenderer;
        private ILaserCollisionDetector _collisionDetector;
        private IAudioSource _audioSource;
        private ILaserSoundPlayer _laserSoundPlayer;
        private float _damagePerS;
        private ITarget _parent;
        private LaserImpact _laserImpact;
        private IParticleSystemGroup _laserMuzzleEffect;
        private IDeltaTimeProvider _deltaTimeProvider;

        public LayerMask unitsLayerMask, shieldsLayerMask;

        private ISettableBroadcastingProperty<bool> _isLaserFiring;
        public IBroadcastingProperty<bool> IsLaserFiring { get; private set; }

        void Awake()
        {
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            _laserRenderer = new LaserRenderer(lineRenderer);

            AudioSource audioSource = GetComponentInChildren<AudioSource>();
            Assert.IsNotNull(audioSource);
            _audioSource = new AudioSourceBC(audioSource);

            _laserImpact = GetComponentInChildren<LaserImpact>();
            Assert.IsNotNull(_laserImpact);
            _laserImpact.Initialise();

            IParticleSystemGroupInitialiser laserMuzzleEffectInitialiser = GetComponentInChildren<IParticleSystemGroupInitialiser>();
            Assert.IsNotNull(laserMuzzleEffectInitialiser);
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
            Logging.Verbose(Tags.LASER, $"parent: {parent}  unitsLayerMask: {unitsLayerMask.value}  shieldsLayerMask: {shieldsLayerMask.value}");
            Helper.AssertIsNotNull(targetFilter, parent, soundFetcher, deltaTimeProvider);
            Assert.IsTrue(damagePerS > 0);

            _damagePerS = damagePerS;
            _parent = parent;
            _deltaTimeProvider = deltaTimeProvider;

            ContactFilter2D contactFilter = new ContactFilter2D()
            {
                useLayerMask = true,
                layerMask = unitsLayerMask.value | shieldsLayerMask.value,
                useTriggers = true
            };
            _collisionDetector = new LaserCollisionDetector(contactFilter, targetFilter);

            _audioSource.AudioClip = await soundFetcher.GetSoundAsync(SoundKeys.Firing.Laser);
            _laserSoundPlayer = new LaserSoundPlayer(_laserRenderer, _audioSource);
        }

        public void FireLaser(float angleInDegrees, bool isSourceMirrored)
        {
            Logging.LogMethod(Tags.LASER);

            ILaserCollision collision = _collisionDetector.FindCollision(transform.position, angleInDegrees, isSourceMirrored);

            if (collision != null)
            {
                Logging.Log(Tags.LASER, $"Have a collision with: {collision.Target} at {collision.CollisionPoint}");

                _laserRenderer.ShowLaser(transform.position, collision.CollisionPoint);
                _laserImpact.Show(collision.CollisionPoint);
                _laserMuzzleEffect.Play();

                float damage = _deltaTimeProvider.DeltaTime * _damagePerS;
                collision.Target.TakeDamage(damage, _parent);

                _isLaserFiring.Value = true;
            }
        }

        public void StopLaser()
        {
            Logging.LogMethod(Tags.LASER);

            _laserRenderer.HideLaser();
            _laserImpact.Hide();
            _laserMuzzleEffect.Stop();
            _isLaserFiring.Value = false;
        }

        public void DisposeManagedState()
        {
            Logging.LogMethod(Tags.LASER);
            _laserSoundPlayer?.DisposeManagedState();
        }
    }
}
