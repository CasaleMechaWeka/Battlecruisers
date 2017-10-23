using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Stats
{
    // Extends MonoBehaviour so can be set in Unity inspector
    public abstract class ProjectileStats : MonoBehaviour
    {
        public float damage;
        public float maxVelocityInMPerS;
        public float initialVelocityMultiplier;
        public bool ignoreGravity;
        public bool hasAreaOfEffectDamage;
        public float damageRadiusInM;

        void Awake()
        {
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
