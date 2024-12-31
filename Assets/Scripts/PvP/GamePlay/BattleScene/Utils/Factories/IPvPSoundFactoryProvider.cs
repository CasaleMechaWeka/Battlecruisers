using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    public interface IPvPSoundFactoryProvider
    {
        ISoundFetcher SoundFetcher { get; }
        IPvPSoundPlayer SoundPlayer { get; set; }

        /// <summary>
        /// Plays a single sound at a time.  Used by in game events, such as
        /// building completed or cruiser events.
        /// </summary>
        IPvPPrioritisedSoundPlayer PrioritisedSoundPlayer { get; }
        IPvPPrioritisedSoundPlayer DummySoundPlayer { get; }
        ISingleSoundPlayer UISoundPlayer { get; }
        ISoundPlayerFactory SoundPlayerFactory { get; }
    }
}