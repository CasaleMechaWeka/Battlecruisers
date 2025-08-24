using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Buildables;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Stats
{
    // Extends MonoBehaviour so can be set in Unity inspector
    public class ProjectileStats : MonoBehaviour, IDamageStats, IProjectileFlightStats
    {
        public float initialVelocityMultiplier;

        public float damage;
        public float Damage => damage;

        public float maxVelocityInMPerS;
        public float MaxVelocityInMPerS => maxVelocityInMPerS;

        public float gravityScale;
        public float GravityScale => gravityScale;

        public float damageRadiusInM;
        public float DamageRadiusInM => damageRadiusInM;
        public float secondaryDamage;
        public float SecondaryDamage => secondaryDamage;
        public float secondaryRadiusInM;
        public float SecondaryRadiusInM => secondaryRadiusInM;
        public float InitialVelocityInMPerS { get; private set; }
        protected bool isAppliedVariant = false;
        [Header("Cruising")]
        public float cruisingAltitudeInM;
        public float CruisingAltitudeInM => cruisingAltitudeInM;
        public bool isAccurate = false;
        public bool IsAccurate => isAccurate;
        [Header("Smart")]
        public float detectionRangeM;
        public float DetectionRangeM => detectionRangeM;
        public List<TargetType> attackCapabilities;
        public ReadOnlyCollection<TargetType> AttackCapabilities { get; private set; }

        void Awake()
        {
            Assert.IsTrue(damage > 0);
            Assert.IsTrue(maxVelocityInMPerS > 0);
            Assert.IsTrue(initialVelocityMultiplier >= 0);
            Assert.IsTrue(gravityScale >= 0);

            Assert.IsTrue(damageRadiusInM >= 0);
            Assert.IsTrue(secondaryRadiusInM >= 0);

            Assert.IsTrue(cruisingAltitudeInM >= 0);

            Assert.IsTrue(detectionRangeM >= 0);

            InitialVelocityInMPerS = MaxVelocityInMPerS * initialVelocityMultiplier;
            AttackCapabilities = attackCapabilities.AsReadOnly();

            OnAwake();
        }

        public virtual void ApplyVariantStats(StatVariant statVariant)
        {
            if (!isAppliedVariant)
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
                // This forced ALL weapons to use AreaOfEffectDamageApplier, resulting in zero damage bug in slowmo.
                // damageRadiusInM = damageRadiusInM <= 0 ? 0.1f : damageRadiusInM;

                cruisingAltitudeInM += statVariant.cruising_altitude;

                detectionRangeM += statVariant.detection_range;

                isAppliedVariant = true;
            }

        }
        protected virtual void OnAwake() { }
    }
}
