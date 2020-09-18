using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using NSubstitute;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.UI
{
    public class StoryDialogTestGod : TestGodBase
    {
        public TrashScreenController trashScreen;
        public TrashTalkData trashData;
        [Range(1, 25)]
        public int startingLevelNum = 1;

        protected override void Setup(Utilities.Helper helper)
        {
            base.Setup(helper);

            Helper.AssertIsNotNull(trashScreen, trashData);

            ISingleSoundPlayer soundPlayer = Substitute.For<ISingleSoundPlayer>();
            IScreensSceneGod screensSceneGod = Substitute.For<IScreensSceneGod>();
            IList<ILevel> levels = ApplicationModelProvider.ApplicationModel.DataProvider.Levels;
            Assert.IsTrue(startingLevelNum <= levels.Count);
            ILevel level = levels[startingLevelNum - 1];
            IPrefabFetcher prefabFetcher = new PrefabFetcher();
            HullKey playerCruiser = StaticPrefabKeys.Hulls.Eagle;

            trashScreen.Initialise(soundPlayer, screensSceneGod, trashData, level, helper.PrefabFactory, playerCruiser);
        }
    }
}