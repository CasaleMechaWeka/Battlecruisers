using BattleCruisers.UI.Sound.Players;
using BattleCruisers.UI.Sound.ProjectileSpawners;

namespace BattleCruisers.Utils.Factories
{
    public interface ISoundFactoryProvider
    {
        /// <summary>
        /// Plays a single sound at a time.  Used by in game events, such as
        /// building completed or cruiser events.
        /// </summary>
        IPrioritisedSoundPlayer IPrioritisedSoundPlayer { get; }
        IPrioritisedSoundPlayer DummySoundPlayer { get; }
        SingleSoundPlayer UISoundPlayer { get; }
        SoundPlayerFactory SoundPlayerFactory { get; }
    }
}