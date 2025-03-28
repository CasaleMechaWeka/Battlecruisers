using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Sprites;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public class TrashScreenController : ScreenController
    {
        private ITrashTalkProvider _levelTrashDataList, _sideQuestTrashDataList;
        private IMusicPlayer _musicPlayer;

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
        private Camera _cameraOfCaptains;

        public void SetCamera(Camera camera)
        {
            _cameraOfCaptains = camera;
        }

        public void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            ITrashTalkProvider levelTrashDataList,
            ITrashTalkProvider sideQuestTrashDataList,
            IMusicPlayer musicPlayer)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(trashTalkBubbles, cruisers, sky, enemyPrefab, startBattleButton, levelTrashDataList, homeButton);
            Helper.AssertIsNotNull(levelTrashDataList, musicPlayer);

            _levelTrashDataList = levelTrashDataList;
            _sideQuestTrashDataList = sideQuestTrashDataList;
            _musicPlayer = musicPlayer;

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

            if (_cameraOfCaptains != null)
            {
                _cameraOfCaptains.orthographicSize = 8;
            }

            ITrashTalkData trashTalkData;
            ICruiser enemyCruiserPrefab;
            string skyPath;
            if (ApplicationModel.Mode == GameMode.SideQuest)
            {
                int sideQuestID = ApplicationModel.SelectedSideQuestID;
                ISideQuestData sideQuestData = StaticData.SideQuests[sideQuestID];
                enemyCruiserPrefab = PrefabFactory.GetCruiserPrefab(sideQuestData.Hull);
                skyPath = SKY_SPRITE_ROOT_PATH + sideQuestData.SkyMaterial + SPRITES_FILE_EXTENSION;

                trashTalkData = await _sideQuestTrashDataList.GetTrashTalkAsync(ApplicationModel.SelectedSideQuestID + 1);
            }
            else
            {
                int levelIndex = ApplicationModel.SelectedLevel - 1;
                ILevel level = StaticData.Levels[levelIndex];
                enemyCruiserPrefab = PrefabFactory.GetCruiserPrefab(level.Hull);
                skyPath = SKY_SPRITE_ROOT_PATH + level.SkyMaterialName + SPRITES_FILE_EXTENSION;

                trashTalkData = await _levelTrashDataList.GetTrashTalkAsync(ApplicationModel.SelectedLevel);
            }

            trashTalkBubbles.Initialise(trashTalkData);
            SetupEnemyCharacter(trashTalkData);

            // Cruisers
            ICruiser playerCruiserPrefab = PrefabFactory.GetCruiserPrefab(DataProvider.GameModel.PlayerLoadout.Hull);
            cruisers.Initialise(playerCruiserPrefab, enemyCruiserPrefab);

            // Sky
            sky.sprite = await SpriteFetcher.GetSpriteAsync(skyPath);

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
            if (DataProvider.GameModel.FirstNonTutorialBattle || ApplicationModel.Mode == GameMode.CoinBattle)
            {
                _screensSceneGod.GotoHubScreen();
            }
            else
            {
                _screensSceneGod.GoToLevelsScreen();
            }

            if (_cameraOfCaptains != null)
            {
                _cameraOfCaptains.orthographicSize = 5;
            }
        }
    }
}
