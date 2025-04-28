using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using UnityEngine;

namespace BattleCruisers.Projectiles.ActivationArgs
{
    public class ProjectileActivationArgs
    {
        public Vector3 Position { get; }
        public ProjectileStats ProjectileStats { get; }
        public Vector2 InitialVelocityInMPerS { get; }
        public ITargetFilter TargetFilter { get; }
        public ITarget Parent { get; }
        public AudioClipWrapper ImpactSound { get; }

        // Homing
        public ITarget Target { get; }

        // Smart
        public CruiserTargetFactoriesProvider TargetFactories { get; }
        public ICruiser EnemyCruiser { get; }

        public ProjectileActivationArgs(
            Vector3 position,
            ProjectileStats projectileStats,
            Vector2 initialVelocityInMPerS,
            ITargetFilter targetFilter,
            ITarget parent,
            AudioClipWrapper impactSound,
            ITarget target = null,
            CruiserTargetFactoriesProvider targetFactories = null,
            ICruiser enemyCruiser = null)
        {
            Helper.AssertIsNotNull(projectileStats, targetFilter, parent, impactSound);

            Position = position;
            ProjectileStats = projectileStats;
            InitialVelocityInMPerS = initialVelocityInMPerS;
            TargetFilter = targetFilter;
            Parent = parent;
            ImpactSound = impactSound;

            Target = target;

            TargetFactories = targetFactories;
            EnemyCruiser = enemyCruiser;
        }
    }
}