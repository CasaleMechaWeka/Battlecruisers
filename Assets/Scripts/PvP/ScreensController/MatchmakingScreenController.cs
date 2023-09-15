using System.Collections;
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
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Cache;

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

        public static MatchmakingScreenController Instance { get; private set; }


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
            fleeButton.SetActive(false);
            vsAIButton.SetActive(false);
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
            charlie.gameObject.transform.localScale = Vector3.one * 0.5f;
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
        }
        public void SetFoudVictimString()
        {
            LookingForOpponentsText.text = commonStrings.GetString("LoadingAssets");
            //    LoadingBarParent.SetActive(true);

            // Iterate through all child objects of ContainerCaptain
            foreach (Transform child in ContainerCaptain)
            {
                // Try to get an Animator component from the child object
                Animator animator = child.GetComponent<Animator>();

                // If an Animator exists
                if (animator != null)
                {
                    // Get all animation clips
                    AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

                    // Iterate through all clips and play the one ending with "_Celebrate"
                    foreach (AnimationClip clip in clips)
                    {
                        if (clip.name.EndsWith("_Celebrate"))
                        {
                            animator.Play(clip.name);
                            break; // Exit loop once found and played
                        }
                    }
                }
            }
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
            if (GameObject.Find("ConnectionManager") != null)
                GameObject.Find("ConnectionManager").GetComponent<ConnectionManager>().ChangeState(GameObject.Find("ConnectionManager").GetComponent<ConnectionManager>().m_Offline);
        }

        public void VsAI()
        {
            ApplicationModelProvider.ApplicationModel.Mode = Data.GameMode.CoinBattle;
            SaveCoinBattleSettings();

            int maxLevel = dataProvider.GameModel.NumOfLevelsCompleted; //might need null or not-0 check?
            int levelIndex = UnityEngine.Random.Range(1, maxLevel);
            LandingSceneGod.Instance.coinBattleLevelNum = levelIndex;
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
        IEnumerator iTrashTalk()
        {
            yield return new WaitForSeconds(2f);
            trashTalkBubbles.gameObject.SetActive(true);
            trashTalkBubbles.Initialise(_trashTalkData, _commonStrings, _storyStrings);
        }

        public void NotFound()
        {
            PopupPanel popupPanel = PopupManager.ShowPopupPanel("Matchmaking Error", "You did not find online competitor." +
                " Please check Internet connection!", true, FailedMatchmaking);
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
            Destroy(gameObject);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}

