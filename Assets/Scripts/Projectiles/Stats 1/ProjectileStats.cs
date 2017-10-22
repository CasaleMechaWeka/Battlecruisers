using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Stats.TEMP
{
    // Extends MonoBehaviour so can be set in Unity inspector
    public abstract class ProjectileStats<TPrefab> : MonoBehaviour where TPrefab : ProjectileController
    {
        // FELIX  Wrapping class for subclasses so consuming code cannot set these fields?
        public TPrefab projectilePrefab;
        public float damage;
        public float maxVelocityInMPerS;
        public float initialVelocityMultiplier;
        public bool ignoreGravity;
        public bool areaOfEffectDamage;
        public float damageRadiusInM;

		public float InitialVelocityInMPerS { get { return maxVelocityInMPerS * initialVelocityMultiplier; } }

        void Awake()
        {
            Assert.IsNotNull(projectilePrefab);
            Assert.IsTrue(damage > 0);
            Assert.IsTrue(maxVelocityInMPerS > 0);
            Assert.IsTrue(initialVelocityMultiplier >= 0);

            if (areaOfEffectDamage)
            {
                Assert.IsTrue(damageRadiusInM > 0);
            }

            OnAwake();
        }

        protected virtual void OnAwake() { }
	}
}
