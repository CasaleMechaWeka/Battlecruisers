using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.Fetchers;
using UnityEngine.Assertions;

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

        public SoundFactoryProvider(IBattleSceneGodComponents components, IPoolProviders poolProviders)
		{
            Helper.AssertIsNotNull(components, poolProviders);

            SoundFetcher = new SoundFetcher();
            SoundPlayer = new SoundPlayerV2(SoundFetcher, poolProviders.AudioSourcePool);
            ISingleSoundPlayer singleSoundPlayer = new SingleSoundPlayer(SoundFetcher, components.PrioritisedSoundPlayerAudioSource);
            PrioritisedSoundPlayer = new PrioritisedSoundPlayer(singleSoundPlayer);
            UISoundPlayer = new SingleSoundPlayer(SoundFetcher, components.UISoundsAudioSource);
            SoundPlayerFactory = new SoundPlayerFactory(SoundFetcher, components.Deferrer);
            DummySoundPlayer = new DummySoundPlayer();
        }
	}
}
