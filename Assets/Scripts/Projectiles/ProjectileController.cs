using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.ActivationArgs;
using System.Collections.Generic;

namespace BattleCruisers.Projectiles
{
    public class ProjectileController : ProjectileControllerBase<ProjectileActivationArgs>
    {
        public List<TargetType> AttackCapabilities { get; set; }

        // empty
    }
}