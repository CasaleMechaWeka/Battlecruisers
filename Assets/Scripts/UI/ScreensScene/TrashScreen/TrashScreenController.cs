using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public class TrashScreenController : ScreenController
    {
        private IApplicationModel _appModel;
        private IPrefabFactory _prefabFactory;
        private ISpriteFetcher _spriteFetcher;
        private ITrashTalkDataList _trashDataList;

        public TrashTalkBubblesController trashTalkBubbles;
        public BackgroundCruisersController cruisers;
        public Image sky, enemyCharacter;
        public ActionButton startBattleButton, homeButton;

        private const string SKY_SPRITE_ROOT_PATH = "Assets/Resources_moved/Sprites/Skies/";
        private const string SPRITES_FILE_EXTENSION = ".png";

        public void Initialise(
            ISingleSoundPlayer soundPlayer, 
            IScreensSceneGod screensSceneGod,
            IApplicationModel appModel,
            IPrefabFactory prefabFactory,
            ISpriteFetcher spriteFetcher,
            ITrashTalkDataList trashDataList)
		{
			base.Initialise(soundPlayer, screensSceneGod);

            Helper.AssertIsNotNull(trashTalkBubbles, cruisers, sky, enemyCharacter, startBattleButton, trashDataList, homeButton);
            Helper.AssertIsNotNull(prefabFactory, spriteFetcher);

            _appModel = appModel;
            _prefabFactory = prefabFactory;
            _spriteFetcher = spriteFetcher;
            _trashDataList = trashDataList;

            startBattleButton.Initialise(soundPlayer, StartBattle);
            homeButton.Initialise(soundPlayer, Cancel);
		}

        public async override void OnPresenting(object activationParameter)
        {
            base.OnPresenting(activationParameter);

            int levelIndex = _appModel.SelectedLevel - 1;
            ILevel level = _appModel.DataProvider.Levels[levelIndex];

            ITrashTalkData trashTalkData = _trashDataList.GetTrashTalk(_appModel.SelectedLevel);
            enemyCharacter.sprite = trashTalkData.EnemyImage;
            trashTalkBubbles.Initialise(trashTalkData);

            // Cruisers
            ICruiser playerCruiserPrefab = _prefabFactory.GetCruiserPrefab(_appModel.DataProvider.GameModel.PlayerLoadout.Hull);
            ICruiser enemyCruiserPrefab = _prefabFactory.GetCruiserPrefab(level.Hull);
            cruisers.Initialise(playerCruiserPrefab.Sprite, enemyCruiserPrefab.Sprite);

            // Sky
            string skyPath = SKY_SPRITE_ROOT_PATH + level.SkyMaterialName + SPRITES_FILE_EXTENSION;
            ISpriteWrapper skySprite = await _spriteFetcher.GetSpriteAsync(skyPath);
            sky.sprite = skySprite.Sprite;
        }

        private void StartBattle()
        {
            _screensSceneGod.LoadBattleScene();
        }

        public override void Cancel()
        {
            _screensSceneGod.GoToHomeScreen();
        }
    }
}
