using BattleCruisers.Buildables.Boost;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public interface ITurretStatsBoostables : IManagedDisposable
    {
        void Initialise(IBoostProvidersManager boostProvidersManager);

        float AccuracyMultiplier { get; }
        float FireRateMultiplier { get; }
    }
}
