using BattleCruisers.UI.Music;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using NSubstitute;
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

        protected override void Setup(Utilities.Helper helper)
        {
            Helper.AssertIsNotNull(trashScreen, trashDataList, levelButtonsPanel);

            trashDataList.Initialise();

            trashScreen
                .Initialise(
                    Substitute.For<IScreensSceneGod>(),
                    Substitute.For<SingleSoundPlayer>(),
                    trashDataList,
                    trashDataList,
                    Substitute.For<MusicPlayer>());

            levelButtonsPanel
                .Initialise(
                    trashScreen,
                    startingLevelNum);
        }
    }
}