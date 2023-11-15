using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.ProjectileSpawners;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    public interface IPvPSoundFactoryProvider
    {
        IPvPSoundFetcher SoundFetcher { get; }
        IPvPSoundPlayer SoundPlayer { get; set; }

        /// <summary>
        /// Plays a single sound at a time.  Used by in game events, such as
        /// building completed or cruiser events.
        /// </summary>
        IPvPPrioritisedSoundPlayer PrioritisedSoundPlayer { get; }
        IPvPPrioritisedSoundPlayer DummySoundPlayer { get; }
        IPvPSingleSoundPlayer UISoundPlayer { get; }
        IPvPSoundPlayerFactory SoundPlayerFactory { get; }
    }
}