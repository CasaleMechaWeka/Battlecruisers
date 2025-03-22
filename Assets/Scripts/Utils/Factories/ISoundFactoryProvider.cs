using BattleCruisers.UI.Sound.Players;
using BattleCruisers.UI.Sound.ProjectileSpawners;

namespace BattleCruisers.Utils.Factories
{
    public interface ISoundFactoryProvider
    {
        ISoundPlayer SoundPlayer { get; }

        /// <summary>
        /// Plays a single sound at a time.  Used by in game events, such as
        /// building completed or cruiser events.
        /// </summary>
        IPrioritisedSoundPlayer PrioritisedSoundPlayer { get; }
        IPrioritisedSoundPlayer DummySoundPlayer { get; }
        ISingleSoundPlayer UISoundPlayer { get; }
        ISoundPlayerFactory SoundPlayerFactory { get; }
    }
}