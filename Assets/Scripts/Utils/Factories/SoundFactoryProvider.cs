using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.UI.Sound.Players;
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

        public SoundFactoryProvider(IBattleSceneGodComponents components, IPoolProviders poolProviders)
		{
            Helper.AssertIsNotNull(components, poolProviders);

            SoundFetcher = new SoundFetcher();
            SoundPlayer = new SoundPlayer(SoundFetcher, poolProviders.AudioSourcePool);
            UISoundPlayer = new SingleSoundPlayer(SoundFetcher, components.UISoundsAudioSource);
            SoundPlayerFactory = new SoundPlayerFactory(SoundFetcher, components.Deferrer);
            DummySoundPlayer = new DummySoundPlayer();

            PrioritisedSoundPlayer 
                = new PrioritisedSoundPlayer(
                    new SingleSoundPlayer(
                        SoundFetcher, 
                        components.PrioritisedSoundPlayerAudioSource));
        }
    }
}
