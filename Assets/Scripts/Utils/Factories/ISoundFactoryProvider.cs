using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Utils.Factories
{
    public interface ISoundFactoryProvider
    {
        ISoundFetcher SoundFetcher { get; }
        ISoundPlayer SoundPlayer { get; }

        /// <summary>
        /// Plays a single sound at a time.  Used by in game events, such as
        /// building completed or cruiser events.
        /// </summary>
        IPrioritisedSoundPlayer PrioritisedSoundPlayer { get; }
        IPrioritisedSoundPlayer DummySoundPlayer { get; }
        ISoundPlayerFactory SoundPlayerFactory { get; }
    }
}