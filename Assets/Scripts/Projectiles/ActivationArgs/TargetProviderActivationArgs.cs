using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.ActivationArgs
{
    public class TargetProviderActivationArgs<TStats> : ProjectileActivationArgs<TStats> where TStats : IProjectileStats
    {
        ITarget Target { get; }

        public TargetProviderActivationArgs(
            TStats projectileStats, 
            Vector2 initialVelocityInMPerS, 
            ITargetFilter targetFilter, 
            ITarget parent,
            ITarget target) 
            : base(projectileStats, initialVelocityInMPerS, targetFilter, parent)
        {
            Assert.IsNotNull(target);
            Target = target;
        }
    }
}