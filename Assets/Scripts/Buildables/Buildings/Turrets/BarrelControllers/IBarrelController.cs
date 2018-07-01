using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetProviders;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public interface IBarrelController : ITargetConsumer, ITargetProvider
    {
		Transform Transform { get; }
        ITurretStats TurretStats { get; }
        IProjectileStats ProjectileStats { get; }
        Vector3 ProjectileSpawnerPosition { get; }
        bool IsSourceMirrored { get; }

        void Fire(float angleInDegrees);
    }
}
