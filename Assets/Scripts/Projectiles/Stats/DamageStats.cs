namespace BattleCruisers.Projectiles.Stats
{
    public class DamageStats : IDamageStats
    {
        public float Damage { get; }
        public float DamageRadiusInM { get; }

        public DamageStats(float damage, float damageRadiusInM)
        {
            Damage = damage;
            DamageRadiusInM = damageRadiusInM;
        }
    }
}
