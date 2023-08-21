using System;
using System.Collections;
using System.Collections.Generic;
using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Scenes;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Common;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using BattleCruisers.Network.Multiplay.Gameplay.UI;
using BattleCruisers.Network.Multiplay.Infrastructure;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.Utils.Localisation;
using UnityEngine.UI;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using BattleCruisers.Data.Static;
using static BattleCruisers.Data.Static.StaticPrefabKeys;
using System.Runtime.CompilerServices;
using BattleCruisers.Utils.DataStrctures;
using static Unity.Collections.AllocatorManager;

namespace BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen
{
    public class MatchmakingScreenController : ScreenController
    {
        private ISceneNavigator _sceneNavigator;
        private ITrashTalkData _trashTalkData;
        private ILocTable _storyStrings;
        public Animator animator;
        public TrashTalkBubblesController trashTalkBubbles;

        public Text leftPlayerName;
        public Image leftPlayerRankImage;
        public Text leftPlayerRankName;
        public Text leftCruiserName;

        public Text rightPlayerName;
        public Image rightPlayerRankeImage;
        public Text rightPlayerRankeName;
        public Text rightCruiserName;

        public Text vsTitile;

        private ILocTable commonStrings;
        private IDataProvider dataProvider;
        private SpriteFetcher spriteFetcher;
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
            _sceneNavigator = LandingSceneGod.SceneNavigator;
            commonStrings = await LocTableFactory.Instance.LoadCommonTableAsync();
            dataProvider = ApplicationModelProvider.ApplicationModel.DataProvider;
            spriteFetcher = new SpriteFetcher();
            DontDestroyOnLoad(gameObject);

            leftCruiserName.text = dataProvider.GameModel.PlayerLoadout.Hull.PrefabName;
            leftPlayerName.text = dataProvider.GameModel.PlayerName;
            int rank = CalculateRank(dataProvider.GameModel.LifetimeDestructionScore);
            leftPlayerRankName.text = commonStrings.GetString(StaticPrefabKeys.Ranks.AllRanks[rank].RankNameKeyBase);
            leftPlayerRankImage.sprite = (await spriteFetcher.GetSpriteAsync("Assets/Resources_moved/Sprites/UI/ScreensScene/DestructionScore/" + StaticPrefabKeys.Ranks.AllRanks[rank].RankImage + ".png")).Sprite;
        }

        public void SetTraskTalkData(ITrashTalkData trashTalkData, ILocTable commonString, ILocTable storyString)
        {
            _trashTalkData = trashTalkData;
            _commonStrings = commonString;
            _storyStrings = storyString;
        }

        public async void FoundCompetitor()
        {
            
           
            if (SynchedServerData.Instance.GetTeam() == Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Team.LEFT)
            {
                leftPlayerName.text = SynchedServerData.Instance.playerAName.Value;
                leftCruiserName.text = SynchedServerData.Instance.playerAPrefabName.Value;
                int rankA = CalculateRank(SynchedServerData.Instance.playerAScore.Value);
                ISpriteWrapper spriteWrapperA = await spriteFetcher.GetSpriteAsync("Assets/Resources_moved/Sprites/UI/ScreensScene/DestructionScore/" + StaticPrefabKeys.Ranks.AllRanks[rankA].RankImage + ".png");
                leftPlayerRankImage.sprite = spriteWrapperA.Sprite;
                leftPlayerRankName.text = commonStrings.GetString(StaticPrefabKeys.Ranks.AllRanks[rankA].RankNameKeyBase);

                rightPlayerName.text = SynchedServerData.Instance.playerBName.Value;
                rightCruiserName.text = SynchedServerData.Instance.playerBPrefabName.Value;
                int rankB = CalculateRank(SynchedServerData.Instance.playerBScore.Value);
                ISpriteWrapper spriteWrapperB = await spriteFetcher.GetSpriteAsync("Assets/Resources_moved/Sprites/UI/ScreensScene/DestructionScore/" + StaticPrefabKeys.Ranks.AllRanks[rankB].RankImage + ".png");
                rightPlayerRankeImage.sprite = spriteWrapperB.Sprite;
                rightPlayerRankeName.text = commonStrings.GetString(StaticPrefabKeys.Ranks.AllRanks[rankB].RankNameKeyBase);
            }
            else
            {
                leftPlayerName.text = SynchedServerData.Instance.playerBName.Value;
                leftCruiserName.text = SynchedServerData.Instance.playerBPrefabName.Value;
                int rankB = CalculateRank(SynchedServerData.Instance.playerBScore.Value);
                ISpriteWrapper spriteWrapperB = await spriteFetcher.GetSpriteAsync("Assets/Resources_moved/Sprites/UI/ScreensScene/DestructionScore/" + StaticPrefabKeys.Ranks.AllRanks[rankB].RankImage + ".png");
                leftPlayerRankImage.sprite = spriteWrapperB.Sprite;
                leftPlayerRankName.text = commonStrings.GetString(StaticPrefabKeys.Ranks.AllRanks[rankB].RankNameKeyBase);

                rightPlayerName.text = SynchedServerData.Instance.playerAName.Value;
                rightCruiserName.text = SynchedServerData.Instance.playerAPrefabName.Value;
                int rankA = CalculateRank(SynchedServerData.Instance.playerAScore.Value);
                ISpriteWrapper spriteWrapperA = await spriteFetcher.GetSpriteAsync("Assets/Resources_moved/Sprites/UI/ScreensScene/DestructionScore/" + StaticPrefabKeys.Ranks.AllRanks[rankA].RankImage + ".png");
                rightPlayerRankeImage.sprite = spriteWrapperA.Sprite;
                rightPlayerRankeName.text = commonStrings.GetString(StaticPrefabKeys.Ranks.AllRanks[rankA].RankNameKeyBase);
            }

            switch (SynchedServerData.Instance.map.Value)
            {
                case Map.PracticeWreckyards:
                    vsTitile.text = "PracticeWreckyards";
                    break;
                case Map.OzPenitentiary:
                    vsTitile.text = "OzPenitentiary";
                    break;
                case Map.UACUltimate:
                    vsTitile.text = "UACUltimate";
                    break;
                case Map.RioBattlesport:
                    vsTitile.text = "RioBattlesport";
                    break;
                case Map.NuclearDome:
                    vsTitile.text = "NuclearDome";
                    break;
                case Map.MercenaryOne:
                    vsTitile.text = "MercenaryOne";
                    break;
                case Map.SanFranciscoFightClub:
                    vsTitile.text = "SanFranciscoFightClub";
                    break;
                case Map.UACArena:
                    vsTitile.text = "UACArena";
                    break;
                case Map.UACBattleNight:
                    vsTitile.text = "UACBattleNight";
                    break;
            }

            await Task.Delay(100);
            animator.SetBool("Found", true);
            //   StartCoroutine(iTrashTalk());
        }

        private int CalculateRank(long score)
        {

            for (int i = 0; i <= StaticPrefabKeys.Ranks.AllRanks.Count; i++)
            {
                long x = 2500 + 2500 * i * i;
                //Debug.Log(x);
                if (score < x)
                {
                    return i;
                }
            }
            return StaticPrefabKeys.Ranks.AllRanks.Count;
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

        private void FailedMatchmaking()
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("ShouldBeDestroyedOnNonPvP");
            foreach (GameObject obj in objs)
            {
                Destroy(obj);
            }
            _sceneNavigator.SceneLoaded(SceneNames.PvP_BOOT_SCENE);
            _sceneNavigator.GoToScene(SceneNames.SCREENS_SCENE, true);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

    }
}

