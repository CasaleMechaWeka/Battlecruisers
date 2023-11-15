namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats
{
    public interface IPvPCruisingProjectileStats : IPvPProjectileStats
    {
        float CruisingAltitudeInM { get; }
        bool IsAccurate { get; }
    }
}
