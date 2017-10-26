namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public interface IBasicTurretStats
    {
        float FireRatePerS { get; }
        float RangeInM { get; }
        float MeanFireRatePerS { get; }
    }
}
