using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Pools;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.UI.Sound.Pools;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.BattleScene.Pools;
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

        private Pool<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs> _audioSourcePool;

        private const int AUDIO_SOURCE_INITIAL_CAPACITY = 20;

        public PvPSoundFactoryProvider(PvPBattleSceneGodComponents components /*, IPvPPoolProviders poolProviders */)
        {
            PvPHelper.AssertIsNotNull(components /*, poolProviders*/);
            IPoolableFactory<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs> audioSourceFactory = new PvPAudioSourcePoolableFactory();
            _audioSourcePool = new Pool<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs>(audioSourceFactory);
            /*            _audioSourcePool.AddCapacity(AUDIO_SOURCE_INITIAL_CAPACITY);
                        SoundFetcher = new PvPSoundFetcher();
                        SoundPlayer = new PvPSoundPlayer(SoundFetcher , _audioSourcePool*//*, poolProviders.AudioSourcePool*//*);*/

            LoadAudioSourcePool();
            UISoundPlayer = new SingleSoundPlayer(components.UISoundsAudioSource);
            SoundPlayerFactory = new SoundPlayerFactory(components.Deferrer);
            DummySoundPlayer = new DummySoundPlayer();

            PrioritisedSoundPlayer
                = new PrioritisedSoundPlayer(
                    new SingleSoundPlayer(
                        components.PrioritisedSoundPlayerAudioSource));
        }

        private void LoadAudioSourcePool()
        {
            _audioSourcePool.AddCapacity(AUDIO_SOURCE_INITIAL_CAPACITY);
            SoundPlayer = new SoundPlayer(_audioSourcePool/*, poolProviders.AudioSourcePool*/);
        }
    }
}
