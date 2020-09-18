using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Cache;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public class TrashScreenController : ScreenController
    {
        public TrashTalkBubblesController trashTalkBubbles;

        public void Initialise(
            ISingleSoundPlayer soundPlayer, 
            IScreensSceneGod screensSceneGod,
            ITrashTalkData trashTalkData,
            ILevel level,
            IPrefabFactory prefabFactory,
            HullKey playerCruiser)
		{
			base.Initialise(soundPlayer, screensSceneGod);

            Assert.IsNotNull(trashTalkBubbles);
            Helper.AssertIsNotNull(trashTalkData, level, prefabFactory, playerCruiser);

            trashTalkBubbles.Initialise(trashTalkData);

            // FELIX  NEXT :D
		}

        public override void Cancel()
        {
            _screensSceneGod.GoToHomeScreen();
        }
    }
}
