using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.ProjectileSpawners;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    public class PvPSoundFactoryProvider : IPvPSoundFactoryProvider
    {
        public IPvPSoundFetcher SoundFetcher { get; }
        public IPvPSoundPlayer SoundPlayer { get; set; }
        public IPvPPrioritisedSoundPlayer PrioritisedSoundPlayer { get; }
        public IPvPPrioritisedSoundPlayer DummySoundPlayer { get; }
        public IPvPSingleSoundPlayer UISoundPlayer { get; }
        public IPvPSoundPlayerFactory SoundPlayerFactory { get; }

        private PvPPool<IPvPAudioSourcePoolable, PvPAudioSourceActivationArgs> _audioSourcePool;

        private const int AUDIO_SOURCE_INITIAL_CAPACITY = 9;

        public PvPSoundFactoryProvider(IPvPBattleSceneGodComponents components, PvPFactoryProvider factoryProvider /*, IPvPPoolProviders poolProviders */)
        {
            PvPHelper.AssertIsNotNull(components /*, poolProviders*/);
            IPvPAudioSourcePoolableFactory audioSourceFactory = new PvPAudioSourcePoolableFactory(factoryProvider.PrefabFactory, factoryProvider.DeferrerProvider.RealTimeDeferrer);
            _audioSourcePool = new PvPPool<IPvPAudioSourcePoolable, PvPAudioSourceActivationArgs>(audioSourceFactory);
            SoundFetcher = new PvPSoundFetcher();
            /*            _audioSourcePool.AddCapacity(AUDIO_SOURCE_INITIAL_CAPACITY);
                        SoundFetcher = new PvPSoundFetcher();
                        SoundPlayer = new PvPSoundPlayer(SoundFetcher , _audioSourcePool*//*, poolProviders.AudioSourcePool*//*);*/

            LoadAudioSourcePool();
            UISoundPlayer = new PvPSingleSoundPlayer(SoundFetcher, components.UISoundsAudioSource);
            SoundPlayerFactory = new PvPSoundPlayerFactory(SoundFetcher, components.Deferrer);
            DummySoundPlayer = new PvPDummySoundPlayer();

            PrioritisedSoundPlayer
                = new PvPPrioritisedSoundPlayer(
                    new PvPSingleSoundPlayer(
                        SoundFetcher,
                        components.PrioritisedSoundPlayerAudioSource));
        }

        private async void LoadAudioSourcePool()
        {
            await _audioSourcePool.AddCapacity(AUDIO_SOURCE_INITIAL_CAPACITY);
            SoundPlayer = new PvPSoundPlayer(SoundFetcher, _audioSourcePool/*, poolProviders.AudioSourcePool*/);
        }
    }
}
