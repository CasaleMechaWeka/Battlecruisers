namespace BattleCruisers.Projectiles.Stats
{
    public interface IDamageStats
    {
        float Damage { get; }
        float DamageRadiusInM { get; }
        float SecondaryDamage { get; }
        float SecondaryRadiusInM { get; }
    }
}
