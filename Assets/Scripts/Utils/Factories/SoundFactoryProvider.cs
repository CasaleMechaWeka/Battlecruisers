using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Utils.Factories
{
    public class SoundFactoryProvider : ISoundFactoryProvider
    {
        public ISoundPlayer SoundPlayer { get; }
        public IPrioritisedSoundPlayer PrioritisedSoundPlayer { get; }
        public IPrioritisedSoundPlayer DummySoundPlayer { get; }
        public ISingleSoundPlayer UISoundPlayer { get; }
        public ISoundPlayerFactory SoundPlayerFactory { get; }

        public SoundFactoryProvider(IBattleSceneGodComponents components, PoolProviders poolProviders)
        {
            Helper.AssertIsNotNull(components, poolProviders);

            SoundPlayer = new SoundPlayer(poolProviders.AudioSourcePool);
            UISoundPlayer = new SingleSoundPlayer(components.UISoundsAudioSource);
            SoundPlayerFactory = new SoundPlayerFactory(components.Deferrer);
            DummySoundPlayer = new DummySoundPlayer();

            PrioritisedSoundPlayer
                = new PrioritisedSoundPlayer(
                    new SingleSoundPlayer(
                        components.PrioritisedSoundPlayerAudioSource));
        }
    }
}
