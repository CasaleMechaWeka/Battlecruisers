using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Utils.Factories
{
    public class SoundFactoryProvider : ISoundFactoryProvider
    {
        public ISoundFetcher SoundFetcher { get; }
        public ISoundPlayer SoundPlayer { get; }
        public IPrioritisedSoundPlayer PrioritisedSoundPlayer { get; }
        public ISoundPlayerFactory SoundPlayerFactory { get; }
        public IPrioritisedSoundPlayer DummySoundPlayer { get; }

        public SoundFactoryProvider(IDeferrer deferrer, IGameObject audioListener, IAudioSource audioSource)
		{
            Helper.AssertIsNotNull(deferrer, audioListener, audioSource);

            SoundFetcher = new SoundFetcher();
            SoundPlayer = new SoundPlayer(SoundFetcher, new AudioClipPlayer(), audioListener);
            ISingleSoundPlayer singleSoundPlayer = new SingleSoundPlayer(SoundFetcher, audioSource);
            PrioritisedSoundPlayer = new PrioritisedSoundPlayer(singleSoundPlayer);
            SoundPlayerFactory = new SoundPlayerFactory(SoundFetcher, deferrer);
            DummySoundPlayer = new DummySoundPlayer();
        }
	}
}
