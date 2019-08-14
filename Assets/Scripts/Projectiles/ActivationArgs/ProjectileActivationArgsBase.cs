using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine;

namespace BattleCruisers.Projectiles.ActivationArgs
{
    public class ProjectileActivationArgsBase<TStats> where TStats : IProjectileStats
    {
        TStats ProjectileStats { get; }
        Vector2 InitialVelocityInMPerS { get; }
        public ITargetFilter TargetFilter { get; }
        public ITarget Parent { get; }
    }
}