namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public interface ITurretStats : IBasicTurretStats
    {
        float Accuracy { get; }
        float TurretRotateSpeedInDegrees { get; }
        bool IsInBurst { get; }
    }
}