using BattleCruisers.Data.Settings;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Utils.Factories
{
    public class SoundFactoryProvider : ISoundFactoryProvider
    {
        public ISoundFetcher SoundFetcher { get; }
        public ISoundPlayer SoundPlayer { get; }
        public IPrioritisedSoundPlayer PrioritisedSoundPlayer { get; }
        public IPrioritisedSoundPlayer DummySoundPlayer { get; }
        public ISingleSoundPlayer UISoundPlayer { get; }
        public ISoundPlayerFactory SoundPlayerFactory { get; }

        public SoundFactoryProvider(IBattleSceneGodComponents components, IPoolProviders poolProviders, ISettingsManager settingsManager)
		{
            Helper.AssertIsNotNull(components, poolProviders, settingsManager);

            SoundFetcher = new SoundFetcher();
            SoundPlayer = new SoundPlayer(SoundFetcher, poolProviders.AudioSourcePool);
            UISoundPlayer = new SingleSoundPlayer(SoundFetcher, components.UISoundsAudioSource);
            SoundPlayerFactory = new SoundPlayerFactory(SoundFetcher, components.Deferrer, settingsManager);
            DummySoundPlayer = new DummySoundPlayer();

            ISingleSoundPlayer singleSoundPlayer = new SingleSoundPlayer(SoundFetcher, components.PrioritisedSoundPlayerAudioSource);
            PrioritisedSoundPlayer = settingsManager.MuteVoices ? DummySoundPlayer : new PrioritisedSoundPlayer(singleSoundPlayer);
        }
	}
}
