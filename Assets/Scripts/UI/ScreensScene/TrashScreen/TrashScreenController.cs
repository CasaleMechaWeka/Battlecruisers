using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public class TrashScreenController : ScreenController
    {
        public TrashTalkBubblesController trashTalkBubbles;
        public BackgroundCruisersController cruisers;
        public Image sky;

        private const string SKY_SPRITE_ROOT_PATH = "Assets/Resources_moved/Sprites/Skies/";
        private const string SPRITES_FILE_EXTENSION = ".png";

        public async Task InitialiseAsync(
            ISingleSoundPlayer soundPlayer, 
            IScreensSceneGod screensSceneGod,
            ITrashTalkData trashTalkData,
            ILevel level,
            IPrefabFactory prefabFactory,
            HullKey playerCruiser,
            ISpriteFetcher spriteFetcher)
		{
			base.Initialise(soundPlayer, screensSceneGod);

            Helper.AssertIsNotNull(trashTalkBubbles, cruisers, sky);
            Helper.AssertIsNotNull(trashTalkData, level, prefabFactory, playerCruiser, spriteFetcher);

            trashTalkBubbles.Initialise(trashTalkData);

            // Cruisers
            ICruiser playerCruiserPrefab = prefabFactory.GetCruiserPrefab(playerCruiser);
            ICruiser enemyCruiserPrefab = prefabFactory.GetCruiserPrefab(level.Hull);
            cruisers.Initialise(playerCruiserPrefab.Sprite, enemyCruiserPrefab.Sprite);

            // Sky
            string skyPath = SKY_SPRITE_ROOT_PATH + level.SkyMaterialName + SPRITES_FILE_EXTENSION;
            ISpriteWrapper skySprite = await spriteFetcher.GetSpriteAsync(skyPath);
            sky.sprite = skySprite.Sprite;
		}

        public override void Cancel()
        {
            _screensSceneGod.GoToHomeScreen();
        }
    }
}
