using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.UI.Sound.ProjectileSpawners;

namespace BattleCruisers.Utils.Factories
{
    public class SoundFactoryProvider : ISoundFactoryProvider
    {
        public IPrioritisedSoundPlayer IPrioritisedSoundPlayer { get; }
        public IPrioritisedSoundPlayer DummySoundPlayer { get; }
        public SingleSoundPlayer UISoundPlayer { get; }
        public SoundPlayerFactory SoundPlayerFactory { get; }

        public SoundFactoryProvider(BattleSceneGodComponents components)
        {
            Helper.AssertIsNotNull(components);

            UISoundPlayer = new SingleSoundPlayer(components.UISoundsAudioSource);
            SoundPlayerFactory = new SoundPlayerFactory(components.Deferrer);
            DummySoundPlayer = new DummySoundPlayer();

            IPrioritisedSoundPlayer
                = new PrioritisedSoundPlayer(
                    new SingleSoundPlayer(
                        components.PrioritisedSoundPlayerAudioSource));
        }
    }
}
