using System.Collections.Generic;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public interface IDamageStats
    {
        float DamagePerS { get; }
        IList<TargetType> AttackCapabilities { get; }
    }
}
