using System.Collections.ObjectModel;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public interface IBasicTurretStats
    {
        float FireRatePerS { get; }
        float RangeInM { get; }
        float MinRangeInM { get; }
        float MeanFireRatePerS { get; }
        ReadOnlyCollection<TargetType> AttackCapabilities { get; }
    }
}
