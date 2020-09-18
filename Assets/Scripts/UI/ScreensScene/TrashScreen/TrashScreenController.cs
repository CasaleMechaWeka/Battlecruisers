using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public class TrashScreenController : ScreenController
    {
        public TrashTalkBubblesController trashTalkBubbles;
        public BackgroundCruisersController cruisers;

        public void Initialise(
            ISingleSoundPlayer soundPlayer, 
            IScreensSceneGod screensSceneGod,
            ITrashTalkData trashTalkData,
            ILevel level,
            IPrefabFactory prefabFactory,
            HullKey playerCruiser)
		{
			base.Initialise(soundPlayer, screensSceneGod);

            Helper.AssertIsNotNull(trashTalkBubbles, cruisers);
            Helper.AssertIsNotNull(trashTalkData, level, prefabFactory, playerCruiser);

            trashTalkBubbles.Initialise(trashTalkData);

            ICruiser playerCruiserPrefab = prefabFactory.GetCruiserPrefab(playerCruiser);
            ICruiser enemyCruiserPrefab = prefabFactory.GetCruiserPrefab(level.Hull);
            cruisers.Initialise(playerCruiserPrefab.Sprite, enemyCruiserPrefab.Sprite);

            // FELIX Skies
		}

        public override void Cancel()
        {
            _screensSceneGod.GoToHomeScreen();
        }
    }
}
