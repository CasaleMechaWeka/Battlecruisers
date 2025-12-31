using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public class DamageCapability : IDamageCapability
    {
        public float DamagePerS { get; }
        public IList<TargetType> AttackCapabilities { get; }

        public DamageCapability(float damagePerS, IList<TargetType> attackCapabilities)
        {
            Assert.IsTrue(damagePerS > 0);
            Assert.IsTrue(attackCapabilities.Count > 0);

            DamagePerS = damagePerS;
            AttackCapabilities = attackCapabilities;
        }

        public DamageCapability(IList<IDamageCapability> damageStats)
        {
            Assert.IsTrue(damageStats.Count > 0);
            DamagePerS = damageStats.Sum(damageStat => damageStat.DamagePerS);
            AttackCapabilities = damageStats[0].AttackCapabilities;
        }
    }
}
