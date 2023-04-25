namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats
{
    public interface IPvPTurretStats : IPvPBasicTurretStats
    {
        float Accuracy { get; }
        float TurretRotateSpeedInDegrees { get; }
        bool IsInBurst { get; }
        int BurstSize { get; }
    }
}
