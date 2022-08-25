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
        private ITrashTalkProvider _trashDataList;
        private IMusicPlayer _musicPlayer;
        private ILocTable _commonStrings;
        private ILocTable _storyStrings;

        public TrashTalkBubblesController trashTalkBubbles;
        public BackgroundCruisersController cruisers;
        public Image sky, enemyCharacter;
        public CanvasGroupButton startBattleButton, homeButton;

        private const string SKY_SPRITE_ROOT_PATH = "Assets/Resources_moved/Sprites/Skies/";
        private const string SPRITES_FILE_EXTENSION = ".png";

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

            Helper.AssertIsNotNull(trashTalkBubbles, cruisers, sky, enemyCharacter, startBattleButton, trashDataList, homeButton);
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
            enemyCharacter.sprite = trashTalkData.EnemyImage;
            enemyCharacter.transform.localPosition = trashTalkData.EnemyPosition;
            enemyCharacter.transform.localScale = new Vector3(trashTalkData.EnemyScale, trashTalkData.EnemyScale, 1);
        }

        private void StartBattle()
        {
            _screensSceneGod.LoadBattleScene();
        }

        private void LoadBattle()
        {
            Invoke("StartBattle", 0.5f);
        }

        public override void Cancel()
        {
            if (_appModel.DataProvider.GameModel.FirstNonTutorialBattle)
            {
                _screensSceneGod.GoToHomeScreen();
            }
            else
            {
                _screensSceneGod.GoToLevelsScreen();
            }
        }
    }
}
