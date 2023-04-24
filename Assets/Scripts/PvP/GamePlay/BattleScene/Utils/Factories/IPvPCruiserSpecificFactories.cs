using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildable.Units.Aircraft.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    /// <summary>
    /// Factories that are cruiser specific.  Hence each cruiser will need their own instance of these.
    /// </summary>
    public interface IPvPCruiserSpecificFactories
    {
        IPvPAircraftProvider AircraftProvider { get; }
        IPvPPrioritisedSoundPlayer BuildableEffectsSoundPlayer { get; }
        IPvPDroneFeedbackFactory DroneFeedbackFactory { get; }
        IPvPGlobalBoostProviders GlobalBoostProviders { get; }
        IPvPTurretStatsFactory TurretStatsFactory { get; }
        IPvPCruiserTargetFactoriesProvider Targets { get; }
    }
}
