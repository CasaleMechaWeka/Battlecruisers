using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using System.Collections.Generic;

namespace BattleCruisers.Projectiles
{
    public class ProjectileController : ProjectileControllerBase<ProjectileActivationArgs<ProjectileStats>, ProjectileStats>
    {
        public List<TargetType> AttackCapabilities { get; set; }

        // empty
    }
}