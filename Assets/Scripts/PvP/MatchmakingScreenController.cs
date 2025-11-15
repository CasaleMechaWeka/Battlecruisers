using System.Collections.Generic;
using BattleCruisers.Data;
using BattleCruisers.Utils;
using UnityEngine;
using System.Threading.Tasks;
using BattleCruisers.Utils.Localisation;
using UnityEngine.UI;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.ApplicationLifecycle;
using BattleCruisers.Network.Multiplay.ConnectionManagement;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI;
using BattleCruisers.UI.ScreensScene.BattleHubScreen;
using BattleCruisers.Network.Multiplay.Scenes;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;
using BattleCruisers.Utils.Fetchers.Cache;
using BattleCruisers.PostBattleScreen;
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
            leftPlayerRankName.text = LocTableCache.CommonTable.GetString(StaticPrefabKeys.Ranks.AllRanks[rank].RankNameKeyBase);
            leftPlayerRankImage.sprite = await SpriteFetcher.GetSpriteAsync("Assets/Resources_moved/Sprites/UI/ScreensScene/DestructionScore/" + StaticPrefabKeys.Ranks.AllRanks[rank].RankImage + ".png");
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
            Debug.Log("PVP: MatchmakingScreenController.OnFlee - User clicked FLEE");
            fleeButton.SetActive(false);

            if (PvPBattleSceneGodClient.Instance != null)
            {
                PvPBattleSceneGodClient.Instance.WasLeftMatch = true;
                PvPBattleSceneGodClient.Instance.HandleClientDisconnected();
            }

            FailedMatchmaking();
        }
        public async void FoundCompetitor()
        {
            if (leftPlayerName == null) return;
            leftPlayerName.text = SynchedServerData.Instance.playerAName.Value;
            if (leftPlayerBounty == null) return;
            leftPlayerBounty.text = SynchedServerData.Instance.playerABounty.Value.ToString();
            //    leftCruiserName.text = SynchedServerData.Instance.playerAPrefabName.Value;
            int rankA = DestructionRanker.CalculateRank(SynchedServerData.Instance.playerAScore.Value);
            Sprite spriteA = await SpriteFetcher.GetSpriteAsync("Assets/Resources_moved/Sprites/UI/ScreensScene/DestructionScore/" + StaticPrefabKeys.Ranks.AllRanks[rankA].RankImage + ".png");
            if (leftPlayerRankImage != null) leftPlayerRankImage.sprite = spriteA;
            if (leftPlayerRankName != null) leftPlayerRankName.text = LocTableCache.CommonTable.GetString(StaticPrefabKeys.Ranks.AllRanks[rankA].RankNameKeyBase);
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
            Debug.Log("PVP: MatchmakingScreenController.FailedMatchmaking - Matchmaking failed");

            if (backgroundMusic != null && backgroundMusic.isPlaying)
                backgroundMusic.Stop();

            if (isPvPBattleScenePreloaded)
            {
                Debug.Log("PVP: Unloading pre-loaded PvPBattleScene (FailedMatchmaking)");
                SceneManager.UnloadSceneAsync("PvPBattleScene");
                isPvPBattleScenePreloaded = false;
                disabledRootObjects.Clear();
            }

            if (ArenaSelectPanelReference != null)
            {
                Debug.Log("PVP: Resetting ArenaSelectPanel state (FailedMatchmaking)");
                ArenaSelectPanelReference.ResetBattleButtonState();
            }

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
    }
}
