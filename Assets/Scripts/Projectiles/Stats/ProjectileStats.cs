using BattleCruisers.UI.ScreensScene.ProfileScreen;
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
        protected bool isAppliedVariant = false;

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

        public virtual void ApplyVariantStats(StatVariant statVariant)
        {
            if(!isAppliedVariant)
            {
                initialVelocityMultiplier += statVariant.initial_velocity_multiplier;
                damage *= statVariant.damage;
                maxVelocityInMPerS += statVariant.max_velocity;
                gravityScale += statVariant.gravity_scale;
                damageRadiusInM += statVariant.damage_radius;

                initialVelocityMultiplier = initialVelocityMultiplier <= 0 ? 0.1f : initialVelocityMultiplier;
                damage = damage <= 0 ? 0.1f : damage;
                maxVelocityInMPerS = maxVelocityInMPerS <= 0 ? 0.1f : maxVelocityInMPerS;
                gravityScale = gravityScale < 0 ? 0 : gravityScale;
                damageRadiusInM = damageRadiusInM <= 0 ? 0.1f : damageRadiusInM;

                isAppliedVariant = true;
            }
            
        }
        protected virtual void OnAwake() { }
	}
}
