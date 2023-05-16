using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Music;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.AudioSources;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Wind;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Lifetime;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene;
using Unity.Multiplayer.Samples.Utilities;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene
{
    [RequireComponent(typeof(NetcodeHooks))]
    public class PvPBattleSceneGodComponentsClient : MonoBehaviour, IPvPBattleSceneGodComponentsClient
    {
        public AudioSource prioritisedSoundPlayerAudioSource;
        public IPvPAudioSource PrioritisedSoundPlayerAudioSource { get; private set; }

        public AudioSource uiSoundsAudioSource;
        public IPvPAudioSource UISoundsAudioSource { get; private set; }

        public PvPLayeredMusicPlayerInitialiser musicPlayerInitialiser;
        public PvPLayeredMusicPlayerInitialiser MusicPlayerInitialiser => musicPlayerInitialiser;

        public PvPWindInitialiser windInitialiser;
        public PvPWindInitialiser WindInitialiser => windInitialiser;

        public PvPCloudInitialiser cloudInitialiser;
        public PvPCloudInitialiser CloudInitialiser => cloudInitialiser;

        public PvPSkyboxInitialiser SkyboxInitialiser { get; private set; }
        public PvPClickableEmitter backgroundClickableEmitter;
        public IPvPClickableEmitter BackgroundClickableEmitter => backgroundClickableEmitter;

        public PvPTargetIndicatorController targetIndicator;
        public IPvPTargetIndicator TargetIndicator => targetIndicator;

        public PvPHotkeyInitialiser hotkeyInitialiser;
        public PvPHotkeyInitialiser HotkeyInitialiser => hotkeyInitialiser;
        [SerializeField] NetcodeHooks m_NetcodeHooks;

        private void Awake()
        {
            m_NetcodeHooks.OnNetworkSpawnHook += OnNetworkSpawn;
            m_NetcodeHooks.OnNetworkDespawnHook += OnNetworkDespawn;
        }
        void OnNetworkSpawn()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                enabled = false;
                return;
            }
        }

        void OnNetworkDespawn()
        {

        }

        void Destroy()
        {
            if (m_NetcodeHooks)
            {
                m_NetcodeHooks.OnNetworkSpawnHook -= OnNetworkSpawn;
                m_NetcodeHooks.OnNetworkDespawnHook -= OnNetworkDespawn;
            }
        }

        public void Initialise(ISettingsManager settingsManager)
        {
            PvPHelper.AssertIsNotNull(
                backgroundClickableEmitter,
                targetIndicator,
                prioritisedSoundPlayerAudioSource,
                uiSoundsAudioSource,
                musicPlayerInitialiser,
                windInitialiser,
                cloudInitialiser,
                hotkeyInitialiser);
            Assert.IsNotNull(settingsManager);

            PrioritisedSoundPlayerAudioSource
                = new PvPEffectVolumeAudioSource(
                    new PvPAudioSourceBC(prioritisedSoundPlayerAudioSource),
                    settingsManager, 0);
            UISoundsAudioSource
                = new PvPEffectVolumeAudioSource(
                    new PvPAudioSourceBC(uiSoundsAudioSource),
                    settingsManager, 1);

            SkyboxInitialiser = GetComponent<PvPSkyboxInitialiser>();
            Assert.IsNotNull(SkyboxInitialiser);


            PvPLifetimeEventBroadcaster lifetimeEvents = GetComponent<PvPLifetimeEventBroadcaster>();
            targetIndicator.Initialise();
        }

        void Start()
        {

        }

    }
}
