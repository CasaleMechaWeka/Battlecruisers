using UnityEngine;

namespace BattleCruisers.Projectiles.Stats.Wrappers
{
    public class NukeStatsWrapper : CruisingProjectileStatsWrapper, INukeStats
    {
        public Vector2 InitialVelocity { get { return new Vector2(0, 0); } }

        public NukeStatsWrapper(CruisingProjectileStats stats) 
            : base(stats) { }

        public NukeStatsWrapper(
            float damage,
            float maxVelocityInMPerS,
            bool ignoreGravity,
            bool hasAreaOfEffectDamage,
            float damageRadiusInM,
            float initialVelocityMultiplier,
            float cruisingAltitudeInM)
            : base(
                damage,
                maxVelocityInMPerS,
                ignoreGravity,
                hasAreaOfEffectDamage,
                damageRadiusInM,
                initialVelocityMultiplier,
                cruisingAltitudeInM)
        { }            
    }
}
