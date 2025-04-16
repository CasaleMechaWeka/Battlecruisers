using BattleCruisers.UI.Sound.Players;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.Factories;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    public class PvPSoundFactoryProvider : ISoundFactoryProvider
    {
        public SoundPlayer SoundPlayer { get; set; }
        public IPrioritisedSoundPlayer PrioritisedSoundPlayer { get; }
        public IPrioritisedSoundPlayer DummySoundPlayer { get; }
        public ISingleSoundPlayer UISoundPlayer { get; }
        public ISoundPlayerFactory SoundPlayerFactory { get; }

        public PvPSoundFactoryProvider(PvPBattleSceneGodComponents components /*, IPvPPoolProviders poolProviders */)
        {
            PvPHelper.AssertIsNotNull(components /*, poolProviders*/);
            /*            _audioSourcePool.AddCapacity(AUDIO_SOURCE_INITIAL_CAPACITY);
                        SoundFetcher = new PvPSoundFetcher();
                        SoundPlayer = new PvPSoundPlayer(SoundFetcher , _audioSourcePool*//*, poolProviders.AudioSourcePool*//*);*/

            SoundPlayer = new SoundPlayer();
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
