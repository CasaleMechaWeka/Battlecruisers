namespace BattleCruisers.Projectiles.Stats
{
    public class DamageStats : IDamageStats
    {
        public float Damage { get; private set; }
        public float DamageRadiusInM { get; private set; }

        public DamageStats(float damage, float damageRadiusInM)
        {
            Damage = damage;
            DamageRadiusInM = damageRadiusInM;
        }
    }
}
