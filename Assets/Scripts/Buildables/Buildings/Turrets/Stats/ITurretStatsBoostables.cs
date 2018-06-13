using BattleCruisers.Buildables.Boost;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public interface ITurretStatsBoostables
    {
        IBoostable AccuracyBoostable { get; }
        IBoostable FireRateBoostable { get; }
    }
}
