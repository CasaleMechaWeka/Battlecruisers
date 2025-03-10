namespace BattleCruisers.Projectiles.Stats
{
    public class DamageStats : IDamageStats
    {
        public float Damage { get; }
        public float DamageRadiusInM { get; }
        public float SecondaryDamage { get; }
        public float SecondaryRadiusInM { get; }

        public DamageStats(float damage, float damageRadiusInM, float secondaryDamage = 0, float secondaryRadiusInM = 0)
        {
            Damage = damage;
            DamageRadiusInM = damageRadiusInM;
            SecondaryDamage = secondaryDamage;
            SecondaryRadiusInM = secondaryRadiusInM;
        }
    }
}
