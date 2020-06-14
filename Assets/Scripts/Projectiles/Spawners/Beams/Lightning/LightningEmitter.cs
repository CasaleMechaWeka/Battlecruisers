using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders.Filters;
using DigitalRuby.LightningBolt;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners.Beams.Lightning
{
    public class LightningEmitter : BeamEmitter
    {
        private float _damage;

        public LightningBoltScript lightningBolt;

        public void Initialise(ITargetFilter targetFilter, float damage, ITarget parent)
        {
            base.Initialise(targetFilter, parent);

            Assert.IsTrue(damage > 0);
            _damage = damage;
        }

        protected override void HandleCollision(IBeamCollision collision)
        {
            lightningBolt.StartPosition = transform.position;
            lightningBolt.EndPosition = collision.CollisionPoint;
            lightningBolt.Trigger();

            collision.Target.TakeDamage(_damage, _parent);
        }
    }
}
