using System.Collections.Generic;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    // FELIX  Rename please :(
    public interface IDamage
    {
        float DamagePerS { get; }
        IList<TargetType> AttackCapabilities { get; }
    }
}
