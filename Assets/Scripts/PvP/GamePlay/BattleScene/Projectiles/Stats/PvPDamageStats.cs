using BattleCruisers.Projectiles.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats
{
    public class PvPDamageStats : IDamageStats
    {
        public float Damage { get; }
        public float DamageRadiusInM { get; }
        public float SecondaryDamage { get; }
        public float SecondaryRadiusInM { get; }

        public PvPDamageStats(float damage, float damageRadiusInM, float secondaryDamage = 0, float secondaryRadiusInM = 0)

        {
            Damage = damage;
            DamageRadiusInM = damageRadiusInM;
            SecondaryDamage = secondaryDamage;
            SecondaryRadiusInM = secondaryRadiusInM;
        }
    }
}
