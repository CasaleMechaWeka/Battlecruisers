using BattleCruisers.Buildables;
using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using DigitalRuby.LightningBolt;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners.Beams.Lightning
{
    public class LightningEmitter : MonoBehaviour, IBeamEmitter
    {
        private IBeamCollisionDetector _collisionDetector;
        private ITarget _parent;
        private float _damage;

        // FELIX  Duplicated with laser emitter :)
        public BroadcastingParticleSystem constantSparks;

        public LightningBoltScript lightningBolt;
        public LayerMask unitsLayerMask, shieldsLayerMask;

        void Awake()
        {
            Helper.AssertIsNotNull(constantSparks, lightningBolt);
            constantSparks.Initialise();
        }

        public void Initialise(ITargetFilter targetFilter, float damage, ITarget parent)
        {
            Logging.Verbose(Tags.LASER, $"parent: {parent}  unitsLayerMask: {unitsLayerMask.value}  shieldsLayerMask: {shieldsLayerMask.value}");
            Helper.AssertIsNotNull(targetFilter, parent);
            Assert.IsTrue(damage > 0);

            _damage = damage;
            _parent = parent;

            // FELIX  Duplicated with laser emitter :)
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
            // FELIX  Fix logging tag :)  Beam?  Lightning?
            Logging.LogMethod(Tags.LASER);

            IBeamCollision collision = _collisionDetector.FindCollision(transform.position, angleInDegrees, isSourceMirrored);
            if (collision == null)
            {
                Logging.Warn(Tags.LASER, "Lighting should only be fired if there is a target in our sights, so should always get a collision :/");
                return;
            }

            Logging.Log(Tags.LASER, $"Have a collision with: {collision.Target} at {collision.CollisionPoint}");
            lightningBolt.StartPosition = transform.position;
            lightningBolt.EndPosition = collision.CollisionPoint;
            lightningBolt.Trigger();

            // FELIX  Duplicated with laser emitter :)  (Ish? :P)
            collision.Target.TakeDamage(_damage, _parent);
        }

        public void DisposeManagedState()
        {
            constantSparks.Stop();
        }
    }
}
