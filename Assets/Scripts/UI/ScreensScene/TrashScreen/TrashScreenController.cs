using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public class TrashScreenController : ScreenController
    {
        private IApplicationModel _appModel;
        private IPrefabFactory _prefabFactory;
        private ISpriteFetcher _spriteFetcher;
        private ITrashTalkProvider _levelTrashDataList, _sideQuestTrashDataList;
        private IMusicPlayer _musicPlayer;
        private ILocTable _commonStrings;
        private ILocTable _storyStrings;

        public TrashTalkBubblesController trashTalkBubbles;
        public BackgroundCruisersController cruisers;
        public Image sky;
        public GameObject enemyPrefab;
        public CanvasGroupButton startBattleButton, homeButton;
        public Transform containerCaptains;
        public GameObject characterCamera;
        public Transform containerCharlie;
        public Transform enemyCharacter;

        private const string SKY_SPRITE_ROOT_PATH = "Assets/Resources_moved/Sprites/Skies/";
        private const string SPRITES_FILE_EXTENSION = ".png";
        private GameObject enemyModel;
        [SerializeField]

        public void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            IApplicationModel appModel,
            IPrefabFactory prefabFactory,
            ISpriteFetcher spriteFetcher,
            ITrashTalkProvider levelTrashDataList,
            ITrashTalkProvider sideQuestTrashDataList,
            IMusicPlayer musicPlayer,
            ILocTable commonStrings,
            ILocTable storyStrings)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(trashTalkBubbles, cruisers, sky, enemyPrefab, startBattleButton, levelTrashDataList, homeButton);
            Helper.AssertIsNotNull(appModel, prefabFactory, spriteFetcher, levelTrashDataList, musicPlayer, commonStrings);

            _appModel = appModel;
            _prefabFactory = prefabFactory;
            _spriteFetcher = spriteFetcher;
            _levelTrashDataList = levelTrashDataList;
            _sideQuestTrashDataList = sideQuestTrashDataList;
            _musicPlayer = musicPlayer;
            _commonStrings = commonStrings;
            _storyStrings = storyStrings;

            startBattleButton.Initialise(soundPlayer, StartBattle);
            homeButton.Initialise(soundPlayer, Cancel);
        }

        private void PlayTauntAnimationOnCaptains()
        {
            foreach (Transform child in containerCaptains)
            {
                Animator animator = child.GetComponent<Animator>();
                if (animator == null)
                {
                    animator = child.GetComponentInChildren<Animator>();
                }
                if (animator != null)
                {
                    Debug.Log($"Setting taunt trigger for captain: {child.name}");
                    animator.SetTrigger("taunt");
                }
                else
                {
                    Debug.LogWarning($"No Animator component found on captain or its children: {child.name}");
                }
            }

            foreach (Transform child in containerCharlie)
            {
                Animator animator = child.GetComponent<Animator>();
                if (animator == null)
                {
                    animator = child.GetComponentInChildren<Animator>();
                }
                if (animator != null)
                {
                    Debug.Log($"Setting taunt trigger for charlie: {child.name}");
                    animator.SetTrigger("taunt");
                }
                else
                {
                    Debug.LogWarning($"No Animator component found on charlie or its children: {child.name}");
                }
            }
        }

        public async override void OnPresenting(object activationParameter)
        {
            characterCamera.SetActive(true);
            containerCharlie.GetChild(0).gameObject.SetActive(true);
            base.OnPresenting(activationParameter);


            ITrashTalkData trashTalkData;
            ICruiser enemyCruiserPrefab;
            string skyPath;
            if (_appModel.Mode == GameMode.SideQuest)
            {
                int sideQuestID = _appModel.SelectedSideQuestID;
                ISideQuestData sideQuestData = _appModel.DataProvider.SideQuests[sideQuestID];
                enemyCruiserPrefab = _prefabFactory.GetCruiserPrefab(sideQuestData.Hull);
                skyPath = SKY_SPRITE_ROOT_PATH + sideQuestData.SkyMaterial + SPRITES_FILE_EXTENSION;

                trashTalkData = await _sideQuestTrashDataList.GetTrashTalkAsync(_appModel.SelectedSideQuestID + 1);
            }
            else
            {
                int levelIndex = _appModel.SelectedLevel - 1;
                ILevel level = _appModel.DataProvider.Levels[levelIndex];
                enemyCruiserPrefab = _prefabFactory.GetCruiserPrefab(level.Hull);
                skyPath = SKY_SPRITE_ROOT_PATH + level.SkyMaterialName + SPRITES_FILE_EXTENSION;

                trashTalkData = await _levelTrashDataList.GetTrashTalkAsync(_appModel.SelectedLevel);
            }

            trashTalkBubbles.Initialise(trashTalkData, _commonStrings, _storyStrings);
            SetupEnemyCharacter(trashTalkData);

            // Cruisers
            ICruiser playerCruiserPrefab = _prefabFactory.GetCruiserPrefab(_appModel.DataProvider.GameModel.PlayerLoadout.Hull);
            cruisers.Initialise(playerCruiserPrefab, enemyCruiserPrefab);

            // Sky
            ISpriteWrapper skySprite = await _spriteFetcher.GetSpriteAsync(skyPath);
            sky.sprite = skySprite.Sprite;

            _musicPlayer.PlayTrashMusic();
            
            // Play taunt animation on captains
            PlayTauntAnimationOnCaptains();
        }

        private void SetupEnemyCharacter(ITrashTalkData trashTalkData)
        {
            //enemyCharacter.sprite = trashTalkData.EnemySprite;
            enemyPrefab = trashTalkData.EnemyPrefab;
            containerCaptains.transform.parent.gameObject.SetActive(true);
            enemyModel = Instantiate(enemyPrefab, containerCaptains.position, Quaternion.identity, containerCaptains);
            enemyModel.transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        private void StartBattle()
        {
            if (enemyModel != null)
                Destroy(enemyModel);
            _screensSceneGod.LoadBattleScene();
        }

        public override void Cancel()
        {
            LandingSceneGod.Instance.coinBattleLevelNum = -1;

            if (enemyModel != null)
                Destroy(enemyModel);
            if (_appModel.DataProvider.GameModel.FirstNonTutorialBattle || _appModel.Mode == GameMode.CoinBattle)
            {
                _screensSceneGod.GotoHubScreen();
            }
            else
            {
                _screensSceneGod.GoToLevelsScreen();
            }
        }
    }
}
