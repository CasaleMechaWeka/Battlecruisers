using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.TEMP
{
    // Extends MonoBehaviour so can be set in Unity inspector
    public abstract class ProjectileStats<TPrefab> : MonoBehaviour where TPrefab : ProjectileController
    {
        public TPrefab projectilePrefab;
        public float damage;
        public float maxVelocityInMPerS;
        public float initialVelocityMultiplier;
        public bool ignoreGravity;
        public bool hasAreaOfEffectDamage;
        public float damageRadiusInM;

        void Awake()
        {
            Assert.IsNotNull(projectilePrefab);
            Assert.IsTrue(damage > 0);
            Assert.IsTrue(maxVelocityInMPerS > 0);
            Assert.IsTrue(initialVelocityMultiplier >= 0);

            if (hasAreaOfEffectDamage)
            {
                Assert.IsTrue(damageRadiusInM > 0);
            }

            OnAwake();
        }

        protected virtual void OnAwake() { }
	}
}
