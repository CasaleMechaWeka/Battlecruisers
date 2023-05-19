using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Lifetime;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using Unity.Multiplayer.Samples.Utilities;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene
{
    [RequireComponent(typeof(NetcodeHooks))]
    public class PvPBattleSceneGodComponentsServer : MonoBehaviour, IPvPBattleSceneGodComponentsServer
    {

        public IPvPDeferrer Deferrer { get; private set; }
        public IPvPDeferrer RealTimeDeferrer { get; private set; }

        public IPvPLifetimeEventBroadcaster LifetimeEvents { get; private set; }

        private PvPUpdaterProvider _updaterProvider;
        public IPvPUpdaterProvider UpdaterProvider => _updaterProvider;


        public AudioSource prioritisedSoundPlayerAudioSource;
        public IPvPAudioSource PrioritisedSoundPlayerAudioSource { get; private set; }

        public AudioSource uiSoundsAudioSource;
        public IPvPAudioSource UISoundsAudioSource { get; private set; }

        [SerializeField] NetcodeHooks m_NetcodeHooks;

        private void Awake()
        {
            m_NetcodeHooks.OnNetworkSpawnHook += OnNetworkSpawn;
            m_NetcodeHooks.OnNetworkDespawnHook += OnNetworkDespawn;
        }

        void OnNetworkSpawn()
        {
            if (!NetworkManager.Singleton.IsServer)
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
        public void Initialise()
        {
            Deferrer = GetComponent<PvPTimeScaleDeferrer>();
            Assert.IsNotNull(Deferrer);

            RealTimeDeferrer = GetComponent<PvPRealTimeDeferrer>();
            Assert.IsNotNull(RealTimeDeferrer);

            _updaterProvider = GetComponentInChildren<PvPUpdaterProvider>();
            Assert.IsNotNull(_updaterProvider);
            _updaterProvider.Initialise();

            PvPLifetimeEventBroadcaster lifetimeEvents = GetComponent<PvPLifetimeEventBroadcaster>();
            Assert.IsNotNull(lifetimeEvents);
            LifetimeEvents = lifetimeEvents;
        }
        void Start()
        {

        }

    }
}

