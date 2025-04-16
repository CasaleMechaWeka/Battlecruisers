using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Buildables.Units.Aircraft;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    /// <summary>
    /// Factories that are cruiser specific.  Hence each cruiser will need their own instance of these.
    /// </summary>
    public interface IPvPCruiserSpecificFactories
    {
        AircraftProvider AircraftProvider { get; }
        IPrioritisedSoundPlayer BuildableEffectsSoundPlayer { get; }
        DroneFeedbackFactory DroneFeedbackFactory { get; }
        GlobalBoostProviders GlobalBoostProviders { get; }
        ITurretStatsFactory TurretStatsFactory { get; }
        IPvPCruiserTargetFactoriesProvider Targets { get; }
    }
}
