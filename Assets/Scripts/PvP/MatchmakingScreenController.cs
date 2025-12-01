using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.ApplicationLifecycle;
using BattleCruisers.Network.Multiplay.ConnectionManagement;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.Network.Multiplay.Scenes;
using BattleCruisers.PostBattleScreen;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.BattleHubScreen;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Cache;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen.MatchmakingScreenController.MMStatus;

namespace BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen
{
    public enum ConnectionQuality
    {
        DEAD,
        LOW,
        MID,
        HIGH
    }

    public class MatchmakingScreenController : ScreenController
    {
        public Animator animator;

        public PvPMessageBox messageBox;

        public Text leftPlayerName;
        public Image leftPlayerRankImage;
        public Text leftPlayerRankName;
        public Text leftPlayerBounty;
        public Text leftCruiserName;
        public Image leftCruiserImage;

        public Text LookingForOpponentsText;

        public GameObject fleeButton;
        public GameObject vsAIButton;

        public static bool MatchmakingFailed;

        private CaptainExo charlie;
        public Transform ContainerCaptain;

        public AudioSource backgroundMusic;
        public AudioSource enemyFoundMusic;

        public static MatchmakingScreenController Instance { get; private set; }
        public static ArenaSelectPanelScreenController ArenaSelectPanelReference;

        public enum MMStatus
        {
            FINDING_LOBBY,
            JOIN_LOBBY,
            CREATING_LOBBY,
            CONNECTING,
            LOADING_ASSETS,
            LOOKING_VICTIM
        }
        public MMStatus status = FINDING_LOBBY;

        private ConnectionQuality connection_Quality;
        public ConnectionQuality Connection_Quality
        {
            set
            {
                connection_qualities[(int)connection_Quality].SetActive(false);
                connection_Quality = value;
                connection_qualities[(int)connection_Quality].SetActive(true);
            }
        }
        public GameObject[] connection_qualities;

        public override void OnPresenting(object activationParameter)
        {

        }
        async void Start()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning($"PVP: MatchmakingScreenController duplicate detected - destroying OLD instance IMMEDIATELY (this={gameObject.GetInstanceID()}, existing={Instance.gameObject.GetInstanceID()})");
                DestroyImmediate(Instance.gameObject); // Use DestroyImmediate to prevent old instance's coroutines from running
            }

            Instance = this;

            if (ApplicationController.Instance != null)
            {
                ApplicationController.Instance.InitialiseServices();
            }
            else
            {
                Debug.LogError("PVP: MatchmakingScreenController.Start - ApplicationController.Instance is NULL!");
            }

            Connection_Quality = ConnectionQuality.HIGH;

            DontDestroyOnLoad(gameObject);
            Debug.Log("PVP: Pre-loading PvPBattleScene");
            StartCoroutine(PreloadBattleSceneCoroutine());

            Logging.Log(Tags.SCREENS_SCENE_GOD, "Pre prefab cache load");
            //    leftCruiserName.text = DataProvider.GameModel.PlayerLoadout.Hull.PrefabName;
            leftPlayerName.text = DataProvider.GameModel.PlayerName;
            int rank = DestructionRanker.CalculateRank(DataProvider.GameModel.LifetimeDestructionScore);
            leftPlayerRankName.text = LocTableCache.CommonTable.GetString($"Rank{rank}");
            leftPlayerRankImage.sprite = await SpriteFetcher.GetSpriteAsync($"{SpritePaths.RankImagesPath}Rank{rank}.png");
#if !ENABLE_BOUNTIES
            leftPlayerBounty.gameObject.SetActive(false);
#endif
            leftPlayerBounty.text = DataProvider.GameModel.Bounty.ToString();
            // show bodykit of left player in MM if owned
            int id_bodykitA = DataProvider.GameModel.PlayerLoadout.SelectedBodykit;
            if (id_bodykitA != -1)
            {
                Bodykit bodykit = PrefabFactory.GetBodykit(StaticPrefabKeys.BodyKits.GetBodykitKey(id_bodykitA));
                if (bodykit.cruiserType == StaticPrefabKeys.Hulls.GetHullType(DataProvider.GameModel.PlayerLoadout.Hull))
                {
                    leftCruiserName.text = LocTableCache.CommonTable.GetString(StaticData.Bodykits[id_bodykitA].NameStringKeyBase);
                    leftCruiserImage.sprite = bodykit.BodykitImage;
                }
            }
            else
            {
                leftCruiserName.text = LocTableCache.CommonTable.GetString("Cruisers/" + DataProvider.GameModel.PlayerLoadout.Hull.PrefabName + "Name");
                leftCruiserImage.sprite = PrefabCache.GetCruiser(DataProvider.GameModel.PlayerLoadout.Hull).Sprite;
            }

