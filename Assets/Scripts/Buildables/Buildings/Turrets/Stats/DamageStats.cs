using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public class DamageStats : IDamageStats
    {
        public float DamagePerS { get; private set; }
        public IList<TargetType> AttackCapabilities { get; private set; }

        public DamageStats(float damagePerS, IList<TargetType> attackCapabilities)
        {
            Assert.IsTrue(damagePerS > 0);
            Assert.IsTrue(attackCapabilities.Count > 0);

            DamagePerS = damagePerS;
            AttackCapabilities = attackCapabilities;
        }
    }
}
