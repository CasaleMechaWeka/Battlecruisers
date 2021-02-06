using BattleCruisers.Data.Settings;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.Fetchers;
using System;

namespace BattleCruisers.Utils.Factories
{
    public class SoundFactoryProvider : ISoundFactoryProvider
    {
        private readonly ISettingsManager _settingsManager;
        private readonly ISingleSoundPlayer _prioritisedSoundPlayerCore;

        public ISoundFetcher SoundFetcher { get; }
        public ISoundPlayer SoundPlayer { get; }
        public IPrioritisedSoundPlayer PrioritisedSoundPlayer { get; }
        public IPrioritisedSoundPlayer DummySoundPlayer { get; }
        public ISingleSoundPlayer UISoundPlayer { get; }
        public ISoundPlayerFactory SoundPlayerFactory { get; }

        public SoundFactoryProvider(IBattleSceneGodComponents components, IPoolProviders poolProviders, ISettingsManager settingsManager)
		{
            Helper.AssertIsNotNull(components, poolProviders, settingsManager);

            _settingsManager = settingsManager;

            SoundFetcher = new SoundFetcher();
            SoundPlayer = new SoundPlayer(SoundFetcher, poolProviders.AudioSourcePool);
            UISoundPlayer = new SingleSoundPlayer(SoundFetcher, components.UISoundsAudioSource);
            SoundPlayerFactory = new SoundPlayerFactory(SoundFetcher, components.Deferrer, _settingsManager);
            DummySoundPlayer = new DummySoundPlayer();

            _prioritisedSoundPlayerCore = new SingleSoundPlayer(SoundFetcher, components.PrioritisedSoundPlayerAudioSource);
            PrioritisedSoundPlayer = new PrioritisedSoundPlayer(_prioritisedSoundPlayerCore);

            _settingsManager.SettingsSaved += SettingsManager_SettingsSaved;
        }

        private void SettingsManager_SettingsSaved(object sender, EventArgs e)
        {
            UISoundPlayer.Volume = _settingsManager.EffectVolume;
            _prioritisedSoundPlayerCore.Volume = _settingsManager.EffectVolume;
        }
    }
}
