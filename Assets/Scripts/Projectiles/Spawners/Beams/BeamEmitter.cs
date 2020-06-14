using BattleCruisers.Buildables;
using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners.Beams
{
    public abstract class BeamEmitter : MonoBehaviour, IBeamEmitter
    {
        private IBeamCollisionDetector _collisionDetector;
        protected ITarget _parent;

        [SerializeField]
        private AudioSource _platformAudioSource;
        protected IAudioSource _audioSource;

        public BroadcastingParticleSystem constantSparks;
        public LayerMask unitsLayerMask, shieldsLayerMask;

        protected virtual void Awake()
        {
            Assert.IsNotNull(constantSparks);
            constantSparks.Initialise();

            Assert.IsNotNull(_platformAudioSource);
            _audioSource = new AudioSourceBC(_platformAudioSource);
        }

        protected void Initialise(ITargetFilter targetFilter, ITarget parent)
        {
            Logging.Verbose(Tags.BEAM, $"parent: {parent}  unitsLayerMask: {unitsLayerMask.value}  shieldsLayerMask: {shieldsLayerMask.value}");
            Helper.AssertIsNotNull(targetFilter, parent);

            _parent = parent;

            ContactFilter2D contactFilter = new ContactFilter2D()
            {
                useLayerMask = true,
                layerMask = unitsLayerMask.value | shieldsLayerMask.value,
                useTriggers = true
            };
            _collisionDetector = new BeamCollisionDetector(contactFilter, targetFilter);

            constantSparks.Play();
        }

        public void FireBeam(float angleInDegrees, bool isSourceMirrored)
        {
            Logging.LogMethod(Tags.BEAM);

            IBeamCollision collision = _collisionDetector.FindCollision(transform.position, angleInDegrees, isSourceMirrored);
            if (collision == null)
            {
                Logging.Warn(Tags.BEAM, "Beam should only be fired if there is a target in our sights, so should always get a collision :/");
                return;
            }

            Logging.Log(Tags.BEAM, $"Have a collision with: {collision.Target} at {collision.CollisionPoint}");
            HandleCollision(collision);
        }

        protected abstract void HandleCollision(IBeamCollision collision);

        public virtual void DisposeManagedState()
        {
            constantSparks.Stop();
        }
    }
}
