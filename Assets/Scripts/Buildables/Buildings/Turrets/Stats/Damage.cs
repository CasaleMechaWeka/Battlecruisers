using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public class Damage : IDamage
    {
        public float DamagePerS { get; private set; }
        public IList<TargetType> AttackCapabilities { get; private set; }

        public Damage(float damagePerS, IList<TargetType> attackCapabilities)
        {
            Assert.IsTrue(damagePerS > 0);
            Assert.IsTrue(attackCapabilities.Count > 0);

            DamagePerS = damagePerS;
            AttackCapabilities = attackCapabilities;
        }

        public Damage(IList<IDamage> damageStats)
        {
            Assert.IsTrue(damageStats.Count > 0);
            DamagePerS = damageStats.Sum(damageStat => damageStat.DamagePerS);
            AttackCapabilities = damageStats[0].AttackCapabilities;
        }
    }
}
