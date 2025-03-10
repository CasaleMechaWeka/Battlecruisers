using System.Collections.Generic;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public interface IDamageCapability
    {
        float DamagePerS { get; }
        IList<TargetType> AttackCapabilities { get; }
    }
}
