using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Targets.Factories;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Utils.Factories
{
    /// <summary>
    /// Factories that are cruiser specific.  Hence each cruiser will need their own instance of these.
    /// </summary>
    public interface ICruiserSpecificFactories
    {
        IAircraftProvider AircraftProvider { get; }
        IPrioritisedSoundPlayer BuildableEffectsSoundPlayer { get; }
        IGlobalBoostProviders GlobalBoostProviders { get; }
        ITurretStatsFactory TurretStatsFactory { get; }
        ICruiserTargetFactoriesProvider Targets { get; }

        // FELIX  Remove :)
        ITargetProviderFactory TargetProviderFactory { get; }
    }
}