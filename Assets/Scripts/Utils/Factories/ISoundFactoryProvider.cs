using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Utils.Factories
{
    public interface ISoundFactoryProvider
    {
        ISoundFetcher SoundFetcher { get; }
        ISoundPlayer SoundPlayer { get; }
        IPrioritisedSoundPlayer PrioritisedSoundPlayer { get; }
        IPrioritisedSoundPlayer BuildableEffectsSoundPlayer { get; }
        IPrioritisedSoundPlayer DummySoundPlayer { get; }
        ISoundPlayerFactory SoundPlayerFactory { get; }
    }
}