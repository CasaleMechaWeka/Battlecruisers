using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.ActivationArgs
{
    public class TargetProviderActivationArgs<TStats> : ProjectileActivationArgs<TStats> where TStats : IProjectileStats
    {
        public ITarget Target { get; }

        public TargetProviderActivationArgs(
            Vector3 position,
            TStats projectileStats, 
            Vector2 initialVelocityInMPerS, 
            ITargetFilter targetFilter, 
            ITarget parent,
            IAudioClipWrapper impactSound,
            ITarget target) 
            : base(position, projectileStats, initialVelocityInMPerS, targetFilter, parent, impactSound)
        {
            Assert.IsNotNull(target);
            Target = target;
        }
    }
}