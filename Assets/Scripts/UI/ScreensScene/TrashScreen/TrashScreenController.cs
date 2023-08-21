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
using UnityEngine.Animations;
using UnityEditor.Animations;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public class TrashScreenController : ScreenController
    {
        private IApplicationModel _appModel;
        private IPrefabFactory _prefabFactory;
        private ISpriteFetcher _spriteFetcher;
        private ITrashTalkProvider _trashDataList;
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

        public void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            IApplicationModel appModel,
            IPrefabFactory prefabFactory,
            ISpriteFetcher spriteFetcher,
            ITrashTalkProvider trashDataList,
            IMusicPlayer musicPlayer,
            ILocTable commonStrings,
            ILocTable storyStrings)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(trashTalkBubbles, cruisers, sky, enemyPrefab, startBattleButton, trashDataList, homeButton);
            Helper.AssertIsNotNull(appModel, prefabFactory, spriteFetcher, trashDataList, musicPlayer, commonStrings);

            _appModel = appModel;
            _prefabFactory = prefabFactory;
            _spriteFetcher = spriteFetcher;
            _trashDataList = trashDataList;
            _musicPlayer = musicPlayer;
            _commonStrings = commonStrings;
            _storyStrings = storyStrings;

            startBattleButton.Initialise(soundPlayer, LoadBattle);
            homeButton.Initialise(soundPlayer, Cancel);
        }

        public async override void OnPresenting(object activationParameter)
        {
            characterCamera.SetActive(true);
            containerCharlie.GetChild(0).gameObject.SetActive(true);
            base.OnPresenting(activationParameter);

            int levelIndex = _appModel.SelectedLevel - 1;
            ILevel level = _appModel.DataProvider.Levels[levelIndex];

            ITrashTalkData trashTalkData = await _trashDataList.GetTrashTalkAsync(_appModel.SelectedLevel);
            trashTalkBubbles.Initialise(trashTalkData, _commonStrings, _storyStrings);
            SetupEnemyCharacter(trashTalkData);

            // Cruisers
            ICruiser playerCruiserPrefab = _prefabFactory.GetCruiserPrefab(_appModel.DataProvider.GameModel.PlayerLoadout.Hull);
            ICruiser enemyCruiserPrefab = _prefabFactory.GetCruiserPrefab(level.Hull);
            cruisers.Initialise(playerCruiserPrefab, enemyCruiserPrefab);

            // Sky
            string skyPath = SKY_SPRITE_ROOT_PATH + level.SkyMaterialName + SPRITES_FILE_EXTENSION;
            ISpriteWrapper skySprite = await _spriteFetcher.GetSpriteAsync(skyPath);
            sky.sprite = skySprite.Sprite;

            _musicPlayer.PlayTrashMusic();
        }

        private void SetupEnemyCharacter(ITrashTalkData trashTalkData)
        {
            //enemyCharacter.sprite = trashTalkData.EnemySprite;
            enemyPrefab = trashTalkData.EnemyPrefab;
            containerCaptains.transform.parent.gameObject.SetActive(true);
            enemyModel = Instantiate(enemyPrefab, containerCaptains.position, Quaternion.identity, containerCaptains);
            enemyModel.transform.localScale = new Vector3(-.5f, .5f, 1f);
            enemyCharacter.localPosition = trashTalkData.EnemyPosition;
        }

        private void StartBattle()
        {
            if (enemyModel != null)
                Destroy(enemyModel);
            _screensSceneGod.LoadBattleScene();
        }

        private void LoadBattle()
        {
            Invoke("StartBattle", 0.5f);
        }

        public override void Cancel()
        {
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
