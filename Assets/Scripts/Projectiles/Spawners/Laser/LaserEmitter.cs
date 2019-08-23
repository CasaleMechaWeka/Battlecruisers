using BattleCruisers.Buildables;
using BattleCruisers.Effects.Laser;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityCommon.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners.Laser
{
    public class LaserEmitter : MonoBehaviour, IManagedDisposable
	{
        private ILaserRenderer _laserRenderer;
        private ILaserCollisionDetector _collisionDetector;
        private IAudioSource _audioSource;
        private ILaserSoundPlayer _laserSoundPlayer;
		private float _damagePerS;
        private ITarget _parent;
        private LaserImpact _laserImpact;
        private IDeltaTimeProvider _deltaTimeProvider;

		public LayerMask unitsLayerMask, shieldsLayerMask;

		void Awake() 
		{
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            _laserRenderer = new LaserRenderer(lineRenderer);

            AudioSource audioSource = GetComponent<AudioSource>();
            Assert.IsNotNull(audioSource);
			_audioSource = new AudioSourceBC(audioSource);

            _laserImpact = GetComponentInChildren<LaserImpact>();
            Assert.IsNotNull(_laserImpact);
            _laserImpact.Initialise();
        }

        public void Initialise(
            ITargetFilter targetFilter, 
            float damagePerS, 
            ITarget parent, 
            ISoundFetcher soundFetcher, 
            IDeltaTimeProvider deltaTimeProvider)
        {
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

            _laserSoundPlayer = new LaserSoundPlayer(_laserRenderer, _audioSource, soundFetcher);
		}

		public void FireLaser(float angleInDegrees, bool isSourceMirrored)
		{
            ILaserCollision collision = _collisionDetector.FindCollision(transform.position, angleInDegrees, isSourceMirrored);
			
			if (collision != null)
			{
                _laserRenderer.ShowLaser(transform.position, collision.CollisionPoint);
                _laserImpact.Show(collision.CollisionPoint);

				float damage = _deltaTimeProvider.DeltaTime * _damagePerS;
                collision.Target.TakeDamage(damage, _parent);
			}
		}

		public void StopLaser()
		{
            _laserRenderer.HideLaser();
            _laserImpact.Hide();
		}

        public void DisposeManagedState()
        {
            _laserSoundPlayer.DisposeManagedState();
        }
    }
}
