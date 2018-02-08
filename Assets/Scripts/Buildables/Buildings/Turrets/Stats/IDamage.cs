using System.Collections.Generic;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public interface IDamage
    {
        float DamagePerS { get; }
        IList<TargetType> AttackCapabilities { get; }
    }
}
