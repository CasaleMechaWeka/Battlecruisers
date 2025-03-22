using System.Collections.Generic;
using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine;
using System.Threading.Tasks;
using BattleCruisers.Network.Multiplay.Gameplay.UI;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.Utils.Localisation;
using UnityEngine.UI;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.ApplicationLifecycle;
using BattleCruisers.Network.Multiplay.ConnectionManagement;
using BattleCruisers.Network.Multiplay.Infrastructure;
using System;
using BattleCruisers.Data.Models;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Cache;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Scenes;
using BattleCruisers.Network.Multiplay.UnityServices;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI;

namespace BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen
{
    public class MatchmakingScreenController : ScreenController
    {
        private ISceneNavigator _sceneNavigator;
        private PrefabFactory _prefabFactory;
        private IApplicationModel _applicationModel;
        private IDataProvider _dataProvider;
        private IGameModel _gameModel;
        private ITrashTalkData _trashTalkData;
        private ILocTable _storyStrings;
        public Animator animator;
        public TrashTalkBubblesController trashTalkBubbles;
        public PvPMessageBox messageBox;


        public Text leftPlayerName;
        public Image leftPlayerRankImage;
        public Text leftPlayerRankName;
        public Text leftCruiserName;
        public Image leftCruiserImage;

        public Text rightPlayerName;
        public Image rightPlayerRankeImage;
        public Text rightPlayerRankeName;
        public Text rightCruiserName;
        public Image rightCruiserImage;


        public Text vsTitile;
        public Text LookingForOpponentsText;
        public Text FoundOpponentText;
        public Slider LoadingBar;
        public GameObject LoadingBarParent;

        // private ILocTable commonStrings;
        private IDataProvider dataProvider;

        private ILocTable screensSceneStrings;

        public Sprite BlackRig;
        public Sprite Bullshark;
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
        public Action CanceledMatchmaking;
        public GameObject fleeButton;
        public GameObject vsAIButton;

        // PlayerA data should be stored here temporalliy
        public string playerAPrefabName;
        public ulong playerAClientNetworkId;
        public string playerAName;
        public long playerAScore;
        public string captainAPrefabName;
        public float playerRating;
        public int playerABodykit;
        public string playerASelectedVariants;

        private CaptainExo charlie;
        public GameObject characterOfCharlie;
        public Transform ContainerCaptain;
        public GameObject cameraOfCharacter;
        public RawImage leftCaptain, rightCaptain;
        public RenderTexture hostTexture, clientTexture;
        public AudioSource backgroundMusic;
        public AudioSource enemyFoundMusic;

