using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;

namespace BattleCruisers.Projectiles.ActivationArgs
{
    public class ProjectileActivationArgs<TStats> where TStats : IProjectileStats
    {
        public Vector3 Position { get; }
        public TStats ProjectileStats { get; }
        public Vector2 InitialVelocityInMPerS { get; }
        public ITargetFilter TargetFilter { get; }
        public ITarget Parent { get; }
        public IAudioClipWrapper ImpactSound { get; }

        public ProjectileActivationArgs(
            Vector3 position,
            TStats projectileStats,
            Vector2 initialVelocityInMPerS,
            ITargetFilter targetFilter,
            ITarget parent,
            IAudioClipWrapper impactSound)
        {
            Helper.AssertIsNotNull(projectileStats, targetFilter, parent, impactSound);

            Position = position;
            ProjectileStats = projectileStats;
            InitialVelocityInMPerS = initialVelocityInMPerS;
            TargetFilter = targetFilter;
            Parent = parent;
            ImpactSound = impactSound;
        }
    }
}