            LookingForOpponentsText.text = LocTableCache.CommonTable.GetString("LookingForOpponents");

            CaptainExo charliePrefab = PrefabFactory.GetCaptainExo(DataProvider.GameModel.PlayerLoadout.CurrentCaptain);
            charlie = Instantiate(charliePrefab, ContainerCaptain);
            charlie.gameObject.transform.localScale = Vector3.one * 0.4f;

            //    m_TimeLimitLookingVictim = new RateLimitCooldown(5f);
            if (backgroundMusic != null)
            {
                backgroundMusic.Play();
                if (enemyFoundMusic.isPlaying && enemyFoundMusic != null)
                    enemyFoundMusic.Stop();
            }

            Debug.Log($"PVP: MatchmakingScreenController.Start - PrivateMatch={ArenaSelectPanelScreenController.PrivateMatch}");
            if (!ArenaSelectPanelScreenController.PrivateMatch)
            {
                if (PvPBootManager.Instance != null)
                {
                    _ = PvPBootManager.Instance.TryJoinLobby();
                }
                else
                {
                    Debug.LogError("PVP: MatchmakingScreenController.Start - PvPBootManager.Instance is NULL! Cannot start matchmaking.");
                }
            }
        }
        public void ShowBadInternetMessageBox()
        {
            //refactor to using translation string tool
            messageBox.ShowMessage("Sorry your internet connection is too rangi for muliplayer, try again when you have a more stable connection", () => { messageBox.HideMessage(); FailedMatchmaking(); }, false);
        }

        public bool isProcessing = false;
        public bool isLoaded = false;

        async void Update()
        {
            if (!isProcessing && !isLoaded)
            {
                isProcessing = true;
                await iLoadingAssets();
            }
        }

        async Task iLoadingAssets()
        {
            await Task.Delay(10);
            if (isLoaded)
                return;

            isProcessing = false;
        }

        public void SetMMStatus(MMStatus _status)
        {
            status = _status;

            string locTableKey = status switch
            {
                FINDING_LOBBY => "FindingLobby",
                JOIN_LOBBY => "JoiningLobby",
                CONNECTING => "Connecting",
                CREATING_LOBBY => "CreatingLobby",
                LOADING_ASSETS => "LoadingAssets",
                LOOKING_VICTIM => "LookingVictim",
                _ => "",
            };

            LookingForOpponentsText.text = LocTableCache.CommonTable.GetString(locTableKey);
        }

        public void LockLobby()
        {
            if (GameObject.Find("ConnectionManager") != null)
                GameObject.Find("ConnectionManager").GetComponent<ConnectionManager>().LockLobby();
        }

        public void LeaveLobby()
        {
            if (GameObject.Find("ConnectionManager") != null)
                GameObject.Find("ConnectionManager").GetComponent<ConnectionManager>().LeaveLobby();
        }

        public void SetFoundVictimString()
        {
            SetMMStatus(LOADING_ASSETS);
            // Iterate through all child objects of ContainerCaptain
            foreach (Transform child in ContainerCaptain)
            {
                // Try to get an Animator component from the child object
                Animator animator = child.GetComponentInChildren<Animator>();
                if (animator != null)
                    animator.SetTrigger("happy");
            }
            LockLobby();
        }
        public void OnFlee()
        {
            Debug.Log("PVP: MatchmakingScreenController.OnFlee - User clicked FLEE, starting async cleanup");
            fleeButton.SetActive(false);
            _ = OnFleeAsync();
        }
        async System.Threading.Tasks.Task OnFleeAsync()
        {
            Debug.Log("PVP: OnFleeAsync - stopping matchmaking (user clicked FLEE)");
            isCancelled = true;
            StopCoroutine(nameof(LobbyLoop));

            string lobbyIdToDelete = null;
            bool wasHost = PvPBootManager.Instance?.IsLocalUserHost ?? false;

            if (PvPBootManager.Instance?.LobbyServiceFacade != null)
            {
                Unity.Services.Lobbies.Models.Lobby currentLobby = PvPBootManager.Instance.LobbyServiceFacade.CurrentUnityLobby;
                if (currentLobby != null && !string.IsNullOrEmpty(currentLobby.Id))
                {
                    lobbyIdToDelete = currentLobby.Id;
                    Debug.Log($"PVP: OnFleeAsync - stored lobby ID {lobbyIdToDelete} for deletion (IsHost={wasHost})");
                }

                Debug.Log("PVP: OnFleeAsync - stopping lobby tracking (EndTracking will clear cached relay allocation)");
                await PvPBootManager.Instance.LobbyServiceFacade.EndTracking();

                if (!string.IsNullOrEmpty(lobbyIdToDelete) && wasHost)
                {
                    try
                    {
                        Debug.Log($"PVP: OnFleeAsync - HOST deleting lobby {lobbyIdToDelete}");
                        await PvPBootManager.Instance.LobbyServiceFacade.DeleteLobbyAsync(lobbyIdToDelete);
                        Debug.Log($"PVP: OnFleeAsync - lobby {lobbyIdToDelete} deleted successfully");
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError($"PVP: OnFleeAsync - failed to delete lobby: {ex.Message}");
                    }
                }
                else if (!wasHost)
                {
                    Debug.Log("PVP: OnFleeAsync - CLIENT leaving lobby (not deleting)");
                }
            }

            if (PvPBattleSceneGodClient.Instance != null)
            {
                PvPBattleSceneGodClient.Instance.WasLeftMatch = true;
                PvPBattleSceneGodClient.Instance.HandleClientDisconnected();
            }

            lastKnownPlayerCount = 0;
            gameStarted = false;
            isCancelled = false;

            if (ApplicationController.Instance != null)
            {
                Debug.Log("PVP: OnFleeAsync - reinitializing services");
                ApplicationController.Instance.ReinitialiseServicesForFLEE();
            }

            if (backgroundMusic != null && backgroundMusic.isPlaying)
                backgroundMusic.Stop();

            UnityEngine.EventSystems.EventSystem[] eventSystemsBefore = UnityEngine.Object.FindObjectsOfType<UnityEngine.EventSystems.EventSystem>();
            Debug.Log($"PVP: FLEE - Found {eventSystemsBefore.Length} EventSystems before cleanup");

            Debug.Log($"PVP: FLEE - Active scenes before cleanup: {UnityEngine.SceneManagement.SceneManager.sceneCount}");
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
            {
                UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
                Debug.Log($"  Scene {i}: {scene.name}, rootCount={scene.rootCount}, isLoaded={scene.isLoaded}");
            }

            if (isPvPBattleScenePreloaded)
            {
                Debug.Log("PVP: OnFleeAsync - unloading pre-loaded PvPBattleScene");
                SceneManager.UnloadSceneAsync("PvPBattleScene");
                isPvPBattleScenePreloaded = false;
                disabledRootObjects.Clear();
            }

            if (ArenaSelectPanelReference != null)
            {
                Debug.Log("PVP: Resetting ArenaSelectPanel state");
                ArenaSelectPanelReference.ResetBattleButtonState();
            }

            // Set flag so ScreensSceneGod.Start() navigates to HubScreen (ArenaSelectPanel) if needed
            Debug.Log("PVP: OnFleeAsync - setting MatchmakingFailed=true");
            MatchmakingFailed = true;

            Debug.Log("PVP: OnFleeAsync - destroying MatchmakingScreenController (DontDestroyOnLoad)");
            Destroy(gameObject);

            Debug.Log("PVP: OnFleeAsync - unloading PvPInitializeScene");
            SceneManager.UnloadSceneAsync(SceneNames.PvP_INITIALIZE_SCENE);
        }
        public async void FoundCompetitor()
        {
            if (leftPlayerName == null) return;
            leftPlayerName.text = SynchedServerData.Instance.playerAName.Value;
#if ENABLE_BOUNTIES
            if (leftPlayerBounty == null) return;
            leftPlayerBounty.text = SynchedServerData.Instance.playerABounty.Value.ToString();
#endif
            //    leftCruiserName.text = SynchedServerData.Instance.playerAPrefabName.Value;
            int rankA = DestructionRanker.CalculateRank(SynchedServerData.Instance.playerAScore.Value);
            Sprite spriteA = await SpriteFetcher.GetSpriteAsync($"{SpritePaths.RankImagesPath}Rank{rankA}.png");
            if (leftPlayerRankImage != null) leftPlayerRankImage.sprite = spriteA;
            if (leftPlayerRankName != null) leftPlayerRankName.text = LocTableCache.CommonTable.GetString($"Rank{rankA}");
            //    leftCruiserImage.sprite = sprites.ContainsKey(SynchedServerData.Instance.playerAPrefabName.Value) ? sprites[SynchedServerData.Instance.playerAPrefabName.Value] : Trident;

            // apply bodykit of left player
            int id_bodykitA = SynchedServerData.Instance.playerABodykit.Value;
            if (id_bodykitA != -1)
            {
                Bodykit bodykit = PrefabFactory.GetBodykit(StaticPrefabKeys.BodyKits.GetBodykitKey(id_bodykitA));
                if (bodykit.cruiserType == (HullType)SynchedServerData.Instance.playerACruiserID.Value)
                {
                    if (leftCruiserName != null) leftCruiserName.text = LocTableCache.CommonTable.GetString(StaticData.Bodykits[id_bodykitA].NameStringKeyBase);
                    if (leftCruiserImage != null) leftCruiserImage.sprite = bodykit.bodykitImage;
                }
            }
            else
            {
                if (leftCruiserName != null) leftCruiserName.text = LocTableCache.CommonTable.GetString("Cruisers/" + SynchedServerData.Instance.playerACruiserID.Value + "Name");
                leftCruiserImage.sprite = PrefabCache.GetCruiser(DataProvider.GameModel.PlayerLoadout.Hull).Sprite;
            }

            if (backgroundMusic != null && backgroundMusic.isPlaying)
                backgroundMusic.Stop();

            if (enemyFoundMusic != null)
                enemyFoundMusic.Play();

            animator.SetBool("Found", true);
            LeaveLobby();
        }
        public void FailedMatchmaking()
        {
            Debug.Log($"PVP: MatchmakingScreenController.FailedMatchmaking - isPvPBattleScenePreloaded={isPvPBattleScenePreloaded}");

            if (backgroundMusic != null && backgroundMusic.isPlaying)
                backgroundMusic.Stop();

            if (isPvPBattleScenePreloaded)
            {
                Debug.LogWarning("PVP: FailedMatchmaking - PvPBattleScene still marked as pre-loaded, unloading now (should have been done in OnFleeAsync)");
                SceneManager.UnloadSceneAsync("PvPBattleScene");
                isPvPBattleScenePreloaded = false;
                disabledRootObjects.Clear();
            }

            if (ArenaSelectPanelReference != null)
            {
                Debug.Log("PVP: Resetting ArenaSelectPanel state (FailedMatchmaking)");
                ArenaSelectPanelReference.ResetBattleButtonState();
            }

            // Set flag so ScreensSceneGod.Start() navigates to HubScreen (ArenaSelectPanel)
            Debug.Log("PVP: FailedMatchmaking - setting MatchmakingFailed=true to return to HubScreen");
            MatchmakingFailed = true;

            Destroy(gameObject);
            Debug.Log("PVP: Unloading PvPInitializeScene (FailedMatchmaking)");
            SceneManager.UnloadSceneAsync(SceneNames.PvP_INITIALIZE_SCENE);
        }
        public void Destroy()
        {
            Destroy(gameObject);
        }

        void OnDestroy()
        {
            Debug.Log("PVP: MatchmakingScreenController.OnDestroy - clearing singleton reference");
            if (Instance == this)
            {
                Instance = null;
            }
        }
        public void DisableAllAnimatedGameObjects()
        {
            Animator animator = GetComponent<Animator>();
            if (animator != null)
                foreach (Transform child in animator.transform)
                    child.gameObject.SetActive(false);
        }
        private bool isPvPBattleScenePreloaded = false;
        private List<GameObject> disabledRootObjects = new List<GameObject>();

        IEnumerator PreloadBattleSceneCoroutine()
        {
            Debug.Log("PVP: Pre-loading PvPBattleScene");
            AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(
            "PvPBattleScene",
            LoadSceneMode.Additive);
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
                        Debug.Log($"PVP: Disabled root GameObject '{rootObject.name}' to prevent premature NetworkObject spawning");
                    }
                }
            }

            Debug.Log("PVP: PvPBattleScene pre-loaded successfully");
            isPvPBattleScenePreloaded = true;
        }
        public void ReEnableBattleSceneGameObjects()
        {
            EventSystem matchmakingEventSystem = GetComponentInChildren<EventSystem>();
            if (matchmakingEventSystem != null)
            {
                Destroy(matchmakingEventSystem.gameObject);
                Debug.Log("PVP: Destroyed matchmaking EventSystem");
            }

            if (disabledRootObjects != null && disabledRootObjects.Count > 0)
            {
                Debug.Log($"PVP: Re-enabling {disabledRootObjects.Count} disabled root GameObjects");
                foreach (GameObject rootObject in disabledRootObjects)
                {
                    if (rootObject != null)
                    {
                        rootObject.SetActive(true);
                        Debug.Log($"PVP: Re-enabled root GameObject '{rootObject.name}'");
                    }
                }
                disabledRootObjects.Clear();
            }
        }

        private bool gameStarted = false;
        private bool isCancelled = false;
        private int lastKnownPlayerCount = 0;

        public bool IsGameStarted => gameStarted;
        public bool IsCancelled => isCancelled;

        public void StartLobbyLoop()
        {
            gameStarted = false;
            isCancelled = false;
            lastKnownPlayerCount = 0;
            StartCoroutine(nameof(LobbyLoop));
        }

        public void StartRelayRefreshLoop()
        {
            StartCoroutine(nameof(RelayRefreshLoop));
        }

        System.Collections.IEnumerator RelayRefreshLoop()
        {
            float elapsedTime = 0f;
            const float relayRefreshInterval = 50f;

            while (!gameStarted && !isCancelled)
            {
                yield return new WaitForSeconds(1f);
                elapsedTime += 1f;

                if (elapsedTime >= relayRefreshInterval)
                {
                    Debug.Log($"PVP: Refreshing relay allocation ({relayRefreshInterval}s elapsed)");
                    if (PvPBootManager.Instance?.ConnectionManager != null)
                    {
                        PvPBootManager.Instance.ConnectionManager.ClearCachedRelay();
                        Task refreshTask = PvPBootManager.Instance.ConnectionManager.SetupRelayForMatchmaking(BattleCruisers.Data.DataProvider.GameModel.PlayerName);
                        yield return new WaitUntil(() => refreshTask.IsCompleted);

                        if (refreshTask.IsFaulted)
                        {
                            Debug.LogError($"PVP: Relay refresh failed: {refreshTask.Exception?.GetBaseException().Message}");
                        }
                        else
                        {
                            Debug.Log("PVP: Relay allocation refreshed");
                        }
                    }
                    elapsedTime = 0f;
                }
            }

            Debug.Log("PVP: RelayRefreshLoop stopped");
        }
        System.Collections.IEnumerator LobbyLoop()
        {
            Unity.Services.Lobbies.Models.Lobby lobby = PvPBootManager.Instance?.LobbyServiceFacade?.CurrentUnityLobby;
            while (lobby != null)
            {
                yield return new WaitForSeconds(0.1f);
                Unity.Services.Lobbies.Models.Lobby liveLobby = PvPBootManager.Instance?.LobbyServiceFacade?.CurrentUnityLobby;
                if (liveLobby != null)
                {
                    int currentPlayerCount = liveLobby.Players.Count;
                    if (currentPlayerCount != lastKnownPlayerCount)
                    {
                        lastKnownPlayerCount = currentPlayerCount;
                        Debug.Log($"PVP: LobbyLoop - player count changed to {currentPlayerCount}/{liveLobby.MaxPlayers}");
                    }
                    if (currentPlayerCount == 2 && !gameStarted && !isCancelled)
                    {
                        gameStarted = true;
                        Debug.Log("PVP: LobbyLoop - opponent found (Players=2/2), starting match");
                        if (backgroundMusic != null && backgroundMusic.isPlaying)
                            backgroundMusic.Stop();
                        if (enemyFoundMusic != null)
                            enemyFoundMusic.Play();
                        if (fleeButton != null)
                        {
                            fleeButton.SetActive(false);
                            Debug.Log("PVP: LobbyLoop - hiding FLEE button");
                        }
                        if (vsAIButton != null)
                            vsAIButton.SetActive(false);

                        if (!ArenaSelectPanelScreenController.PrivateMatch && PvPBootManager.Instance != null && PvPBootManager.Instance.IsLocalUserHost)
                        {
                            Debug.Log("PVP: LobbyLoop - PublicPVP HOST at Players=2/2, calling StartHostLobby to start NetworkManager");
                            PvPBootManager.Instance.ConnectionManager.StartHostLobby(DataProvider.GameModel.PlayerName);
                        }

                        Debug.Log("PVP: LobbyLoop - re-enabling PvPBattleScene GameObjects at Players=2/2");
                        ReEnableBattleSceneGameObjects();
                        Debug.Log("PVP: LobbyLoop - GameObjects re-enabled, match ready to start");
                        StopCoroutine(nameof(LobbyLoop));
                        yield break;
                    }
                    if (isCancelled)
                    {
                        yield break;
                    }
                }
            }
        }
    }
}
