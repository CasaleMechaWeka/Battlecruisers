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
using BattleCruisers.Data.Models;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI;
using BattleCruisers.UI.ScreensScene.BattleHubScreen;
using BattleCruisers.Network.Multiplay.Scenes;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;

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
        private GameModel _gameModel;
        public Animator animator;

        public PvPMessageBox messageBox;

        public Text leftPlayerName;
        public Image leftPlayerRankImage;
        public Text leftPlayerRankName;
        public Text leftPlayerBounty;
        public Text leftCruiserName;
        public Image leftCruiserImage;

        public Text rightPlayerName;
        public Image rightPlayerRankeImage;
        public Text rightPlayerRankeName;
        public Text rightPlayerBounty;
        public Text rightCruiserName;
        public Image rightCruiserImage;

        public Text vsTitile;
        public Text LookingForOpponentsText;
        public Text FoundOpponentText;

        public Sprite BlackRig;
        public Sprite BasicRig;
        public Sprite Bullshark;
        public Sprite Cricket;
        public Sprite Eagle;
        public Sprite Flea;
        public Sprite Goatherd;
        public Sprite Hammerhead;
        public Sprite HuntressBoss;
        public Sprite Longbow;
        public Sprite ManOfWarBoss;
        public Sprite Megalodon;
        public Sprite Megalith;
        public Sprite Microlodon;
        public Sprite Raptor;
        public Sprite Rickshaw;
        public Sprite Rockjaw;
        public Sprite Pistol;
        public Sprite Shepherd;
        public Sprite TasDevil;
        public Sprite Trident;
        public Sprite Yeti;

        private Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

        public GameObject fleeButton;
        public GameObject vsAIButton;

        public static bool MatchmakingFailed;

        private CaptainExo charlie;
        public GameObject characterOfCharlie;
        public Transform ContainerCaptain;

        public RawImage leftCaptain, rightCaptain;
        public RenderTexture hostTexture, clientTexture;
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
        public MMStatus status = MMStatus.FINDING_LOBBY;

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
            sprites.Add("BlackRig", BlackRig);
            sprites.Add("BasicRig", BasicRig);
            sprites.Add("Bullshark", Bullshark);
            sprites.Add("Cricket", Cricket);
            sprites.Add("Eagle", Eagle);
            sprites.Add("Flea", Flea);
            sprites.Add("Goatherd", Goatherd);
            sprites.Add("Hammerhead", Hammerhead);
            sprites.Add("HuntressBoss", HuntressBoss);
            sprites.Add("Longbow", Longbow);
            sprites.Add("ManOfWarBoss", ManOfWarBoss);
            sprites.Add("Megalodon", Megalodon);
            sprites.Add("Megalith", Megalith);
            sprites.Add("Microlodon", Microlodon);
            sprites.Add("Raptor", Raptor);
            sprites.Add("Rickshaw", Rickshaw);
            sprites.Add("Rockjaw", Rockjaw);
            sprites.Add("Pistol", Pistol);
            sprites.Add("Shepherd", Shepherd);
            sprites.Add("TasDevil", TasDevil);
            sprites.Add("Trident", Trident);
            sprites.Add("Yeti", Yeti);

            DontDestroyOnLoad(gameObject);
            Debug.Log("PVP: Pre-loading PvPBattleScene");
            StartCoroutine(PreloadBattleSceneCoroutine());

            _gameModel = DataProvider.GameModel;
            Logging.Log(Tags.SCREENS_SCENE_GOD, "Pre prefab cache load");
            //    leftCruiserName.text = DataProvider.GameModel.PlayerLoadout.Hull.PrefabName;
            leftPlayerName.text = DataProvider.GameModel.PlayerName;
            int rank = CalculateRank(DataProvider.GameModel.LifetimeDestructionScore);
            leftPlayerRankName.text = LocTableCache.CommonTable.GetString(StaticPrefabKeys.Ranks.AllRanks[rank].RankNameKeyBase);
            leftPlayerRankImage.sprite = await SpriteFetcher.GetSpriteAsync("Assets/Resources_moved/Sprites/UI/ScreensScene/DestructionScore/" + StaticPrefabKeys.Ranks.AllRanks[rank].RankImage + ".png");
            leftPlayerBounty.text = DataProvider.GameModel.Bounty.ToString();
            // show bodykit of left player in MM if owned
            int id_bodykitA = DataProvider.GameModel.PlayerLoadout.SelectedBodykit;
            if (id_bodykitA != -1)
            {
                Bodykit bodykit = PrefabFactory.GetBodykit(StaticPrefabKeys.BodyKits.GetBodykitKey(id_bodykitA));
                if (bodykit.cruiserType == GetHullType(DataProvider.GameModel.PlayerLoadout.Hull.PrefabName))
                {
                    leftCruiserName.text = LocTableCache.CommonTable.GetString(StaticData.Bodykits[id_bodykitA].NameStringKeyBase);
                    leftCruiserImage.sprite = bodykit.BodykitImage;
                }
            }
            else
            {
                leftCruiserName.text = LocTableCache.CommonTable.GetString("Cruisers/" + DataProvider.GameModel.PlayerLoadout.Hull.PrefabName + "Name");
                leftCruiserImage.sprite = sprites[DataProvider.GameModel.PlayerLoadout.Hull.PrefabName];
            }

            LookingForOpponentsText.text = LocTableCache.CommonTable.GetString("LookingForOpponents");
            FoundOpponentText.text = LocTableCache.CommonTable.GetString("FoundOpponent");


            CaptainExo charliePrefab = PrefabFactory.GetCaptainExo(_gameModel.PlayerLoadout.CurrentCaptain);
            charlie = Instantiate(charliePrefab, ContainerCaptain);
            charlie.gameObject.transform.localScale = Vector3.one * 0.4f;
            characterOfCharlie = charlie.gameObject;

            switch ((Map)DataProvider.GameModel.GameMap)
            {
                case Map.PracticeWreckyards:
                    vsTitile.text = LocTableCache.ScreensSceneTable.GetString("Arena01Name");
                    break;
                case Map.OzPenitentiary:
                    vsTitile.text = LocTableCache.ScreensSceneTable.GetString("Arena02Name");
                    break;
                case Map.UACUltimate:
                    vsTitile.text = LocTableCache.ScreensSceneTable.GetString("Arena08Name");
                    break;
                case Map.RioBattlesport:
                    vsTitile.text = LocTableCache.ScreensSceneTable.GetString("Arena07Name");
                    break;
                case Map.NuclearDome:
                    vsTitile.text = LocTableCache.ScreensSceneTable.GetString("Arena05Name");
                    break;
                case Map.MercenaryOne:
                    vsTitile.text = LocTableCache.ScreensSceneTable.GetString("Arena09Name");
                    break;
                case Map.SanFranciscoFightClub:
                    vsTitile.text = LocTableCache.ScreensSceneTable.GetString("Arena03Name");
                    break;
                case Map.UACArena:
                    vsTitile.text = LocTableCache.ScreensSceneTable.GetString("Arena06Name");
                    break;
                case Map.UACBattleNight:
                    vsTitile.text = LocTableCache.ScreensSceneTable.GetString("Arena04Name");
                    break;
            }

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


        private HullType GetHullType(string hullName)
        {
            return hullName switch
            {
                "Trident" => HullType.Trident,
                "BlackRig" => HullType.BlackRig,
                "BasicRig" => HullType.BasicRig,
                "Bullshark" => HullType.Bullshark,
                "Cricket" => HullType.Cricket,
                "Eagle" => HullType.Eagle,
                "Flea" => HullType.Flea,
                "Goatherd" => HullType.Goatherd,
                "Hammerhead" => HullType.Hammerhead,
                "Longbow" => HullType.Longbow,
                "Megalodon" => HullType.Megalodon,
                "Megalith" => HullType.Megalith,
                "Microlodon" => HullType.Microlodon,
                "Raptor" => HullType.Raptor,
                "Rickshaw" => HullType.Rickshaw,
                "Rockjaw" => HullType.Rockjaw,
                "Pistol" => HullType.Pistol,
                "Shepherd" => HullType.Shepherd,
                "TasDevil" => HullType.TasDevil,
                "Yeti" => HullType.Yeti,
                _ => HullType.None,
            };

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
            SetMMString(status);

            /*            switch(status)
                        {
                            case MMStatus.LOOKING_VICTIM:
                                m_TimeLimitLookingVictim.PutOnCooldown();
                                break;
                        }*/
        }

        private void SetMMString(MMStatus _status)
        {
            switch (_status)
            {
                case MMStatus.FINDING_LOBBY:
                    LookingForOpponentsText.text = LocTableCache.CommonTable.GetString("FindingLobby");
                    break;
                case MMStatus.JOIN_LOBBY:
                    LookingForOpponentsText.text = LocTableCache.CommonTable.GetString("JoiningLobby");
                    break;
                case MMStatus.CONNECTING:
                    LookingForOpponentsText.text = LocTableCache.CommonTable.GetString("Connecting");
                    break;
                case MMStatus.CREATING_LOBBY:
                    LookingForOpponentsText.text = LocTableCache.CommonTable.GetString("CreatingLobby");
                    break;
                case MMStatus.LOADING_ASSETS:
                    LookingForOpponentsText.text = LocTableCache.CommonTable.GetString("LoadingAssets");
                    break;
                case MMStatus.LOOKING_VICTIM:
                    LookingForOpponentsText.text = LocTableCache.CommonTable.GetString("LookingVictim");
                    break;
            }
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
            SetMMStatus(MMStatus.LOADING_ASSETS);
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
            int rankA = CalculateRank(SynchedServerData.Instance.playerAScore.Value);
            Sprite spriteA = await SpriteFetcher.GetSpriteAsync("Assets/Resources_moved/Sprites/UI/ScreensScene/DestructionScore/" + StaticPrefabKeys.Ranks.AllRanks[rankA].RankImage + ".png");
            if (leftPlayerRankImage != null) leftPlayerRankImage.sprite = spriteA;
            if (leftPlayerRankName != null) leftPlayerRankName.text = LocTableCache.CommonTable.GetString(StaticPrefabKeys.Ranks.AllRanks[rankA].RankNameKeyBase);
            //    leftCruiserImage.sprite = sprites.ContainsKey(SynchedServerData.Instance.playerAPrefabName.Value) ? sprites[SynchedServerData.Instance.playerAPrefabName.Value] : Trident;

            if (rightPlayerName == null) return;
            rightPlayerName.text = SynchedServerData.Instance.playerBName.Value;
            if (rightPlayerBounty == null) return;
            rightPlayerBounty.text = SynchedServerData.Instance.playerBBounty.Value.ToString();
            //    rightCruiserName.text = SynchedServerData.Instance.playerBPrefabName.Value;
            int rankB = CalculateRank(SynchedServerData.Instance.playerBScore.Value);
            Sprite spriteB = await SpriteFetcher.GetSpriteAsync("Assets/Resources_moved/Sprites/UI/ScreensScene/DestructionScore/" + StaticPrefabKeys.Ranks.AllRanks[rankB].RankImage + ".png");
            if (rightPlayerRankeImage != null) rightPlayerRankeImage.sprite = spriteB;
            if (rightPlayerRankeName != null) rightPlayerRankeName.text = LocTableCache.CommonTable.GetString(StaticPrefabKeys.Ranks.AllRanks[rankB].RankNameKeyBase);
            //    rightCruiserImage.sprite = sprites.ContainsKey(SynchedServerData.Instance.playerBPrefabName.Value) ? sprites[SynchedServerData.Instance.playerBPrefabName.Value] : Trident;

            // apply bodykit of right player


            // apply bodykit of left player
            int id_bodykitA = SynchedServerData.Instance.playerABodykit.Value;
            if (id_bodykitA != -1)
            {
                Bodykit bodykit = PrefabFactory.GetBodykit(StaticPrefabKeys.BodyKits.GetBodykitKey(id_bodykitA));
                if (bodykit.cruiserType == GetHullType(SynchedServerData.Instance.playerAPrefabName.Value))
                {
                    if (leftCruiserName != null) leftCruiserName.text = LocTableCache.CommonTable.GetString(StaticData.Bodykits[id_bodykitA].NameStringKeyBase);
                    if (leftCruiserImage != null) leftCruiserImage.sprite = bodykit.bodykitImage;
                }
            }
            else
            {
                if (leftCruiserName != null) leftCruiserName.text = LocTableCache.CommonTable.GetString("Cruisers/" + SynchedServerData.Instance.playerAPrefabName.Value + "Name");
                if (leftCruiserImage != null) leftCruiserImage.sprite = sprites.ContainsKey(SynchedServerData.Instance.playerAPrefabName.Value) ? sprites[SynchedServerData.Instance.playerAPrefabName.Value] : Trident;
            }

            int id_bodykitB = SynchedServerData.Instance.playerBBodykit.Value;
            if (id_bodykitB != -1)
            {
                Bodykit bodykit = PrefabFactory.GetBodykit(StaticPrefabKeys.BodyKits.GetBodykitKey(id_bodykitB));
                if (bodykit.cruiserType == GetHullType(SynchedServerData.Instance.playerBPrefabName.Value))
                {
                    if (rightCruiserName != null) rightCruiserName.text = LocTableCache.CommonTable.GetString(StaticData.Bodykits[id_bodykitB].NameStringKeyBase);
                    if (rightCruiserImage != null) rightCruiserImage.sprite = bodykit.bodykitImage;
                }
            }
            else
            {
                if (rightCruiserName != null) rightCruiserName.text = LocTableCache.CommonTable.GetString("Cruisers/" + SynchedServerData.Instance.playerBPrefabName.Value + "Name");
                if (rightCruiserImage != null) rightCruiserImage.sprite = sprites.ContainsKey(SynchedServerData.Instance.playerBPrefabName.Value) ? sprites[SynchedServerData.Instance.playerBPrefabName.Value] : Trident;
            }


            if (SynchedServerData.Instance.GetTeam() == Team.LEFT)
            {
                if (rightCaptain != null) rightCaptain.texture = clientTexture;
            }
            else
            {
                if (leftCaptain != null) leftCaptain.texture = hostTexture;
                if (rightCaptain != null) rightCaptain.texture = clientTexture;
            }

            if (backgroundMusic != null && backgroundMusic.isPlaying)
                backgroundMusic.Stop();

            if (enemyFoundMusic != null)
                enemyFoundMusic.Play();

            animator.SetBool("Found", true);
            LeaveLobby();
        }

        private int CalculateRank(long score)
        {
            for (int i = 0; i <= StaticPrefabKeys.Ranks.AllRanks.Count - 1; i++)
            {
                long x = 2500 + 2500 * i * i;
                //Debug.Log(x);
                if (score < x)
                {
                    return i;
                }
            }
            return StaticPrefabKeys.Ranks.AllRanks.Count - 1;
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
