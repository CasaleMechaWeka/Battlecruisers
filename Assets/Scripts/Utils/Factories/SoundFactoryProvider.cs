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
        public ISoundFetcher SoundFetcher { get; private set; }
        public ISoundPlayer SoundPlayer { get; private set; }
        public IPrioritisedSoundPlayer PrioritisedSoundPlayer { get; private set; }
        public ISoundPlayerFactory SoundPlayerFactory { get; private set; }
        public IPrioritisedSoundPlayer BuildableCompletedSoundPlayer { get; private set; }

        public SoundFactoryProvider(IVariableDelayDeferrer deferrer, ICamera soleCamera, bool isPlayerCruiser, IAudioSource audioSource)
		{
            Helper.AssertIsNotNull(deferrer, soleCamera, audioSource);

            SoundFetcher = new SoundFetcher();
            SoundPlayer = new SoundPlayer(SoundFetcher, new AudioClipPlayer(), soleCamera);
            ISingleSoundPlayer singleSoundPlayer = new SingleSoundPlayer(SoundFetcher, audioSource);
            PrioritisedSoundPlayer = new PrioritisedSoundPlayer(singleSoundPlayer);
            SoundPlayerFactory = new SoundPlayerFactory(SoundFetcher, deferrer);
            BuildableCompletedSoundPlayer = isPlayerCruiser ? PrioritisedSoundPlayer : new DummySoundPlayer();
        }
	}
}