        public static MatchmakingScreenController Instance { get; private set; }

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
        public enum ConnectionQuality
        {
            DEAD,
            LOW,
            MID,
            HIGH
        }
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
        private RateLimitCooldown m_TimeLimitLookingVictim;
        public override void OnPresenting(object activationParameter)
        {

        }
        public void Initialise(IScreensSceneGod matchmakingLoadingSceneGod,
                           ISingleSoundPlayer soundPlayer,
                           IDataProvider dataProvider)
        {
            base.Initialise(matchmakingLoadingSceneGod);
            Helper.AssertIsNotNull(dataProvider);
        }
        async void Start()
        {
            Instance = this;
            Connection_Quality = ConnectionQuality.HIGH;
            LoadingBarParent.SetActive(false);
            _sceneNavigator = LandingSceneGod.SceneNavigator;
            _commonStrings = await LocTableFactory.LoadCommonTableAsync();
            screensSceneStrings = await LocTableFactory.LoadScreensSceneTableAsync();
            dataProvider = ApplicationModelProvider.ApplicationModel.DataProvider;
            sprites.Add("BlackRig", BlackRig);
            sprites.Add("Bullshark", Bullshark);
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
            _applicationModel = ApplicationModelProvider.ApplicationModel;
            _dataProvider = _applicationModel.DataProvider;
            _gameModel = _dataProvider.GameModel;
            PrefabCacheFactory prefabCacheFactory = new PrefabCacheFactory(_commonStrings);

            Logging.Log(Tags.SCREENS_SCENE_GOD, "Pre prefab cache load");
            PrefabCache prefabCache = await prefabCacheFactory.CreatePrefabCacheAsync();
            _prefabFactory = new PrefabFactory(prefabCache, _dataProvider.SettingsManager, _commonStrings);
            //    leftCruiserName.text = dataProvider.GameModel.PlayerLoadout.Hull.PrefabName;
            leftPlayerName.text = dataProvider.GameModel.PlayerName;
            int rank = CalculateRank(dataProvider.GameModel.LifetimeDestructionScore);
            leftPlayerRankName.text = _commonStrings.GetString(StaticPrefabKeys.Ranks.AllRanks[rank].RankNameKeyBase);
            leftPlayerRankImage.sprite = await SpriteFetcher.GetSpriteAsync("Assets/Resources_moved/Sprites/UI/ScreensScene/DestructionScore/" + StaticPrefabKeys.Ranks.AllRanks[rank].RankImage + ".png");
            // show bodykit of left player in MM if owned
            int id_bodykitA = _dataProvider.GameModel.PlayerLoadout.SelectedBodykit;
            if (id_bodykitA != -1)
            {
                Bodykit bodykit = _prefabFactory.GetBodykit(StaticPrefabKeys.BodyKits.GetBodykitKey(id_bodykitA));
                if (bodykit.cruiserType == GetHullType(_dataProvider.GameModel.PlayerLoadout.Hull.PrefabName))
                {
                    leftCruiserName.text = _commonStrings.GetString(dataProvider.StaticData.Bodykits[id_bodykitA].NameStringKeyBase);
                    leftCruiserImage.sprite = bodykit.BodykitImage;
                }
            }
            else
            {
                leftCruiserName.text = _commonStrings.GetString("Cruisers/" + dataProvider.GameModel.PlayerLoadout.Hull.PrefabName + "Name");
                leftCruiserImage.sprite = sprites[dataProvider.GameModel.PlayerLoadout.Hull.PrefabName];
            }

            LookingForOpponentsText.text = _commonStrings.GetString("LookingForOpponents");
            FoundOpponentText.text = _commonStrings.GetString("FoundOpponent");


            CaptainExo charliePrefab = _prefabFactory.GetCaptainExo(_gameModel.PlayerLoadout.CurrentCaptain);
            charlie = Instantiate(charliePrefab, ContainerCaptain);
            charlie.gameObject.transform.localScale = Vector3.one * 0.4f;
            characterOfCharlie = charlie.gameObject;

            switch ((Map)dataProvider.GameModel.GameMap)
            {
                case Map.PracticeWreckyards:
                    vsTitile.text = screensSceneStrings.GetString("Arena01Name");
                    break;
                case Map.OzPenitentiary:
                    vsTitile.text = screensSceneStrings.GetString("Arena02Name");
                    break;
                case Map.UACUltimate:
                    vsTitile.text = screensSceneStrings.GetString("Arena08Name");
                    break;
                case Map.RioBattlesport:
                    vsTitile.text = screensSceneStrings.GetString("Arena07Name");
                    break;
                case Map.NuclearDome:
                    vsTitile.text = screensSceneStrings.GetString("Arena05Name");
                    break;
                case Map.MercenaryOne:
                    vsTitile.text = screensSceneStrings.GetString("Arena09Name");
                    break;
                case Map.SanFranciscoFightClub:
                    vsTitile.text = screensSceneStrings.GetString("Arena03Name");
                    break;
                case Map.UACArena:
                    vsTitile.text = screensSceneStrings.GetString("Arena06Name");
                    break;
                case Map.UACBattleNight:
                    vsTitile.text = screensSceneStrings.GetString("Arena04Name");
                    break;
            }

            //    m_TimeLimitLookingVictim = new RateLimitCooldown(5f);
            if (backgroundMusic != null)
            {
                backgroundMusic.Play();
                if (enemyFoundMusic.isPlaying && enemyFoundMusic != null)
                {
                    enemyFoundMusic.Stop();
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
            switch (hullName)
            {
                case "Trident":
                    return HullType.Trident;
                case "BlackRig":
                    return HullType.BlackRig;
                case "Bullshark":
                    return HullType.Bullshark;
                case "Eagle":
                    return HullType.Eagle;
                case "Flea":
                    return HullType.Flea;
                case "Goatherd":
                    return HullType.Goatherd;
                case "Hammerhead":
                    return HullType.Hammerhead;
                case "Longbow":
                    return HullType.Longbow;
                case "Megalodon":
                    return HullType.Megalodon;
                case "Megalith":
                    return HullType.Megalith;
                case "Microlodon":
                    return HullType.Microlodon;
                case "Raptor":
                    return HullType.Raptor;
                case "Rickshaw":
                    return HullType.Rickshaw;
                case "Rockjaw":
                    return HullType.Rockjaw;
                case "Pistol":
                    return HullType.Pistol;
                case "Shepherd":
                    return HullType.Shepherd;
                case "TasDevil":
                    return HullType.TasDevil;
                case "Yeti":
                    return HullType.Yeti;
            }
            return HullType.None;
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
            /*if(status == MMStatus.LOOKING_VICTIM && m_TimeLimitLookingVictim.CanCall)
            {
                SetFoundVictimString();
                if (GameObject.Find("ConnectionManager") != null)
                    GameObject.Find("ConnectionManager").GetComponent<ConnectionManager>().LockLobby();
                if(GameObject.Find("PvPBattleSceneGod") != null)
                    GameObject.Find("PvPBattleSceneGod").GetComponent<PvPBattleSceneGodServer>().RunPvP_AIMode();
                m_TimeLimitLookingVictim.PutOnCooldown(9999f);
            }*/
        }

        async Task iLoadingAssets()
        {
            await Task.Delay(10);
            if (isLoaded)
                return;

            NetworkObject[] objs = GameObject.FindObjectsOfType<NetworkObject>();
            LoadingBar.value = objs.Length;
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
                    LookingForOpponentsText.text = _commonStrings.GetString("FindingLobby");
                    break;
                case MMStatus.JOIN_LOBBY:
                    LookingForOpponentsText.text = _commonStrings.GetString("JoiningLobby");
                    break;
                case MMStatus.CONNECTING:
                    LookingForOpponentsText.text = _commonStrings.GetString("Connecting");
                    break;
                case MMStatus.CREATING_LOBBY:
                    LookingForOpponentsText.text = _commonStrings.GetString("CreatingLobby");
                    break;
                case MMStatus.LOADING_ASSETS:
                    LookingForOpponentsText.text = _commonStrings.GetString("LoadingAssets");
                    break;
                case MMStatus.LOOKING_VICTIM:
                    LookingForOpponentsText.text = _commonStrings.GetString("LookingVictim");
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
            LoadingBarParent.SetActive(true);
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
        public void AddProgress(int step)
        {
            LoadingBar.value += step;
        }
        public void SetTraskTalkData(ITrashTalkData trashTalkData, ILocTable commonString, ILocTable storyString)
        {
            _trashTalkData = trashTalkData;
            _commonStrings = commonString;
            _storyStrings = storyString;
        }

        public void OnFlee()
        {
            fleeButton.SetActive(false);
            if (PvPBootManager.Instance != null)
                PvPBootManager.Instance.m_CancellationToken.Cancel();
            if (PvPBattleSceneGodClient.Instance != null)
            {
                PvPBattleSceneGodClient.Instance.WasLeftMatch = true;
                PvPBattleSceneGodClient.Instance.HandleClientDisconnected();
            }
            if (GameObject.Find("ConnectionManager") != null)
                GameObject.Find("ConnectionManager").GetComponent<ConnectionManager>().ChangeState(GameObject.Find("ConnectionManager").GetComponent<ConnectionManager>().m_Offline);

            if (backgroundMusic.isPlaying)
                backgroundMusic.Stop();
        }

        public void VsAI()
        {
            vsAIButton.SetActive(false);
            ApplicationModelProvider.ApplicationModel.Mode = Data.GameMode.CoinBattle;
            SaveCoinBattleSettings();
            int maxLevel = dataProvider.GameModel.NumOfLevelsCompleted; //might need null or not-0 check?
            int levelIndex = UnityEngine.Random.Range(1, maxLevel);
            LandingSceneGod.Instance.coinBattleLevelNum = levelIndex;
            if (PvPBattleSceneGodClient.Instance != null)
            {
                PvPBattleSceneGodClient.Instance.WasLeftMatch = true;
                PvPBattleSceneGodClient.Instance.HandleClientDisconnected();
            }
            if (GameObject.Find("ConnectionManager") != null)
                GameObject.Find("ConnectionManager").GetComponent<ConnectionManager>().ChangeState(GameObject.Find("ConnectionManager").GetComponent<ConnectionManager>().m_Offline);

            if (backgroundMusic.isPlaying)
                backgroundMusic.Stop();
        }

        private void SaveCoinBattleSettings()
        {
            dataProvider.GameModel.CoinBattle
                = new CoinBattleModel(
                    dataProvider.SettingsManager.AIDifficulty,
                    dataProvider.GameModel.PlayerLoadout.Hull
                    );
            dataProvider.SaveGame();
        }
        public async void FoundCompetitor()
        {
            leftPlayerName.text = SynchedServerData.Instance.playerAName.Value;
            //    leftCruiserName.text = SynchedServerData.Instance.playerAPrefabName.Value;
            int rankA = CalculateRank(SynchedServerData.Instance.playerAScore.Value);
            leftPlayerRankImage.sprite = await SpriteFetcher.GetSpriteAsync("Assets/Resources_moved/Sprites/UI/ScreensScene/DestructionScore/" + StaticPrefabKeys.Ranks.AllRanks[rankA].RankImage + ".png");
            leftPlayerRankName.text = _commonStrings.GetString(StaticPrefabKeys.Ranks.AllRanks[rankA].RankNameKeyBase);
            //    leftCruiserImage.sprite = sprites.ContainsKey(SynchedServerData.Instance.playerAPrefabName.Value) ? sprites[SynchedServerData.Instance.playerAPrefabName.Value] : Trident;

            rightPlayerName.text = SynchedServerData.Instance.playerBName.Value;
            //    rightCruiserName.text = SynchedServerData.Instance.playerBPrefabName.Value;
            int rankB = CalculateRank(SynchedServerData.Instance.playerBScore.Value);
            rightPlayerRankeImage.sprite = await SpriteFetcher.GetSpriteAsync("Assets/Resources_moved/Sprites/UI/ScreensScene/DestructionScore/" + StaticPrefabKeys.Ranks.AllRanks[rankB].RankImage + ".png");
            rightPlayerRankeName.text = _commonStrings.GetString(StaticPrefabKeys.Ranks.AllRanks[rankB].RankNameKeyBase);
            //    rightCruiserImage.sprite = sprites.ContainsKey(SynchedServerData.Instance.playerBPrefabName.Value) ? sprites[SynchedServerData.Instance.playerBPrefabName.Value] : Trident;

            // apply bodykit of right player


            // apply bodykit of left player
            int id_bodykitA = SynchedServerData.Instance.playerABodykit.Value;
            if (id_bodykitA != -1)
            {
                Bodykit bodykit = _prefabFactory.GetBodykit(StaticPrefabKeys.BodyKits.GetBodykitKey(id_bodykitA));
                if (bodykit.cruiserType == GetHullType(SynchedServerData.Instance.playerAPrefabName.Value))
                {
                    leftCruiserName.text = _commonStrings.GetString(dataProvider.StaticData.Bodykits[id_bodykitA].NameStringKeyBase);
                    leftCruiserImage.sprite = bodykit.bodykitImage;
                }
            }
            else
            {
                leftCruiserName.text = _commonStrings.GetString("Cruisers/" + SynchedServerData.Instance.playerAPrefabName.Value + "Name");
                leftCruiserImage.sprite = sprites.ContainsKey(SynchedServerData.Instance.playerAPrefabName.Value) ? sprites[SynchedServerData.Instance.playerAPrefabName.Value] : Trident;
            }

            int id_bodykitB = SynchedServerData.Instance.playerBBodykit.Value;
            if (id_bodykitB != -1)
            {
                Bodykit bodykit = _prefabFactory.GetBodykit(StaticPrefabKeys.BodyKits.GetBodykitKey(id_bodykitB));
                if (bodykit.cruiserType == GetHullType(SynchedServerData.Instance.playerBPrefabName.Value))
                {
                    rightCruiserName.text = _commonStrings.GetString(dataProvider.StaticData.Bodykits[id_bodykitB].NameStringKeyBase);
                    rightCruiserImage.sprite = bodykit.bodykitImage;
                }
            }
            else
            {
                rightCruiserName.text = _commonStrings.GetString("Cruisers/" + SynchedServerData.Instance.playerBPrefabName.Value + "Name");
                rightCruiserImage.sprite = sprites.ContainsKey(SynchedServerData.Instance.playerBPrefabName.Value) ? sprites[SynchedServerData.Instance.playerBPrefabName.Value] : Trident;
            }


            if (SynchedServerData.Instance.GetTeam() == Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Team.LEFT)
            {
                rightCaptain.texture = clientTexture;

            }
            else
            {
                leftCaptain.texture = hostTexture;
                rightCaptain.texture = clientTexture;
            }

            if (backgroundMusic != null && backgroundMusic.isPlaying)
            {
                backgroundMusic.Stop();
            }

            if (enemyFoundMusic != null)
            {
                enemyFoundMusic.Play();
            }
            await Task.Delay(100);
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
            // CanceledMatchmaking();
            if (GameObject.Find("ApplicationController") != null)
                GameObject.Find("ApplicationController").GetComponent<ApplicationController>().DestroyNetworkObject();

            if (GameObject.Find("ConnectionManager") != null)
                GameObject.Find("ConnectionManager").GetComponent<ConnectionManager>().DestroyNetworkObject();

            if (GameObject.Find("PopupPanelManager") != null)
                GameObject.Find("PopupPanelManager").GetComponent<PopupManager>().DestroyNetworkObject();

            if (GameObject.Find("UIMessageManager") != null)
                GameObject.Find("UIMessageManager").GetComponent<ConnectionStatusMessageUIManager>().DestroyNetworkObject();

            if (GameObject.Find("UpdateRunner") != null)
                GameObject.Find("UpdateRunner").GetComponent<UpdateRunner>().DestroyNetworkObject();

            if (GameObject.Find("NetworkManager") != null)
                GameObject.Find("NetworkManager").GetComponent<BCNetworkManager>().DestroyNetworkObject();

            _sceneNavigator.SceneLoaded(SceneNames.PvP_BOOT_SCENE);
            _sceneNavigator.GoToScene(SceneNames.SCREENS_SCENE, true);

            if (backgroundMusic.isPlaying)
                backgroundMusic.Stop();

            Invoke("Destroy", 0.5f);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public void DisableAllAnimatedGameObjects()
        {
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                foreach (Transform child in animator.transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
}


