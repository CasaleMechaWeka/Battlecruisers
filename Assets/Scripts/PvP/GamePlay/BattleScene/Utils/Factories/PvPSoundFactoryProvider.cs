using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.UI.Sound.Pools;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    public class PvPSoundFactoryProvider : ISoundFactoryProvider
    {
        public ISoundFetcher SoundFetcher { get; }
        public ISoundPlayer SoundPlayer { get; set; }
        public IPrioritisedSoundPlayer PrioritisedSoundPlayer { get; }
        public IPrioritisedSoundPlayer DummySoundPlayer { get; }
        public ISingleSoundPlayer UISoundPlayer { get; }
        public ISoundPlayerFactory SoundPlayerFactory { get; }

        private PvPPool<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs> _audioSourcePool;

        private const int AUDIO_SOURCE_INITIAL_CAPACITY = 20;

        public PvPSoundFactoryProvider(IPvPBattleSceneGodComponents components, PvPFactoryProvider factoryProvider /*, IPvPPoolProviders poolProviders */)
        {
            PvPHelper.AssertIsNotNull(components /*, poolProviders*/);
            IPoolableFactory<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs> audioSourceFactory = new PvPAudioSourcePoolableFactory(factoryProvider.PrefabFactory, factoryProvider.DeferrerProvider.RealTimeDeferrer);
            _audioSourcePool = new PvPPool<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs>(audioSourceFactory);
            SoundFetcher = new SoundFetcher();
            /*            _audioSourcePool.AddCapacity(AUDIO_SOURCE_INITIAL_CAPACITY);
                        SoundFetcher = new PvPSoundFetcher();
                        SoundPlayer = new PvPSoundPlayer(SoundFetcher , _audioSourcePool*//*, poolProviders.AudioSourcePool*//*);*/

            LoadAudioSourcePool();
            UISoundPlayer = new SingleSoundPlayer(SoundFetcher, components.UISoundsAudioSource);
            SoundPlayerFactory = new SoundPlayerFactory(SoundFetcher, components.Deferrer);
            DummySoundPlayer = new PvPDummySoundPlayer();

            PrioritisedSoundPlayer
                = new PvPPrioritisedSoundPlayer(
                    new SingleSoundPlayer(
                        SoundFetcher,
                        components.PrioritisedSoundPlayerAudioSource));
        }

        private void LoadAudioSourcePool()
        {
            _audioSourcePool.AddCapacity(AUDIO_SOURCE_INITIAL_CAPACITY);
            SoundPlayer = new PvPSoundPlayer(SoundFetcher, _audioSourcePool/*, poolProviders.AudioSourcePool*/);
        }
    }
}
