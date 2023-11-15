using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats
{
    public class PvPDamageCapability : IPvPDamageCapability
    {
        public float DamagePerS { get; }
        public IList<PvPTargetType> AttackCapabilities { get; }

        public PvPDamageCapability(float damagePerS, IList<PvPTargetType> attackCapabilities)
        {
            Assert.IsTrue(damagePerS > 0);
            Assert.IsTrue(attackCapabilities.Count > 0);

            DamagePerS = damagePerS;
            AttackCapabilities = attackCapabilities;
        }

        public PvPDamageCapability(IList<IPvPDamageCapability> damageStats)
        {
            Assert.IsTrue(damageStats.Count > 0);
            DamagePerS = damageStats.Sum(damageStat => damageStat.DamagePerS);
            AttackCapabilities = damageStats[0].AttackCapabilities;
        }
    }
}
