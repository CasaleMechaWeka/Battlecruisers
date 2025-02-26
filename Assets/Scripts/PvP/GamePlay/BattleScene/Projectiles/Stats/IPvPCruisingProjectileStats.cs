using BattleCruisers.Projectiles.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats
{
    public interface IPvPCruisingProjectileStats : IProjectileStats
    {
        float CruisingAltitudeInM { get; }
        bool IsAccurate { get; }
    }
}
