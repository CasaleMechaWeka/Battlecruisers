using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine;

// FELIX  Move to Pools namespace?
namespace BattleCruisers.Projectiles.ActivationArgs
{
    public class ProjectileActivationArgs<TStats> where TStats : IProjectileStats
    {
        public Vector3 Position { get; }
        public TStats ProjectileStats { get; }
        public Vector2 InitialVelocityInMPerS { get; }
        public ITargetFilter TargetFilter { get; }
        public ITarget Parent { get; }

        public ProjectileActivationArgs(
            Vector3 position,
            TStats projectileStats,
            Vector2 initialVelocityInMPerS,
            ITargetFilter targetFilter,
            ITarget parent)
        {
            Helper.AssertIsNotNull(projectileStats, targetFilter, parent);

            Position = position;
            ProjectileStats = projectileStats;
            InitialVelocityInMPerS = initialVelocityInMPerS;
            TargetFilter = targetFilter;
            Parent = parent;
        }
    }
}