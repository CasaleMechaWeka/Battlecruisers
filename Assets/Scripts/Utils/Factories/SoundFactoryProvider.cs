using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Utils.Factories
{
    public class SoundFactoryProvider : ISoundFactoryProvider
    {
        public ISoundFetcher SoundFetcher { get; private set; }
        public ISoundPlayer SoundPlayer { get; private set; }
        public ISoundPlayerFactory SoundPlayerFactory { get; private set; }
        public ISoundPlayer BuildableCompletedSoundPlayer { get; private set; }

        public SoundFactoryProvider(IVariableDelayDeferrer deferrer, ICamera soleCamera, bool isPlayerCruiser)
		{
            Helper.AssertIsNotNull(deferrer, soleCamera);

            SoundFetcher = new SoundFetcher();
            SoundPlayer = new SoundPlayer(SoundFetcher, new AudioClipPlayer(), soleCamera);
            SoundPlayerFactory = new SoundPlayerFactory(SoundFetcher, deferrer);
            BuildableCompletedSoundPlayer = isPlayerCruiser ? SoundPlayer : new DummySoundPlayer();
        }
	}
}
