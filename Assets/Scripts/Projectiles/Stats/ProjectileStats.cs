using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Stats
{
    // Extends MonoBehaviour so can be set in Unity inspector
    public class ProjectileStats : MonoBehaviour, IProjectileStats
    {
        public float initialVelocityMultiplier;

        public float damage;
        public float Damage => damage;

        public float maxVelocityInMPerS;
        public float MaxVelocityInMPerS => maxVelocityInMPerS;

        public float gravityScale;
        public float GravityScale => gravityScale;

        public bool hasAreaOfEffectDamage;
        public bool HasAreaOfEffectDamage => hasAreaOfEffectDamage;

        public float damageRadiusInM;
        public float DamageRadiusInM => damageRadiusInM;

        public float InitialVelocityInMPerS { get; private set; }

        void Awake()
        {
            Assert.IsTrue(damage > 0);
            Assert.IsTrue(maxVelocityInMPerS > 0);
            Assert.IsTrue(initialVelocityMultiplier >= 0);
            Assert.IsTrue(gravityScale >= 0);

            if (hasAreaOfEffectDamage)
            {
                Assert.IsTrue(damageRadiusInM > 0);
            }

            InitialVelocityInMPerS = MaxVelocityInMPerS * initialVelocityMultiplier;

            OnAwake();
        }

        protected virtual void OnAwake() { }
	}
}
