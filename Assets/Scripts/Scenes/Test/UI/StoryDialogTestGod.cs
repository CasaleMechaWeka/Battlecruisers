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
        public TrashTalkDataList trashDataList;
        public LevelButtonsPanel levelButtonsPanel;

        [Header("Peter can change these :D")]
        [Range(1, 25)]
        public int startingLevelNum = 2;
        public PrefabKeyName playerCruiserKey;

        protected override void Setup(Utilities.Helper helper)
        {
            Helper.AssertIsNotNull(trashScreen, trashDataList, levelButtonsPanel);

            trashDataList.Initialise();
            HullKey playerCruiser = StaticPrefabKeyHelper.GetPrefabKey<HullKey>(playerCruiserKey);

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