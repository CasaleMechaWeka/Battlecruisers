using BattleCruisers.Buildables.Buildings.Turrets.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats
{
    public interface IPvPTurretStats : IBasicTurretStats
    {
        float Accuracy { get; }
        float TurretRotateSpeedInDegrees { get; }
        bool IsInBurst { get; }
        int BurstSize { get; }
    }
}
