using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Utils.Factories
{
    public interface ISoundFactoryProvider
    {
        ISoundFetcher SoundFetcher { get; }
        ISoundManager SoundManager { get; }
        ISoundPlayerFactory SoundPlayerFactory { get; }
    }
}