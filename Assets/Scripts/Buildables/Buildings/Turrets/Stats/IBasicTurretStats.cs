using System.Collections.ObjectModel;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public interface IBasicTurretStats : IDurationProvider
    {
        float FireRatePerS { get; }
        float RangeInM { get; }
        float MinRangeInM { get; }
        float MeanFireRatePerS { get; }
        ReadOnlyCollection<TargetType> AttackCapabilities { get; }
    }
}
