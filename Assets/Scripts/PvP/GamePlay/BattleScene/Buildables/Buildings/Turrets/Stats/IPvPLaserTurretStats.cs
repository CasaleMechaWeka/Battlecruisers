using BattleCruisers.Buildables.Buildings.Turrets.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats
{
    public interface IPvPLaserTurretStats : ITurretStats
    {
        float DamagePerS { get; }
        float LaserDurationInS { get; }
    }
}
