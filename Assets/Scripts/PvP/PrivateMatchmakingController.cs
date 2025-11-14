using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Qos;
using BattleCruisers.Network.Multiplay.ApplicationLifecycle;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI;
using BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    public class PrivateMatchmakingController : MonoBehaviour
    {
        private const int LOBBY_CLEANUP_DELAY_MS = 500;

        public static PrivateMatchmakingController Instance { get; private set; }
        public static BattleHubScreensController HubControllerReference { get; set; }

        public bool isProcessing = false;
        public bool isLoaded = false;
        public Animator animator;

        public AudioSource backgroundMusic;
        public AudioSource enemyFoundMusic;

        public PvPMessageBox messageBox;
        public GameObject arenaBackgroundPrefab;

        public GameObject[] connection_qualities;
        private ConnectionQuality connection_Quality;
        public ConnectionQuality Connection_Quality
        {
            set
            {
                if (connection_qualities != null && connection_qualities.Length == 4)
                {
                    connection_qualities[(int)connection_Quality].SetActive(false);
                    connection_Quality = value;
                    connection_qualities[(int)connection_Quality].SetActive(true);
                }
            }
        }

        public UnityEngine.UI.Button backButton;

        private SingleSoundPlayer soundPlayer;
        private PrivateMatchmakingPanel cachedPanel;

        void Start()
        {
            Debug.Log("PVP: PrivateMatchmakingController.Start");
            _ = StartAsync().ContinueWith(t =>
            {
                if (t.IsFaulted)
                    Debug.LogError($"PrivateMatchmakingController.Start failed: {t.Exception}");
            });
        }
        async Task StartAsync()
        {
            Debug.Log("PVP: PrivateMatchmakingController.StartAsync - begin initialization");
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("Duplicate PrivateMatchmakingController detected");
                enabled = false;
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            ArenaSelectPanelScreenController.PrivateMatch = true;
            Debug.Log("PVP: Starting latency check (async)");
            Task<(bool success, IList<IQosResult> qosResults)> latencyTask = FetchLatencyByRegion();
            ArenaSelectPanelScreenController.LatencyCheck = latencyTask;

            if (ApplicationController.Instance != null && !ApplicationController.Instance.NetworkManager)
            {
                ApplicationController.Instance.InitialiseServices();
            }
            else if (ApplicationController.Instance == null)
            {
                Debug.LogError("ApplicationController.Instance is NULL");
            }

            GameObject uiSoundsObject = new GameObject("UI_Sounds");
            uiSoundsObject.transform.SetParent(transform);
            AudioSource uiAudioSource = uiSoundsObject.AddComponent<AudioSource>();
            uiAudioSource.playOnAwake = false;
            uiAudioSource.loop = false;

            soundPlayer = new SingleSoundPlayer(
            new EffectVolumeAudioSource(
            new AudioSourceBC(uiAudioSource), 1));

            if (backButton != null)
            {
                backButton.onClick.AddListener(OnBackButtonClicked);
                backButton.gameObject.SetActive(true);
            }

            Debug.Log("PVP: Finding PrivateMatchmakingPanel");
            cachedPanel = FindFirstObjectByType<PrivateMatchmakingPanel>();

            if (cachedPanel == null)
            {
                Debug.LogError("PrivateMatchmakingPanel not found in scene");
            }
            else
            {
                Debug.Log("PVP: Initializing PrivateMatchmakingPanel");
                cachedPanel.Initialise(soundPlayer);
                ArenaSelectPanelScreenController arenaPanel = HubControllerReference?.arenaSelectPanel;
                if (arenaPanel != null && arenaBackgroundPrefab != null)
                {
                    cachedPanel.SetArenaBackground(arenaBackgroundPrefab, arenaPanel.IndexCurrentArena);
                }
                Debug.Log("PVP: PrivateMatchmakingPanel initialized - UI READY FOR INTERACTION");
            }

            Debug.Log("PVP: Starting PvPBattleScene pre-load (async in background)");
            StartCoroutine(PreloadBattleSceneCoroutine());

            Debug.Log("PVP: Waiting for latency check to complete");
            (bool success, IList<IQosResult> qosResults) = await latencyTask;
            if (success && qosResults != null && qosResults.Count > 0)
            {
                int latency = qosResults[0].AverageLatencyMs;
                ConnectionQuality quality = latency switch
                {
                    < 50 => ConnectionQuality.HIGH,
                    < 100 => ConnectionQuality.MID,
                    < 150 => ConnectionQuality.LOW,
                    _ => ConnectionQuality.DEAD
                };
                Connection_Quality = quality;
            }
            else
            {
                Connection_Quality = ConnectionQuality.HIGH;
                Debug.LogWarning("Latency check failed, defaulting to HIGH");
            }

            AudioListener[] listeners = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
            if (listeners.Length > 0)
            {
                AudioSource globalMusic = listeners[0].GetComponent<AudioSource>();
                if (globalMusic != null && globalMusic.isPlaying)
                {
                    globalMusic.Stop();
                }
            }

            if (backgroundMusic != null && !backgroundMusic.isPlaying)
            {
                backgroundMusic.Play();
            }

            Debug.Log("PVP: PrivateMatchmakingController initialization COMPLETE - lobby fully ready");
        }
        void OnBackButtonClicked()
        {
            _ = OnBackButtonClickedAsync().ContinueWith(t =>
            {
                if (t.IsFaulted)
                    Debug.LogError($"OnBackButtonClicked failed: {t.Exception}");
            });
        }
        async Task OnBackButtonClickedAsync()
        {
            if (backgroundMusic != null && backgroundMusic.isPlaying)
                backgroundMusic.Stop();

            ArenaSelectPanelScreenController.PrivateMatch = false;

            if (HubControllerReference != null && HubControllerReference.privateMatchButton != null)
            {
                CanvasGroup buttonCanvasGroup = HubControllerReference.privateMatchButton.GetComponent<CanvasGroup>();
                if (buttonCanvasGroup != null)
                {
                    buttonCanvasGroup.blocksRaycasts = true;
                    buttonCanvasGroup.interactable = true;
                }
            }

            if (cachedPanel != null && (cachedPanel.isHosting || cachedPanel.isWaitingInLobby))
            {
                cachedPanel.CancelHosting();
                await Task.Delay(LOBBY_CLEANUP_DELAY_MS);
            }

            if (isPvPBattleScenePreloaded)
            {
                Debug.Log("PVP: Unloading pre-loaded PvPBattleScene");
                SceneManager.UnloadSceneAsync("PvPBattleScene");
                isPvPBattleScenePreloaded = false;
            }

            Destroy(gameObject);
            SceneManager.UnloadSceneAsync(SceneNames.PRIVATE_PVP_INITIALIZER_SCENE);
        }
        public void FoundCompetitor()
        {
            if (backgroundMusic != null && backgroundMusic.isPlaying)
                backgroundMusic.Stop();

            if (enemyFoundMusic != null && !enemyFoundMusic.isPlaying)
                enemyFoundMusic.Play();

            if (animator != null)
                animator.SetBool("Found", true);
        }

        public void DisableAllAnimatedGameObjects()
        {
            gameObject.SetActive(false);
        }
        public void FailedMatchmaking()
        {
            if (backgroundMusic != null && backgroundMusic.isPlaying)
                backgroundMusic.Stop();

            if (isPvPBattleScenePreloaded)
            {
                Debug.Log("PVP: Unloading pre-loaded PvPBattleScene (FailedMatchmaking)");
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("PvPBattleScene");
                isPvPBattleScenePreloaded = false;
            }

            ArenaSelectPanelScreenController.PrivateMatch = false;
            Destroy(gameObject);
            SceneManager.UnloadSceneAsync(SceneNames.PRIVATE_PVP_INITIALIZER_SCENE);
        }
        public void ShowBadInternetMessageBox()
        {
            if (messageBox != null)
            {
                messageBox.ShowMessage("Sorry your internet connection is too rangi for multiplayer, try again when you have a more stable connection",
                    () => { messageBox.HideMessage(); FailedMatchmaking(); }, false);
            }
        }

        void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private static async Task<(bool success, IList<IQosResult> qosResults)> FetchLatencyByRegion()
        {
            IList<IQosResult> qosResults = await QosService.Instance.GetSortedQosResultsAsync("relay", null);
            if (qosResults == null || qosResults.Count == 0)
                return (false, null);

            return (true, qosResults);
        }
        private bool isPvPBattleScenePreloaded = false;
        System.Collections.IEnumerator PreloadBattleSceneCoroutine()
        {
            Debug.Log("PVP: Pre-loading PvPBattleScene...");
            AsyncOperation sceneLoad = SceneManager.LoadSceneAsync("PvPBattleScene", LoadSceneMode.Additive);
            sceneLoad.allowSceneActivation = true;

            yield return sceneLoad;

            Scene battleScene = SceneManager.GetSceneByName("PvPBattleScene");
            if (battleScene.IsValid())
            {
                GameObject[] rootObjects = battleScene.GetRootGameObjects();
                foreach (GameObject rootObject in rootObjects)
                {
                    if (rootObject.activeSelf)
                    {
                        rootObject.SetActive(false);
                        disabledRootObjects.Add(rootObject);
                    }
                }
            }

            Debug.Log("PVP: PvPBattleScene pre-loaded successfully");
            isPvPBattleScenePreloaded = true;
        }
        private List<GameObject> disabledRootObjects = new List<GameObject>();
        public void ReEnableBattleSceneGameObjects()
        {
            EventSystem initializerEventSystem = GetComponentInChildren<EventSystem>();
            if (initializerEventSystem != null)
            {
                Destroy(initializerEventSystem.gameObject);
            }

            if (disabledRootObjects != null && disabledRootObjects.Count > 0)
            {
                Debug.Log($"PVP: Re-enabling {disabledRootObjects.Count} disabled root GameObjects");
                foreach (GameObject rootObject in disabledRootObjects)
                {
                    if (rootObject != null)
                    {
                        rootObject.SetActive(true);
                    }
                }
                disabledRootObjects.Clear();
            }
        }
    }
}
