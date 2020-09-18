using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.UI
{
    public class StoryDialogTestGod : TestGodBase
    {
        public TrashScreenController trashScreen;
        public TrashTalkData trashData;
        public TrashTalkDataList trashDataList;
        public LevelButtonsPanel levelButtonsPanel;

        [Range(1, 25)]
        public int startingLevelNum = 2;

        protected override void Setup(Utilities.Helper helper)
        {
            Helper.AssertIsNotNull(trashScreen, trashData, trashDataList, levelButtonsPanel);

            trashDataList.Initialise();
            HullKey playerCruiser = StaticPrefabKeys.Hulls.Trident;

            levelButtonsPanel
                .Initialise(
                    trashScreen,
                    helper.PrefabFactory,
                    startingLevelNum,
                    playerCruiser,
                    trashDataList);
        }
    }
}