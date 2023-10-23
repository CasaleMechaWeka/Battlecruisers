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
using BattleCruisers.Utils.PlatformAbstractions.UI;
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
using static BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen.MatchmakingScreenController;
using BattleCruisers.Network.Multiplay.UnityServices;

namespace BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen
{
    public class MatchmakingScreenController : ScreenController
    {
        private ISceneNavigator _sceneNavigator;
        private IPrefabFactory _prefabFactory;
        private IApplicationModel _applicationModel;
        private IDataProvider _dataProvider;
        private IGameModel _gameModel;
        private ITrashTalkData _trashTalkData;
        private ILocTable _storyStrings;
        public Animator animator;
        public TrashTalkBubblesController trashTalkBubbles;

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

        private ILocTable commonStrings;
        private IDataProvider dataProvider;
        private SpriteFetcher spriteFetcher;

        private ILocTable screensSceneStrings;

        public Sprite BlackRig;
        public Sprite Bullshark;
        public Sprite Eagle;
        public Sprite Hammerhead;
        public Sprite HuntressBoss;
        public Sprite Longbow;
        public Sprite ManOfWarBoss;
        public Sprite Megalodon;
        public Sprite Raptor;
        public Sprite Rickshaw;
        public Sprite Rockjaw;
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

        private CaptainExo charlie;
        public GameObject characterOfCharlie;
        public Transform ContainerCaptain;
        public GameObject cameraOfCharacter;
        public RawImage leftCaptain, rightCaptain;
        public RenderTexture hostTexture, clientTexture;

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
            commonStrings = await LocTableFactory.Instance.LoadCommonTableAsync();
            screensSceneStrings = await LocTableFactory.Instance.LoadScreensSceneTableAsync();
            dataProvider = ApplicationModelProvider.ApplicationModel.DataProvider;
            spriteFetcher = new SpriteFetcher();
            sprites.Add("BlackRig", BlackRig);
            sprites.Add("Bullshark", Bullshark);
            sprites.Add("Eagle", Eagle);
            sprites.Add("Hammerhead", Hammerhead);
            sprites.Add("HuntressBoss", HuntressBoss);
            sprites.Add("Longbow", Longbow);
            sprites.Add("ManOfWarBoss", ManOfWarBoss);
            sprites.Add("Megalodon", Megalodon);
            sprites.Add("Raptor", Raptor);
            sprites.Add("Rickshaw", Rickshaw);
            sprites.Add("Rockjaw", Rockjaw);
            sprites.Add("TasDevil", TasDevil);
            sprites.Add("Trident", Trident);
            sprites.Add("Yeti", Yeti);

            DontDestroyOnLoad(gameObject);
            _applicationModel = ApplicationModelProvider.ApplicationModel;
            _dataProvider = _applicationModel.DataProvider;
            _gameModel = _dataProvider.GameModel;
            IPrefabCacheFactory prefabCacheFactory = new PrefabCacheFactory(commonStrings, _dataProvider);

            Logging.Log(Tags.SCREENS_SCENE_GOD, "Pre prefab cache load");
            IPrefabCache prefabCache = await prefabCacheFactory.CreatePrefabCacheAsync(new PrefabFetcher());

            leftCruiserName.text = dataProvider.GameModel.PlayerLoadout.Hull.PrefabName;
            leftPlayerName.text = dataProvider.GameModel.PlayerName;
            int rank = CalculateRank(dataProvider.GameModel.LifetimeDestructionScore);
            leftPlayerRankName.text = commonStrings.GetString(StaticPrefabKeys.Ranks.AllRanks[rank].RankNameKeyBase);
            leftPlayerRankImage.sprite = (await spriteFetcher.GetSpriteAsync("Assets/Resources_moved/Sprites/UI/ScreensScene/DestructionScore/" + StaticPrefabKeys.Ranks.AllRanks[rank].RankImage + ".png")).Sprite;
            leftCruiserImage.sprite = sprites[dataProvider.GameModel.PlayerLoadout.Hull.PrefabName];

