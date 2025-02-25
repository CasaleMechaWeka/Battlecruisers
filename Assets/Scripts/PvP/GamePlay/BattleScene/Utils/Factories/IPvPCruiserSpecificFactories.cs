using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    /// <summary>
    /// Factories that are cruiser specific.  Hence each cruiser will need their own instance of these.
    /// </summary>
    public interface IPvPCruiserSpecificFactories
    {
        IPvPAircraftProvider AircraftProvider { get; }
        IPrioritisedSoundPlayer BuildableEffectsSoundPlayer { get; }
        IDroneFeedbackFactory DroneFeedbackFactory { get; }
        IPvPGlobalBoostProviders GlobalBoostProviders { get; }
        ITurretStatsFactory TurretStatsFactory { get; }
        IPvPCruiserTargetFactoriesProvider Targets { get; }
    }
}
