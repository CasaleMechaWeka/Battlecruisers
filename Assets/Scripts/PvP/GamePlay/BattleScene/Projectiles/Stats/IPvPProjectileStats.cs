namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats
{
    public interface IPvPProjectileStats : IPvPDamageStats, IPvPProjectileFlightStats
    {
        bool HasAreaOfEffectDamage { get; }
        float InitialVelocityInMPerS { get; }
    }
}
