using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Targets;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public interface IBarrelController : ITargetConsumer
    {
		Transform Transform { get; }
        ITurretStats TurretStats { get; }
        IProjectileStats ProjectileStats { get; }
        Vector3 ProjectileSpawnerPosition { get; }
        bool IsSourceMirrored { get; }
        ITarget CurrentTarget { get; }
        float BarrelAngleInDegrees { get; }

        // Basic projectiles CAN be fired without a target, as their trajectory
        // is determined by their initial velocity and gravity.  Missiles and 
        // rockets home into their target, and hence CANNOT be fired without
        // a target.
        bool CanFireWithoutTarget { get; }

        void Fire(float angleInDegrees);

        void CleanUp();
    }
}
