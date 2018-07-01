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
    }
}