            LookingForOpponentsText.text = commonStrings.GetString("LookingForOpponents");
            FoundOpponentText.text = commonStrings.GetString("FoundOpponent");

            _prefabFactory = new PrefabFactory(prefabCache, _dataProvider.SettingsManager, commonStrings);
            charlie = Instantiate(_prefabFactory.GetCaptainExo(_gameModel.PlayerLoadout.CurrentCaptain), ContainerCaptain);
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
        }

        bool isProcessing = false;
        public bool isLoaded = false;
        async void Update()
        {
            if (!isProcessing && !isLoaded)
            {
                isProcessing = true;
                await iLoadingAssets();
            }
/*            if(status == MMStatus.LOOKING_VICTIM && m_TimeLimitLookingVictim.CanCall)
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
                    LookingForOpponentsText.text = commonStrings.GetString("FindingLobby");
                    break;
                case MMStatus.JOIN_LOBBY:
                    LookingForOpponentsText.text = commonStrings.GetString("JoiningLobby");
                    break;
                case MMStatus.CONNECTING:
                    LookingForOpponentsText.text = commonStrings.GetString("Connecting");
                    break;
                case MMStatus.CREATING_LOBBY:
                    LookingForOpponentsText.text = commonStrings.GetString("CreatingLobby");
                    break;
                case MMStatus.LOADING_ASSETS:
                    LookingForOpponentsText.text = commonStrings.GetString("LoadingAssets");
                    break;
                case MMStatus.LOOKING_VICTIM:
                    LookingForOpponentsText.text = commonStrings.GetString("LookingVictim");
                    break;
            }
        }

        public void LockLobby()
        {
            if (GameObject.Find("ConnectionManager") != null)
                GameObject.Find("ConnectionManager").GetComponent<ConnectionManager>().LockLobby();
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
            leftCruiserName.text = SynchedServerData.Instance.playerAPrefabName.Value;
            int rankA = CalculateRank(SynchedServerData.Instance.playerAScore.Value);
            ISpriteWrapper spriteWrapperA = await spriteFetcher.GetSpriteAsync("Assets/Resources_moved/Sprites/UI/ScreensScene/DestructionScore/" + StaticPrefabKeys.Ranks.AllRanks[rankA].RankImage + ".png");
            leftPlayerRankImage.sprite = spriteWrapperA.Sprite;
            leftPlayerRankName.text = commonStrings.GetString(StaticPrefabKeys.Ranks.AllRanks[rankA].RankNameKeyBase);
            leftCruiserImage.sprite = sprites.ContainsKey(SynchedServerData.Instance.playerAPrefabName.Value) ? sprites[SynchedServerData.Instance.playerAPrefabName.Value] : Trident;

            rightPlayerName.text = SynchedServerData.Instance.playerBName.Value;
            rightCruiserName.text = SynchedServerData.Instance.playerBPrefabName.Value;
            int rankB = CalculateRank(SynchedServerData.Instance.playerBScore.Value);
            ISpriteWrapper spriteWrapperB = await spriteFetcher.GetSpriteAsync("Assets/Resources_moved/Sprites/UI/ScreensScene/DestructionScore/" + StaticPrefabKeys.Ranks.AllRanks[rankB].RankImage + ".png");
            rightPlayerRankeImage.sprite = spriteWrapperB.Sprite;
            rightPlayerRankeName.text = commonStrings.GetString(StaticPrefabKeys.Ranks.AllRanks[rankB].RankNameKeyBase);
            rightCruiserImage.sprite = sprites.ContainsKey(SynchedServerData.Instance.playerBPrefabName.Value) ? sprites[SynchedServerData.Instance.playerBPrefabName.Value] : Trident;
            if (SynchedServerData.Instance.GetTeam() == Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Team.LEFT)
            {
                rightCaptain.texture = clientTexture;
            }
            else
            {
                leftCaptain.texture = hostTexture;
                rightCaptain.texture = clientTexture;
            }
            await Task.Delay(100);
            animator.SetBool("Found", true);
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
            Invoke("Destroy", 0.5f);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}

