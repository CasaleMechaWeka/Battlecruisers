namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public interface ILaserTurretStats : ITurretStats
    {
        float DamagePerS { get; }
        float LaserDurationInS { get; }
    }
}
