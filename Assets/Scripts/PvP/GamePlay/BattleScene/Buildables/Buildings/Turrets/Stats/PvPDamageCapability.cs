using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats
{
    public class PvPDamageCapability : IDamageCapability
    {
        public float DamagePerS { get; }
        public IList<TargetType> AttackCapabilities { get; }

        public PvPDamageCapability(float damagePerS, IList<TargetType> attackCapabilities)
        {
            Assert.IsTrue(damagePerS > 0);
            Assert.IsTrue(attackCapabilities.Count > 0);

            DamagePerS = damagePerS;
            AttackCapabilities = attackCapabilities;
        }

        public PvPDamageCapability(IList<IDamageCapability> damageStats)
        {
            Assert.IsTrue(damageStats.Count > 0);
            DamagePerS = damageStats.Sum(damageStat => damageStat.DamagePerS);
            AttackCapabilities = damageStats[0].AttackCapabilities;
        }
    }
}
